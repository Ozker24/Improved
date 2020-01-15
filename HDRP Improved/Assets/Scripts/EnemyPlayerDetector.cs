using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    [Header("Dependences")]
    [SerializeField] EnemyTest enemy;
    [SerializeField] SphereCollider playerDetector;

    [Header("Detector Variables")]
    public float timeCounter;
    public float timeToDetect;
    public float timeRestCounter;
    public float timeToRest;
    public float initialRestValue;
    public float finalRestValue;
    public float restValue;
    public bool canRest;
    public float radiusOfDetection;
    public bool onArea;
    public bool hearingPlayer;

    [Header("Sound Detector Variables")]
    public float distanceToDetectSound;

    public void Initialize()
    {
        enemy = GetComponentInParent<EnemyTest>();
        playerDetector = GetComponent<SphereCollider>();
        timeToRest = initialRestValue;
        playerDetector.radius = radiusOfDetection;
        timeToDetect = enemy.stealth.timeToDetect;


        /*if (stealth.maxTimeOutVolume != timeToDetect)
        {
            stealth.maxTimeOutVolume = timeToDetect;
        }*/
    }

    public void MyUpdate()
    {
        if (enemy.stealth.importantAudio)
        {
            CalculateDistanceSound();
        }

        if (canRest)
        {
            RestStealth();
        }

        //SetSendDetectionInfo();
    }

    public void CountTimeToDetect()
    {
        if (!enemy.Detected && !enemy.dead)
        {
            if (timeCounter >= timeToDetect)
            {
                enemy.Detected = true;
                enemy.stealth.DetectedSound();
                timeCounter = 0;
                hearingPlayer = false;
            }
            else
            {
                timeCounter += Time.deltaTime;
                hearingPlayer = true;
            }
        }
    }

    public void RestStealth()
    {
        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime / timeToDetect;
            hearingPlayer = false;
        }
        else
        {
            timeRestCounter = 0;
            hearingPlayer = false;
            canRest = false;
            timeCounter = 0;
        }
    }

   public void SetSendDetectionInfo()
    {
        if (timeCounter > 0)
        {
            enemy.sendDetectionInfo = true;
        }
        else
        {
            enemy.sendDetectionInfo = false;
        }
    }

    public void CalculateDistanceSound()
    {
        if (Vector3.Distance(transform.position, enemy.player.transform.position) <= enemy.stealth.actualSoundDistance)
        {
            enemy.Detected = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onArea = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!enemy.Detected)
        {
            if (other.tag == "Player")
            {
                if (enemy.stealth.canBeDetected)
                {
                    CountTimeToDetect();
                    timeToRest = initialRestValue;
                    canRest = false;
                    timeRestCounter = 0;
                }
                else
                {
                    if (timeCounter > 0)
                    {
                        canRest = true;
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!enemy.Detected)
        {
            if (other.tag == "Player")
            {
                onArea = false;
                finalRestValue = initialRestValue;
                canRest = true;
            }
        }
    }
}
