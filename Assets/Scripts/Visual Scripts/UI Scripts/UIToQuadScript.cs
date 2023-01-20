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
   /*
    *   Sets up for the screenshot and 
    *   cleanup for the screenshot.
    */
    public void UpdateTexture(string filename)
    {
        //  Turns the UI element it is capturing on.
        uiElement.SetActive(true);

        #region Create Camera
        //  Creates a camera that is positioned to look at the UI element.
        screenshotCamera = Instantiate(cameraPrefab, transform.forward * -2 + transform.position, Quaternion.identity).GetComponent<Camera>();
        screenshotCamera.transform.forward = transform.position - screenshotCamera.transform.position;
        #endregion

        #region Sets Texture
        //  Sets the texture of the material equal to the image found in the resource folder.
        Texture2D tex = TakeScreenshot("UI Images/" + filename);
        meshRenderer.material.mainTexture = tex;
        #endregion

        #region Cleanup
        //  Destroy the screenshot camera and turn the UI element off.
        Destroy(screenshotCamera.gameObject);
        uiElement.SetActive(false);
        #endregion
    }

    /*
     *   Takes a screenshot, saves it if
     *   the name is not found in the folder.
     *   Otherwise it will just return the
     *   existing image.
     */
    public Texture2D TakeScreenshot(string pathToResource)
    {
        #region Returns Image
        //  Check if there is already an image. Returns early if found.
        Texture2D tex = Resources.Load(pathToResource) as Texture2D;
        if (tex != null) return tex;
        #endregion

        #region Take Screenshot
        //  Create a render texture to store the texture of the camera.
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

        //  Sets the render texture of the camera to the one that was created.
        screenshotCamera.targetTexture = rt;
        
        //  Render one frame of it to take a screenshot of.
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        #endregion

        #region Save Image
        //  Adds it to the resources folder.
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Assets/Resources/" + pathToResource + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);
        #endregion 

        #region Cleanup
        //  Removes the render texture from the camera.
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;

        //  Cleanup
        Destroy(rt);
        #endregion

        #region Return Texture
        //  Finds the file and returns it.
        tex = Resources.Load(pathToResource) as Texture2D;
        return tex;
        #endregion
    }
    #endregion
}
