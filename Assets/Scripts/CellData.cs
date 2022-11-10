using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    public Vector2Int ids;

    public int idTotal;

    public Vector3 pos;

    public bool isEnemy;
    public bool obstacle;
    public bool canMove;
    public Button btn;

    public PlayerCtrl.Type typeCard;
    public GameObject card;
    public GameObject obst;
    public Color[] colors;

    private void Start()
    {
        btn.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        btn.GetComponent<Image>().color = colors[0];
    }

    public void ActiveBtn(bool _active)
    {
        if (!obstacle)
        {
            btn.gameObject.SetActive(_active);
            if (isEnemy) btn.GetComponent<Image>().color = colors[1];
            else btn.GetComponent<Image>().color = colors[0];
        }
    }
    public void ActiveBtn(bool _active, int _color)
    {
        btn.gameObject.SetActive(_active);
        btn.GetComponent<Image>().color = colors[_color];
    }
    public void ActiveBtn(bool _active, Color _color)
    {
        btn.gameObject.SetActive(_active);
        btn.GetComponent<Image>().color = _color;
    }

    public void ShowCell(bool _active, int _color, float _time = .5f)
    {
        StartCoroutine(ShowCellTime(_active, _color, _time));
    }

    public IEnumerator ShowCellTime(bool _active, int _color, float _time)
    {
        bool playered = btn.gameObject.activeInHierarchy;
        Color prevColor = btn.GetComponent<Image>().color;
        ActiveBtn(_active, _color);
        btn.interactable = false;
        yield return new WaitForSeconds(_time);
        btn.interactable = true;
        ActiveBtn(playered, prevColor);
    }
}
