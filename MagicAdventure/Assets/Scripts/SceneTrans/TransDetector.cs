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
        transManager = GameObject.Find("SceneTransManager").GetComponent<SceneTransManager>();
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

            // // Check player movement from input
            // Vector2 inputVector = new Vector2();
            // inputVector.x = Input.GetAxis("Horizontal");
            // Vector3 diff = other.transform.position - transform.position;
            // // float curDistance = diff.sqrMagnitude;
            // Debug.Log(diff.x);
            // GameObject closest = null;
            // float distance = Mathf.Infinity;
            // Vector3 position = other.transform.position;
            // foreach (GameObject go in detectors)
            // {
            //     Vector3 diff = go.transform.position - position;
            //     float curDistance = diff.sqrMagnitude;
            //     if (curDistance < distance)
            //     {
            //         closest = go;
            //         distance = curDistance;
            //     }
            // }
            // Debug.Log(closest);
            // detectors.Add(closest);
        }
        
    }
}
