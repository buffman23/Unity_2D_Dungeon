using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> ignore;

    public float fuse;

    public static float damage = 25f;
    public static float explosionForce = 20f;

    private float _fuseCount;
    private float _redBombThreshold;
    private Color _baselineColor;
    private SpriteRenderer _SR;

    private LayerMask _hitLayerMask;
    private int _playerLayer, _skeleLayer, _boulderLayer, _destroyableLayer;

    private bool exploding = false;

    private AudioSource _AS;
    private static AudioClip[] _boomAudios;


    private float _maxTimeAlive = 15f, _timeAlive;
    // Start is called before the first frame update
    void Start()
    {
        InitReferences();

        _redBombThreshold = fuse * .50f;
        _SR = GetComponent<SpriteRenderer>();
        _baselineColor = _SR.color;

        _hitLayerMask = LayerMask.GetMask("Player", "Skeleton", "Boulder", "Destroyable");
        _playerLayer = LayerMask.NameToLayer("Player");
        _skeleLayer = LayerMask.NameToLayer("Skeleton");
        _boulderLayer = LayerMask.NameToLayer("Boulder");
        _destroyableLayer = LayerMask.NameToLayer("Destroyable");
    }


    // Update is called once per frame
    void Update()
    {
        _timeAlive += Time.deltaTime;
        _fuseCount += Time.deltaTime;

        if (_timeAlive >= _maxTimeAlive)
        {
            Destroy(this.gameObject);
        }

        if(_fuseCount >= _redBombThreshold)
        {
            float percent = (_fuseCount - _redBombThreshold) / (fuse - _redBombThreshold);
            float intensity = Mathf.Abs(Mathf.Sin(percent * Mathf.PI * 2.5f));

            _SR.color = new Color(1, intensity, intensity);

            if(_fuseCount > fuse && !exploding)
            {
                StartCoroutine(Explode());
            }
        }
    }

    void InitReferences()
    {
        _AS = GetComponent<AudioSource>();

        if(_boomAudios == null)
        {
            _boomAudios = new AudioClip[3];
            _boomAudios[0] = SoundController.instance.getClip("explosion01");
            _boomAudios[1] = SoundController.instance.getClip("explosion02");
            _boomAudios[2] = SoundController.instance.getClip("explosion03");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (ignore.Contains(collision.gameObject))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
        /*
        if (collision.gameObject.tag.Equals("Player") || collision.gameObject.tag.Equals("Boom"))
            if(!exploding)
                StartCoroutine(Explode());
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public IEnumerator Explode()
    {
        if(exploding)
            yield break;

        exploding = true;

        int randIdx = Random.Range(0, _boomAudios.Length);
        _AS.clip = _boomAudios[randIdx];
        _AS.Play();

        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider>());

        GameObject explosionGO = transform.Find("Explosion").gameObject;
        explosionGO.SetActive(true);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, _hitLayerMask);
        foreach(Collider2D collider in colliders)
        {
           

            if (collider.gameObject.layer == _playerLayer)
            {
                CharacterController cc;
                if ((cc = collider.gameObject.GetComponent<CharacterController>()) != null)
                {
                    Vector2 centerOfMass = cc.gameObject.GetComponent<Rigidbody2D>().centerOfMass;
                    Vector3 explosionDirection = (new Vector3(centerOfMass.x, centerOfMass.y, transform.position.z) + collider.transform.position - this.transform.position).normalized;
                    Debug.DrawRay(transform.position, explosionDirection * Bomb.explosionForce, Color.white);
                    cc.Damage(Bomb.damage * GameController.instance.difficultyMultiplier);
                    cc.Knockback(explosionDirection * Bomb.explosionForce);
                }
            }
            else if (collider.gameObject.layer == _skeleLayer)
            {
                Skeleton skele;
                if ((skele = collider.gameObject.GetComponent<Skeleton>()) != null)
                {
                    Vector2 centerOfMass = skele.gameObject.GetComponent<Rigidbody2D>().centerOfMass;
                    Vector3 explosionDirection = (new Vector3(centerOfMass.x, centerOfMass.y, transform.position.z) + collider.transform.position - this.transform.position).normalized;
                    skele.Damage(Bomb.damage);
                    skele.Knockback(explosionDirection * Bomb.explosionForce / 2);
                }
            }
            else if(collider.gameObject.layer == _destroyableLayer)
            {
                Destroy(collider.gameObject);
            }
            else
            {
                Bomb bomb;
                if ((bomb = collider.gameObject.GetComponent<Bomb>()) != null)
                {
                    if(bomb != this)
                        StartCoroutine(bomb.Explode());
                }
                else
                {
                    Rigidbody2D rb;
                    if ((rb = collider.gameObject.GetComponent<Rigidbody2D>()) != null)
                    {
                        Vector2 centerOfMass = rb.centerOfMass;
                        Vector3 explosionDirection = (new Vector3(centerOfMass.x, centerOfMass.y, transform.position.z) + collider.transform.position - this.transform.position).normalized;
                        rb.AddForce(explosionDirection * Bomb.explosionForce, ForceMode2D.Impulse);
                    }
                }
            }
        }

        yield return new WaitForSeconds(.3333f);

        explosionGO.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        while (_AS.isPlaying)
        {
            yield return new WaitForSeconds(.1f);
        }

        Destroy(this.gameObject);
    }

}
