using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinController: MonoBehaviour
{
    private Button _mainMenuButton;
    private GameObject _health;
    private GameObject _dagger;
    // Start is called before the first frame update
    void Start()
    {
        InitReferneces();

        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _health.SetActive(false);
        CharacterController.instance.frozen = true;

        
    }

    void InitReferneces()
    {
        _mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        _dagger = GameObject.Find("dagger");
        _health = GameObject.Find("Health");
    }

    void OnMainMenuButtonClick()
    {
        LoadScene("MenuScene");
    }

    private void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }


}
