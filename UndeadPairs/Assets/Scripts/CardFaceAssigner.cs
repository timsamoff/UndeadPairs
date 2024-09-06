using UnityEngine;
using System.Collections.Generic;

public class CardFaceAssigner : MonoBehaviour
{
    [Header("Materials List")]
    public Material[] materialLibrary;

    private void Start()
    {
        AssignMaterials();
    }

    private void AssignMaterials()
    {
        // Find all card prefabs
        GameObject[] cardPrefabs = GetCardPrefabs();

        // Check if there are enough materials
        int requiredPairs = cardPrefabs.Length / 2;
        if (materialLibrary.Length < requiredPairs)
        {
            Debug.LogError("Not enough materials to assign to all card prefabs.");
            return;
        }

        // Create a list from the material library and shuffle it
        List<Material> shuffledMaterials = new List<Material>(materialLibrary);
        Shuffle(shuffledMaterials);

        // List of pairs of materials
        List<Material> materialPairs = new List<Material>();

        // Pairs of materials, ensuring that each game can use different materials
        for (int i = 0; i < requiredPairs; i++)
        {
            materialPairs.Add(shuffledMaterials[i]);
            materialPairs.Add(shuffledMaterials[i]);
        }

        // Shuffle the material pairs
        Shuffle(materialPairs);

        // Assign materials
        for (int i = 0; i < cardPrefabs.Length; i++)
        {
            var card = cardPrefabs[i];
            var frontPlane = card.transform.Find("CardFront").GetComponent<Renderer>();

            if (frontPlane != null)
            {
                // Assign the material
                frontPlane.material = materialPairs[i];

                // Rotate the texture 180 degrees on the Y axis
                frontPlane.material.mainTextureOffset = new Vector2(1, 0);
                frontPlane.material.mainTextureScale = new Vector2(-1, 1);
            }
            else
            {
                Debug.LogWarning($"CardFront not found in card prefab {card.name}");
            }
        }
    }

    private GameObject[] GetCardPrefabs()
    {
        // Find all of the cards
        Transform[] children = GetComponentsInChildren<Transform>();

        // Collect unique card prefabs
        List<GameObject> cardPrefabs = new List<GameObject>();

        foreach (var child in children)
        {
            if (child.name == "CardFront")
            {
                GameObject parentObj = child.parent.gameObject;

                if (!cardPrefabs.Contains(parentObj))
                {
                    cardPrefabs.Add(parentObj);
                }
            }
        }

        return cardPrefabs.ToArray();
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}