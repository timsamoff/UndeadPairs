using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFadeIn : MonoBehaviour
{
    [SerializeField] private float timeBeforeFade = 1f;
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private Graphic[] buttons; // Drag your button UI elements here
    [SerializeField] private Slider[] sliders;  // Drag your slider UI elements here

    private Color[] sliderBackgroundOriginalColors;
    private Color[] sliderFillOriginalColors;
    private Color[] sliderBackgroundTargetColors;
    private Color[] sliderFillTargetColors;

    private void Start()
    {
        // Initialize colors for sliders
        sliderBackgroundOriginalColors = new Color[sliders.Length];
        sliderFillOriginalColors = new Color[sliders.Length];
        sliderBackgroundTargetColors = new Color[sliders.Length];
        sliderFillTargetColors = new Color[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] != null)
            {
                var backgroundGraphic = sliders[i].GetComponentsInChildren<Graphic>()[0]; // Assume the first Graphic is the background
                var fillGraphic = sliders[i].GetComponentsInChildren<Graphic>()[1]; // Assume the second Graphic is the fill

                if (backgroundGraphic != null)
                {
                    sliderBackgroundOriginalColors[i] = backgroundGraphic.color;
                    sliderBackgroundTargetColors[i] = new Color(sliderBackgroundOriginalColors[i].r, sliderBackgroundOriginalColors[i].g, sliderBackgroundOriginalColors[i].b, sliderBackgroundOriginalColors[i].a);
                    backgroundGraphic.color = new Color(sliderBackgroundOriginalColors[i].r, sliderBackgroundOriginalColors[i].g, sliderBackgroundOriginalColors[i].b, 0f);
                }

                if (fillGraphic != null)
                {
                    sliderFillOriginalColors[i] = fillGraphic.color;
                    sliderFillTargetColors[i] = new Color(sliderFillOriginalColors[i].r, sliderFillOriginalColors[i].g, sliderFillOriginalColors[i].b, sliderFillOriginalColors[i].a);
                    fillGraphic.color = new Color(sliderFillOriginalColors[i].r, sliderFillOriginalColors[i].g, sliderFillOriginalColors[i].b, 0f);
                }
            }
        }

        // Set initial alpha to 0 for buttons
        foreach (var button in buttons)
        {
            if (button != null)
            {
                var color = button.color;
                button.color = new Color(color.r, color.g, color.b, 0f);
            }
        }

        // Start the fade-in sequence
        StartCoroutine(FadeInAfterDelay());
    }

    private IEnumerator FadeInAfterDelay()
    {
        // Wait for the specified delay before starting the fade
        yield return new WaitForSeconds(timeBeforeFade);

        float elapsedTime = 0f;

        // Gradually increase the alpha value from 0 to 1 for all UI elements
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeTime);

            // Fade buttons to 100% opacity
            SetAlphaForButtons(alpha);
            // Fade sliders to their original alpha values
            SetAlphaForSliders(alpha);

            yield return null;
        }

        // Ensure all UI elements reach their final alpha values
        SetAlphaForButtons(1f);
        SetAlphaForSliders(1f);
    }

    private void SetAlphaForButtons(float alpha)
    {
        foreach (var button in buttons)
        {
            if (button != null)
            {
                var color = button.color;
                button.color = new Color(color.r, color.g, color.b, alpha);
            }
        }
    }

    private void SetAlphaForSliders(float alpha)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] != null)
            {
                var backgroundGraphic = sliders[i].GetComponentsInChildren<Graphic>()[0];
                var fillGraphic = sliders[i].GetComponentsInChildren<Graphic>()[1];

                if (backgroundGraphic != null)
                {
                    backgroundGraphic.color = new Color(
                        sliderBackgroundOriginalColors[i].r,
                        sliderBackgroundOriginalColors[i].g,
                        sliderBackgroundOriginalColors[i].b,
                        Mathf.Lerp(0f, sliderBackgroundTargetColors[i].a, alpha)
                    );
                }

                if (fillGraphic != null)
                {
                    fillGraphic.color = new Color(
                        sliderFillOriginalColors[i].r,
                        sliderFillOriginalColors[i].g,
                        sliderFillOriginalColors[i].b,
                        Mathf.Lerp(0f, sliderFillTargetColors[i].a, alpha)
                    );
                }
            }
        }
    }
}