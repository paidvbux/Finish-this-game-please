using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToQuadScript : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject cameraPrefab;
    public GameObject uiElement;

    [Header("Dimensions")]
    public int width = 1080;
    public int height = 1080;

    MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    Camera screenshotCamera;

    void Start()
    {
        meshRenderer.material.color = Color.white;
        UpdateTexture("Other/Empty");
    }

    public void UpdateTexture(string filename)
    {
        uiElement.SetActive(true);
        screenshotCamera = Instantiate(cameraPrefab, transform.forward * -2 + transform.position, Quaternion.identity).GetComponent<Camera>();
        screenshotCamera.transform.forward = transform.position - screenshotCamera.transform.position;

        Texture2D tex = TakeScreenshot("UI Images/" + filename);
        meshRenderer.material.mainTexture = tex;

        Destroy(screenshotCamera.gameObject);
        uiElement.SetActive(false);
    }

    public Texture2D TakeScreenshot(string pathToResource)
    {
        Texture2D tex = Resources.Load(pathToResource) as Texture2D;
        if (tex != null) return tex;

        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        screenshotCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
        
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Assets/Resources/" + pathToResource + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);

        tex = Resources.Load(pathToResource) as Texture2D;

        Destroy(rt);
        return tex;
    }
}
