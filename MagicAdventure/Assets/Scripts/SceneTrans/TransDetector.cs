using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransDetector : MonoBehaviour
{
    public int sceneToLoad;
    public Vector3 playerDestination;
    public float triggerDirection;

    private SceneTransManager transManager;

    private void Awake()
    {
        transManager = GameObject.Find("SceneTransitionManager").GetComponent<SceneTransManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Fart!");
        if (other.tag == "Player")
        {
            // Check trigger direction 
            Vector3 diff = other.transform.position - transform.position;
            Debug.Log(diff.x);
            if (diff.x * triggerDirection > 0.0f)
            {
                Debug.Log("Change Scene");
                transManager.playerDestination = playerDestination;
                transManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Wrong way");
            }
        }
        
    }
}
