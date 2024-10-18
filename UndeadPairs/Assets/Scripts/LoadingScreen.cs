using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image loadingImage;
    [SerializeField] private float loadingTime = 3f;
    [SerializeField] private float delayBeforeSceneChange = 2f;
    [SerializeField] private Color loadingColor = new Color(0.69f, 0.06f, 0.02f); // Default to #B10F06

    private float currentTime = 0f;

    void Start()
    {

        // Limit framerate to 30fps.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        loadingImage.color = loadingColor;

        loadingImage.fillAmount = 0f;
    }

    void Update()
    {
        if (currentTime < loadingTime)
        {
            currentTime += Time.deltaTime;
            loadingImage.fillAmount = currentTime / loadingTime;
        }
        else
        {
            loadingImage.fillAmount = 1f;

            Invoke("LoadMainMenu", delayBeforeSceneChange);
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}