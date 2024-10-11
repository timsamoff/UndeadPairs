using UnityEngine;

public class TableMaterialAssigner : MonoBehaviour
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

        // Stretch the plane proportionately to fit the screen
        StretchPlaneToFitScreen();
    }

    private void StretchPlaneToFitScreen()
    {
        Camera mainCamera = Camera.main;

        // Get the screen aspect ratio
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Define the original size of the plane
        float planeOriginalWidth = 6.0f;   // X-axis (original width)
        float planeOriginalHeight = 3.5f;  // Z-axis (original height)

        // Calculate the distance from the camera to the plane
        float distance = Mathf.Abs(mainCamera.transform.position.y - transform.position.y);

        float screenWorldHeight = 2f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float screenWorldWidth = screenWorldHeight * screenAspect;

        float widthScaleFactor = screenWorldWidth / planeOriginalWidth;
        float heightScaleFactor = screenWorldHeight / planeOriginalHeight;

        float scaleFactor = Mathf.Min(widthScaleFactor, heightScaleFactor, 1f);

        transform.localScale = new Vector3(
            planeOriginalWidth * scaleFactor,
            transform.localScale.y,
            planeOriginalHeight * scaleFactor
        );
    }
}