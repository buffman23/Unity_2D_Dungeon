using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public static float damage = 10;
    public static float knockBack = 7;


    private float _maxTimeAlive = 15f, _timeAlive;
    private Rigidbody2D _RB;
    private AudioSource _AS;
    private static AudioClip _shootAudio, _wobbleAudio;

    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
        _AS = GetComponent<AudioSource>();

        if(_shootAudio == null)
        {
            _shootAudio = SoundController.instance.getClip("shoot");
            _wobbleAudio = SoundController.instance.getClip("arrow_wobble");
        }

        _AS.clip = _shootAudio;
        _AS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _timeAlive += Time.deltaTime;

        if (_timeAlive >= _maxTimeAlive)
        {
            Destroy(this.gameObject);
        }
    }

    void InitReferences()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        CharacterController cc;

        if((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
        {
            cc.Damage(Dart.damage);
            cc.Knockback(Dart.knockBack * _RB.velocity.normalized);
        }
        else if(collision.gameObject.GetComponent<Skeleton>() == null)
        {
            Rigidbody2D rb;
            if ((rb = collision.gameObject.GetComponent<Rigidbody2D>()) != null)
            {
                rb.AddForce(Dart.knockBack * .2f * _RB.velocity.normalized, ForceMode2D.Impulse);
            }
        }
        

        StartCoroutine(WobbleAndDestroy());
    }


    IEnumerator WobbleAndDestroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        //_AS.clip = _wobbleAudio;
        //_AS.time = .1f;
        //_AS.Play();

        while (_AS.isPlaying)
        {
            yield return new WaitForSeconds(.1f);
        }

        Destroy(this.gameObject);
    }
}

