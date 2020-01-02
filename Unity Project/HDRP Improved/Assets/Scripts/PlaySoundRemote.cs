using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundRemote : MonoBehaviour
{
    public AudioArray selectionClips;
    public AudioSource source;

    public void PlayChangeItemSound(int index)
    {
        source.PlayOneShot(selectionClips.clips[index]);
    }
}
