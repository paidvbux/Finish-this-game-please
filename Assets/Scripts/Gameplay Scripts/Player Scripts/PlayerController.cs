using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Static Variables
    public static bool rotatingObject;
    #endregion

    #region Camera Variables/Settings
    [Header("Camera Settings")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] bool lockCursor = true;
    #endregion

    #region Movement Variables/Settings
    [Header("Movement Settings")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float initialJumpSpeed = 9.0f;
    [SerializeField] float walkSpeed = 10.0f;
    [SerializeField] float runSpeed = 15.0f;
    [SerializeField][Range(0.0f, 1f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 1f)] float mouseSmoothTime = 0.03f;
    [SerializeField][Range(30.0f, 150f)] float walkFOV = 60f;
    [SerializeField][Range(60.0f, 180f)] float runFOV = 90f;
    [SerializeField][Range(0.0f, 1f)] float lerpSpeed = 0.1f;
    #endregion

    #region Hidden/Private Variables
    Rigidbody rigidBody => GetComponent<Rigidbody>();

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
    float cameraPitch = 0.0f;

    /*-----------------------------------------------------------------*/

    float velocityY = 0.0f;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        #region Initialization
        rotatingObject = false;

        //  Checks whether or not to lock the cursor or not.
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion
    }

    void LateUpdate()
    {
        #region Update Movement & Camera
        //  Updates movement and camera direction.
        UpdateMovement();
        if (!rotatingObject && !GameManager.dialogueActive)
        {
            UpdateMouseLook();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
   /*
    *   Updates the mouse rotation so that the player can look around.
    */
    void UpdateMouseLook()
    {
        //  Get the mouse position input.
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        #region Change FOV
        //  Change the FOV depending if the player is sprinting or not.
        float currentFOV = playerCamera.GetComponent<Camera>().fieldOfView;
        if (Input.GetKey(KeyCode.LeftShift)) playerCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentFOV, runFOV, lerpSpeed);
        else playerCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentFOV, walkFOV, lerpSpeed);
        #endregion

        #region Rotate
        //  Smoothly moves the mouse position from its position to its new position.
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        //  Changes the camera pitch by how much the mouse moves up and down. Clamps it to straight up and straight down.
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        //  Updates the calculated camera rotation.
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        #endregion
    }

   /*
    *   Updates the velocity and position 
    *   of the player.
    */
    void UpdateMovement()
    {
        #region Get Movement Input
        //  Gets inputs of the player, normalizes it and stores it as a 2D vector.
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        Vector3 velocity;
        #endregion

        //  Checks if the player is touching the ground.
        if (isGrounded())
        {
            #region Calculate Velocity
            //  Gets the current direction and calculates the current velocity.
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            #endregion

            #region Update Velocity
            //  Moves the velocity by walkSpeed if the player is walking. Moves by runSpeed otherwise.
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
            
            //  Resets the Y velocity to prevent buildup in the negative Y axis.
            velocityY = 0.0f;
            #endregion

            #region Jump
            //  Checks if the player wants to jump and jumps if they do.
            if (Input.GetButton("Jump"))
                velocityY = initialJumpSpeed;
            #endregion
        }
        else
        {
            #region Calculate Velocity
            //  Gets the current direction and calculates the current velocity.
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, 0.6f);
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            
            //  Adds downwards velocity to account for gravity.
            velocityY -= 9.8f * Time.deltaTime;
            #endregion

            #region Update Velocity
            //  Moves the velocity by walkSpeed if the player is walking. Moves by runSpeed otherwise.
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
            #endregion
        }

        //  Sets the velocity.
        rigidBody.velocity = velocity;
    }

   /*
    * Returns whether or not the player is grounded.
    */
    bool isGrounded()
    {
        return Physics.OverlapCapsule(transform.position - Vector3.up, transform.position - (Vector3.up * 1.01f), 0.25f, groundMask).Length != 0;
    }
    #endregion
}