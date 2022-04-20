using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadScene(nextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController cc;

        if((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
        {
            SoundController.instance.PlayAudio("door", .5f);
            LoadScene(nextScene);
        }
    }



    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
