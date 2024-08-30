using UnityEngine;
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
        Debug.Log($"Card clicked: {gameObject.name}, Collider Enabled: {GetComponent<Collider>().enabled}");

        if (!isAnimating)
        {
            FlipCard(!isFlipped);
        }
    }

    public void FlipCard(bool toFlipped)
    {
        if (toFlipped != isFlipped)
        {
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
    }

    private IEnumerator FlipAnimation(bool toFlipped)
    {
        isAnimating = true;

        float currentZRotation = transform.rotation.eulerAngles.z;
        float targetZRotation = toFlipped ? (currentZRotation + 180f) % 360f : (currentZRotation - 180f + 360f) % 360f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZRotation);

        Vector3 nudgeDirection = Vector3.zero;
        float centerX = (cardPlacement.GridColumns - 1) / 2.0f;
        float centerY = (cardPlacement.GridRows - 1) / 2.0f;
        float normalizedX = (gridPosition.x - centerX) / centerX;
        float normalizedY = (gridPosition.y - centerY) / centerY;

        // Reversed nudge directions due to camera rotation
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
    }
}