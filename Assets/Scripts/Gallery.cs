using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gallery : MonoBehaviour
{
    public GameObject itemGallery;
    public Transform contentGallery;

    public GridGenerator _gridGen;

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
            GameObject newItem = Instantiate(itemGallery, contentGallery);
            newItem.transform.Find("Code").GetComponent<TextMeshProUGUI>().text = data[2];

            ScreenshotHandler.instance.GetTextureStart(data[2]);
            yield return new WaitWhile(() => ScreenshotHandler.instance.finishLoadImage == false);
            if(ScreenshotHandler.instance.finishTxtr != null)
            {
                newItem.transform.Find("Preview").GetComponent<RawImage>().texture = ScreenshotHandler.instance.finishTxtr;
            }

            string _ni = data[0];
            newItem.transform.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(() => LoadMapById(_ni));
        }
        contentGallery.GetComponent<RectTransform>().sizeDelta = new Vector2(contentGallery.GetComponent<RectTransform>().sizeDelta.x, (contentGallery.childCount / 3) * 220);
    }

    public void LoadMapById(string id)
    {
        _gridGen.LoadNewMap(id);
        gameObject.SetActive(false);
    }
}
