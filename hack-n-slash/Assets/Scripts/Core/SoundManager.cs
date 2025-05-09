using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _sound, float volume = 0.5f)
    {
        source.PlayOneShot(_sound, volume);
    }

    public void ChangeSoundVolume(float _change)
    {
        float currentVolume = PlayerPrefs.GetFloat("soundVolume");
        currentVolume += _change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;
        
        source.volume = currentVolume;

        PlayerPrefs.SetFloat("soundVolume", currentVolume);
    }

    public void ChangeMusicVolume(float _change)
    {
        float currentVolume = 1;
        currentVolume += _change;
        musicSource.volume = currentVolume;
    }
}
