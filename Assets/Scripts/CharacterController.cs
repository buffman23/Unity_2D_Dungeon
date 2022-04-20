using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float decceleration;
    public float gravity;
    public float jumpHeight;
    public float floorThreshold;
    public float jumpCooldown;
    public float maxHealth;
    public float health;

    [HideInInspector]
    public bool allowAttack;

    public static CharacterController instance;

    public UnityEvent died;

    public bool frozen;

    private Vector3 _speedVec;
    private float _maxSpeed;
    private Rigidbody2D _rigidBody;
    private Transform _floorCheck;
    private bool _jumping;
    private float _timeSinceLastJump;
    private Vector3 _originalScale;
    private Vector3 _flippedScale;
    private Transform _headTrans, _headBoneTrans, _torsoBoneTrans;
    private Animator _animator;
    private bool _leftClick;
    private Attack _currentAttack, _previousAttack;
    private List<Skeleton> _skeles = new List<Skeleton>(4);
    private List<GameObject> bones = new List<GameObject>();
    private bool _hitSkeleton;
    private GameObject _playerCameraGO;
    
    private int _faceDirection;

    private int _debrisMask;
    private int _floorMask;

    private Image _headDisplay, _greenbar, _redbar;
    private GameObject _canvasGO;
    private Sprite _happyHead, _okayHead, _sadHead, _deadHead;
    private Sprite[] _headSprites;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitReferences();

        _originalScale = transform.localScale;
        _flippedScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
        _debrisMask = LayerMask.NameToLayer("Debris");
        _floorMask = LayerMask.GetMask(new string[] {"Default", "Skeleton", "DartBoard", "Boulder", "Destroyable"});

        foreach (Transform childTrans in transform)
        {
            SpriteRenderer renderer = childTrans.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                bones.Add(childTrans.gameObject);
            }
        }

        updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0 || frozen)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Attack", true);
        }
        else
        {
            _animator.SetBool("Attack", false);
        }
    }

    private void FixedUpdate()
    {
        if (health == 0 || frozen)
            return;

        float accX = 0;
        float direction = 1;
        bool grounded = isGrounded();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseScreenX = Input.mousePosition.x;
        float mouseScreenY = Input.mousePosition.y;
        float mouseWorldX = mouseWorldPosition.x;
        float mouseWorldY = mouseWorldPosition.y;

        //Debug.Log("Mathf.Atan2(" + mouseWorldY +  "-" + _headTrans.position.y + "," + mouseWorldX + "-" + _headTrans.position.x + ")");
        float headRotation = Mathf.Atan2(mouseWorldY - _headTrans.position.y, mouseWorldX - _headTrans.position.x );

        _headBoneTrans.rotation = Quaternion.Euler(0f, 0f, headRotation * Mathf.Rad2Deg + 90f);

        if (mouseScreenX < Screen.width / 2)
        {
            transform.localScale = _flippedScale;
            _faceDirection = -1;
        }
        else
        {
            _faceDirection = 1;
            transform.localScale = _originalScale;
        }

        if (Input.GetKey(KeyCode.D))
        {
            accX = acceleration;
        }

        if (Input.GetKey(KeyCode.A))
        {
            accX = acceleration;
            direction = -1;
        }

        if (_faceDirection != (_rigidBody.velocity.x > 0 ? 1 : -1))
        {
            _maxSpeed = maxSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _maxSpeed = maxSpeed * 2;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _maxSpeed = maxSpeed / 2;
            
        } 

        if (grounded)
        {
            _speedVec.y = Mathf.Max(_speedVec.y, -2f);

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && _timeSinceLastJump > jumpCooldown)
            {
                _speedVec.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _timeSinceLastJump = 0f;
            }
        } else
        {
            _speedVec.y = gravity * Time.fixedDeltaTime;
        }

        accX = Mathf.Min(accX, _maxSpeed);

        if (accX == 0 && _rigidBody.velocity.x != 0)
        {
            direction = _rigidBody.velocity.x > 0 ? 1 : -1;
            _speedVec.x = Mathf.Min(decceleration, _rigidBody.velocity.x * direction) * -direction;
        }
        else
        {
            _speedVec.x = accX * direction;

            float velocityDirection = _rigidBody.velocity.x > 0 ? 1 : -1;
            if(velocityDirection == direction)
            {
                _speedVec.x = Mathf.Min(_maxSpeed - _rigidBody.velocity.x * direction, _speedVec.x * direction) * direction;
            }
        }
        //Debug.Log("velocity:" + _rigidBody.velocity);
        //Debug.Log("_speedVec:" +_speedVec);
        //_rigidBody.MovePosition(_speedVec);

        _rigidBody.AddForce(_speedVec * _rigidBody.mass, ForceMode2D.Impulse);
        _animator.SetFloat("VelocityX", _rigidBody.velocity.x * _faceDirection);
        _animator.SetFloat("SpeedX", Mathf.Abs(_rigidBody.velocity.x));
        _animator.SetFloat("FallMultiplier", Mathf.Abs(_rigidBody.velocity.x) * (grounded ? 1f :.5f));

        _timeSinceLastJump += Time.fixedDeltaTime;

        if (_currentAttack != Attack.None && _currentAttack != _previousAttack && allowAttack)
        {
            
            foreach (Skeleton skele in new List<Skeleton>(_skeles))
            {
                skele.Knockback(new Vector3(3 * _faceDirection, 2, 0) * skele.gameObject.GetComponent<Rigidbody2D>().mass);
                
                switch (_currentAttack)
                {
                    case Attack.SlashDown:
                        skele.Damage(10);
                        
                        break;
                    case Attack.SlashUp:
                        skele.Damage(20);
                        break;
                    case Attack.Stab:
                        skele.Damage(30);
                        break;
                }
                _previousAttack = _currentAttack;
            }

            _hitSkeleton = _skeles.Count > 0;
        }
    }

    private void InitReferences()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();
        _floorCheck = transform.Find("FloorCheck");
        _headTrans = transform.Find("Head");
        _headBoneTrans = transform.Find("lower_torso/upper_torso/head");
        _torsoBoneTrans = transform.Find("lower_torso/upper_torso/");
        _animator = transform.GetComponent<Animator>();
        _headDisplay = GameObject.Find("Canvas/Health/Head").GetComponent<Image>();
        _redbar = GameObject.Find("Canvas/Health/Bar").GetComponent<Image>();
        _greenbar = GameObject.Find("Canvas/Health/Bar/Green").GetComponent<Image>();
        _canvasGO = GameObject.Find("Canvas");
        _playerCameraGO = transform.Find("PlayerCamera").gameObject;

        _headSprites = Resources.LoadAll<Sprite>("Sprites/Heads");

        foreach(Sprite sprite in _headSprites)
        {
            if (sprite.name.Equals("Dead"))
                _deadHead = sprite;
            else if (sprite.name.Equals("High"))
                _happyHead = sprite;
            else if (sprite.name.Equals("Medium"))
                _okayHead = sprite;
            else if (sprite.name.Equals("Low"))
                _sadHead = sprite;
        }
    }

    public bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_floorCheck.position, -Vector2.up, floorThreshold, _floorMask);
        return hit.collider != null;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public void Knockback(Vector3 force)
    {
        _rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Skeleton skele = other.gameObject.GetComponent<Skeleton>();
        if(skele != null)
        {
            if (!_skeles.Contains(skele))
            {
                _skeles.Add(skele);
                //Debug.Log(_skeles.Count);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Skeleton skele = other.gameObject.GetComponent<Skeleton>();
        if (skele != null)
        {
            _skeles.Remove(skele);
            //Debug.Log(_skeles.Count);
        }
    }

    public void setAttack(Attack attack)
    {
        _currentAttack = attack;
        if (_currentAttack == Attack.None)
            _previousAttack = Attack.None;
    }

    public bool HitSkeleton()
    {
        return _hitSkeleton;
    }

    public void Damage(float damage)
    {
        health = Mathf.Max(0, health - damage);
        updateHealthBar();
        if(health == 0)
        {
            died.Invoke();
            StartCoroutine(FallApart());
            _playerCameraGO.transform.SetParent(null);
            //_playerCameraGO.

        }
    }

    public void Heal(float healing)
    {
        health = Mathf.Min(100, health + healing);
        updateHealthBar();
    }

    private void updateHealthBar()
    {
        _greenbar.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 100 * health / maxHealth);
        _greenbar.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 100);

        if (health >= 66)
        {
            _headDisplay.sprite = _happyHead;
        } 
        else if (health >= 33)
        {
            _headDisplay.sprite = _okayHead;
        }
        else if (health > 0)
        {
            _headDisplay.sprite = _sadHead;
        }
        else
        {
            _headDisplay.sprite = _deadHead;
        }
    }

    IEnumerator FallApart()
    {
        Destroy(transform.Find("lower_torso").gameObject);
        Destroy(transform.GetComponent<BoxCollider2D>());

        foreach (BoxCollider2D collider in transform.GetComponents<BoxCollider2D>())
            Destroy(collider);

        foreach (GameObject bone in bones)
        {
            bone.AddComponent<Rigidbody2D>();
            bone.AddComponent<BoxCollider2D>();
            bone.layer = LayerMask.NameToLayer("Debris");
        }

        yield return new WaitForSeconds(0f);

        /*
        yield return new WaitForSeconds(3);

        while (bones.Count > 0)
        {
            int idx = Random.Range(0, bones.Count);
            GameObject bone = bones[idx];
            bones.RemoveAt(idx);
            Destroy(bone.GetComponent<BoxCollider2D>());
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);

        */
    }
}



