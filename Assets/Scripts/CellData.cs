using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    public int xID;
    public int zID;

    public int idTotal;

    public Vector3 pos;

    public bool isEnemy;
    public bool canMove;
    public Button btn;

    public PlayerCtrl.Type typeCard;
    public Color[] colors;

    private void Start()
    {
        btn.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        btn.GetComponent<Image>().color = colors[0];
    }

    public void ActiveBtn(bool _active)
    {
        btn.gameObject.SetActive(_active);
        if(isEnemy) btn.GetComponent<Image>().color = colors[1];
        else btn.GetComponent<Image>().color = colors[0];
    }
    public void ActiveBtn(bool _active, int _color)
    {
        btn.gameObject.SetActive(_active);
        btn.GetComponent<Image>().color = colors[_color];
    }
}
