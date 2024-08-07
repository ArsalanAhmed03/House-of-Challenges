using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NonNetworkPlayer : MonoBehaviour
{
    // Global Variables
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float maxLookAngle;
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Transform firePointStart;
    [SerializeField] private Transform firePointEnd;

    // Internal Variables
    private float xRotation = 0f;
    private float playerSpeed;
    private float moveHorizontal;
    private float moveVertical;
    private bool block = false;
    private bool isAiming = false;
    private Transform cameraTransform;

    void Start()
    {
        InitializePlayer();
    }

    void InitializePlayer()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.useGravity = true;
        cameraTransform = MainCamera.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        Move();
        Rotate();
        Jump();
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
        if (Input.GetButtonDown("Jump") && IsGrounded() && !block)
        {
            PlayerAnimator.SetBool("isJumping", true);
            ResetMovementAnimationStates();

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

    private void ResetMovementAnimationStates()
    {
        PlayerAnimator.SetBool("isWalking", false);
        PlayerAnimator.SetBool("isWalkingBack", false);
        PlayerAnimator.SetBool("isWalkingRight", false);
        PlayerAnimator.SetBool("isWalkingLeft", false);
        PlayerAnimator.SetBool("isRunning", false);
        PlayerAnimator.SetBool("isRunningBack", false);
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

    
}
