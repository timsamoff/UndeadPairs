using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionSfx : MonoBehaviour
{
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip click;
    private AudioSource audioSource;

    private bool soundOn = true;

    private GameObject lastHoveredElement = null;  // Store the last hovered element

    private Vector3 lastMousePosition;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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

        // Hit the UI?
        if (results.Count > 0)
        {
            GameObject hoveredElement = results[0].gameObject;

            // Traverse up the hierarchy to find the button or toggle (checking tags)
            GameObject targetElement = GetRelevantUIElement(hoveredElement);

            if (targetElement != null && targetElement != lastHoveredElement)
            {
                PlayHoverSound();
                lastHoveredElement = targetElement;
            }

            // Handle Clicks for both Button and Toggle
            if (targetElement != null && Input.GetMouseButtonDown(0))
            {
                PlayClickSound();
            }
        }
        else
        {
            // Reset last hovered element
            lastHoveredElement = null;
        }
    }

    // Traverse the hierarchy to find a parent with the correct tag
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
            audioSource.PlayOneShot(hover);
        }
    }

    private void PlayClickSound()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1 && soundOn)
        {
            audioSource.PlayOneShot(click);
        }
    }
}