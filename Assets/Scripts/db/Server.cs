using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[CreateAssetMenu(fileName = "ServerConfig", menuName = "Chessformer/Database" , order = 1)]
public class Server : ScriptableObject
{
    public string server;
    public Service[] services;

    public Response response;
    public Parser parser;

    public IEnumerator UseService(string _name, string[] _data)
    {
        ServerCtrl.Instance.serviceFinish = false;
        WWWForm form = new WWWForm();
        Service s = new Service();
        for (int i = 0; i < services.Length; i++)
        {
            if (services[i].name.Equals(_name))
            {
                s = services[i];
            }
        }
        Debug.Log(server + "/" + s.URL);
        for (int i = 0; i < s.parameters.Length; i++)
        {
            Debug.Log("Params " + s.parameters[i]);
            Debug.Log("Data " + _data[i]);
            form.AddField(s.parameters[i], _data[i]);
        }

        UnityWebRequest www = UnityWebRequest.Post(server + "/" + s.URL, form);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success)
        {
            response = new Response();
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            
            response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
        }
        ServerCtrl.Instance.serviceFinish = true;
    }
}

[System.Serializable]
public class Service
{
    public string name;
    public string URL;
    public string[] parameters;
}

[System.Serializable]
public class Response
{
    public int code;
    public string message;
    public string response;

    public Response()
    {
        code = 404;
        message = "Error";
    }
}