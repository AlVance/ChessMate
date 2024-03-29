using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemGallery : MonoBehaviour
{
    public TextMeshProUGUI author_txt;
    public TextMeshProUGUI code_txt;
    public TextMeshProUGUI size_txt;
    public TextMeshProUGUI enemies_txt;
    public TextMeshProUGUI percVict_txt;
    public TextMeshProUGUI visits_txt;
    public TextMeshProUGUI likes_txt;

    [Space]
    public RawImage preview_rimg;

    public GameObject towerCrd_obj;
    public GameObject horseCrd_obj;
    public GameObject bishopCrd_obj;

    public GameObject alertTxt;

    public ButtonLongPress btn;

    public GameObject masks;
    public GameObject data;

    [Header("Animaciones")]
    public RectTransform[] infoLevelTransform;
    public RectTransform[] infoCommunityTransform, nameTransform, codeTransform;
    public float animDuration;

    public bool onScreen;
    bool opened;

    private void Start()
    {
        //data.SetActive(false);
        //masks.SetActive(true);
        alertTxt.SetActive(false);
    }
    private void LateUpdate()
    {
        if (onScreen && !opened)
        {
            OpenAnim(true);
        }
        else if(!onScreen && opened)
        {
            OpenAnim(false);
        }
    }

    public void CopyCode()
    {
        alertTxt.SetActive(true);
        GUIUtility.systemCopyBuffer = code_txt.text;
        Debug.Log("El codigo copiado es " + code_txt.text);
        Invoke("OffAlert", 1f);
    }

    public void OpenAnim(bool _open = true)
    {
        opened = _open;
        int value = 0;
        if (_open) value = 0;
        else value = -1;

        for (int iL = 0; iL < infoLevelTransform.Length; iL++)
        {
            Debug.Log("Size info level " + (-value * (infoLevelTransform[iL].sizeDelta.x)));
            infoLevelTransform[iL].DOAnchorPosX(-value * (infoLevelTransform[iL].sizeDelta.x), animDuration, true);
        }

        for (int iC = 0; iC < infoCommunityTransform.Length; iC++)
        {
            infoCommunityTransform[iC].DOAnchorPosX(value * (infoCommunityTransform[iC].sizeDelta.x), animDuration, true);
        }

        for (int n = 0; n < nameTransform.Length; n++)
        {
            nameTransform[n].DOAnchorPosY(value * (nameTransform[n].sizeDelta.y), animDuration, true);
        }

        for (int c = 0; c < codeTransform.Length; c++)
        {
            codeTransform[c].DOAnchorPosY(-value * (codeTransform[c].sizeDelta.y), animDuration, true);
        }
    }

    public void OffAlert()
    {
        alertTxt.SetActive(false);
    }

    public void SetAuthor(string _author){ author_txt.text = _author; }
    public void SetCode(string _code){ code_txt.text = _code; }
    public void SetSize(string _size) { size_txt.text = _size; }
    public void SetEnemies(string _enemiesCount) { enemies_txt.text = _enemiesCount; }
    public void SetPercent(string _percent) { percVict_txt.text = _percent; }
    public void SetVisits(string _visits) { visits_txt.text = _visits; }
    public void SetLikes(string _likes) { likes_txt.text = _likes; }
    public void SetPreview(Texture2D _texture) { preview_rimg.texture = _texture; }
    public void SetTower(bool _act) { towerCrd_obj.SetActive(_act); }
    public void SetHorse(bool _act) { horseCrd_obj.SetActive(_act); }
    public void SetBishop(bool _act) { bishopCrd_obj.SetActive(_act); }

    public void FinishLoad()
    {
        masks.SetActive(false);
        data.SetActive(true);
    }
}
