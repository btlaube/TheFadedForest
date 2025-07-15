using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyController : MonoBehaviour
{
    [Header("Movement")]
    public float niceSpeed;
    public float meanSpeed;
    private float moveSpeed;
    public float deceleration;
    public float niceTime;
    public float freezeTime;
    public float damage;

    [Header("Death")]
    public float deathDuration;
    public float deathSpeed;
    public bool isDead;

    private Transform player;
    private Vector2 targetPosition;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float previousSpeed;

    // Disable controls
    private bool isDisabled;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    private void Update()
    {
        targetPosition = player.position;
    }

    private void FixedUpdate()
    {
        if (isDisabled) return;
        if (isDead) return;
        Vector2 direction = targetPosition - rb.position;
        direction.Normalize();

        // Calculate the target horizontal velocity based on the input
        float targetVelocityX = direction.x * moveSpeed;
        float targetVelocityY = direction.y * moveSpeed;

        // Apply deceleration to the horizontal velocity
        float currentVelocityX = rb.velocity.x;
        float currentVelocityY = rb.velocity.y;

        float newVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, deceleration * Time.deltaTime);
        float newVelocityY = Mathf.MoveTowards(currentVelocityY, targetVelocityY, deceleration * Time.deltaTime);

        rb.velocity = new Vector2(newVelocityX, newVelocityY);

        // Flip Sprite
        // Check if the direction has changed
        if (rb.velocity.x != 0f && rb.velocity.x != previousSpeed)
        {
            // Flip the sprite horizontally based on the direction
            FlipSprite(rb.velocity.x);
        }
        // Update the previous direction for the next frame
        previousSpeed = rb.velocity.x;
    }

    private void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public IEnumerator SpawnCycle()
    {
        // while (true)
        // {
            // animator.SetTrigger("Nice");
            animator.SetBool("IsMean", false);
            SetMoveSpeed(niceSpeed);
            yield return new WaitForSeconds(niceTime);

            SetMoveSpeed(0f);
            yield return new WaitForSeconds(freezeTime);

            animator.SetBool("IsMean", true);
            SetMoveSpeed(meanSpeed);
        // }
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0f)
        {
            // sr.flipX = true; // flipped (facing right)
            transform.localScale = new Vector3(-3f, 3f, 3f);
        }
        else if (direction < 0f)
        {
            sr.flipX = false; // Not Flipped (facing left)
            transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    public void Die()
    {
        isDead = true;

        Vector2 velocity = rb.velocity;
        velocity.x = rb.velocity.x < 0 ? deathSpeed : -deathSpeed;
        rb.velocity = velocity;

        animator.SetTrigger("Die");
        Invoke("DestroySelf", deathDuration);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage();
            Destroy(gameObject);
        }
        else if (other.tag == "Campfire")
        {
            Destroy(gameObject);
        }
    }

    public void DisableControls()
    {
        isDisabled = true;
    }

    public void EnableControls()
    {
        isDisabled = false;
    }

}
