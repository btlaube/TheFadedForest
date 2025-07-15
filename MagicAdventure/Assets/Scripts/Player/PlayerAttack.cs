using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackSpawnPoint;
    public float attackCooldown;
    private float attackTimer;
    private bool hasAttacked;

    public List<KeyCode> attackKeys;
    public GameObject attackPrefab;
    private SpriteRenderer sr;
    // private AudioHandler audioHandler;
    private Animator animator;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // audioHandler = GetComponent<AudioHandler>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        hasAttacked = false;
        attackTimer = 0f;
        attackKeys = new List<KeyCode> { KeyCode.X };
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAnyKeyDown(attackKeys) && !hasAttacked)
        {
            Debug.Log("Attack!");
            // audioHandler.Play("Attack");
            // animator.SetTrigger("Attack");
            StartCoroutine(ProjectileAttack());
            hasAttacked = true;
        }
        if (hasAttacked)
        {
            attackTimer += Time.deltaTime;
        }
        if (attackTimer >= attackCooldown)
        {
            hasAttacked = false;
            attackTimer = 0f;
        }
    }

    // Input Helper methods
    private bool IsAnyKeyHeld(List<KeyCode> attackKeys)
    {
        foreach (var key in attackKeys)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }

    private bool IsAnyKeyDown(List<KeyCode> attackKeys)
    {
        foreach (var key in attackKeys)
        {
            if (Input.GetKeyDown(key))
                return true;
        }
        return false;
    }

    public IEnumerator Attack()
    {

        // Move attack to left or right of player
        GameObject attackObject = Instantiate(attackPrefab, attackSpawnPoint.transform.position, Quaternion.identity, transform);
        attackObject.transform.localPosition += (sr.flipX ? new Vector3(-1, 0.5f, 0) : new Vector3(1, 0.5f, 0));

        yield return new WaitForSeconds(0.125f);

        Destroy(attackObject);
    }

    public IEnumerator ProjectileAttack()
    {
        animator.SetTrigger("Attack");
        // Move attack to left or right of player
        GameObject attackObject = Instantiate(attackPrefab, attackSpawnPoint.transform.position, Quaternion.identity);
        float lookDirection = transform.localScale.x < 0.0f ? -1.0f : 1.0f;
        attackObject.GetComponent<Rigidbody2D>().velocity = new Vector2(10f * lookDirection, 0f);
        attackObject.transform.localScale = new Vector3(attackObject.transform.localScale.x * lookDirection, attackObject.transform.localScale.y, attackObject.transform.localScale.z);

        yield return new WaitForSeconds(5f);

        Destroy(attackObject);
    }

}
