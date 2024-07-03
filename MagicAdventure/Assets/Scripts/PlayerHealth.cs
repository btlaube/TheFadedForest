using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    private int health;

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
        health = 3;
        // audioManager.Play("HeartBeat");
    }

    private void FixedUpdate()
    {
        //Handle Audio
        // if (health > 0 && !isBeating)
        // {
        //     StartBeatAudio();
        //     isBeating = true;
        // }
        if (health <= 0)
        {
            StopBeatAudio();
            isBeating = false;
        }
    }

    public void TakeDamage()
    {
        StartCoroutine("DestroyHeart");        
    }

    public IEnumerator DestroyHeart()
    {
        hearts[health-1].GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(hearts[health-1]);
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
        audioManager.Play("DeepHeartBeat");
    }

    public void StopBeatAudio()
    {
        isBeating = false;
        audioManager.Stop("DeepHeartBeat");
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
    }
}
