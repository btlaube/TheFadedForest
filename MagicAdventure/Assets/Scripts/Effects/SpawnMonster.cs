using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Vector2 monsterSpawnPos;

    private AudioManager audioManager;
    private GameObject spawnedMonster;

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    public void Spawn()
    {
        Vector3 monsterSpawnPos = GameObject.Find("MonsterSpawnPoint").transform.position;
        if (spawnedMonster != null) Destroy(spawnedMonster);
        spawnedMonster = Instantiate(monsterPrefab, monsterSpawnPos, Quaternion.identity);
        audioManager.Play("Spawn");
    }

    public GameObject GetSpawnedMonster()
    {
        return spawnedMonster;
    }
}
