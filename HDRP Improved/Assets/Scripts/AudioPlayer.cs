using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;

    public void Play(int index)
    {
        Play(index, 1.0f);
    }

    public void Play(int index, float volume)
    {
        Play(index, volume, 1.0f);
    }

    public void Play(int index, float volume, float pitch)
    {
        Play(index, volume, pitch, 0.0f);
    }

    public void Play(int index, float volume, float pitch, float SpacialBlend)
    {
        // Crear audiosource
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = clips[index];
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = SpacialBlend;

        // Reproducir audio
        source.Play();

        // Eliminar el source cuando el audio termine de reproducirse
        Destroy(source, clips[index].length);
    }

    public void PlayIgnoringTime(int index, float volume)
    {
        AudioSource.PlayClipAtPoint(clips[index], Vector3.zero, volume);
    }
}
