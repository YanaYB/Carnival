using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic; // ������ ��� �����

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
            audioSource.loop = true; // ����������� ������
            audioSource.volume = 0.5f; // ��������� (����� ��������)
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
