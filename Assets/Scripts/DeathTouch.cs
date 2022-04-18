using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTouch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterController cc;

        if((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
        {
            cc.Damage(100000);
            return;
        }

        Skeleton skele;

        if ((skele = collision.gameObject.GetComponent<Skeleton>()) != null)
        {
            skele.Damage(100000);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc;

        if ((cc = other.gameObject.GetComponent<CharacterController>()) != null)
        {
            cc.Damage(100000);
            return;
        }

        Skeleton skele;

        if ((skele = other.gameObject.GetComponent<Skeleton>()) != null)
        {
            skele.Damage(100000);
        }
    }
}
