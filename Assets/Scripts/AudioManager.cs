using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateVolume();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void UpdateVolume()
    {
        musicSource.volume = musicVolume;
    }

    public AudioClip CreateReversedClip(AudioClip original)
    {
        int samples = original.samples * original.channels;
        float[] data = new float[samples];
        float[] reversedData = new float[samples];

        original.GetData(data, 0);

        // Reverse the audio sample array
        for (int i = 0; i < samples; i++)
        {
            reversedData[i] = data[samples - 1 - i];
        }

        AudioClip reversedClip = AudioClip.Create(
            original.name + "_Reversed",
            original.samples,
            original.channels,
            original.frequency,
            false
        );

        reversedClip.SetData(reversedData, 0);
        return reversedClip;
    }
}
