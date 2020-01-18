using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Dependances")]
    [SerializeField] PlayerController player;
    public AudioSource stealthAudioSource;
    public AudioSource detectedAudioSource;

    [Header("Can Be Detected")]
    public bool canBeDetected = false;
    public bool importantAudio;
    public float actualSoundDistance;
    //public bool beeingDetected;

    [Header("Detecting By Sound")]
    public float timeToDetect;
    public float volume;
    public float maxVolume;
    public float maxTimeInVolume;
    public float maxTimeOutVolume;
    public bool playingDetectingSound;
    public bool detected;

    [Header("Detecting By Sight")]
    public Transform sightFinalRayPos;

    [Header("Detecting Sound")]
    public AudioClip detectedSound;

    [Header("Enemy List")]
    public List<EnemyTest> enemies = new List<EnemyTest>();

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        maxTimeInVolume = timeToDetect / 4;
        maxTimeOutVolume = timeToDetect;
    }

    public void MyUpdate()
    {
        DetectAction();
        if (!detected)
        {
            DetectingSound();
        }
    }

    public void DetectAction()
    {
        if (player.walking | player.running && !player.crouching)
        {
            canBeDetected = true;
        }

        else if (!player.moving)
        {
            canBeDetected = false;
        }

        else if (player.crouching && !player.walking && !player.running)
        {
            canBeDetected = false;
        }
    }

    public void MakeImportantAudio (float distance)
    {
        importantAudio = true;
        actualSoundDistance = distance;
        StartCoroutine(ImportantAudioFalse());
    }

    public void DetectingSound()
    {
        if (!playingDetectingSound && enemies.Count > 0)
        {
            stealthAudioSource.Play();
            Debug.Log("Play");
            playingDetectingSound = true;
        }

        if (enemies.Count > 0)
        {
            if (stealthAudioSource.volume < maxVolume)
            {
                stealthAudioSource.volume += Time.deltaTime / maxTimeInVolume; //volume / maxTime = tarde x segundos respecto lo que ha de recorrer
            }
            else
            {
                stealthAudioSource.volume = maxVolume;
            }
        }
        else
        {
            if (stealthAudioSource.volume > 0)
            {
                stealthAudioSource.volume -= Time.deltaTime / maxTimeOutVolume;
            }
            else
            {
                stealthAudioSource.volume = 0;
                stealthAudioSource.Stop();
                playingDetectingSound = false;
            }
        }
    }

    public void DetectedSound()
    {
        if (!detected)
        {
            stealthAudioSource.Stop();
            stealthAudioSource.volume = 0;
            player.CollectItemSource.PlayOneShot(detectedSound);
            detected = true;
        }
    }

    IEnumerator ImportantAudioFalse()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        importantAudio = false;
    }
}
