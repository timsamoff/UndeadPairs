using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseHealth : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float healthDecreasePercent = 10f;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Color defaultFillColor = new Color(0f, 0f, 100f, 0.2f);  // Default color
    [SerializeField] private Color damageFillColor = new Color(1f, 0f, 0f, 0.5f);  // Damage color
    [SerializeField] private float damageFillDuration = 0.5f;  // Damage display time
    [SerializeField] private float fillFadeTime = 0.5f;
    private float currentHealth = 100f;

    [Header("Settings")]
    [SerializeField] private CanvasGroup uiCanvasGroup;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private string loseSceneName = "Lose";
    [SerializeField] private BackgroundMusic backgroundMusic;
    [SerializeField] private bool practiceMode = false;

    private CanvasGroup loseCanvasGroup;
    private bool isDead = false;

    private PauseScreen pauseScreen;

    private EndGameAudio endGameAudio;

    private void Start()
    {
        pauseScreen = FindObjectOfType<PauseScreen>();

        endGameAudio = GetComponent<EndGameAudio>();

        practiceMode = false;

        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 1;
            uiCanvasGroup.interactable = true;
            uiCanvasGroup.blocksRaycasts = true;
        }
    }

    private void Update()
    {
        if (parentObject == null && !isDead)
        {
            Debug.LogError("Parent object destroyed or missing!");
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
        currentHealth = 100f;

        if (healthBar != null)
        {
            healthBar.value = currentHealth / 100f;
        }
        isDead = false;
        Time.timeScale = 1f;
        Debug.Log("Health reset to 100.");
    }

    public bool PracticeMode
    {
        get { return practiceMode; }
        set { practiceMode = value; }
    }

    public void ReduceHealth()
    {
        if (!practiceMode) // Health decreases when not in Practice Mode
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

                CameraShake cameraShake = FindObjectOfType<CameraShake>();
                cameraShake.TriggerShake();

                StartCoroutine(FlashRed());
            }

            // Check if health is at 0%
            if (currentHealth <= 0)
            {
                Image backgroundImage = healthBar.GetComponentsInChildren<Image>()[1];
                Debug.Log("Health has reached 0%!");

                if (!isDead)
                {
                    backgroundImage.color = damageFillColor;

                    endGameAudio.PlayLoseAudio();

                    StartCoroutine(backgroundMusic.FadeOutMusic());
                    StartCoroutine(LoadLoseScene());
                }
            }
        }
        else
        {
            CameraShake cameraShake = FindObjectOfType<CameraShake>();
            cameraShake.TriggerShake();

            // Just flash red when in Practice Mode
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        fillFadeTime = 0f;

        Color startColor = healthBarFill.color;

        healthBarFill.color = damageFillColor;

        while (fillFadeTime < damageFillDuration)
        {
            healthBarFill.color = Color.Lerp(damageFillColor, defaultFillColor, fillFadeTime / damageFillDuration);
            fillFadeTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Set final color to normal color
        healthBarFill.color = defaultFillColor;
    }

    private IEnumerator LoadLoseScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loseSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene loseScene = SceneManager.GetSceneByName(loseSceneName);
        GameObject[] rootObjects = loseScene.GetRootGameObjects();
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
        if (!practiceMode) // Prevent LoseGame logic in practice mode
        {
            Time.timeScale = 0f;
            isDead = true;
        }
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

    private void DisableCardClicks()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent object is missing or has been destroyed.");
            return;
        }

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