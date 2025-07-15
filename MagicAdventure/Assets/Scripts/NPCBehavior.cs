using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float groundDeceleration = 10f;
    private bool isWalking;
    private bool isGrounded;
    public float walkInterval = 0.5f;
    public float walkMinPitch = 0.8f;
    public float walkMaxPitch = 0.9f;
    
    // public float walkDuration;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private AudioManager audioManager;

    private float previousSpeed;

    private Vector2 movement;

    private void Awake()
    {
        movement = Vector2.zero;

        // SetSlowVisual();

        rb = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioManager>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        isGrounded = true;
        animator.SetBool("IsGrounded", isGrounded);
    }
  
    public void StartWalk(float direction, float duration)
    {
        if (direction > 0)
        {
            StartCoroutine(WalkRight(duration));
        }
        else
        {
            StartCoroutine(WalkLeft(duration));
        }
    }

    public IEnumerator WalkRight(float duration)
    {
        movement.x = 1.0f;
        yield return new WaitForSeconds(duration);
        movement.x = 0f;
    }

    public IEnumerator WalkLeft(float duration)
    {
        movement.x = -1.0f;
        yield return new WaitForSeconds(duration);
        movement.x = 0f;
    }

    private void FixedUpdate()
    {
        // -- Handle input and movement --
        // Vector2 inputVector = Vector2.zero;
        // inputVector.x = Input.GetAxis("Horizontal");
        // rb.velocity = new Vector2(inputVector.x * moveSpeed, rb.velocity.y);

        // float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.velocity;

        // Apply deceleration based on whether the player is on the ground or in the air
        // float deceleration = isGrounded ? groundDeceleration : airDeceleration;

        // if (isGrounded)
        // {
        //     isFlying = false;
        //     rb.gravityScale = 1f;
        // }

        // if (isFlying)
        // {
        //     rb.gravityScale = 0f;

        //     // Calculate the target horizontal velocity based on the input
        //     float targetVelocityX = inputVector.x * flySpeed;

        //     // Flying mode behavior
        //     // float verticalInput = Input.GetAxis("Vertical");
        //     float targetVelocityY = inputVector.y * flySpeed;
        //     float tiltAngle = targetVelocityY * flyTiltSpeed;

        //     // Apply deceleration to the horizontal velocity
        //     velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, flyDeceleration * Time.deltaTime);

        //     // Tilt the player
        //     rb.rotation = tiltAngle;

        //     // Move up or glide when the jump button is pressed or released
        //     if (inputVector.y != 0)
        //     {
        //         Debug.Log(inputVector.y);
        //         // velocity.y = Mathf.MoveTowards(velocity.y, targetVelocityY, deceleration * Time.deltaTime);
        //         velocity.y = Mathf.MoveTowards(velocity.y, targetVelocityY, flyDeceleration * Time.deltaTime);
        //     }
        //     else
        //     {
        //         velocity.y = Mathf.MoveTowards(velocity.y, -glideSpeed, deceleration * Time.deltaTime);
        //     }
        // }
        // else
        // {

            // Calculate the target horizontal velocity based on the input
            float targetVelocityX = movement.x * moveSpeed;

            // Apply deceleration to the horizontal velocity
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, groundDeceleration * Time.deltaTime);
        // }

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
        if (velocity.x != 0 && !isWalking)
        {
            StartWalkAudio();
            isWalking = true;
        }
        if (velocity.x == 0)
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
        // StopAllCoroutines();
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
}
