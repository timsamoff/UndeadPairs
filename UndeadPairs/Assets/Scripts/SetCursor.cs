using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;
    private Vector2 defaultCursorHotspot;

    // Layer for clickable objects
    [SerializeField] private LayerMask clickableLayer;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        defaultCursorHotspot = new Vector2(defaultCursorTexture.width / 2, defaultCursorTexture.height / 2);
        Cursor.SetCursor(defaultCursorTexture, defaultCursorHotspot, CursorMode.ForceSoftware); // Set default cursor initially
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
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(defaultCursorTexture, defaultCursorHotspot, CursorMode.ForceSoftware);
        }
    }
}