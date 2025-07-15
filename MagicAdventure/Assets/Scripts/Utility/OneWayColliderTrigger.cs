using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneWayColliderTrigger : MonoBehaviour
{
    public float triggerDirection;
    public UnityEvent myEvent;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Check trigger direction 
            Vector3 diff = other.transform.position - transform.position;
            if (diff.x * triggerDirection > 0.0f)
            {
                // Correct way 
                myEvent.Invoke();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Wrong way");
            }
        }
        
    }
}
