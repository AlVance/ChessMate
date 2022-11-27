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
    public string typeEnemy;
    public bool isKing;
    public bool isCard;
    public int typeCard;
    public bool isObstacle;
    public bool onRoute;

    TextMeshProUGUI _tmp;
    Image _img;
    public Sprite[] typeImg;

    private void Start()
    {
        _tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _img = transform.Find("Image").GetComponent<Image>();
    }

    public void ChangeColor(Color _color)
    {
        _img.color = _color;
    }

    public void ChangeImage(int indx)
    {
        _img.sprite = typeImg[indx];
    }

    public void SetRoute(int _indx)
    {
        if (typeEnemy == "") typeEnemy = _indx.ToString();
        else typeEnemy += _indx.ToString();
        if (_tmp.gameObject.activeInHierarchy)
        {
            _tmp.text = typeEnemy;
        }
        else
        {
            _tmp.gameObject.SetActive(true);
            _tmp.text = typeEnemy.ToString();
        }
        onRoute = true;
    }

    public void ClearRoute(int _indx)
    {
        Debug.Log("Busco " + _indx + " en " + typeEnemy);
        if (typeEnemy.Contains(_indx.ToString()))
        {
            typeEnemy = typeEnemy.Replace(_indx.ToString(), "");
        }
        _tmp.text = typeEnemy;
        Debug.Log("typeEnemy " + typeEnemy + " " + typeEnemy.Length);
        if(typeEnemy.Length == 0)
        {
            onRoute = false;
            ChangeImage(0);
        }
        else
        {
            onRoute = true;
            ChangeImage(7);
        }
    }
}
