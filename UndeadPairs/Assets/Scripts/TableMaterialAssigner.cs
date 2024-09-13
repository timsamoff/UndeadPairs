using UnityEngine;

public class TableMaterialAssigner : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private Renderer planeRenderer;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();

        if (materials.Length > 0)
        {
            Material randomMaterial = materials[Random.Range(0, materials.Length)];

            planeRenderer.material = randomMaterial;
        }
        else
        {
            Debug.LogWarning("No materials assigned to the array.");
        }
    }
}