using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip songTheme;
    public AudioClip soundtrack;
    public GameObject loadingObject;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (loadingObject.activeSelf)
        {
            PlayClip(songTheme);
        }
        else
        {
            PlayClip(soundtrack);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
