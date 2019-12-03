using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    [Header("Dependences")]
    [SerializeField] EnemyTest enemy;
    [SerializeField] PlayerController player;
    [SerializeField] SphereCollider playerDetector;

    [Header("Detector Variables")]
    public float timeCounter;
    public float timeToDetect;
    public float timeRestCounter;
    public float timeToRest;
    public float initialRestValue;
    public float finalRestValue;
    public float restValue;
    public float radius;
    public bool onArea;

    public void Initialize()
    {
        enemy = GetComponentInParent<EnemyTest>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerDetector = GetComponent<SphereCollider>();
        timeToRest = initialRestValue;
    }

    public void MyUpdate()
    {
        playerDetector.radius = radius;
    }

    public void CountTimeToDetect()
    {
        if (!enemy.Detected)
        {
            if (timeCounter >= timeToDetect)
            {
                enemy.Detected = true;
                timeCounter = 0;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void RestStealth()
    {
        if (timeCounter > 0)
        {
            if (timeRestCounter >= timeToRest)
            {
                timeToRest = finalRestValue;
                timeCounter -= restValue;
                timeRestCounter = 0;
            }
            else
            {
                timeRestCounter += Time.deltaTime;
            }
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
            }
            else
            {
                RestStealth();
            }

            if (timeCounter < 0)
            {
                timeCounter = 0;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onArea = false;
            restValue = initialRestValue;
            timeCounter = 0;
        }
    }
}
