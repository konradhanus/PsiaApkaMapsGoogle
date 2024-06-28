using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;
    public float fadeOutDuration = 1.0f;
    
    private static AudioSource audioSource;
    private static bool isFadingOut = false;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void OnEnable()
    {
        if (!audioSource.isPlaying && !isFadingOut)
        {
            audioSource.Play();
        }
    }

    void OnDisable()
    {
        if (audioSource.isPlaying && !isFadingOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFadingOut = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        isFadingOut = false;
    }
}
