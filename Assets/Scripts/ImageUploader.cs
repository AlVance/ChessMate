using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageUploader : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteImage;
    [SerializeField] string url;

    void Start()
    {

    }

    IEnumerator StartUploading(byte[] newTexture)
    {
		WWWForm form = new WWWForm();
		byte[] textureBytes = null;

		Texture2D imageTexture = GetTextureCopy(spriteImage.sprite.texture);
		textureBytes = imageTexture.EncodeToPNG();

		form.AddBinaryData("myimage", textureBytes, "imageFromUnity.png", "image/png");

		WWW w = new WWW(url, form);

		yield return w;
		if(w.error != null)
        {
			Debug.Log("error : " + w.error);
        }
        else
        {
			Debug.Log(w.text);
        }
		w.Dispose();
    }

	Texture2D GetTextureCopy(Texture2D source)
	{
		//Create a RenderTexture
		RenderTexture rt = RenderTexture.GetTemporary(
							   source.width,
							   source.height,
							   0,
							   RenderTextureFormat.Default,
							   RenderTextureReadWrite.Linear
						   );

		//Copy source texture to the new render (RenderTexture) 
		Graphics.Blit(source, rt);

		//Store the active RenderTexture & activate new created one (rt)
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = rt;

		//Create new Texture2D and fill its pixels from rt and apply changes.
		Texture2D readableTexture = new Texture2D(source.width, source.height);
		readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		readableTexture.Apply();

		//activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(rt);

		return readableTexture;
	}
}
