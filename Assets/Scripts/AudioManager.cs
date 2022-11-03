using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;
    private bool masterVolumeOn = true;

    // Start is called before the first frame update
    void Start()
    {
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

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        PlayClip("MenuBGM");
    }

    void Update()
    {
        if (!masterVolumeOn)
        {
            foreach (Sound sound in sounds)
            {
                sound.source.mute = true;
            }
        }
        else
        {
            foreach (Sound sound in sounds)
            {
                sound.source.mute = false;
            }
        }
    }

    public void PlayClip(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.soundName == name) sound.source.Play();
        }
    }

    public void Toggle()
    {
        masterVolumeOn = !masterVolumeOn;
    }


    public void PauseClip(string name) {
        foreach (Sound sound in sounds)
        {
            if (sound.soundName == name) sound.source.Pause();
        }  
    }

    public void StopClip(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.soundName == name) sound.source.Stop();
        }
    }

    public bool IsPlaying(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.soundName == name) return sound.source.isPlaying;
        }
        return false;
    }
}
