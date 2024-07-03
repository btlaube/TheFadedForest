using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFairySpawnerTest : MonoBehaviour
{
    public GameObject fairyPrefab;
    // Entered left to right. Ex: [0: leftmost, 1: middle, 2: rightmost]
    public Transform[] spawnLocations;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnAtLeftmost()
    {
        Instantiate(fairyPrefab, spawnLocations[0].position, Quaternion.identity, spawnLocations[0]);
    }

    public void SpawnAtRightmost()
    {
        Instantiate(fairyPrefab, spawnLocations[spawnLocations.Length-1].position, Quaternion.identity, spawnLocations[spawnLocations.Length-1]);
    }

    public void SpawnAtLocation(int locationIndex)
    {
        Instantiate(fairyPrefab, spawnLocations[locationIndex].position, Quaternion.identity, spawnLocations[locationIndex]);
    }

    public void StartSequence(int sequenceIndex)
    {
        animator.SetTrigger("Sequence"+sequenceIndex.ToString());
    }

}
