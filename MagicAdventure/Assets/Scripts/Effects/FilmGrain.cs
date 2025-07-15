using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilmGrain : MonoBehaviour
{
    public Sprite[] sprites;
    public Image effectImage;
    public float effectSpeed = 0.1f;

    void Start()
    {
        StartCoroutine(FilmGrainCoroutine());
    }

    IEnumerator FilmGrainCoroutine()
    {
        foreach (Sprite sprite in sprites)
        {
            effectImage.sprite = sprite;
            yield return new WaitForSeconds(effectSpeed); // Wait for 1 second
            
        }

        // Restart the coroutine
        StartCoroutine(FilmGrainCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
