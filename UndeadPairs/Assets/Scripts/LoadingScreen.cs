using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image loadingRing = null;
    [SerializeField] private float fillSpeed = 0.1f;
    [SerializeField] private float maxTimer = 1.0f;
    [SerializeField] private float delayBeforeNextScene = 0.5f;

    private bool isLoading = true;

    private void Start()
    {
        isLoading = true;
        loadingRing.enabled = false;
    }

    private void Update()
    {
        if (isLoading)
        {
            loadingRing.enabled = true;
            loadingRing.fillAmount += fillSpeed * Time.deltaTime;

            if (loadingRing.fillAmount >= maxTimer)
            {
                isLoading = false;
                loadingRing.fillAmount = maxTimer;
                Invoke("LoadNextScene", delayBeforeNextScene);
            }
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}