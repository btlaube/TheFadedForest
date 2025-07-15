using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerInputData
{
    public Vector2 moveDirection;
    public bool isSprinting;
    public bool isJumping;
    public bool isCrouching;

    public PlayerInputData(Vector2 move, bool sprint, bool jump, bool crouch)
    {
        moveDirection = move;
        isSprinting = sprint;
        isJumping = jump;
        isCrouching = crouch;
    }
}

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    private PlayerInputData lastInput;
    public PlayerInputData LastInput => lastInput;

    public event Action OnSceneLoadedWithData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    public void SaveInput(PlayerInputData input)
    {
        lastInput = input;
    }

    public PlayerInputData LoadInput()
    {
        return lastInput;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Wait one frame to ensure everything is loaded
        StartCoroutine(DelayedSceneLoadEvent());
    }

    private System.Collections.IEnumerator DelayedSceneLoadEvent()
    {
        yield return null; // wait one frame
        OnSceneLoadedWithData?.Invoke();
    }
}
