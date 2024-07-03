using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player.SetActive(false);
        Invoke("SpawnPlayer", 3f);
    }

    private void SpawnPlayer()
    {
        player.SetActive(true);
    }

}
