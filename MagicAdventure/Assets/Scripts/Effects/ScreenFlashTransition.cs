using System.Collections;
using UnityEngine;

public class ScreenFlashTransition : MonoBehaviour
{
    public GameObject darkScene;
    public GameObject lightScene;

    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private float minFlashDelay = 0.05f;
    [SerializeField] private float maxFlashDelay = 0.4f;

    private bool isDark = true;

    public void StartScreenFlash()
    {
        StartCoroutine(ScreenFlash());
    }

    public IEnumerator ScreenFlash()
    {
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            if (isDark)
                SetLightScene();
            else
                SetDarkScene();

            isDark = !isDark;

            float delay = Random.Range(minFlashDelay, maxFlashDelay);
            yield return new WaitForSeconds(delay);
            elapsed += delay;
        }

        // Optional: finish on a specific state
        SetDarkScene();
    }

    public void SetDarkScene()
    {
        darkScene.SetActive(true);
        lightScene.SetActive(false);
    }

    public void SetLightScene()
    {
        darkScene.SetActive(false);
        lightScene.SetActive(true);
    }
}
