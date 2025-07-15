using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireManager : MonoBehaviour
{

    public FireBehavior[] campfires;

    public void DisableAllCampfires()
    {
        foreach (FireBehavior campfire in campfires)
        {
            campfire.DisableInteract();
        }
    }

    public void EnableAllCampfires()
    {
        foreach (FireBehavior campfire in campfires)
        {
            campfire.EnableInteract();
        }
    }

}
