using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{

    public Vector2 playerRespawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerRespawnPoint = new Vector2(20f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerRespawnPosition()
    {
        playerRespawnPoint = PlayerController.Instance.transform.position;
    }
}
