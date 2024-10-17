using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionSfx : MonoBehaviour
{
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip click;
    [SerializeField] private float interactionVolume = 1f;
    private AudioSource audioSource;

    private bool soundOn = true;

    private GameObject lastHoveredElement = null;  // Store the last hovered element

    private Vector3 lastMousePosition;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            soundOn = true;
        }
    }

    private void Update()
    {
        if (Input.mousePosition != lastMousePosition || Input.GetMouseButtonDown(0))
        {
            RaycastForUIElements(); // Only raycast if the mouse moves or on click
            lastMousePosition = Input.mousePosition;
        }
    }

    public void SoundOn(bool enabled)
    {
        soundOn = enabled;
    }

    private void RaycastForUIElements()
    {
        // Check if the mouse is over a UI element
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Raycast the mouse position on UI
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Iterate through the raycast results to find the highest relevant UI element
        GameObject targetElement = null;
        foreach (var result in results)
        {
            GameObject potentialElement = GetRelevantUIElement(result.gameObject);
            if (potentialElement != null)
            {
                targetElement = potentialElement;
                break;  // Stop at the first relevant UI element
            }
        }

        if (targetElement != null)
        {
            // Hovering over a new element
            if (targetElement != lastHoveredElement)
            {
                PlayHoverSound();
                lastHoveredElement = targetElement;
            }

            // Handle Clicks for both button and toggle
            if (Input.GetMouseButtonDown(0))
            {
                PlayClickSound();
            }
        }
        else
        {
            // Reset last hovered element when moving away from UI
            if (lastHoveredElement != null)
            {
                lastHoveredElement = null;
            }
        }
    }


    // Look through hierarchy to find a parent with the correct tag
    private GameObject GetRelevantUIElement(GameObject hoveredElement)
    {
        Transform currentTransform = hoveredElement.transform;

        while (currentTransform != null)
        {
            if (currentTransform.CompareTag("UIButton") || currentTransform.CompareTag("UIToggle"))
            {
                return currentTransform.gameObject;  // Return the first matching parent
            }

            // Move up to the parent
            currentTransform = currentTransform.parent;
        }

        return null;  // No relevant UI element found
    }

    private void PlayHoverSound()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1 && soundOn)
        {
            audioSource.PlayOneShot(hover, interactionVolume);
            Debug.Log("Playing hover sound.");
        }
    }

    private void PlayClickSound()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1 && soundOn)
        {
            audioSource.PlayOneShot(click, interactionVolume);
            Debug.Log("Playing click sound.");
        }
    }
}