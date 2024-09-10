/*using UnityEngine;
using System.Collections;

public class CardPlacement : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridColumns = 3;
    [SerializeField] private int gridRows = 2;

    public int GridColumns => gridColumns;
    public int GridRows => gridRows;

    [Header("Card Start Height")]
    [SerializeField] private float cardStartHeight = 10f;

    [Header("Animation Settings")]
    [SerializeField] private Vector2 delayRange = new Vector2(0f, 2f); // Range for random delay before falling
    [SerializeField] private Vector2 bounceAmountRange = new Vector2(0.02f, 0.1f); // Range for how high the object bounces
    [SerializeField] private float fallSpeed = 1.0f; // Adjust the fall speed in the inspector

    [Header("Bounce Settings")]
    [SerializeField] private float bounceSpeed = 0.1f; // Speed of the bounce (higher value = quicker bounce)

    // Rotation amount range for both X and Z axes (in degrees)
    [SerializeField] private Vector2 rotationRange = new Vector2(0f, 1f);

    [Header("For Testing")]
    // Checkboxes to enable/disable X and Z axis rotation
    [SerializeField] private bool enableXRotation = true;
    [SerializeField] private bool enableZRotation = true;

    private void Start()
    {
        // Disable cards at start
        foreach (Transform child in transform)
        {
            //child.gameObject.SetActive(false);
        }

        // Card drop
        foreach (Transform child in transform)
        {
            StartCoroutine(FallAndBounce(child));
        }
    }

    private IEnumerator FallAndBounce(Transform prefabParent)
    {
        // Save the original position and rotation of the empty parent object
        Vector3 originalPosition = prefabParent.position;
        Quaternion originalRotation = prefabParent.rotation;

        // Where the cards will start falling from
        Vector3 startPosition = new Vector3(originalPosition.x, Camera.main.transform.position.y + cardStartHeight, originalPosition.z);
        prefabParent.position = startPosition;

        // Random delay before the cards start falling
        float delay = Random.Range(delayRange.x, delayRange.y);
        yield return new WaitForSeconds(delay);

        // Enable the cards
        prefabParent.gameObject.SetActive(true);

        // Drop duration (based on fallSpeed)
        float elapsedTime = 0;
        Vector3 targetPosition = originalPosition;

        // Drop the cards to their original starting positions
        while (elapsedTime < fallSpeed)
        {
            prefabParent.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / fallSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        prefabParent.position = targetPosition;

        // Bounce the card
        float bounceAmount = Random.Range(bounceAmountRange.x, bounceAmountRange.y);
        Vector3 bounceUp = new Vector3(originalPosition.x, originalPosition.y + bounceAmount, originalPosition.z);

        // Randomize the rotation amounts for X and Z axes
        float randomRotationX = Random.Range(rotationRange.x, rotationRange.y);
        float randomRotationZ = Random.Range(rotationRange.x, rotationRange.y);

        // Random positive or negative rotation amounts during bounce
        float randomRotationDirectionX = enableXRotation && Random.value > 0.5f ? 1f : -1f;
        float randomRotationDirectionZ = enableZRotation && Random.value > 0.5f ? 1f : -1f;

        // Apply the rotation to the cards on bounce
        float actualRotationXAmount = enableXRotation ? randomRotationX * randomRotationDirectionX : 0f;
        float actualRotationZAmount = enableZRotation ? randomRotationZ * randomRotationDirectionZ : 0f;

        // Bounce and rotation
        elapsedTime = 0;
        while (elapsedTime < bounceSpeed)
        {
            prefabParent.position = Vector3.Lerp(originalPosition, bounceUp, elapsedTime / bounceSpeed);
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
            prefabParent.position = Vector3.Lerp(bounceUp, originalPosition, elapsedTime / bounceSpeed);
            float lerpValue = elapsedTime / bounceSpeed;
            Quaternion targetRotation = Quaternion.Euler(
                originalRotation.eulerAngles.x + (enableXRotation ? Mathf.Lerp(actualRotationXAmount, 0, lerpValue) : 0f),
                originalRotation.eulerAngles.y,
                originalRotation.eulerAngles.z + (enableZRotation ? Mathf.Lerp(actualRotationZAmount, 0, lerpValue) : 0f));
            prefabParent.rotation = targetRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the cards return to their original positions after bouncing
        prefabParent.position = originalPosition;
        prefabParent.rotation = originalRotation;
    }
}*/