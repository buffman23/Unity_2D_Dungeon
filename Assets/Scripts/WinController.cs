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
    private Text _message;
    // Start is called before the first frame update
    void Start()
    {
        InitReferneces();

        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        CharacterController.instance.frozen = true;

        if(GameController.instance.playerName.Length > 0)
        {
            _message.text = _message.text.Replace("You", GameController.instance.playerName);
        }
        
    }

    void InitReferneces()
    {
        _mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        _dagger = GameObject.Find("dagger");
        _health = GameObject.Find("Health");
        _message = GameObject.Find("MessageText").GetComponent<Text>();
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
