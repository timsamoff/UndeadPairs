using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeDuration = 2.0f;

    private float currentTime = 0f;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image not assigned. Please assign an Image in the Inspector.");
            return;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
        StartCoroutine(FadeInScene());
    }

    private IEnumerator FadeInScene()
    {
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, currentTime / fadeDuration);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        fadeImage.gameObject.SetActive(false);
    }
}