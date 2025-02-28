using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic; // Музыка для сцены

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; // Бесконечный повтор
            audioSource.volume = 0.5f; // Громкость (можно изменить)
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
