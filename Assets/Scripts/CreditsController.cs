using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();

        backButton.onClick.AddListener(() => LoadScene("MenuScene"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }
}
