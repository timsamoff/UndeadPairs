using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private string pauseSceneName = "Pause";  // Name of the Pause scene

    private CanvasGroup pauseCanvasGroup;
    private bool isPaused = false;

    private void Start()
    {
        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }

    public float FadeTime
    {
        get { return fadeTime; }
    }

    public void OnPauseButtonPressed()
    {
        if (!isPaused)
        {
            StartCoroutine(LoadPauseScene());
        }
    }

    public void OnResumeButtonPressed()
    {
        if (isPaused)
        {
            StartCoroutine(UnloadPauseScene());
        }
    }

    private IEnumerator LoadPauseScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(pauseSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene pauseScene = SceneManager.GetSceneByName(pauseSceneName);
        GameObject[] rootObjects = pauseScene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            CanvasGroup cg = obj.GetComponentInChildren<CanvasGroup>();
            if (cg != null)
            {
                pauseCanvasGroup = cg;
                break;
            }
        }

        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = 0;
            pauseCanvasGroup.interactable = false;
            pauseCanvasGroup.blocksRaycasts = false;
            pauseCanvasGroup.gameObject.SetActive(true);
        }

        PauseGame();
        DisableCardClicks();
        StartCoroutine(FadeOutCanvasGroup(uiCanvasGroup));
        StartCoroutine(FadeInCanvasGroup(pauseCanvasGroup));
    }

    private IEnumerator UnloadPauseScene()
    {
        if (pauseCanvasGroup != null)
        {
            // Fade out the pause scene UI
            yield return StartCoroutine(FadeOutCanvasGroup(pauseCanvasGroup));
        }

        // Unload the Pause scene
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(pauseSceneName);

        // Wait until the scene is fully unloaded
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // After the pause scene is unloaded, fade in the main UI canvas
        ResumeGame();
        StartCoroutine(FadeInCanvasGroup(uiCanvasGroup));
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Resuming game... Time.timeScale set to: " + Time.timeScale);
        isPaused = false;
        EnableCardClicks();
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