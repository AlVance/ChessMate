using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public static ScreenshotHandler instance;

    public Camera myCamera;
    public bool takeScreenshotOnNextFrame;

    int _width = 128;
    int _height = 128;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    public byte[] lastScreenshot = new byte[0];
    public string lastScreenStr;

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            lastScreenStr = renderResult.ToString();
            lastScreenshot = renderResult.EncodeToPNG();
            string result = String.Join(" ", lastScreenshot);
            Debug.Log("photoSTR " + result);
            System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", lastScreenshot);
            Debug.Log("Saved Screenshot " + Application.dataPath);

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
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

    public void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static byte[] GetScreen()
    {
        return ScreenshotHandler.instance.GetScreenshot();
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
