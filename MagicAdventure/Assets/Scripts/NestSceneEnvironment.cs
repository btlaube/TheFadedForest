using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestSceneEnvironment : MonoBehaviour
{
    public GameObject player;
    public GameObject nest;
    public float nestTime;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("Wind");
        StartCoroutine("IntroSequence");
    }

    public IEnumerator IntroSequence()
    {
        player.SetActive(false);
        nest.GetComponent<Animator>().SetTrigger("Shake");

        yield return new WaitForSeconds(nestTime);

        nest.GetComponent<Animator>().SetTrigger("StopShake");
        player.SetActive(true);

    }
}
