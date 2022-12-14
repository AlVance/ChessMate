using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class Gallery : MonoBehaviour
{
    public GameObject itemGallery;
    public Transform contentGallery;

    public GridGenerator _gridGen;
    public CustomEditor _customEditor;

    public void GetCountTotal()
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
            GameObject newItem = Instantiate(itemGallery, contentGallery);
            newItem.transform.Find("Code").GetComponent<TextMeshProUGUI>().text = _code;

            StartCoroutine(SetTexture(data[2], newItem.transform.Find("Preview").GetComponent<RawImage>()));

            /*
            ScreenshotHandler.instance.GetTextureStart(data[2]);
            yield return new WaitWhile(() => ScreenshotHandler.instance.finishLoadImage == false);
            if(ScreenshotHandler.instance.finishTxtr != null)
            {
                newItem.transform.Find("Preview").GetComponent<RawImage>().texture = ScreenshotHandler.instance.finishTxtr;
            }
            */
            newItem.transform.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(() => LoadMapById(_id, _code));
        }
        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x, ((contentGallery.childCount / 3) +1 )* 220);
    }

    bool editing;
    public void SetEditing(bool _onEdit)
    {
        editing = _onEdit;
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

    public void LoadMapById(string id, string code)
    {
        if (!editing)
        {
            _gridGen.LoadNewMap(id);
            gameObject.SetActive(false);
        }
        else
        {
            _customEditor.StartEdit(code);
            gameObject.SetActive(false);
            editing = false;
        }
    }
}
