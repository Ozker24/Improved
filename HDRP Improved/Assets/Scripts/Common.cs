using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common 
{
    public static Common common;
    [SerializeField] float maxVolume;
    [SerializeField] float minVolume;
    [SerializeField] float maxPitch;
    [SerializeField] float minPitch;

    public void ChangePitchAndVolume(AudioSource ComSource)
    {
        ComSource.volume = Random.Range(minVolume, maxVolume);
        ComSource.pitch = Random.Range(minPitch, maxPitch);
    }
}
