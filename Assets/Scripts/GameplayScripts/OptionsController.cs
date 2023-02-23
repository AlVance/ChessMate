using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuGO; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCloseOptionsMenu()
    {
        optionsMenuGO.SetActive(!optionsMenuGO.activeInHierarchy);
    }

    public void CopyCodeGame()
    {
        GUIUtility.systemCopyBuffer = PlayerPrefs.GetString("currentMapCode");
    }

    public void ShareLevelCode()
    {
        StartCoroutine(ShareLevel());
    }

    private IEnumerator ShareLevel()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://kiwiteam.es/gallery/" + PlayerPrefs.GetString("currentMapCode") + "_prev.png");
        yield return www.SendWebRequest();
        string path = "";

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var screenImage = DownloadHandlerTexture.GetContent(www);
            yield return new WaitForEndOfFrame();
            path = Application.persistentDataPath + "/Screenshots/";
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            path += "currentmap.png";
            yield return new WaitForEndOfFrame();
            byte[] imageBytes = screenImage.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, imageBytes);
            yield return new WaitForEndOfFrame();
            new NativeShare().SetText("Check this #Chessformers level!\n" + PlayerPrefs.GetString("currentMapCode")).AddFile(path).Share();
        }

        
    }
}
