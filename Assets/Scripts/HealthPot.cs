using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPot : MonoBehaviour
{
    public float healthRestore;
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

        if ((cc = collision.gameObject.GetComponent<CharacterController>()) == null)
            return;

        cc.Heal(healthRestore);

        SoundController.instance.PlayAudio("bottle");

        Destroy(this.gameObject);
    }
}
