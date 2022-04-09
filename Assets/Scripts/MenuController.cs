using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Button _playButton, _characterCreatorButton, _creditsButton, _exitButton;

    // Start is called before the first frame update
    void Start()
    {
        InitReferences();

        _playButton.onClick.AddListener(() => LoadScene("GameScene"));
        _characterCreatorButton.onClick.AddListener(() => LoadScene("PlayerCreateScene"));
        _creditsButton.onClick.AddListener(() => LoadScene("CreditsScene"));
        _exitButton.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitReferences()
    {
        _playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        _characterCreatorButton = GameObject.Find("CharacterCreator").GetComponent<Button>();
        _creditsButton = GameObject.Find("CreditsButton").GetComponent<Button>();
        _exitButton = GameObject.Find("ExitButton").GetComponent<Button>();


    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
