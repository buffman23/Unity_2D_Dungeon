using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float pickupCooldown = 1f;

    private float _pickupCooldownCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _pickupCooldownCount += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController cc;
        if(_pickupCooldownCount >= pickupCooldown && (cc = collision.GetComponent<CharacterController>()) != null)
        {
            GameController.instance.addPoints(1);
            Destroy(this.gameObject);
        }
    }
}
