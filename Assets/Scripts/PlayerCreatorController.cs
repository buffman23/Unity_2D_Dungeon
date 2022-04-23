using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCreatorController : MonoBehaviour
{
    private InputField _nameInput;
    private Button _backButton;
    private Slider _hueSlider, _difficultySlider;
    private Text _difficultyText;
    private float _sliderMin, _sliderMax;
    private Image _playerImage;
    private Texture2D _playerTexture;
    private Sprite[] _sprites;
    private Color[] _colors;

    private int _totalColors = 20;
    private int _oldValue;


    // Start is called before the first frame update
    void Start()
    {
        InitReferences();

        _nameInput.onValueChanged.AddListener(NameChaneged);
        _backButton.onClick.AddListener(() => LoadScene("MenuScene"));
        _hueSlider.onValueChanged.AddListener(HueSliderValueChanged);
        _difficultySlider.onValueChanged.AddListener(DiffcultySliderValueChanged);


        _difficultySlider.value = GameController.instance.difficultyMultiplier /2f;
        _nameInput.text = GameController.instance.playerName;

        float H, S, V;

        for (int i = 0;  i < _sprites.Length; ++i) {
            Texture2D texture = new Texture2D(_playerTexture.width, _playerTexture.height);
            Graphics.CopyTexture(_playerTexture, texture);

            Color color = Color.HSVToRGB((float)i/_totalColors, .5f, .5f);
            _colors[i] = color;

            for (int j = 0; j < _playerTexture.width; ++j)
            {
                for (int k = 0; k < _playerTexture.height; ++k)
                {
                    Color pixelColor = _playerTexture.GetPixel(j, k);
                    //float H, S, V;
                    Color.RGBToHSV(pixelColor, out H, out S, out V);
                    if (V > .5f && pixelColor.a == 1)
                    {
                        _playerTexture.SetPixel(j, k, color);
                    }
                }
            }
            texture.Apply();

            _sprites[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        _hueSlider.value = GameController.instance.hueSliderValue;
    }

    void InitReferences()
    {
        _nameInput = GameObject.Find("NameInput").GetComponent<InputField>();
        _backButton = GameObject.Find("BackButton").GetComponent<Button>();
        _hueSlider = GameObject.Find("HueSlider").GetComponent<Slider>();
        _difficultySlider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        _difficultyText = GameObject.Find("DifficultyText").GetComponent<Text>();
        _playerImage = GameObject.Find("Character").GetComponent<Image>();
        Texture2D originalTexture = (Texture2D)_playerImage.mainTexture;
        _playerTexture = new Texture2D(originalTexture.width, originalTexture.height);
        Graphics.CopyTexture(originalTexture, _playerTexture);
        GameObject.Find("Character").GetComponent<Image>().sprite = Sprite.Create(_playerTexture, new Rect(0, 0, _playerTexture.width, _playerTexture.height), new Vector2(0.5f, 0.5f));

        _sliderMin = _hueSlider.minValue;
        _sliderMax = _hueSlider.maxValue;


        _sprites = new Sprite[_totalColors];
        _colors = new Color[_totalColors];
    }

    private void NameChaneged(string newName)
    {
        GameController.instance.playerName = newName;
    }

    private void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }

    void HueSliderValueChanged(float newValue)
    {

        int newValueInt = (int)(newValue * _totalColors / (_sliderMax - _sliderMin + 1));
        Debug.Log(newValueInt);
        if (newValueInt != _oldValue)
        {
            _playerImage.sprite = _sprites[newValueInt];
            if (newValueInt != 0)
            {
                GameController.instance.playerColor = _colors[newValueInt - 1];
                //GameObject.Find("Handle").GetComponent<Image>().color = _colors[newValueInt - 1];
            }
            else
            {
                GameController.instance.playerColor = Color.white;
            }
            GameController.instance.hueSliderValue = newValue;
            _oldValue = newValueInt;
        }

        
    }

    void DiffcultySliderValueChanged(float newValue)
    {
        int newValueInt = (int)(newValue * 2);

        if(newValue == 0)
        {
            _difficultyText.text = "Easy";
        }
        else if(newValue == 1)
        {
            _difficultyText.text = "Normal";
        }
        else if (newValue == 2)
        {
            _difficultyText.text = "Hard";
        }

        GameController.instance.difficultyMultiplier = (newValueInt + 2f) /2f;
    }
}
