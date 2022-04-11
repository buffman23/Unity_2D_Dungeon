using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Color low, high;
    public Vector3 offset;
    public float maxHealth;

    private CanvasGroup _canvasGroup;
    private Image _greenImage, _redImage;
    private GameObject _hbGO, _child;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");

        _hbGO = new GameObject();
        _hbGO.AddComponent<CanvasGroup>();
        _hbGO.transform.SetParent(canvas.transform);
        _hbGO.name = "HealthBar";
        

        _child = new GameObject();
        _child.transform.SetParent(_hbGO.transform);
        _child.name = "Green";
        

        _redImage = _hbGO.AddComponent<Image>();
        _greenImage = _child.AddComponent<Image>();

        _redImage.rectTransform.sizeDelta = new Vector2(100, 10);
        _greenImage.rectTransform.sizeDelta = new Vector2(100, 10);

        _redImage.color = Color.red;
        _greenImage.color = Color.green;

        //setVisible(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        _hbGO.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
    }

    public void SetHealth(float health)
    {
        _greenImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 100 * health/maxHealth);
        _greenImage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 10);
        //_greenImage.rectTransform.sizeDelta = new Vector2(100f * health/maxHealth, 10);
    }

    public void setVisible(bool b)
    {
        _hbGO.GetComponent<CanvasGroup>().alpha = b ? 1 : 0;
    }

    private void OnDestroy()
    {
        Destroy(_hbGO);
    }
}
