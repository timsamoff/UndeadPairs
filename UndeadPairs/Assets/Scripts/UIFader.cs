using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float delayBeforeFade = 2.0f;
    [SerializeField] private float fadeTime = 1.0f;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false; // Prevent interactions while invisible

        StartCoroutine(FadeInCanvas());
    }

    private IEnumerator FadeInCanvas()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        canvasGroup.blocksRaycasts = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}