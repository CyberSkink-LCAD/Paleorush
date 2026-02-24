using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    private const string speedParamName = "Speed";
    private const string jumpParamName = "Jump";
    private const string groundedParamName = "Grounded";
    private const string fallingParamName = "Falling";
    private const float lookThreshold = 0.01f;
    [Header("Cinemachine")]
    [SerializeField]
    private Transform cameraTarget;
    [Header("Grounded")]
    [SerializeField]
    private Transform groundedCheckPoint;
    [Header("Jump")]
    [SerializeField]
    private float jumpStrength = 7f;
    [Header("Speed")]
    [SerializeField]
    private float movementSpeed = 3f;
    [SerializeField]
    private float lookSpeed = 10f;
    [SerializeField]
    private float topClamp = 70f;
    [SerializeField]
    private float bottomClamp = -30f;
    [SerializeField]
    private float jumpDowntime = 1f;
    private Vector2 move;
    private Vector2 look;
    private bool isRunning;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    private bool isGrounded = true;
    private Rigidbody body;
    private Animator animator;
    private bool canJump = true;
    private float yaw;
    private float pitch;
    private float currentSpeed;

    private void Move()
    {
        float targetSpeed = (isRunning ? movementSpeed * 2f : movementSpeed) * move.magnitude;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * 8f);
        Vector3 forward = cameraTarget.forward;
        Vector3 right = cameraTarget.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = (forward * move.y + right * move.x).normalized;
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            Vector3 currentVelocity = body.linearVelocity;
            body.linearVelocity = new Vector3(moveDirection.x * currentSpeed, currentVelocity.y, moveDirection.z * currentSpeed);
        }
        else
        {
            Vector3 currentVelocity = body.linearVelocity;
            body.linearVelocity = new Vector3(0, currentVelocity.y, 0);
        }
        float normalizedAnimSpeed = currentSpeed / (movementSpeed * 2f);
        animator.SetFloat(speedParamName, normalizedAnimSpeed);
        animator.SetBool(fallingParamName, !isGrounded && body.linearVelocity.y < -0.01f);

    }
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        GroundedCheck();
    }

    void LateUpdate()
    {
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Jump()
    {
        if (!isGrounded || !canJump)
        {
            return;
        }
        body.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        canJump = false;
        animator.SetTrigger(jumpParamName);
        StartCoroutine(JumpDowntimeCoroutine());
    }
    private IEnumerator JumpDowntimeCoroutine()
    { 
    yield return new WaitForSeconds(0.25f);
        var waitForGrounded = new WaitUntil(() => isGrounded);
        yield return waitForGrounded;
        yield return new WaitForSeconds(jumpDowntime);
        canJump = true;
    }
    private void OnMove(InputValue inputValue)
    {
        move = inputValue.Get<Vector2>();
    }
    private void OnDrawGizmosSelected()
    {
        if(groundedCheckPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundedCheckPoint.position, groundCheckRadius);
    }
    private void OnJump()
    {
        Jump();
    }
    private void Look()
    {
        if (look.sqrMagnitude >= lookThreshold)
        { 
        float deltaTimeMultiplier = Time.deltaTime * lookSpeed;
            yaw += look.x * deltaTimeMultiplier;
            pitch -= look.y * deltaTimeMultiplier;
        }
        yaw = ClampAngle(yaw, float.MinValue, float.MaxValue);
        pitch = ClampAngle(pitch, bottomClamp, topClamp);
        cameraTarget.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    { 
    if(lfAngle < -360f)
        {
            lfAngle += 360;
        }
        if (lfAngle > 360f)
        {
            lfAngle -= 360f;
        }
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void GroundedCheck()
    {
        isGrounded = Physics.CheckSphere(groundedCheckPoint.position, groundCheckRadius, groundLayer);
        animator.SetBool(groundedParamName, isGrounded);
    }
    private void OnRun(InputValue inputValue)
    {
        isRunning = inputValue.isPressed;
    }
    private void OnLook(InputValue inputValue)
    {
        look = inputValue.Get<Vector2>();
    }
}