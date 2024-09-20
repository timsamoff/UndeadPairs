using UnityEngine;
using UnityEngine.UI;

public class RandomImageSelector : MonoBehaviour
{
    public Image loadingImage;

    public Sprite[] images;

    void Start()
    {
        if (images.Length > 0)
        {
            int randomIndex = Random.Range(0, images.Length);

            loadingImage.sprite = images[randomIndex];
        }
        else
        {
            Debug.LogWarning("No images assigned!");
        }
    }
}