using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public static SceneTransManager Instance;
    public Animator[] transitions;
    
    [SerializeField] private float transitionTime = 1f;

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
    }

    public void LoadScene(int sceneToLoad) {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(int levelIndex, int transitionIndex = 0) {
        transitions[transitionIndex].SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);        

        transitions[transitionIndex].SetTrigger("End");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.Find("Player");
        Debug.Log(player);
        player.transform.position = playerDestination;
    }

    public void DoTransition(int transitionIndex = 0)
    {
        StartCoroutine(Transition(transitionIndex));
    }

    public IEnumerator Transition(int transitionIndex = 0) 
    {
        Debug.Log("Doing transition...");
        transitions[transitionIndex].SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        transitions[transitionIndex].SetTrigger("End");
    }

    public void StartTransition(int transitionIndex = 0)
    {
        transitions[transitionIndex].SetTrigger("Start");
    }

    public void EndTransition(int transitionIndex = 0)
    {
        transitions[transitionIndex].SetTrigger("End");
    }

}
