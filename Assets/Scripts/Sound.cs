using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0.00f, 1.00f)]
    public float volume;

    [Range(0.0f, 3.0f)]
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;
}
