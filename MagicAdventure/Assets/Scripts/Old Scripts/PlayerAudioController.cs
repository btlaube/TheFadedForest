using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InvokeRepeating("Walk", 1f, 1f);
    }

    private void Walk()
    {
        if (GetComponentInChildren<Animator>().GetFloat("Speed") != 0 && GetComponentInChildren<Animator>().GetBool("IsGrounded"))
        {
            PlaySoundRandomPitch();
        }
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void PlaySoundLoop()
    {
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSoundLooping()
    {
        audioSource.loop = false;
        StopSound();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        PlaySound();
    }

    private void PlaySoundRandomPitch()
    {
        RandomizePitch();
        PlaySound();
    }

    private void RandomizePitch()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
    }
}
