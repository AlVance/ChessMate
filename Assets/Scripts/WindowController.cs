using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    public string path = "";


    public GameObject editor_cnv;
    public GameObject loadMap_cnv;
    public GameObject inGame_cnv;
    public GameObject gallery_cnv;
    public GameObject loading_cnv;
    public GameObject startMenu_cnv;
    public GameObject onTest_cnv;
    public GameObject noMapsNoInt_cnv;


    private void Start()
    {
        path = Application.persistentDataPath + "/Maps";
        Debug.Log("Ruta Mapas " + path);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            if (Directory.GetFiles(path).Length == 0)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Debug.Log("No tienes mapas. Conectate a Internet para la descarga inicial");
                }
                else
                {
                    Debug.Log("No tienes mapas. ¿Descargar paquete inicial?");
                }
            }
        }

        OpenStartMenuCanvas();
    }

    public IEnumerator CicleWindows()
    {
        float time = 1f;
        CloseAll();
        yield return new WaitForSeconds(time);
        OpenEditorCanvas();
        yield return new WaitForSeconds(time);
        OpenInGameCanvas();
        yield return new WaitForSeconds(time);
        OpenGalleryCanvas();
        yield return new WaitForSeconds(time);
        OpenLoadingCanvas();
        yield return new WaitForSeconds(time);
        OpenStartMenuCanvas();
        yield return new WaitForSeconds(time);
        OpenOnTestCanvas();
        yield return new WaitForSeconds(time);
        OpenNoMapsNoInternetCanvas();
        yield return new WaitForSeconds(time);
    }

    public void CheckConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No tienes mapas. Conectate a Internet para la descarga inicial");
        }
        else
        {
            Debug.Log("No tienes mapas. ¿Descargar paquete inicial?");
        }
    }

    public void CloseAll()
    {
        editor_cnv.SetActive(false);
        loadMap_cnv.SetActive(false);
        inGame_cnv.SetActive(false);
        gallery_cnv.SetActive(false);
        loading_cnv.SetActive(false);
        startMenu_cnv.SetActive(false);
        onTest_cnv.SetActive(false);
        noMapsNoInt_cnv.SetActive(false);
    }
    public void OpenEditorCanvas()
    {
        CloseAll();
        editor_cnv.SetActive(true);
    }
    public void OpenLoadMapCanvas()
    {
        CloseAll();
        loadMap_cnv.SetActive(true);
    }
    public void OpenInGameCanvas()
    {
        CloseAll();
        inGame_cnv.SetActive(true);
    }
    public void OpenGalleryCanvas()
    {
        CloseAll();
        gallery_cnv.SetActive(true);
    }
    public void OpenLoadingCanvas()
    {
        CloseAll();
        loading_cnv.SetActive(true);
    }
    public void OpenStartMenuCanvas()
    {
        CloseAll();
        startMenu_cnv.SetActive(true);
    }
    public void OpenOnTestCanvas()
    {
        CloseAll();
        onTest_cnv.SetActive(true);
    }
    public void OpenNoMapsNoInternetCanvas()
    {
        CloseAll();
        noMapsNoInt_cnv.SetActive(true);
    }
}