using UnityEngine;
using UnityEngine.Audio;

public enum SoundType { SFX, Music } // Nuevo enum

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    // 🛑 NUEVA PROPIEDAD
    public SoundType type = SoundType.SFX; // Por defecto es SFX
    
    [Range(0f, 1f)]
    public float volume = 1f;

    // ... (otras propiedades) ...
    
    // Mantenemos la referencia al AudioSource para la MÚSICA
    public AudioSource source; 
}