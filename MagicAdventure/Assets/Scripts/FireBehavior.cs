using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public GameObject light;
    public GameObject fire;
    public GameObject lightPopUp;


    // Start is called before the first frame update
    void Start()
    {
        DouseFire();
    }

    public void LightFire()
    {
        light.SetActive(true);
        fire.SetActive(true);
        DisableInteract();
    }

    public void DouseFire()
    {
        light.SetActive(false);
        fire.SetActive(false);
    }

    public void DisableInteract()
    {
        lightPopUp.SetActive(false);
        GetComponent<InteractPopUp>().enabled = false;
    }

    public void EnableInteract()
    {
        GetComponent<InteractPopUp>().enabled = true;
    }

}
