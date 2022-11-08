using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    public int xID;
    public int zID;

    public int idTotal;

    public bool canMove;
    public Button btn;

    private void Start()
    {
        btn.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void ActiveBtn(bool _active)
    {
        btn.gameObject.SetActive(_active);
    }
}
