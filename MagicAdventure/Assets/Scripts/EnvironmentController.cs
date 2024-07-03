using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentController : MonoBehaviour
{

    [Header("Cutscene")]
    public Transform cutsceneSensor;
    public LayerMask playerLayer;
    public float cutsceneSensorRadius = 0.2f;
    public PlayerController playerController;
    private bool cutsceneTriggered;
    public GameObject cutsceneGroup;
    public GameObject heartParent;
    public float cutsceneLength;
    public UnityEvent enterCutscene;
    public UnityEvent exitCutscene;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("Wind");
    }

    private void Update()
    {
        // Check if the player is grounded
        if (Physics2D.OverlapCircle(cutsceneSensor.position, cutsceneSensorRadius, playerLayer))
        {
            StartCoroutine("TriggerCutscene");
        }
    }

    public IEnumerator TriggerCutscene()
    {
        if(!cutsceneTriggered)
        {
            cutsceneTriggered = true;
            Debug.Log("cutscene triggered");
            
            enterCutscene.Invoke();
            cutsceneGroup.SetActive(true);

            yield return new WaitForSeconds(cutsceneLength);

            exitCutscene.Invoke();
            cutsceneGroup.SetActive(false);
            //Bring out health
            heartParent.SetActive(true);
        }
        
    }

}
