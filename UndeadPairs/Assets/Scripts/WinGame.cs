using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    [Header("Win Settings")]
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private string winSceneName = "Win";  // Name of the Win scene
    private CanvasGroup winCanvasGroup;

    private PauseScreen pauseScreen;

    [SerializeField] private BackgroundMusic backgroundMusic;

    private void Start()
    {
        pauseScreen = FindObjectOfType<PauseScreen>();

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }

    private void Update()
    {
        // Example of how to access the matchedCardCount from CardFlip
        int currentMatchedCards = CardFlip.MatchedCardCount;

        // Check for win condition
        if (currentMatchedCards * 2 == FindObjectsOfType<CardFlip>().Length)
        {
            CheckForWinCondition(currentMatchedCards, FindObjectsOfType<CardFlip>().Length);
        }
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable called.");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset UI or other parameters as necessary
        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
        Debug.Log("Win UI reset.");
    }

    public void CheckForWinCondition(int matchedCardCount, int totalCardCount)
    {
        Debug.Log("Matched Card Count: " + matchedCardCount);
        Debug.Log("Total Card Count: " + totalCardCount);

        if (matchedCardCount == totalCardCount) // Check if all pairs are matched
        {
            Debug.Log("All cards matched!");
            StartCoroutine(LoadWinScene());
        }
        else
        {
            Debug.Log("Not all cards matched yet.");
        }
    }

    private IEnumerator LoadWinScene()
    {
        Debug.Log("Loading Win scene...");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(winSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene winScene = SceneManager.GetSceneByName(winSceneName);
        GameObject[] rootObjects = winScene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            CanvasGroup cg = obj.GetComponentInChildren<CanvasGroup>();
            if (cg != null)
            {
                winCanvasGroup = cg;
                break;
            }
        }

        if (winCanvasGroup != null)
        {
            winCanvasGroup.alpha = 0;
            winCanvasGroup.interactable = false;
            winCanvasGroup.blocksRaycasts = false;
            winCanvasGroup.gameObject.SetActive(true);
        }

        StartCoroutine(FadeOutCanvasGroup(uiCanvasGroup));
        StartCoroutine(FadeInCanvasGroup(winCanvasGroup));
        StartCoroutine(backgroundMusic.FadeOutMusic());
    }

    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < pauseScreen.FadeTime)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / pauseScreen.FadeTime);
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

        while (elapsedTime < pauseScreen.FadeTime)
        {
            canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / pauseScreen.FadeTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }
}