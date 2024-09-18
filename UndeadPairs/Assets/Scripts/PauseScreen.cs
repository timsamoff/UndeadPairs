using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup pauseCanvasGroup;
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject parentObject;  // Reference to parent with children having "Card Flip" component
    [SerializeField] private float fadeTime = 1.0f;

    private bool isPaused = false;

    private void Start()
    {
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = 0;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
            pauseCanvasGroup.gameObject.SetActive(false);
        }

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPauseButtonPressed()
    {
        if (!isPaused)
        {
            PauseGame();
            DisableCardClicks();
            StartCoroutine(FadeInCanvasGroup(pauseCanvasGroup));
            StartCoroutine(FadeOutCanvasGroup(uiCanvasGroup));
        }
    }

    public void OnBackButtonPressed()
    {
        if (isPaused)
        {
            ResumeGame();
            EnableCardClicks();
            StartCoroutine(FadeOutCanvasGroup(pauseCanvasGroup));
            StartCoroutine(FadeInCanvasGroup(uiCanvasGroup));
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseCanvasGroup.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < fadeTime)
        {
            canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    // Disable clicking on all children with a "Card Flip" component
    private void DisableCardClicks()
    {
        CardFlip[] cardFlips = parentObject.GetComponentsInChildren<CardFlip>();

        foreach (CardFlip cardFlip in cardFlips)
        {
            Collider collider = cardFlip.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    // Enable clicking on all children with a "Card Flip" component
    private void EnableCardClicks()
    {
        CardFlip[] cardFlips = parentObject.GetComponentsInChildren<CardFlip>();

        foreach (CardFlip cardFlip in cardFlips)
        {
            Collider collider = cardFlip.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}