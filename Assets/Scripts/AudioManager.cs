using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        foreach(Sound s in sfxSounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = false;
        }

        foreach (Sound s in musicSounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = true;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        s.audioSource.PlayOneShot(s.clip);
    }

    public void PlayTheme(string name)
    {
        Sound s = Array.Find(musicSounds, sound => sound.name == name);
        s.audioSource.Play();
    }

}
