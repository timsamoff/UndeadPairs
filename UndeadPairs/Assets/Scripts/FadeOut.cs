using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeToBlack : MonoBehaviour
{
    public CanvasGroup blackScreen;
    public float fadeDuration = 1f;

    private bool isFading = false;

    public Action onFadeComplete;

    void Start()
    {
        blackScreen.alpha = 0f;
    }

    public void StartFade()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        isFading = false;

        onFadeComplete?.Invoke();
    }
}