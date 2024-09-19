using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image loadingImage;
    [SerializeField] private float loadingTime = 3f;
    [SerializeField] private Color loadingColor = new Color(0.69f, 0.06f, 0.02f); // Default to #B10F06

    private float currentTime = 0f;

    void Start()
    {
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

            LoadMainMenu();
        }

        void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}