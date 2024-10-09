using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [Header("Settings")]

    // Default cursor textures
    [SerializeField] private Texture2D lowResDefaultCursorTexture; // Low res
    [SerializeField] private Texture2D midResDefaultCursorTexture; // Medium res
    [SerializeField] private Texture2D highResDefaultCursorTexture; // High res

    // Hover cursor textures
    [SerializeField] private Texture2D lowResCursorTexture; // Low res
    [SerializeField] private Texture2D midResCursorTexture; // Medium res
    [SerializeField] private Texture2D highResCursorTexture; // High res

    private Vector2 cursorHotspot;
    private Vector2 defaultCursorHotspot;

    // Layer for clickable objects
    [SerializeField] private LayerMask clickableLayer;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SetCursorBasedOnResolution(true); // true for default cursor
    }

    void Update()
    {
        CheckForClickableObject();
    }

    void CheckForClickableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
        {
            // Debug.Log("Hovering over a clickable object. Setting hover cursor.");
            SetCursorBasedOnResolution(false); // Set hover cursor based on resolution
        }
        else
        {
            // Debug.Log("Not hovering over a clickable object. Setting default cursor.");
            SetCursorBasedOnResolution(true); // Set default cursor based on resolution
        }
    }

    void SetCursorBasedOnResolution(bool isDefaultCursor)
    {
        // Determine screen resolution
        int screenWidth = Screen.width;

        Texture2D cursorToSet;
        Vector2 cursorHotspot;

        if (screenWidth <= 1366) // Low res
        {
            cursorToSet = isDefaultCursor ? lowResDefaultCursorTexture : lowResCursorTexture;
        }
        else if (screenWidth <= 1920) // Medium res
        {
            cursorToSet = isDefaultCursor ? midResDefaultCursorTexture : midResCursorTexture;
        }
        else // High res
        {
            cursorToSet = isDefaultCursor ? highResDefaultCursorTexture : highResCursorTexture;
        }

        cursorHotspot = new Vector2(cursorToSet.width / 2, cursorToSet.height / 2);

        // Debug log for the cursor being set
        /*Debug.Log($"Setting cursor: {cursorToSet.name} with hotspot: {cursorHotspot} " +
              $"(Size: {cursorToSet.width}x{cursorToSet.height})");*/

        // Set the cursor
        Cursor.SetCursor(cursorToSet, cursorHotspot, CursorMode.ForceSoftware);
    }
}