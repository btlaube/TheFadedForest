using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float sprintSpeed = 4f;
    public float walkJumpForce = 4f;
    public float sprintJumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private bool isGrounded;

    // Input
    [SerializeField] private Vector2 input;
    private float previousInput;
    private float moveSpeed;
    private float moveInput;
    private float jumpForce;
    private bool isJumping;
    private bool isSprinting;
    private bool isCrouching;

    // Disable controls
    private bool isDisabled;

    private List<KeyCode> sprintKeys;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        sprintKeys = new List<KeyCode> { KeyCode.LeftShift, KeyCode.RightShift };
    }

    void Update()
    {
        if (isDisabled) return;
        previousInput = moveInput;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isSprinting = IsAnyKeyHeld(sprintKeys);
        // isJumping = input.y == 1;
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Move speed & direction
        moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        moveInput = input.x;
        jumpForce = isSprinting ? sprintJumpForce : walkJumpForce;

        // Movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jumping
        if (isJumping && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        // Flip spite
        if (previousInput != moveInput)
        {
            if (moveInput == 1)
            {
                sr.flipX = false;
            }
            if (moveInput == -1)
            {
                sr.flipX = true;
            }
        }
        animator.SetFloat("Speed", Mathf.Abs(moveInput * moveSpeed));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSprinting", isSprinting);
    }

    private bool IsAnyKeyHeld(List<KeyCode> keys)
    {
        foreach (var key in keys)
            if (Input.GetKey(key))
                return true;
        return false;
    }

    public void DisableControls()
    {
        isDisabled = true;
    }

    public void EnableControls()
    {
        isDisabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
