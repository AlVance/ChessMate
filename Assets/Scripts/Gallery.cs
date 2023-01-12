using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class Gallery : MonoBehaviour
{
    public GameObject itemGallery;
    public Transform contentGallery;
    List<ItemGallery> itemList = new List<ItemGallery>();
    float startYContent;

    public GameObject galleryPanel;

    public Image _scrollImg;
    public Color[] _colors;


    public void Start()
    {
        StartCoroutine(LoadCountTotalGallery());
    }

    public IEnumerator LoadCountTotalGallery()
    {
        for (int i = 0; i < contentGallery.childCount; i++)
        {
            Destroy(contentGallery.GetChild(i).gameObject);
        }

        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x,0);
        ServerCtrl.Instance.GetAllMaps();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string totalCount = ServerCtrl.Instance.server.response.response;
        string[] items = totalCount.Split("/");
        Debug.Log("Total hay " + totalCount);

        for (int i = 0; i < items.Length; i++)
        {
            string[] data = items[i].Split("+");
            string _id = data[0];
            string _code = data[2];
            string _map = Parser.instance.ParseNewMapCustomToJson(data[3]);
            NewMap _newMap = JsonUtility.FromJson<NewMap>(Parser.instance.ParseNewMapCustomToJson(data[3]));
            GameObject newItem = Instantiate(itemGallery, contentGallery);
            ItemGallery itemGllr = newItem.GetComponent<ItemGallery>();

            StartCoroutine(SetTexture(data[2], itemGllr.preview_rimg));
            itemGllr.SetAuthor("Nombre");
            itemGllr.SetCode(_code);
            itemGllr.SetSize(_newMap.size.x + "x" + _newMap.size.y);
            int countEnem = 0;
            if (_newMap.enemyRoute00.Count != 0) countEnem++;
            if (_newMap.enemyRoute01.Count != 0) countEnem++;
            if (_newMap.enemyRoute02.Count != 0) countEnem++;
            if (_newMap.enemyRoute03.Count != 0) countEnem++;
            if (_newMap.enemyRoute04.Count != 0) countEnem++;
            itemGllr.SetEnemies(countEnem.ToString());
            itemGllr.SetPercent("27%");
            itemGllr.SetVisits("27k");
            itemGllr.SetLikes("2727");
            itemGllr.SetTower(_newMap.posTrr_crd.Count != 0);
            itemGllr.SetHorse(_newMap.posCab_crd.Count != 0);
            itemGllr.SetBishop(_newMap.posAlf_crd.Count != 0);
            itemGllr.FinishLoad();
            itemGllr.onScreen = true;
            
            itemGllr.btn.onClick.AddListener(() => LoadMapById(_id, _map));
            itemGllr.btn.onLongPress.AddListener(() => itemGllr.CopyCode());

            itemList.Add(itemGllr);
        }

        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x,
            contentGallery.childCount * itemGallery.GetComponent<RectTransform>().sizeDelta.y + contentGallery.GetComponent<VerticalLayoutGroup>().padding.top + contentGallery.GetComponent<VerticalLayoutGroup>().padding.bottom);
        contentGallery.GetComponent<RectTransform>().anchoredPosition = new Vector2(contentGallery.GetComponent<RectTransform>().anchoredPosition.x, contentGallery.GetComponent<RectTransform>().anchoredPosition.y - 200);
    }

    bool editing;
    public void SetEditing()
    {
        editing = !editing;
        if (editing) _scrollImg.color = _colors[1];
        else _scrollImg.color = _colors[0];
    }

    public IEnumerator SetTexture(string code, RawImage finishTxtr)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://kiwiteam.es/gallery/" + code + "_prev.png");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            finishTxtr.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void LoadMapById(string id, string map)
    {
        PlayerPrefs.SetString("currentMap",map);
        if (!editing)
        {
            SceneManager.LoadScene("PlayScene");
            galleryPanel.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("PlayScene");
            galleryPanel.SetActive(false);
            editing = false;
        }
    }

    public void CheckPosition()
    {
        float sizeItem = contentGallery.GetComponent<RectTransform>().sizeDelta.y / contentGallery.childCount;
        int itemsInScreen = Mathf.FloorToInt(contentGallery.transform.parent.GetComponent<RectTransform>().rect.size.y / sizeItem);
        int step = Mathf.FloorToInt(contentGallery.GetComponent<RectTransform>().anchoredPosition.y / sizeItem);
        if (step < itemList.Count && step >= 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].onScreen = false;
            }


            if (step == 0)
            {
                for (int i = 0; i < itemsInScreen + 1; i++)
                {
                    itemList[i].onScreen = true;
                }
            }
            else
            {
                for (int i = 0; i < itemsInScreen + 1; i++)
                {
                    if(step + i < itemList.Count) itemList[step + i].onScreen = true;
                }
            }
        }
    }
}
