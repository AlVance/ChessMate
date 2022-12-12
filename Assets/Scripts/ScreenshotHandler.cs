using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    public static ScreenshotHandler instance { get; private set; }
    public string url;

    public Camera myCamera;
    public RawImage _rwImg;
    public bool takeScreenshotOnNextFrame;

    int _width = 256;
    int _height = 256;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    public byte[] lastScreenshot = new byte[0];

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            lastScreenshot = renderResult.EncodeToPNG();
            string result = String.Join(" ", lastScreenshot);
            Debug.Log("photoSTR " + result);
            //StartCoroutine(StartUploading(lastScreenshot));
        }
    }

    public string newCode;
    public IEnumerator StartUploading(string code = "")
    {
        if (code != "") newCode = code;
        WWWForm form = new WWWForm();
        byte[] textureBytes = lastScreenshot;

        form.AddBinaryData("myimage", textureBytes, newCode + "_prev.png", "image/png");

        WWW w = new WWW(url, form);

        yield return w;
        if (w.error != null)
        {
            Debug.Log("error : " + w.error);
        }
        else
        {
            Debug.Log(w.text);
        }
        w.Dispose();


        RenderTexture.ReleaseTemporary(myCamera.targetTexture);
        myCamera.targetTexture = null;
    }

    public Texture finishTxtr = null;
    public bool finishLoadImage = false;

    public void GetTextureStart(string code)
    {
        StartCoroutine(GetTexture(code));
    }

    public IEnumerator GetTexture(string code)
    {
        finishTxtr = null;
        finishLoadImage = false;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://kiwiteam.es/gallery/" + code + "_prev.png");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            finishLoadImage = true;
        }
        else
        {
            finishTxtr = ((DownloadHandlerTexture)www.downloadHandler).texture;
            finishLoadImage = true;
        }
    }

    public byte[] GetScreenshot()
    {
        if(myCamera.targetTexture == null)
        {
            lastScreenshot = new byte[0];
            TakeScreenshot();
            return null;
        }
        else
        {
            if(lastScreenshot != null)  return lastScreenshot;
            else
            {
                lastScreenshot = new byte[0];
                TakeScreenshot();
                return null;
            }
        }
    }


    public void TakeScreenshot()
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(_width, _height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public void TakeScreenshot(int width, int height, string _code = "")
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
        newCode = _code;
    }
}
