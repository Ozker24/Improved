using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundRemote : MonoBehaviour
{
    public AudioArray selectionClips;

    public void PlayChangeItemSound(int index)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = selectionClips.clips[index];
        source.Play();

        Destroy(source, source.clip.length);
    }
}
