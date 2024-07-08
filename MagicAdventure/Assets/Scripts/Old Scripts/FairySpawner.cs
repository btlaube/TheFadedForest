using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairySpawner : MonoBehaviour
{
    public GameObject fairyPrefab;
    public Transform[] spawnLocations;
    public float spawnInterval;

    public bool activeSpawn;

    public IEnumerator SpawnAllLocations()
    {
        while (true)
        {
            for (int i = 0; i < spawnLocations.Length; i++)
            {
                Instantiate(fairyPrefab, spawnLocations[i].position, Quaternion.identity, spawnLocations[i]);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        
    }

    public void Disable()
    {
        activeSpawn = false;
        StopAllCoroutines();
    }

    public void Enable()
    {
        if(!activeSpawn)
        {
            StartCoroutine("SpawnAllLocations");
        }
        activeSpawn = true;
    }
}
