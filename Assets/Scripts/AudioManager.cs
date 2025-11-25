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

        // 🛑 LÓGICA HÍBRIDA
        if (s.type == SoundType.SFX)
        {
            // Usamos la solución que YA SABEMOS que funciona: PlayClipAtPoint
            AudioSource.PlayClipAtPoint(s.clip, Vector3.zero, s.volume);
            //Debug.Log($"Éxito: SFX {name} reproducido con PlayClipAtPoint.");
        }
        else if (s.type == SoundType.Music)
        {
            // Usamos el AudioSource mapeado manualmente (ideal para bucles y control)
            if (s.source != null)
            {
                // Aseguramos que el clip y volumen se asignen por si acaso
                s.source.clip = s.clip; 
                s.source.volume = s.volume;
                s.source.Play(); 
                //Debug.Log($"Éxito: Música {name} reproducida con AudioSource fijo.");
            }
            else
            {
                //Debug.LogError($"AudioSource para la música '{name}' es NULL.");
            }
        }
    }

    // Nota: El método Stop() solo funcionará para la música (SoundType.Music)
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null)
        {
            s.source.Stop();
        }
    }
}