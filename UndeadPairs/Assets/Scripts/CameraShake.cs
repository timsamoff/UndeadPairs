using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public enum Waveform { Sine, Square, Triangle }

    [Header("Default Shake Settings")]
    [SerializeField] private Waveform defaultWaveform = Waveform.Sine;
    [SerializeField] private float defaultXShakeAmount = 0.5f;
    [SerializeField] private float defaultZShakeAmount = 0.5f;
    [SerializeField] private float defaultShakeDuration = 0.5f;
    [SerializeField] private float defaultShakeFrequency = 10f;
    [SerializeField] private bool defaultUseSampleAndHold = false;
    [SerializeField] private float defaultSampleHoldInterval = 0.1f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Overload to use default settings
    public void TriggerShake()
    {
        TriggerShake(defaultWaveform, defaultXShakeAmount, defaultZShakeAmount, defaultShakeDuration, defaultShakeFrequency, defaultUseSampleAndHold, defaultSampleHoldInterval);
    }

    // Method that allows passing custom shake parameters
    public void TriggerShake(Waveform waveform, float xShakeAmount, float zShakeAmount, float duration, float frequency, bool useSampleAndHold, float sampleHoldInterval)
    {
        StopAllCoroutines();  // Stop any previous shake to avoid overlaps
        StartCoroutine(Shake(waveform, xShakeAmount, zShakeAmount, duration, frequency, useSampleAndHold, sampleHoldInterval));
    }

    private IEnumerator Shake(Waveform waveform, float xShakeAmount, float zShakeAmount, float duration, float frequency, bool useSampleAndHold, float sampleHoldInterval)
    {
        float elapsedTime = 0f;
        float sampleHoldTime = 0f;  // Tracks time for sample-and-hold

        while (elapsedTime < duration)
        {
            // Check if sample-and-hold is enabled and update the offsets only at each interval
            if (!useSampleAndHold || sampleHoldTime >= sampleHoldInterval)
            {
                // Reset the sample-hold timer
                sampleHoldTime = 0f;

                // Generate random directions for X and Z axes at each interval
                int randomXDirection = Random.value > 0.5f ? 1 : -1;
                int randomZDirection = Random.value > 0.5f ? 1 : -1;

                // Calculate offsets based on the chosen waveform
                float xOffset = 0f;
                float zOffset = 0f;

                switch (waveform)
                {
                    case Waveform.Sine:
                        xOffset = Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2) * xShakeAmount * randomXDirection;
                        zOffset = Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2) * zShakeAmount * randomZDirection;
                        break;
                    case Waveform.Square:
                        xOffset = Mathf.Sign(Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2)) * xShakeAmount * randomXDirection;
                        zOffset = Mathf.Sign(Mathf.Sin(elapsedTime * frequency * Mathf.PI * 2)) * zShakeAmount * randomZDirection;
                        break;
                    case Waveform.Triangle:
                        xOffset = (Mathf.PingPong(elapsedTime * frequency, 1f) - 0.5f) * 2 * xShakeAmount * randomXDirection; // Adjust to ensure full range
                        zOffset = (Mathf.PingPong(elapsedTime * frequency, 1f) - 0.5f) * 2 * zShakeAmount * randomZDirection; // Adjust to ensure full range
                        break;
                }

                // Apply the calculated shake offsets
                transform.localPosition = new Vector3(originalPosition.x + xOffset, originalPosition.y, originalPosition.z + zOffset);
            }

            elapsedTime += Time.deltaTime;
            sampleHoldTime += Time.deltaTime;  // Update sample-and-hold timer
            yield return null;  // Wait for the next frame
        }

        // After shaking, reset the camera to its original position
        transform.localPosition = originalPosition;
    }
}