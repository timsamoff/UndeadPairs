using UnityEngine;

public class TableManager : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private Renderer planeRenderer;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();

        // Assign a random material if materials are assigned
        if (materials.Length > 0)
        {
            Material randomMaterial = materials[Random.Range(0, materials.Length)];
            planeRenderer.material = randomMaterial;
        }
        else
        {
            Debug.LogWarning("No materials assigned to the array.");
        }

        // Stretch plane proportionately to fit screen
        StretchPlaneProportionately();
    }

    private void StretchPlaneProportionately()
    {
        Camera mainCamera = Camera.main;

        // Get the screen aspect ratio
        float screenAspect = (float)Screen.width / (float)Screen.height;

        Vector3 initialScale = transform.localScale;
        float originalWidth = 6f;  // Original X scale (width)
        float originalHeight = 3.5f; // Original Z scale (height)

        float screenWorldHeight = originalHeight; // Keep your original height
        float screenWorldWidth = screenWorldHeight * screenAspect; // Adjust width proportionally to aspect ratio

        float widthScaleFactor = screenWorldWidth / originalWidth;  // Scale factor for X axis (width)
        float heightScaleFactor = screenWorldHeight / originalHeight; // Scale factor for Z axis (height)

        float uniformScaleFactor = Mathf.Min(widthScaleFactor, heightScaleFactor);

        transform.localScale = new Vector3(
            initialScale.x * uniformScaleFactor, // Scale the width (X-axis)
            initialScale.y,                      // Keep the Y scale (depth) unchanged
            initialScale.z * uniformScaleFactor  // Scale the height (Z-axis)
        );
    }
}