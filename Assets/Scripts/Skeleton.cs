using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public float activateRadius;
    public float speed;

    public float knockbackCooldown;
    public float health;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private Transform _floorCheck;
    private Transform _headTransform;
    private Vector3 moveVector;
    private int _targetDirection;
    private Vector3 _originalScale;
    private Vector3 _flippedScale;
    private float _knockbackCurrCooldown;
    private HealthBar _healthBar;
    private bool dead = false;

    private List<GameObject> bones = new List<GameObject>();
    private int layerMask;
    private BoxCollider2D _collider;

    private static CharacterController _characterController;
    private static Transform _targetTrans1, _targetTrans2;
    private static int _default_layer = 0;


    // Start is called before the first frame update
    void Start()
    {
        InitReferences();

        foreach(Transform childTrans in transform)
        {
            SpriteRenderer renderer = childTrans.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                bones.Add(childTrans.gameObject);
            }
        }

        layerMask = LayerMask.GetMask(new string[]{"Default", "Player"});
        Debug.Log("Layer Mask = " + layerMask);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        Vector3 velocity = _rigidBody.velocity;
        _animator.SetFloat("VelocityX", velocity.x);
        _animator.SetFloat("SpeedX", Mathf.Abs(velocity.x));

        _targetDirection = GetTargetDirection();

        if(_targetDirection != 0)
        {
            transform.localScale = _targetDirection > 0 ? _originalScale : _flippedScale;
        }

        _knockbackCurrCooldown += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (dead)
            return;

        if (_targetDirection != 0)
        {
            moveVector.x = _targetDirection * speed;
            _rigidBody.AddForce(moveVector * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    private int GetTargetDirection()
    {
        /*
        Vector3 lookVector = (_targetTrans1.position - _headTransform.position).normalized;
        Debug.DrawRay(_headTransform.position, lookVector * activateRadius);
        RaycastHit2D hit = Physics2D.Raycast(_headTransform.position, lookVector, LayerMask.NameToLayer("Default"));

        if (hit.collider != null && hit.distance < activateRadius && hit.collider.gameObject.tag.Equals("Player"))
        {
            return lookVector.x > 0 ? 1 : -1;
        }*/

        Vector3 lookVector = (_targetTrans2.position - _headTransform.position).normalized;
        Debug.DrawRay(_headTransform.position, lookVector * activateRadius);
        RaycastHit2D hit = Physics2D.Raycast(_headTransform.position, lookVector, activateRadius, layerMask);

        if (hit.collider != null && hit.collider.gameObject.tag.Equals("Player"))
        {
            return lookVector.x > 0 ? 1 : -1;
        }

        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
            _animator.SetBool("Attack", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
            _animator.SetBool("Attack", false);
    }

    private void InitReferences()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();
        _animator = transform.GetComponent<Animator>();
        _floorCheck = transform.Find("FloorCheck");
        _headTransform = transform.Find("Head");
        _originalScale = transform.localScale;
        _flippedScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
        _healthBar = GetComponent<HealthBar>();
        _collider = GetComponent<BoxCollider2D>();

        if (_characterController == null)
        {
            _characterController = CharacterController.instance;
            _targetTrans1 = _characterController.transform;
            _targetTrans2 = _characterController.transform.Find("Torso");
        }
    }

    public void Knockback(Vector3 force)
    {
        if (_knockbackCurrCooldown > knockbackCooldown)
        {
            _rigidBody.AddForce(force, ForceMode2D.Impulse);
            _knockbackCurrCooldown = 0;
        }
    }

    public void Attack(float damage)
    {
        health = Mathf.Max(0, health - damage);
        _healthBar.SetHealth(health);

        if(health == 0)
        {
            dead = true;
            



            StartCoroutine(SkeletonDeath());
        }
    }
    IEnumerator SkeletonDeath()
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

        yield return new WaitForSeconds(3);

        while(bones.Count > 0)
        {
            int idx = Random.Range(0, bones.Count);
            GameObject bone = bones[idx];
            bones.RemoveAt(idx);
            Destroy(bone.GetComponent<BoxCollider2D>());
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }
}
