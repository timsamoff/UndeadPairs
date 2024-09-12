using UnityEngine;

public class TableMaterialAssigner : MonoBehaviour
{
    // Array to hold the materials to choose from
    [SerializeField] private Material[] materials;

    // Reference to the plane's Renderer component
    private Renderer planeRenderer;

    void Start()
    {
        // Get the Renderer component of the plane
        planeRenderer = GetComponent<Renderer>();

        // Check if the materials array is not empty
        if (materials.Length > 0)
        {
            // Choose a random material from the array
            Material randomMaterial = materials[Random.Range(0, materials.Length)];

            // Apply the randomly selected material to the plane
            planeRenderer.material = randomMaterial;
        }
        else
        {
            Debug.LogWarning("No materials assigned to the array.");
        }
    }
}