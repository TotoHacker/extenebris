using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private PlayerAttack attack;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isParrying;
    private float moveInput;

    private bool isGrounded = true;

    private Vector3 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attack = GetComponent<PlayerAttack>();

        originalScale = transform.localScale;
    }

    void Update()
    {
        moveInput = 0f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput = 1f;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput = -1f;

        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        if (!isParrying)
            animator.SetBool("walkForward", moveInput != 0 && isGrounded);

        // SALTO
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            isGrounded = false;

            animator.SetBool("isJump", moveInput < 0);
            animator.SetBool("isJumpRigth", moveInput >= 0);

            // ← CORRECTO EN UNITY 6
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // ATAQUE
        if (Keyboard.current.rKey.wasPressedThisFrame && !isParrying)
        {
            isParrying = true;
            animator.SetBool("walkForward", false);
            animator.SetBool("isParry", true);

            attack.Attack();
        }

        if (Keyboard.current.rKey.wasReleasedThisFrame && isParrying)
        {
            isParrying = false;
            animator.SetBool("isParry", false);
        }
    }

    void FixedUpdate()
    {
        // ← CORRECTO EN UNITY 6
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.linearVelocity.y <= 0.1f) // ← CORRECTO
        {
            isGrounded = true;
            animator.SetBool("isJump", false);
            animator.SetBool("isJumpRigth", false);
        }
    }
}
