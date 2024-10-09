using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardFlip : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Vector2 bounceAmountRange = new Vector2(0.02f, 0.1f);
    [SerializeField] private float bounceSpeed = 0.1f;
    // [SerializeField] private float liftAmount = 0.5f;
    [SerializeField] private float liftSpeed = 0.1f;
    [SerializeField] private float flipSpeed = 0.1f;
    [SerializeField] private float endFunctionalityDelay = 0.5f;

    private static bool isAllAnimationsComplete = true;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] zombieGroan;
    [SerializeField] private AudioClip[] cardFlip;
    [SerializeField] private AudioClip[] gunBlast;
    private AudioSource audioSource;

    private bool isFlipped = false;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 screenCenter;
    private bool isAnimating = false;
    private Vector2Int gridPosition;

    private static List<CardFlip> flippedCards = new List<CardFlip>();
    private static int maxFlippedCards = 2;

    private static int totalCardCount;
    private static int matchedCardCount = 0;

    private CardPlacement cardPlacement;

    private MakeSplat makeSplat;

    private LoseHealth loseHealth;

    private AudioControl audioControl;

    private void Start()
    {
        makeSplat = FindObjectOfType<MakeSplat>();

        cardPlacement = GetComponentInParent<CardPlacement>();

        audioControl = FindObjectOfType<AudioControl>();

        matchedCardCount = 0;

        if (cardPlacement == null)
        {
            Debug.LogError("CardPlacement component not found.");
            return;
        }

        originalRotation = transform.rotation;
        screenCenter = CalculateScreenCenter(); // Calculate screen center with offset

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found.");
        }

        loseHealth = FindObjectOfType<LoseHealth>();

        if (loseHealth == null)
        {
            Debug.LogError("LoseHealth component not found.");
        }

        totalCardCount = FindObjectsOfType<CardFlip>().Length;
    }

    private Vector3 CalculateScreenCenter()
    {
        // Apply the verticalCenterOffset
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, (Screen.height / 2) + cardPlacement.VerticalCenterOffset, Camera.main.nearClipPlane));
    }

    private void OnMouseUpAsButton()
    {
        if (!isAnimating && flippedCards.Count < maxFlippedCards)
        {
            Debug.Log("Card clicked: " + gameObject.name);
            // Update the original position to the current position
            originalPosition = transform.position;
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
        isAllAnimationsComplete = false;  // Mark animations as incomplete
        isAnimating = true;

        // Play audio if not muted
        if (cardFlip.Length > 0 && !audioControl.IsSfxMuted())
        {
            AudioClip randomCardFlip = cardFlip[Random.Range(0, cardFlip.Length)];
            audioSource.PlayOneShot(randomCardFlip);
        }

        // Calculate target rotation for flip animation
        float currentZRotation = transform.rotation.eulerAngles.z;
        float targetZRotation = toFlipped ? (currentZRotation + 180f) % 360f : (currentZRotation - 180f + 360f) % 360f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZRotation);

        // Calculate center of the screen in world space
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        screenCenter.y = transform.position.y; // Keep the Y consistent with the card's Y position

        // Calculate the direction vector from the card to the screen center
        Vector3 directionToCenter = (screenCenter - transform.position).normalized;  // Normalize to get the direction
        float distanceToCenter = Vector3.Distance(screenCenter, transform.position);

        // Calculate the nudge amount
        float nudgeAmount = cardPlacement.NudgeHorizAmount;

        // Create nudge vector
        Vector3 nudgePosition = transform.position + directionToCenter * nudgeAmount;

        // Lift position
        nudgePosition.y = transform.position.y + cardPlacement.NudgeVertAmount;

        // Lift and nudge animation
        float elapsedTime = 0;
        while (elapsedTime < liftSpeed)
        {
            transform.position = Vector3.Lerp(transform.position, nudgePosition, elapsedTime / liftSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = nudgePosition;

        // Flip animation
        elapsedTime = 0;
        while (elapsedTime < flipSpeed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime / flipSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        // Return to original position
        Vector3 fallPosition = originalPosition;
        elapsedTime = 0;
        while (elapsedTime < liftSpeed)
        {
            transform.position = Vector3.Lerp(nudgePosition, fallPosition, elapsedTime / liftSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = fallPosition;

        // Bounce animation
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

        yield return new WaitForSeconds(endFunctionalityDelay);

        // Check if all animations are complete
        if (AreAllAnimationsComplete())
        {
            isAllAnimationsComplete = true;
        }

        if (toFlipped)
        {
            if (flippedCards.Count == maxFlippedCards && isAllAnimationsComplete)
            {
                StartCoroutine(CheckForMatch());
            }
        }
    }

    private bool AreAllAnimationsComplete()
    {
        foreach (var card in FindObjectsOfType<CardFlip>())
        {
            if (card.isAnimating)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator CheckForMatch()
    {
        // Wait until all animations are complete
        while (!isAllAnimationsComplete)
        {
            yield return null;
        }

        // Check match logic...
        CardFlip card1 = flippedCards[0];
        CardFlip card2 = flippedCards[1];

        Material material1 = card1.GetCardMaterial();
        Material material2 = card2.GetCardMaterial();

        bool isMatch = material1.name == material2.name;

        if (isMatch)
        {
            Debug.Log("Cards match!");

            matchedCardCount += 2;

            if (matchedCardCount == totalCardCount)
            {
                FindObjectOfType<WinGame>().CheckForWinCondition(matchedCardCount, totalCardCount);
            }

            if (gunBlast.Length > 0 && !audioControl.IsSfxMuted())
            {
                Debug.Log("Playing gun blast sound.");

                AudioClip randomGunBlast = gunBlast[Random.Range(0, gunBlast.Length)];

                AudioControl.Instance.PlayClipAtPosition(randomGunBlast, Camera.main.transform.position);

            }

            if (makeSplat != null)
            {
                makeSplat.SpawnSplat(card1.transform.position);
                makeSplat.SpawnSplat(card2.transform.position);
            }

            Destroy(card1.gameObject);
            Destroy(card2.gameObject);
        }
        else
        {
            Debug.Log("Cards don't match.");

            if (zombieGroan.Length > 0 && !audioControl.IsSfxMuted())
            {
                AudioClip randomZombieGroan = zombieGroan[Random.Range(0, zombieGroan.Length)];
                audioSource.PlayOneShot(randomZombieGroan);
            }

            if (loseHealth != null)
            {
                loseHealth.ReduceHealth();
            }

            card1.FlipCard(false);
            card2.FlipCard(false);
        }

        flippedCards.Clear();
        EnableAllCardColliders();
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

    public static int MatchedCardCount
    {
        get { return matchedCardCount; }
    }
}