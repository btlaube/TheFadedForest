using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class ControllerSettings
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float groundDeceleration = 10f;
    public float walkInterval = 0.5f;
    public float walkMinPitch = 0.8f;
    public float walkMaxPitch = 0.9f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float airDeceleration = 5f;
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float groundDeceleration = 10f;
    private bool isWalking;
    public float walkInterval = 0.5f;
    public float walkMinPitch = 0.8f;
    public float walkMaxPitch = 0.9f;
    // private bool isWalking;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float airDeceleration = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    [Header("Fly")]
    public float flySpeed = 7f;
    public float flyDeceleration = 10f;
    public float flyTiltSpeed = 3f;
    public float glideSpeed = 2f;
    private bool isFlying;

    [Header("Shoot")]
    public Transform fireOrigin;
    public LayerMask enemyLayer;
    public float fireRadius = 0.2f;

    [Header("Visual")]
    public GameObject slowPlayerVisual;
    public GameObject shotgunPlayerVisual;
    private bool hasShotgun;

    [Header("Controller Settings")]
    public ControllerSettings shotgunControllerSettings;
    public ControllerSettings slowControllerSettings;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private AudioManager audioManager;

    private float previousSpeed;

    private void Awake()
    {
        SetSlowVisual();

        rb = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioManager>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    // private void OnEnable()
    // {
    //     playerInput.Player.Jump.performed += Jump;
    //     playerInput.Player.Jump.performed += Fly;
    //     playerInput.Player.Shoot.performed += Shoot;
    //     playerInput.Rhythm.Left.performed += ShootLeft; 
    //     playerInput.Rhythm.Right.performed += ShootRight;
    //     // playerInput.Player.Punch.performed += Punch;
    // }

    // private void OnDisable()
    // {
    //     playerInput.Player.Jump.performed -= Jump;
    //     playerInput.Player.Jump.performed -= Fly;
    //     playerInput.Player.Shoot.performed -= Shoot;
    //     playerInput.Rhythm.Left.performed -= ShootLeft; 
    //     playerInput.Rhythm.Right.performed -= ShootRight; 
    //     // playerInput.Player.Punch.performed -= Punch;
    // }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        // Debug.Log(isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        // -- Handle input and movement --
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        // rb.velocity = new Vector2(inputVector.x * moveSpeed, rb.velocity.y);

        // float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.velocity;

        // Apply deceleration based on whether the player is on the ground or in the air
        float deceleration = isGrounded ? groundDeceleration : airDeceleration;

        if (isGrounded)
        {
            isFlying = false;
            rb.gravityScale = 1f;
        }

        if (isFlying)
        {
            rb.gravityScale = 0f;

            // Calculate the target horizontal velocity based on the input
            float targetVelocityX = inputVector.x * flySpeed;

            // Flying mode behavior
            // float verticalInput = Input.GetAxis("Vertical");
            float targetVelocityY = inputVector.y * flySpeed;
            float tiltAngle = targetVelocityY * flyTiltSpeed;

            // Apply deceleration to the horizontal velocity
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, flyDeceleration * Time.deltaTime);

            // Tilt the player
            rb.rotation = tiltAngle;

            // Move up or glide when the jump button is pressed or released
            if (inputVector.y != 0)
            {
                Debug.Log(inputVector.y);
                // velocity.y = Mathf.MoveTowards(velocity.y, targetVelocityY, deceleration * Time.deltaTime);
                velocity.y = Mathf.MoveTowards(velocity.y, targetVelocityY, flyDeceleration * Time.deltaTime);
            }
            else
            {
                velocity.y = Mathf.MoveTowards(velocity.y, -glideSpeed, deceleration * Time.deltaTime);
            }
        }
        else
        {

            // Calculate the target horizontal velocity based on the input
            float targetVelocityX = inputVector.x * moveSpeed;

            // Apply deceleration to the horizontal velocity
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, deceleration * Time.deltaTime);
        }

        rb.velocity = velocity;

        // Handle animation
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Flip Sprite
        // Check if the direction has changed
        if (rb.velocity.x != 0f && rb.velocity.x != previousSpeed)
        {
            // Flip the sprite horizontally based on the direction
            FlipSprite(rb.velocity.x);
        }

        // Update the previous direction for the next frame
        previousSpeed = rb.velocity.x;

        //Handle Audio
        if (velocity.x != 0 && isGrounded && !isWalking)
        {
            StartWalkAudio();
            isWalking = true;
        }
        if (velocity.x == 0 || !isGrounded)
        {
            StopWalkAudio();
            isWalking = false;
        }

    }

    private void FlipSprite(float direction)
    {
        if (direction > 0f)
        {
            sr.flipX = false; // Not flipped (facing right)
        }
        else if (direction < 0f)
        {
            sr.flipX = true; // Flipped (facing left)
        }
    }

    public void StartWalkAudio()
    {
        isWalking = true;
        StopAllCoroutines();
        StartCoroutine("Walk");
    }

    public void StopWalkAudio()
    {
        isWalking = false;
    }

    public IEnumerator Walk()
    {
        while (isWalking)
        {
            audioManager.PlaySoundRandomPitch("Walk", walkMinPitch, walkMaxPitch);

            yield return new WaitForSeconds(walkInterval);
        }
    }

    private void Jump(InputAction.CallbackContext context) {
        if (isGrounded) {
            // animator.SetTrigger("Jump");
            isGrounded = false;
            animator.SetBool("IsGrounded", isGrounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // groundCheck.Disable(0.2f);
        }
    }

    private void Fly(InputAction.CallbackContext context)
    {
        if (!isGrounded) 
        {
            isFlying = true;
        }
    }

    private void Shoot()
    {
        if(hasShotgun)
        {
            animator.SetTrigger("Shoot");
            audioManager.Play("Shoot");
            Collider2D[] collisions = Physics2D.OverlapCircleAll(fireOrigin.position, fireRadius, enemyLayer);
            foreach (Collider2D collider in collisions)
            {
                collider.gameObject.GetComponent<FairyController>().Die();
            } 
        }   
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if(hasShotgun)
        {
            animator.SetTrigger("Shoot");
            audioManager.Play("Shoot");
            Collider2D[] collisions = Physics2D.OverlapCircleAll(fireOrigin.position, fireRadius, enemyLayer);
            foreach (Collider2D collider in collisions)
            {
                collider.gameObject.GetComponent<FairyController>().Die();
            } 
        }        
    }

    private void ChangeVisual()
    {
        slowPlayerVisual.SetActive(slowPlayerVisual.activeSelf);
        shotgunPlayerVisual.SetActive(shotgunPlayerVisual.activeSelf);
    }

    private void SetShotgunVisual()
    {
        slowPlayerVisual.SetActive(false);
        shotgunPlayerVisual.SetActive(true);
        hasShotgun = true;

        // audioManager.Play("GetShotgun");

        SetShotgunSettings();

        UpdateVisual();
    }

    private void SetSlowVisual()
    {
        slowPlayerVisual.SetActive(true);
        shotgunPlayerVisual.SetActive(false);
        hasShotgun = false;

        SetSlowSettings();

        UpdateVisual();
    }

    private void SetNoVisual()
    {
        slowPlayerVisual.SetActive(false);
        shotgunPlayerVisual.SetActive(false);

        UpdateVisual();
    }

    private void SetShotgunSettings()
    {    
        moveSpeed = shotgunControllerSettings.moveSpeed;
        groundDeceleration = shotgunControllerSettings.groundDeceleration;
        walkInterval = shotgunControllerSettings.walkInterval;
        walkMinPitch = shotgunControllerSettings.walkMinPitch;
        walkMaxPitch = shotgunControllerSettings.walkMaxPitch;

        jumpForce = shotgunControllerSettings.jumpForce;
        airDeceleration = shotgunControllerSettings.airDeceleration;
    }

    private void SetSlowSettings()
    {    
        moveSpeed = slowControllerSettings.moveSpeed;
        groundDeceleration = slowControllerSettings.groundDeceleration;
        walkInterval = slowControllerSettings.walkInterval;
        walkMinPitch = slowControllerSettings.walkMinPitch;
        walkMaxPitch = slowControllerSettings.walkMaxPitch;

        jumpForce = slowControllerSettings.jumpForce;
        airDeceleration = slowControllerSettings.airDeceleration;
    }

    public void TempSetShotgun()
    {
        SetShotgunVisual();
    }

    private void UpdateVisual()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    public void EnterCutscene()
    {
        rb.velocity = Vector2.zero;
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        StopWalkAudio();
        isWalking = false;
        SetNoVisual();
        this.enabled = false;
    }

    public void ExitCutscene()
    {
        SetShotgunVisual();
        this.enabled = true;
        SwitchToRhythmControls();
    }

    public void ShootLeft(InputAction.CallbackContext context)
    {
        FlipSprite(-1f);
        Shoot();
    }

    public void ShootRight(InputAction.CallbackContext context)
    {
        FlipSprite(1f);
        Shoot();
    }

    public void SwitchToRhythmControls()
    {
        // playerInput.Player.Disable();
        // playerInput.Rhythm.Enable();
    }

}
