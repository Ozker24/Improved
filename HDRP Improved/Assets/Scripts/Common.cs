using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utiles 
{
    public static void ChangePitchAndVolume(this AudioSource source, float minV, float maxV, float minP, float maxP)
    {
        source.volume = Random.Range(minV, maxV);
        source.pitch = Random.Range(minP, maxP);
    }
}
