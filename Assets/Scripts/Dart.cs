using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public static float damage = 10;
    public static float knockBack = 7;

    private float _maxTimeAlive = 15f, _timeAlive;
    private Rigidbody2D _RB;
    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
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
        Destroy(this.gameObject);
    }
}
