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

        if (PlayerPrefs.GetInt("SFX_Toggle_State", 1) == 1)
        {
            // Randomly select an audio clip
            if (musicClips.Length > 0)
            {
                AudioClip selectedClip = musicClips[Random.Range(0, musicClips.Length)];
                backgroundMusic.clip = selectedClip;
                backgroundMusic.Play();
            }

            // Fade in music
            backgroundMusic.volume = 0f;
            StartCoroutine(FadeInMusic(backgroundMusic));
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    IEnumerator FadeInMusic(AudioSource audioSource)
    {
        float currentTime = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, currentTime / fadeTime);
            yield return null;
        }

        audioSource.volume = 1f; // Music at full volume when finished fading in
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

        while (currentTime < fadeTime)
        {
            currentTime += Time.unscaledDeltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
            yield return null;
        }

        backgroundMusic.volume = 0f;
        backgroundMusic.Stop();
    }

}