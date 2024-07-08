using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static SceneTransManager Instance;
    public Animator transition;
    
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private string detectorTag;
    [SerializeField] private GameObject[] detectors;

    public Vector3 playerDestination;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        GetDetectors();
    }

    private void GetDetectors()
    {
        detectors = GameObject.FindGameObjectsWithTag(detectorTag);
    }

    public void LoadScene(int sceneToLoad) {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);        

        transition.SetTrigger("End");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.Find("Player");
        Debug.Log(player);
        player.transform.position = playerDestination;
    }
}
