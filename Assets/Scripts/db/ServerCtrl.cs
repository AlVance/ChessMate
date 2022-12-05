using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerCtrl : MonoBehaviour
{
    public static ServerCtrl Instance { get; private set; }

    public Server server;
    public TMP_InputField inpId;
    public TMP_InputField inpMap;
    public TMP_InputField inpPreview;

    public GameObject loading;

    public void Awake()
    {
        Instance = this;        
    }

    public void CheckCode(string code)
    {
        string[] data = new string[1];
        data[0] = code;
        StartCoroutine(UseService("checkCode",data));
    }

    public void GetCountTotal()
    {
        string[] data = new string[0];
        StartCoroutine(UseService("getTotalCount", data));
    }

    public void GetMapsById()
    {
        string[] data = new string[0];
        StartCoroutine(UseService("getCountId", data));
    }

    public void GetAllMaps()
    {
        string[] data = new string[0];
        StartCoroutine(UseService("getAllMaps", data));
    }

    public void CheckConnection()
    {
        StartCoroutine(UseService("checkConn", new string[0]));
    }

    public void LoadMapById()
    {
        string[] data = new string[1];
        data[0] = inpId.text;
        StartCoroutine(UseService("loadMapId", data));
    }

    public void LoadMapById(string _id)
    {
        string[] data = new string[1];
        data[0] = _id;
        StartCoroutine(UseService("loadMapId", data));
    }

    public void LoadMapByCode(string _code)
    {
        string[] data = new string[1];
        data[0] = _code;
        StartCoroutine(UseService("loadMapCode", data));
    }

    public void LoadCodeId(string _id)
    {
        string[] data = new string[1];
        data[0] = _id;
        StartCoroutine(UseService("loadCodeId", data));
    }

    public void LoadMapUserid(string _userid)
    {
        string[] data = new string[1];
        data[0] = _userid;
        StartCoroutine(UseService("loadMapUserid", data));
    }

    public void SaveMap()
    {
        string[] data = new string[2];
        data[0] = SystemInfo.deviceUniqueIdentifier;
        data[1] = inpMap.text;
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
