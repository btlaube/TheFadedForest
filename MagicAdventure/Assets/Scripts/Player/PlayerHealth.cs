using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent OnDeath;

    public int health = 1;
    public GameObject[] hearts;
    public GameObject heartParent;

    [Header("Heart Beat Audio")]
    public float beatInterval = 0.5f;
    public float beatMinPitch = 0.8f;
    public float beatMaxPitch = 0.9f;
    private AudioManager audioManager;
    private bool isBeating;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        // health = 3;
        heartParent.SetActive(false);
        // audioManager.Play("HeartBeat");
    }

    private void FixedUpdate()
    {
        if (heartParent == null) return;
        if (!heartParent.activeSelf) return;
        //Handle Audio
        if (health > 0 && !isBeating)
        {
            StartBeatAudio();
            isBeating = true;
        }
        if (health <= 0)
        {
            StopBeatAudio();
            isBeating = false;
        }
    }

    public void TakeDamage()
    {
        DestroyHeart();
    }

    public void DestroyHeart()
    {
        hearts[health-1].GetComponent<Animator>().SetTrigger("Die");
        // yield return new WaitForSeconds(0.1f);
        // Destroy(hearts[health-1]);
        health -= 1;
        if (health <= 0)
        {
            Die();
            StopBeatAudio();
        }
    }

    public void StartBeatAudio()
    {
        isBeating = true;
        StopAllCoroutines();
        StartCoroutine("Beat");
        audioManager.Play("HeartBeat");
    }

    public void StopBeatAudio()
    {
        isBeating = false;
        audioManager.Stop("HeartBeat");
    }

    public IEnumerator Beat()
    {
        while (isBeating)
        {
            audioManager.PlaySoundRandomPitch("HeartBeat", beatMinPitch, beatMaxPitch);

            yield return new WaitForSeconds(beatInterval);
        }
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        StopHearts();
        OnDeath.Invoke();
    }

    public void Respawn()
    {
        health = 1;
        StartHearts();
    }

    public void StartHearts()
    {
        heartParent.SetActive(true);
        StartBeatAudio();
    }

    public void StopHearts()
    {
        heartParent.SetActive(false);
        StopBeatAudio();
    }

}
