using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{
    public float debounce = 1f;
    private PlatformEffector2D _PE;
    private LayerMask _defultMask, _noPlayerMask;
    private bool _floorOff, _playerTouching;
    private float debounceCount;

    // Start is called before the first frame update
    void Start()
    {
        _PE = GetComponent<PlatformEffector2D>();

        int playerMask = LayerMask.GetMask("Player");

        _defultMask = _PE.colliderMask;
        _noPlayerMask = _defultMask & ~playerMask;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && _playerTouching)
        {
            _PE.colliderMask = _noPlayerMask;
            _floorOff = true;
            debounceCount = 0f;
        }
        

        if(_floorOff)
        {
            if (debounceCount >= debounce) {
                _PE.colliderMask = _defultMask;
                _floorOff = false;
            }
            else
            {
                debounceCount += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController cc;
        if ((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
            _playerTouching = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CharacterController cc;
        if ((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
            _playerTouching = false;
    }
}
