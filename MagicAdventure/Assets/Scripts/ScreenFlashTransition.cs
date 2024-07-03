using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlashTransition : MonoBehaviour
{
    
    public GameObject darkScene;
    public GameObject lightScene;

    private float timer;
    [SerializeField] private float transitionDuration;

   

    public void StartScreenFlash()
    {
        StartCoroutine("ScreenFlash");
    }

    public IEnumerator ScreenFlash()
    {
        darkScene.SetActive(true);
        lightScene.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        darkScene.SetActive(false);
        lightScene.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        darkScene.SetActive(true);
        lightScene.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        darkScene.SetActive(false);
        lightScene.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        darkScene.SetActive(true);
        lightScene.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        SetDarkScene();
        yield return new WaitForSeconds(0.1f);
        SetLightScene();
        yield return new WaitForSeconds(0.2f);
        SetDarkScene();
        yield return new WaitForSeconds(0.3f);
        SetLightScene();
        yield return new WaitForSeconds(0.4f);
        SetDarkScene();
        
    }

    private void SetDarkScene()
    {
        darkScene.SetActive(true);
        lightScene.SetActive(false);
    }

    private void SetLightScene()
    {
        darkScene.SetActive(false);
        lightScene.SetActive(true);
    }
}
