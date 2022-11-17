using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorCell : MonoBehaviour
{
    public Vector2Int ids;
    public int total;
    public bool isPlayer;
    public bool isEnemy;
    public int typeEnemy;
    public bool isKing;
    public bool isCard;
    public int typeCard;
    public bool isObstacle;
    public bool onRoute;

    TextMeshProUGUI _tmp;
    Image _img;

    private void Start()
    {
        _tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _img = transform.Find("Image").GetComponent<Image>();
    }

    public void ChangeColor(Color _color)
    {
        _img.color = _color;
    }

    public void SetRoute(int _indx)
    {
        typeEnemy = _indx;
        if (_tmp.gameObject.activeInHierarchy)
        {
            _tmp.text += " "+ typeEnemy;
        }
        else
        {
            _tmp.gameObject.SetActive(true);
            _tmp.text = typeEnemy.ToString();
        }
    }
}
