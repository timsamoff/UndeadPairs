/* using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardFlip : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Vector2 bounceAmountRange = new Vector2(0.02f, 0.1f);
    [SerializeField] private float bounceSpeed = 0.1f;
    [SerializeField] private float liftAmount = 0.5f;
    [SerializeField] private float liftSpeed = 0.1f;
    [SerializeField] private float flipSpeed = 0.1f;

    private static bool isAnyCardAnimating = false;

    [Header("Move Cards Towards Center When Clicked")]
    [SerializeField] private float nudgeAmount = 0.01f;

    private bool isFlipped = false;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 screenCenter;
    private bool isAnimating = false;
    private Vector2Int gridPosition;

    private static List<CardFlip> flippedCards = new List<CardFlip>();
    private static int maxFlippedCards = 2;

    private CardPlacement cardPlacement;

    private void Start()
    {
        cardPlacement = GetComponentInParent<CardPlacement>();

        if (cardPlacement == null)
        {
            Debug.LogError("CardPlacement component not found in the parent hierarchy.");
            return;
        }

        originalRotation = transform.rotation;
        originalPosition = transform.position;
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        CalculateGridPosition();
    }

    private void CalculateGridPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(originalPosition);
        float gridCellWidth = Screen.width / cardPlacement.GridColumns;
        float gridCellHeight = Screen.height / cardPlacement.GridRows;

        int column = Mathf.FloorToInt(screenPos.x / gridCellWidth);
        int row = Mathf.FloorToInt(screenPos.y / gridCellHeight);

        gridPosition = new Vector2Int(column, row);
    }

    private void OnMouseUpAsButton()
    {
        if (!isAnimating && flippedCards.Count < maxFlippedCards && !isAnyCardAnimating)
        {
            FlipCard(!isFlipped);
        }
    }

    public void FlipCard(bool toFlipped)
    {
        if (toFlipped == isFlipped || isAnimating)
        {
            return;
        }

        if (toFlipped)
        {
            if (flippedCards.Count == 0)
            {
                GetComponent<Collider>().enabled = false;
            }
            else if (flippedCards.Count == 1)
            {
                flippedCards[0].GetComponent<Collider>().enabled = true;
                foreach (var card in FindObjectsOfType<CardFlip>())
                {
                    if (!flippedCards.Contains(card))
                    {
                        card.GetComponent<Collider>().enabled = false;
                    }
                }
                GetComponent<Collider>().enabled = true;
            }

            if (flippedCards.Count < maxFlippedCards)
            {
                flippedCards.Add(this);
            }
            else
            {
                return;
            }
        }
        else
        {
            flippedCards.Remove(this);
            if (flippedCards.Count == 0)
            {
                foreach (var card in FindObjectsOfType<CardFlip>())
                {
                    card.GetComponent<Collider>().enabled = true;
                }
            }
        }

        StopAllCoroutines();
        StartCoroutine(FlipAnimation(toFlipped));
    }

    private IEnumerator FlipAnimation(bool toFlipped)
    {
        isAnyCardAnimating = true;  // Disable all mouse interactions

        isAnimating = true;

        float currentZRotation = transform.rotation.eulerAngles.z;
        float targetZRotation = toFlipped ? (currentZRotation + 180f) % 360f : (currentZRotation - 180f + 360f) % 360f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZRotation);

        Vector3 nudgeDirection = Vector3.zero;
        float centerX = (cardPlacement.GridColumns - 1) / 2.0f;
        float centerY = (cardPlacement.GridRows - 1) / 2.0f;
        float normalizedX = (gridPosition.x - centerX) / centerX;
        float normalizedY = (gridPosition.y - centerY) / centerY;

        nudgeDirection.x = normalizedX * nudgeAmount;
        nudgeDirection.y = normalizedY * nudgeAmount;

        Vector3 nudgePosition = new Vector3(
            transform.position.x + nudgeDirection.x,
            transform.position.y + liftAmount,
            transform.position.z + nudgeDirection.y
        );

        float elapsedTime = 0;
        while (elapsedTime < liftSpeed)
        {
            transform.position = Vector3.Lerp(originalPosition, nudgePosition, elapsedTime / liftSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = nudgePosition;

        elapsedTime = 0;
        while (elapsedTime < flipSpeed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime / flipSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        Vector3 fallPosition = originalPosition;
        elapsedTime = 0;
        while (elapsedTime < liftSpeed)
        {
            transform.position = Vector3.Lerp(nudgePosition, fallPosition, elapsedTime / liftSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = fallPosition;

        float bounceElapsed = 0;
        float bounceHeight = Random.Range(bounceAmountRange.x, bounceAmountRange.y);
        Vector3 bounceStartPosition = transform.position;
        Vector3 bouncePeakPosition = new Vector3(transform.position.x, transform.position.y + bounceHeight, transform.position.z);

        while (bounceElapsed < bounceSpeed)
        {
            transform.position = Vector3.Lerp(bounceStartPosition, bouncePeakPosition, bounceElapsed / bounceSpeed);
            bounceElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = bouncePeakPosition;

        bounceElapsed = 0;
        while (bounceElapsed < bounceSpeed)
        {
            transform.position = Vector3.Lerp(bouncePeakPosition, bounceStartPosition, bounceElapsed / bounceSpeed);
            bounceElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = bounceStartPosition;

        isFlipped = toFlipped;
        isAnimating = false;
        isAnyCardAnimating = false;  // Re-enable mouse interactions

        if (toFlipped)
        {
            if (flippedCards.Count == maxFlippedCards)
            {
                StartCoroutine(CheckForMatch());
            }
        }
    }

    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(0.5f); // Wait for animations to complete

        if (flippedCards.Count != maxFlippedCards)
        {
            Debug.LogWarning("Unexpected number of flipped cards: " + flippedCards.Count);
            yield break;
        }

        CardFlip card1 = flippedCards[0];
        CardFlip card2 = flippedCards[1];

        Material material1 = card1.GetCardMaterial();
        Material material2 = card2.GetCardMaterial();

        // Check if materials are found
        if (material1 == null || material2 == null)
        {
            Debug.LogError("One or both materials are missing.");
            yield break;
        }

        // Compare material names instead of material instances
        bool isMatch = material1.name == material2.name;
        Debug.Log($"Card 1 Material Name: {material1.name}, Card 2 Material Name: {material2.name}");

        if (isMatch)
        {
            Debug.Log("Cards match!");
            Destroy(card1.gameObject);
            Destroy(card2.gameObject);

            // Ensure all remaining cards are clickable again
            EnableAllCardColliders();
        }
        else
        {
            Debug.Log("Cards do not match.");
            card1.FlipCard(false);
            card2.FlipCard(false);

            // Ensure all remaining cards are clickable again
            EnableAllCardColliders();
        }

        flippedCards.Clear();
    }

    private void EnableAllCardColliders()
    {
        foreach (CardFlip card in FindObjectsOfType<CardFlip>())
        {
            card.GetComponent<Collider>().enabled = true;
        }
    }

    private Material GetCardMaterial()
    {
        Transform cardFront = transform.Find("CardFront");
        if (cardFront != null)
        {
            Renderer renderer = cardFront.GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer.material;
            }
        }
        Debug.LogError("CardFront or Renderer not found.");
        return null;
    }
} */