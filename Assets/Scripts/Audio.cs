using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance; // Singleton ��� ����������� �������

    [Header("Audio Sources")]
    private AudioSource audioSource;

    [Header("Sound Clips")]
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip gameOverSound;
    public AudioClip walkSound;

    private void Awake()
    {
        // ���������� Singleton, ����� ��� ������ ���� ��������� SoundEffectManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    public void PlayWalkSound()
    {
        audioSource.PlayOneShot(walkSound);
    }

    public void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound);
    }
}
