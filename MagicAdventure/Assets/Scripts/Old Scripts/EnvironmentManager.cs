using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private AudioSource audioSource;
    public Animator nestAnimator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySound();
        nestAnimator.SetTrigger("Shake");
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
