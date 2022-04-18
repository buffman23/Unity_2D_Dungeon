using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private Text _pointsText;
    private GameObject deathMenu;
    private int _points;
    private Button respawnButton, mainMenuButton;
    private Canvas canvas;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
        if (scene.name.Contains("Lvl"))
        {
            if(canvas != null)
                canvas.enabled = true;

            if (deathMenu != null)
                deathMenu.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        deathMenu = transform.Find("Canvas/DeathPanel").gameObject;
        respawnButton = deathMenu.transform.Find("RespawnButton").GetComponent<Button>();
        mainMenuButton = deathMenu.transform.Find("MainMenuButton").GetComponent<Button>();

        respawnButton.onClick.AddListener(() => Respawn());
        mainMenuButton.onClick.AddListener(() => GotoMainMenu());
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerDeath()
    {
        if(deathMenu != null)
        {
            deathMenu.SetActive(true);
            GameObject.Find("CoinsText").GetComponent<Text>().text = string.Format("{0} coins collected", _points);
        }
    }

    void Init()
    {
        GameObject go;
        if ((go = GameObject.Find("PointsText")) != null)
        {
            _pointsText = go.GetComponent<Text>();
            _pointsText.text = _points.ToString();
        }

        CharacterController.instance.died.AddListener(OnPlayerDeath);
    }

    public void addPoints(int points)
    {
        setPoints(getPoints() + points);
    }

    public void setPoints(int points)
    {
        _points = points;
        _pointsText.text = _points.ToString();
    }

    public int getPoints()
    {
        return _points;
    }

    private void Respawn()
    {
        setPoints(0);
        deathMenu.SetActive(false);
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GotoMainMenu()
    {
        LoadScene("MenuScene");
        canvas.enabled = false;
    }

    private void LoadScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }
}
