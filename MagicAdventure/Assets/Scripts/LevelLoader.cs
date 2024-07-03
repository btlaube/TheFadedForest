using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelLoader : MonoBehaviour
{
    
    public Animator transition;

    [SerializeField] private float transitionTime = 1f;

    // Static reference to the single instance of the class
    private static LevelLoader instance;

    // Public property to access the singleton instance
    public static LevelLoader Instance
    {
        get
        {
            // If the instance hasn't been set yet, find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<LevelLoader>();

                // If there is no instance in the scene, create a new GameObject and attach the script
                if (instance == null)
                {
                    GameObject singletonGO = new GameObject(typeof(LevelLoader).Name);
                    instance = singletonGO.AddComponent<LevelLoader>();
                }
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneToLoad) {
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

        transition.SetTrigger("End");
    }

    public void Quit() {
        Application.Quit();
    }

}
