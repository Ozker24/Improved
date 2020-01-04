using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    [Header("Dependences")]
    [SerializeField] EnemyTest enemy;
    [SerializeField] PlayerController player;
    [SerializeField] SphereCollider playerDetector;
    [SerializeField] StealthSystem stealth;

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

    [Header("Sound Detector Variables")]
    public float distanceToDetectSound;

    public void Initialize()
    {
        enemy = GetComponentInParent<EnemyTest>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerDetector = GetComponent<SphereCollider>();
        stealth = player.GetComponent<StealthSystem>();
        timeToRest = initialRestValue;
        playerDetector.radius = radiusOfDetection;

        if (stealth.maxTimeOutVolume != timeToDetect)
        {
            stealth.maxTimeOutVolume = timeToDetect;
        }
    }

    public void MyUpdate()
    {
        if (player.stealth.importantAudio)
        {
            CalculateDistanceSound();
        }

        if (canRest)
        {
            RestStealth();
        }
    }

    public void CountTimeToDetect()
    {
        if (!enemy.Detected)
        {
            if (timeCounter >= timeToDetect)
            {
                enemy.Detected = true;
                stealth.DetectedSound();
                timeCounter = 0;
                stealth.beeingDetected = false;
            }
            else
            {
                timeCounter += Time.deltaTime;
                stealth.beeingDetected = true;
            }
        }
    }

    public void RestStealth()
    {
        if (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime / timeToDetect;
            stealth.beeingDetected = false;
        }
        else
        {
            timeRestCounter = 0;
            stealth.beeingDetected = false;
            canRest = false;
            timeCounter = 0;
        }

    }

    public void CalculateDistanceSound()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= player.stealth.actualSoundDistance)
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
        if (other.tag == "Player")
        {
            if (player.stealth.canBeDetected)
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

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onArea = false;
            finalRestValue = initialRestValue;
            canRest = true;
        }
    }
}
