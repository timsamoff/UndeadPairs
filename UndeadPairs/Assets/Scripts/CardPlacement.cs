using UnityEngine;
using System.Collections;

public class CardPlacement : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private float cardEndHeight = 0.6f;
    public float CardEndHeight => cardEndHeight;

    [Header("Animation Settings")]
    [SerializeField] private Vector2 delayRange = new Vector2(0f, 2f); // Range for random delay before falling
    [SerializeField] private Vector2 bounceAmountRange = new Vector2(0.02f, 0.1f); // Range for how high the object bounces
    [SerializeField] private float fallSpeed = 1.0f;
    [SerializeField] private Vector2 rotationRange = new Vector2(0f, 1f);

    [Header("For Bounce Testing")]
    // Checkboxes to enable/disable X and Z axis rotation
    [SerializeField] private bool enableXRotation = true;
    [SerializeField] private bool enableZRotation = true;

    [Header("Bounce Settings")]
    [Tooltip("Higher value = quicker bounce")]
    [SerializeField] private float bounceSpeed = 0.1f; // Speed of the bounce

    [Header("Nudge Settings")]
    [SerializeField] private float verticalCenterOffset = -200f; // Offset to allow for HUD
    [SerializeField] private float nudgeHorizAmount = 2f;
    [SerializeField] private float nudgeVertAmount = 2f;

    public float VerticalCenterOffset => verticalCenterOffset;
    public float NudgeHorizAmount => nudgeHorizAmount;
    public float NudgeVertAmount => nudgeVertAmount;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] cardThrow;
    private AudioSource audioSource;

    private PauseScreen pauseScreen;

    private AudioControl audioControl;

    private void Start()
    {
        audioControl = FindObjectOfType<AudioControl>();

        pauseScreen = FindObjectOfType<PauseScreen>();

        // Card drop
        foreach (Transform child in transform)
        {
            StartCoroutine(FallAndBounce(child));
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found.");
        }
    }

    private IEnumerator FallAndBounce(Transform prefabParent)
    {
        // Save original position and rotation of empty parent object
        Vector3 originalPosition = prefabParent.position;
        Quaternion originalRotation = prefabParent.rotation;

        // Calculate start height based on card's original position
        float cardStartHeight = originalPosition.y;

        // Where cards will start falling from
        Vector3 startPosition = new Vector3(originalPosition.x, cardStartHeight, originalPosition.z);
        prefabParent.position = startPosition;

        // Random delay before cards start falling
        float delay = Random.Range(delayRange.x, delayRange.y);
        yield return new WaitForSeconds(delay);

        // Drop duration (based on fallSpeed)
        float elapsedTime = 0;
        Vector3 targetPosition = new Vector3(originalPosition.x, cardEndHeight, originalPosition.z);

        if (cardThrow.Length > 0 && !audioControl.IsSfxMuted())
        {
            AudioClip randomCardThrow = cardThrow[Random.Range(0, cardThrow.Length)];
            audioSource.PlayOneShot(randomCardThrow);
        }

        // Drop the cards
        while (elapsedTime < fallSpeed)
        {
            prefabParent.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / fallSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        prefabParent.position = targetPosition;

        // Bounce card
        float bounceAmount = Random.Range(bounceAmountRange.x, bounceAmountRange.y);
        Vector3 bounceUp = new Vector3(originalPosition.x, cardEndHeight + bounceAmount, originalPosition.z);

        // Randomize rotation amounts for X and Z axes
        float randomRotationX = Random.Range(rotationRange.x, rotationRange.y);
        float randomRotationZ = Random.Range(rotationRange.x, rotationRange.y);

        // Random positive or negative rotation amounts during bounce
        float randomRotationDirectionX = enableXRotation && Random.value > 0.5f ? 1f : -1f;
        float randomRotationDirectionZ = enableZRotation && Random.value > 0.5f ? 1f : -1f;

        // Apply rotation to cards on bounce
        float actualRotationXAmount = enableXRotation ? randomRotationX * randomRotationDirectionX : 0f;
        float actualRotationZAmount = enableZRotation ? randomRotationZ * randomRotationDirectionZ : 0f;

        // Bounce and rotate
        elapsedTime = 0;
        while (elapsedTime < bounceSpeed)
        {
            prefabParent.position = Vector3.Lerp(targetPosition, bounceUp, elapsedTime / bounceSpeed);
            float lerpValue = elapsedTime / bounceSpeed;
            Quaternion targetRotation = Quaternion.Euler(
                originalRotation.eulerAngles.x + (enableXRotation ? Mathf.Lerp(0, actualRotationXAmount, lerpValue) : 0f),
                originalRotation.eulerAngles.y,
                originalRotation.eulerAngles.z + (enableZRotation ? Mathf.Lerp(0, actualRotationZAmount, lerpValue) : 0f));
            prefabParent.rotation = targetRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fall back down after bounce
        elapsedTime = 0;
        while (elapsedTime < bounceSpeed)
        {
            prefabParent.position = Vector3.Lerp(bounceUp, targetPosition, elapsedTime / bounceSpeed);
            float lerpValue = elapsedTime / bounceSpeed;
            Quaternion targetRotation = Quaternion.Euler(
                originalRotation.eulerAngles.x + (enableXRotation ? Mathf.Lerp(actualRotationXAmount, 0, lerpValue) : 0f),
                originalRotation.eulerAngles.y,
                originalRotation.eulerAngles.z + (enableZRotation ? Mathf.Lerp(actualRotationZAmount, 0, lerpValue) : 0f));
            prefabParent.rotation = targetRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure cards return to destinations after bounce
        prefabParent.position = targetPosition;
        prefabParent.rotation = originalRotation;
    }
}