using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGallery : MonoBehaviour
{
    public TextMeshProUGUI author_txt;
    public TextMeshProUGUI code_txt;
    public TextMeshProUGUI size_txt;
    public TextMeshProUGUI enemies_txt;
    public TextMeshProUGUI percVict_txt;
    public TextMeshProUGUI visits_txt;
    public TextMeshProUGUI likes_txt;

    public RawImage preview_rimg;

    public GameObject towerCrd_obj;
    public GameObject horseCrd_obj;
    public GameObject bishopCrd_obj;

    public GameObject alertTxt;

    public ButtonLongPress btn;

    private void Start()
    {
        alertTxt.SetActive(false);   
    }

    public void CopyCode()
    {
        alertTxt.SetActive(true);
        GUIUtility.systemCopyBuffer = code_txt.text;
        Debug.Log("El codigo copiado es " + code_txt.text);
        Invoke("OffAlert", 1f);
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
}