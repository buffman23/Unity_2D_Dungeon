using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float pickupCooldown = 0f;

    private float _pickupCooldownCount;

    private bool _isTouchingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _pickupCooldownCount += Time.deltaTime;

        if (_pickupCooldownCount >= pickupCooldown && _isTouchingPlayer)
        {
            _isTouchingPlayer = false;
            _pickupCooldownCount = 0f;
            GameController.instance.addPoints(1);
            SoundController.instance.PlayAudio("coin", .5f);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController cc;
        if ((cc = collision.GetComponent<CharacterController>()) != null)
        {
            _isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CharacterController cc;
        if ((cc = collision.GetComponent<CharacterController>()) != null)
        {
            _isTouchingPlayer = false;
        }
    }
}
