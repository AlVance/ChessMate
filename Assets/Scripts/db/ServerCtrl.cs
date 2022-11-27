using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerCtrl : MonoBehaviour
{
    public Server server;
    public TMP_InputField inpId;
    public TMP_InputField inpMap;
    public TMP_InputField inpPreview;

    public GameObject loading;

    public void Start()
    {
    }

    public void CheckConnection()
    {
        StartCoroutine(UseService("checkConn", new string[0]));
    }

    public void LoadMap()
    {
        string[] data = new string[1];
        data[0] = inpId.text;
        StartCoroutine(UseService("loadMap", data));
    }

    public void LoadMap(string _id)
    {
        string[] data = new string[1];
        data[0] = _id;
        StartCoroutine(UseService("loadMap", data));
    }

    public void SaveMap()
    {
        string[] data = new string[2];
        data[0] = inpMap.text;
        data[1] = inpPreview.text;
        StartCoroutine(UseService("saveMap", data));
    }

    public void SaveMap(string[] _data)
    {
        StartCoroutine(UseService("saveMap", _data));
    }

    public void EditMap()
    {
        string[] data = new string[3];
        data[0] = inpId.text;
        data[1] = inpMap.text;
        data[2] = inpPreview.text;
        StartCoroutine(UseService("editMap", data));
    }

    public bool serviceFinish = false;
    IEnumerator UseService(string _service, string[] _data)
    {
        serviceFinish = false;
        loading.SetActive(true);
        StartCoroutine(server.UseService(_service, _data));
        yield return new WaitForSeconds(.5f);
        yield return new WaitUntil(() => !server.ocupied);
        loading.SetActive(false);
        serviceFinish = true; 
    }
}
