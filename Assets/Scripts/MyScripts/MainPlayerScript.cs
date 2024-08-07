using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using Mirror;

public class MainPlayerScript : NetworkBehaviour
{
    // Global Variables
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float maxLookAngle;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Transform firePointStart;
    [SerializeField] private Transform firePointEnd;
    private Rigidbody playerRigidBody;

    // Internal Variables
    private float xRotation = 0f;
    private float playerSpeed;
    private float moveHorizontal;
    private float moveVertical;
    private bool block = false;
    private bool isAiming = false;
    private float maxRaycastDistance = 40f;
    private bool isCrouching = false;
    private float normalColliderHeight = 2f;
    private float crouchedColliderHeight = 1.3f;
    private CapsuleCollider collider;
    private Transform cameraTransform;
    private bool gamePaused = false;
    private int playerPoints = 0;
    private Camera cameraComponent;
    private AudioListener audioListener;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        InitializePlayer();
    }

    void InitializePlayer()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.useGravity = true;
        MainCamera.GetComponent<Camera>().enabled = true;
        MainCamera.GetComponent<AudioListener>().enabled = true;
        cameraTransform = MainCamera.transform;
        Cursor.lockState = CursorLockMode.Locked;
        collider = GetComponent<CapsuleCollider>();
        cameraComponent = MainCamera.GetComponent<Camera>();
        audioListener = MainCamera.GetComponent<AudioListener>();
    }


    public void PausePlayer()
    {
        cameraComponent.enabled = false;
        audioListener.enabled = false;
        playerRigidBody.useGravity = false;
        playerRigidBody.velocity = Vector3.zero;

    }

    public void ResumePlayer()
    {
        cameraComponent.enabled = true;
        audioListener.enabled = true;
        playerRigidBody.useGravity = true;
    }

    void MiniGameCall(string gameName)
    {
        PausePlayer();
        if (!FindObjectOfType<GF_GameController>().MoveToMiniGame(gameName))
        {
            ResumePlayer();
        }
    }

    public void MiniGameReturn(bool win)
    {
        ResumePlayer();
        playerPoints += (win) ? 1 : 0;
        Debug.Log(playerPoints);
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (!isLocalPlayer || GameManager.Instance.isMiniGameActive) return;

        Move();
        Rotate();
        Jump();
        ItemSelect();
        Crouch();
    }

    private void Move()
    {
        if (block) return;

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        playerSpeed = (Input.GetKey(KeyCode.LeftShift) && !isAiming) ? sprintSpeed : speed;
        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

        UpdateAnimatorMovement(moveHorizontal, moveVertical);
        playerRigidBody.velocity = new Vector3(movement.x * playerSpeed, playerRigidBody.velocity.y, movement.z * playerSpeed);
    }

    private void UpdateAnimatorMovement(float horizontal, float vertical)
    {
        PlayerAnimator.SetBool("isWalking", vertical > 0);
        PlayerAnimator.SetBool("isWalkingBack", vertical < 0);
        PlayerAnimator.SetBool("isWalkingRight", horizontal > 0);
        PlayerAnimator.SetBool("isWalkingLeft", horizontal < 0);

        if (Input.GetKey(KeyCode.LeftShift) && !isAiming)
        {
            PlayerAnimator.SetBool("isRunning", vertical > 0);
            PlayerAnimator.SetBool("isRunningBack", vertical < 0);
        }
        else
        {
            PlayerAnimator.SetBool("isRunning", false);
            PlayerAnimator.SetBool("isRunningBack", false);
        }
    }

    private void ItemSelect()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            Vector3 direction = firePointEnd.position - firePointStart.position;
            RaycastHit hit;

            if (Physics.Raycast(firePointStart.position, direction, out hit, maxRaycastDistance))
            {
                string hitObjectName = hit.transform.name;
                if (hitObjectName == "BasementDoor2" || hitObjectName == "BasementDoor1" || hitObjectName == "Door015")
                {
                    MiniGameCall("#");
                }
            }
        }
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && !block && !isCrouching)
        {
            PlayerAnimator.SetBool("isJumping", true);
            PlayerAnimator.SetBool("isWalking", false);
            PlayerAnimator.SetBool("isWalkingBack", false);
            PlayerAnimator.SetBool("isWalkingRight", false);
            PlayerAnimator.SetBool("isWalkingLeft", false);
            PlayerAnimator.SetBool("isRunning", false);
            PlayerAnimator.SetBool("isRunningBack", false);

            block = true;

            Vector3 forwardMovement = transform.forward * moveVertical; 

            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerRigidBody.AddForce(forwardMovement * (jumpForce * 0.5f), ForceMode.Impulse); 

            StartCoroutine(ResetJumpState());
        }
        else if (IsGrounded())
        {
            PlayerAnimator.SetBool("isJumping", false);
        }
    }

    private IEnumerator ResetJumpState()
    {
        yield return new WaitForSeconds(0.5f); 
        block = false;
    }

    private bool IsGrounded()
    {
        Vector3 localTransform = transform.position;
        return Physics.Raycast(localTransform + Vector3.up * 0.1f, Vector3.down, 1.5f);
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isCrouching)
        {
            isCrouching = true;
            collider.height = crouchedColliderHeight;
            collider.center = new Vector3(0, 0.65f, 0);
            PlayerAnimator.SetTrigger("Crouch");
            PlayerAnimator.SetBool("isCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            isCrouching = false;
            collider.height = normalColliderHeight;
            collider.center = new Vector3(0, 1f, 0);
            PlayerAnimator.SetBool("isCrouching", false);
        }
    }
}
