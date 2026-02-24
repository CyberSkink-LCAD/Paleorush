using System.Collections;
using Unity.VisualScripting;
using UnityEngine;




public class ThirdPersonController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float speedMult = 2f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float gravity = 9.81f;
    //9.81f for realistic gravity?
    [SerializeField] Vector3 velocity;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundDistance = 0.4f;
    [SerializeField] public float jumpHeight = 3f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public bool isGrounded;
    [SerializeField] private bool triggerDizzy;
    [SerializeField]
    private float fallTime;

{
    if (Input.GetKey("left shift"))
    {
        speed = startSpeed * speedMult;
        animator.SetBool("isSprinting", true);
    }
    else
    {
        speed = startSpeed;
        animator.SetBool("isSprinting", false);
    }
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    if (isGrounded)
    {
        animator.SetBool("isGrounded", true);
    }
    else
    {
        animator.SetBool("isGrounded", false);
    }

    if (isGrounded && velocity.y < 0)
    {
        velocity.y = -2f;
        fallTime = 0f;
    }
    if (!isGrounded && fallTime > 2f && velocity.y < -0.1f)
    {
        animator.SetBool("fellFromHeight", true);

    }


    if (Input.GetButtonDown("Jump") && isGrounded && !triggerDizzy)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        animator.SetBool("isJumping", true);
    }
    else
    {
        animator.SetBool("isJumping", false);
    }
    fallTime = fallTime += Time.deltaTime;
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
    if (animator.GetCurrentAnimatorStateInfo(0).IsName("FACEPLANT") || animator.GetCurrentAnimatorStateInfo(0).IsName("DIZZY"))
    {
        triggerDizzy = true;
        animator.SetBool("fellFromHeight", false);
    }
    else
    {
        triggerDizzy = false;
    }

    if (direction.magnitude >= 0.1f && !triggerDizzy)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }
    if (isGrounded && direction.magnitude >= 0.1f)
    {
        animator.SetBool("isWalking", true);
    }
    else
    {
        animator.SetBool("isWalking", false);
    }
}


