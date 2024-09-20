using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float healthDecreasePercent = 10f;
    [SerializeField] private Slider healthBar;
    private float currentHealth = 100f;

    [Header("Settings")]
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private string loseSceneName = "Lose";  // Name of the Lose scene

    private CanvasGroup loseCanvasGroup;
    private bool isDead = false;

    /*private void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("Health bar Slider not assigned.");
        }
        else
        {
            healthBar.value = currentHealth / 100f;
        }
    }*/

    private void Update()
    {
        Debug.Log("Current Health: " + currentHealth);

        if (parentObject == null && !isDead)
        {
            Debug.LogError("Parent object has been destroyed or missing!");
        }
    }

    private void OnEnable()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = 100f;

        if (healthBar != null)
        {
            healthBar.value = currentHealth / 100f;
        }
        isDead = false;
        Time.timeScale = 1f;
        Debug.Log("Health reset to 100.");
    }

    public void ReduceHealth()
    {
        currentHealth -= healthDecreasePercent;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        if (healthBar == null)
        {
            Debug.LogError("Health bar Slider not assigned.");
        }
        else
        {
            healthBar.value = currentHealth / 100f;
        }

        // Check if health is at 0%
        if (currentHealth <= 0)
        {
            Debug.Log("Health has reached 0%!");

            if (!isDead)
            {
                StartCoroutine(LoadLoseScene());
            }
        }
    }

    private IEnumerator LoadLoseScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loseSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene pauseScene = SceneManager.GetSceneByName(loseSceneName);
        GameObject[] rootObjects = pauseScene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            CanvasGroup cg = obj.GetComponentInChildren<CanvasGroup>();
            if (cg != null)
            {
                loseCanvasGroup = cg;
                break;
            }
        }

        if (loseCanvasGroup != null)
        {
            loseCanvasGroup.alpha = 0;
            loseCanvasGroup.interactable = false;
            loseCanvasGroup.blocksRaycasts = false;
            loseCanvasGroup.gameObject.SetActive(true);
        }

        LoseGame();
        DisableCardClicks();
        StartCoroutine(FadeOutCanvasGroup(uiCanvasGroup));
        StartCoroutine(FadeInCanvasGroup(loseCanvasGroup));
    }

    private void LoseGame()
    {
        Time.timeScale = 0f;
        isDead = true;
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
        if (parentObject == null)
        {
            Debug.LogError("Parent object is missing or has been destroyed.");
            return;
        }

        // CardFlip[] cardFlips = parentObject.GetComponentsInChildren<CardFlip>();
        CardFlip[] cardFlips = parentObject.GetComponentsInChildren<CardFlip>(true);

        foreach (CardFlip cardFlip in cardFlips)
        {
            Collider collider = cardFlip.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}