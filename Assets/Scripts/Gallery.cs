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
        ServerCtrl.Instance.GetCountTotal();
        yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
        string totalCount = ServerCtrl.Instance.server.response.response;
        Debug.Log("Total hay " + totalCount);

        string[] _newIds = totalCount.Split("+");
        for (int i = 0; i < _newIds.Length; i++)
        {
            GameObject newItem = Instantiate(itemGallery, contentGallery);

            ServerCtrl.Instance.LoadCodeId(_newIds[i]);
            yield return new WaitWhile(() => ServerCtrl.Instance.serviceFinish == false);
            string newCode = ServerCtrl.Instance.server.response.response;
            newItem.transform.Find("Code").GetComponent<TextMeshProUGUI>().text = newCode;

            ScreenshotHandler.instance.GetTextureStart(newCode);
            yield return new WaitWhile(() => ScreenshotHandler.instance.finishLoadImage == false);
            if(ScreenshotHandler.instance.finishTxtr != null)
            {
                newItem.transform.Find("Preview").GetComponent<RawImage>().texture = ScreenshotHandler.instance.finishTxtr;
            }

            string _ni = _newIds[i];
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
