using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToQuadScript : MonoBehaviour
{
    public Camera screenshotCamera;
    public int width = 1080;
    public int height = 1080;

    MeshRenderer meshRenderer => GetComponent<MeshRenderer>();

    void Start()
    {
        UpdateTexture();    
    }

    public void UpdateTexture()
    {
        Texture2D tex = TakeScreenshot("UI Images\\");
        meshRenderer.material.mainTexture = tex;
    }

    public Texture2D TakeScreenshot(string pathToResource)
    {
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        screenshotCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Assets\\Resources\\" + pathToResource + "test.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        Texture2D tex = Resources.Load(pathToResource + "test.png") as Texture2D;

        print(Resources.Load(pathToResource + "test.png"));

        Destroy(rt);
        return tex;
    }
}
