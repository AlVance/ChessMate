using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Prueba : MonoBehaviour
{
    public RawImage rawImage;
    public string imageFileName;

    private void Start()
    {
        StartCoroutine(LoadImage(rawImage));
    }

    private IEnumerator LoadImage(RawImage _rImg)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/Previews/", imageFileName);

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error al cargar la imagen: {www.error}");
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _rImg.texture = texture;
                _rImg.SetNativeSize();
            }
        }
        else
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            _rImg.texture = texture;
            _rImg.SetNativeSize();
        }
    }
}
