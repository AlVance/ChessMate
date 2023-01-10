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

    public void TAPStart()
    {
        TAPtransform.DOMoveY(TAP_Y * Screen.height, TAPTimeMoving).SetEase(Ease.InOutBack);
        GameManager.Instance.ChangeSceneDelay("LevelsScene", TAPTimeMoving/2);
    }
}