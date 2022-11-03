using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector] public AudioSource source;
    public string soundName;
    public AudioClip clip;

    [Range(0f, 15f)] public float volume;
    [Range(0.1f, 5f)] public float pitch;
    
    public bool loop;

}
