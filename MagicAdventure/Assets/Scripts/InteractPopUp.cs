using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractPopUp : MonoBehaviour
{
    public GameObject interactPopUp;
    [SerializeField] private float interactDistance;
    private Transform target;
    public UnityEvent interactEvent;

    void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    void OnEnable() {
        interactPopUp.SetActive(true);
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, target.position) < interactDistance)
        {
            interactPopUp.SetActive(true);
            if (Input.GetKeyUp(KeyCode.X))
            {
                InteractAction();
                interactPopUp.SetActive(false);
            }
        }
        else
        {
            interactPopUp.SetActive(false);
        }        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, interactDistance);
    }

    public void InteractAction()
    {
        Debug.Log("You interacted!");
        interactEvent.Invoke();
    }
}
