using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private string pauseSceneName = "Pause";

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

    public float FadeTime => fadeTime;

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
        while (!asyncLoad.isDone) yield return null;

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
            Debug.Log("Fading out pauseCanvasGroup");
            yield return StartCoroutine(FadeOutCanvasGroup(pauseCanvasGroup));
        }
        else
        {
            Debug.LogWarning("pauseCanvasGroup is null, skipping fade out.");
        }

        Debug.Log("Attempting to unload pause scene.");

        // Check if scene is loaded before trying to unload it
        Scene pauseScene = SceneManager.GetSceneByName(pauseSceneName);

        if (pauseScene.isLoaded)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(pauseSceneName);
            if (asyncUnload == null)
            {
                Debug.LogError("Failed to unload pause scene.");
                yield break;
            }

            while (!asyncUnload.isDone)
            {
                Debug.Log("Waiting for scene to unload...");
                yield return null;
            }

            Debug.Log("Scene unloaded, resuming game.");
        }
        else
        {
            Debug.LogWarning($"Pause scene {pauseSceneName} is not loaded. Skipping unload.");
        }

        ResumeGame();

        Debug.Log("Fading in UI Canvas Group");
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
        isPaused = false;
        EnableCardClicks();
        InitializeUI();
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
            if (collider != null) collider.enabled = false;
        }
    }

    private void EnableCardClicks()
    {
        CardFlip[] cardFlips = parentObject.GetComponentsInChildren<CardFlip>();
        foreach (CardFlip cardFlip in cardFlips)
        {
            Collider collider = cardFlip.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;
        }
    }
    private void InitializeUI()
    {
        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }
}