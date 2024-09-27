using UnityEngine;

public class EndGameAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] loseGameAudio;
    [SerializeField] private AudioClip[] winGameAudio;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayLoseAudio()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            if (loseGameAudio.Length == 0)
            {
                Debug.LogWarning("No audio clips assigned!");
                return;
            }

            AudioClip randomClip = loseGameAudio[Random.Range(0, loseGameAudio.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }

    public void PlayWinAudio()
    {
        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            if (winGameAudio.Length == 0)
            {
                Debug.LogWarning("No audio clips assigned!");
                return;
            }

            AudioClip randomClip = winGameAudio[Random.Range(0, loseGameAudio.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }
}