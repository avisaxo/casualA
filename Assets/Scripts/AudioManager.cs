using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("Sound not found or Clip is null: " + name);
            return;
        }

        if (s.type == SoundType.SFX)
        {
            AudioSource.PlayClipAtPoint(s.clip, Vector3.zero, s.volume);
        }
        else if (s.type == SoundType.Music)
        {
            if (s.source != null)
            {
                s.source.clip = s.clip; 
                s.source.volume = s.volume;
                s.source.Play(); 
            }
            else
            {
                //Debug.LogError($"AudioSource para la música '{name}' es NULL.");
            }
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null)
        {
            s.source.Stop();
        }
    }
}