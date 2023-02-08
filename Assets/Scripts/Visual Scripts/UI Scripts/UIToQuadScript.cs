using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToQuadScript : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public GameObject cameraPrefab;
    public GameObject uiElement;
    #endregion

    #region Dimension Variables/Settings
    [Header("Dimensions")]
    public int width = 1080;
    public int height = 1080;
    #endregion 

    #region Hidden/Private Variables
    MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    Camera screenshotCamera;
    #endregion 

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        #region Initialization
        //  Set the material's color to be white.
        meshRenderer.material.color = Color.white;
        
        //  Set the texture to be empty.
        UpdateTexture("Other/Empty");
        #endregion
    }
    #endregion

    #region Custom Functions
    public void UpdateTexture(string filename)
    {
        uiElement.SetActive(true);

        #region Create Camera
        screenshotCamera = Instantiate(cameraPrefab, transform.forward * -2 + transform.position, Quaternion.identity).GetComponent<Camera>();
        screenshotCamera.transform.forward = transform.position - screenshotCamera.transform.position;
        #endregion

        #region Sets Texture
        Texture2D tex = TakeScreenshot("UI Images/" + filename);
        meshRenderer.material.mainTexture = tex;
        #endregion

        #region Cleanup
        Destroy(screenshotCamera.gameObject);
        uiElement.SetActive(false);
        #endregion
    }

    public Texture2D TakeScreenshot(string pathToResource)
    {
        #region Returns Image
        Texture2D tex = Resources.Load(pathToResource) as Texture2D;
        if (tex != null) return tex;
        #endregion

        #region Take Screenshot
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

        screenshotCamera.targetTexture = rt;
        
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        #endregion

        #region Save Image
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Assets/Resources/" + pathToResource + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
        #endregion 

        #region Cleanup
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        Destroy(rt);
        #endregion

        #region Return Texture
        tex = Resources.Load(pathToResource) as Texture2D;
        return tex;
        #endregion
    }
    #endregion
}
