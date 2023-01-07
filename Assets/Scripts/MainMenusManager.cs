using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenusManager : MonoBehaviour
{
    [Header("TAP")]
    public RectTransform TAPtransform;
    public float TAPTimeMoving;
    public float TAP_Y;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TAPStart()
    {
        TAPtransform.DOMoveY(TAP_Y * Screen.height, TAPTimeMoving).SetEase(Ease.InOutBack);

    }
}