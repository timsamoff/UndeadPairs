using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Settings")]
    public float fadeTime = 2.0f; // Time to fade in/out the music
    [SerializeField] private AudioClip[] musicClips;

    private AudioSource backgroundMusic;

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("Music_Toggle_State", 1) == 1)
        {
            PlayMusic();
            StartCoroutine(FadeInMusic());
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    public void PlayMusic()
    {
        // Randomly select an audio clip
        if (musicClips.Length > 0)
        {
            AudioClip selectedClip = musicClips[Random.Range(0, musicClips.Length)];
            backgroundMusic.clip = selectedClip;
            backgroundMusic.Play();
        }
    }

    public IEnumerator FadeInMusic()
    {
        float currentTime = 0f;
        backgroundMusic.volume = 0f; // Music starts at 0

        if (!backgroundMusic.isPlaying)
        {
            PlayMusic(); // Start playing the music if it's not already playing
        }

        // Fade in the music over time
        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;
            backgroundMusic.volume = Mathf.Lerp(0f, 1f, currentTime / fadeTime);
            yield return null;
        }

        backgroundMusic.volume = 1f; // Set music to full volume when done
    }

    public IEnumerator FadeOutMusic()
    {
        Debug.Log("Fading out music");

        if (!backgroundMusic.isPlaying)
        {
            Debug.LogWarning("AudioSource is already stopped!");
            yield break;
        }

        float currentTime = 0f;
        float startVolume = backgroundMusic.volume;

        // Fade out music
        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
            yield return null;
        }

        backgroundMusic.volume = 0f; // Volume set to 0
        backgroundMusic.Stop(); // Stop music after fading out
    }
}