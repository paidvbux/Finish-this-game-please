using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Camera Settings")]
    [SerializeField] Transform playerCamera = null;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] bool lockCursor = true;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    
    float cameraPitch = 0.0f;

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
    Rigidbody rigidBody => GetComponent<Rigidbody>();
    float velocityY = 0.0f;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    #endregion
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        UpdateMovement();
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float currentFOV = playerCamera.GetComponent<Camera>().fieldOfView;
        if (Input.GetKey(KeyCode.LeftShift)) playerCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentFOV, runFOV, lerpSpeed);
        else playerCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentFOV, walkFOV, lerpSpeed);


        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        Vector3 velocity;

        if (isGrounded())
        {
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
            velocityY = 0.0f;
            if (Input.GetButton("Jump"))
            {
                velocityY = initialJumpSpeed;
            }
        }
        else
        {
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, 0.6f);
            velocityY -= 9.8f * Time.deltaTime;
            velocity = (transform.forward * currentDir.y + transform.right * currentDir.x);
            if (Input.GetKey(KeyCode.LeftShift)) velocity = velocity * runSpeed + Vector3.up * velocityY;
            else velocity = velocity * walkSpeed + Vector3.up * velocityY;
        }
        rigidBody.velocity = velocity;
    }

    bool isGrounded()
    {
        return Physics.OverlapCapsule(transform.position - Vector3.up, transform.position - (Vector3.up * 1.01f), 0.25f, groundMask).Length != 0;
    }

}