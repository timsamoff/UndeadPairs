using System.Collections.Generic;
using UnityEngine;

public class MakeSplat : MonoBehaviour
{
    public GameObject splatPrefab;
    public Material[] splatMaterials;

    private List<Material> materialPool;

    void Start()
    {
        ResetMaterialPool();
    }

    private void ResetMaterialPool()
    {
        materialPool = new List<Material>(splatMaterials);
    }

    public void SpawnSplat(Vector3 position)
    {
        Quaternion correctRotation = Quaternion.Euler(0, 180, 0);

        GameObject splat = Instantiate(splatPrefab, position, correctRotation);

        Renderer renderer = splat.GetComponent<Renderer>();

        if (renderer != null && materialPool.Count > 0)
        {
            int randomIndex = Random.Range(0, materialPool.Count);
            Material randomMaterial = materialPool[randomIndex];

            renderer.material = randomMaterial;

            materialPool.RemoveAt(randomIndex);

            if (materialPool.Count == 0)
            {
                ResetMaterialPool();
            }
        }
        else
        {
            Debug.LogError("Renderer not found.");
        }
    }
}