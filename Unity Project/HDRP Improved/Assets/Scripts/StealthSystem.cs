using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Dependances")]
    [SerializeField] PlayerController player;
    [SerializeField] AudioSource stealthAudioSource;

    [Header("Can Be Detected")]
    public bool canBeDetected = false;
    public bool importantAudio;
    public float actualSoundDistance;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void MyUpdate()
    {
        DetectAction();
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

    IEnumerator ImportantAudioFalse()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        importantAudio = false;
    }
}
