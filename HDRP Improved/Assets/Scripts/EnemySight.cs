﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] EnemyTest enemy;

    public float timeCounter;
    public float timeToDetect;
    public bool playerIn;
    public bool watchingPlayer;

    [Header("Check Objects")]
    public Transform initRayPos;
    public Transform FinalRayPos;
    public LayerMask rayLayer;
    public bool noObjectsOccluding;

    public void Initialize()
    {
        enemy = GetComponentInParent<EnemyTest>();
        enemy.stealth = GameObject.FindGameObjectWithTag("Player").GetComponent<StealthSystem>();
        timeToDetect = enemy.stealth.timeToDetect;
        FinalRayPos = enemy.stealth.sightFinalRayPos;
    }

    public void MyUpdate()
    {
        Watching();
        CheckObjects();
        //SetSendDetectionInfo();
    }

    public void Watching()
    {
        if (playerIn && noObjectsOccluding)
        {
            if (timeCounter >= timeToDetect)
            {
                enemy.Detected = true;
                enemy.stealth.DetectedSound();
                enemy.stealth.detected = true;
                watchingPlayer = false;
            }
            else
            {
                timeCounter += Time.deltaTime;
                watchingPlayer = true;
            }
        }

        else
        {
            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
                watchingPlayer = false;
            }
            else
            {
                timeCounter = 0;
            }
        }
    }

    public void CheckObjects()
    {
        if (playerIn)
        {
            RaycastHit hit = new RaycastHit();
            Physics.Linecast(initRayPos.position, FinalRayPos.position, out hit, rayLayer);

            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Player")
            {
                noObjectsOccluding = true;
            }
            else
            {
                noObjectsOccluding = false;
            }
        }
    }

    /*public void SetSendDetectionInfo()
    {
        if (timeCounter > 0)
        {
            enemy.sendDetectionInfo = true;
        }
        else
        {
            enemy.sendDetectionInfo = false;
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (!enemy.Detected && !enemy.dead)
        {
            if (other.tag == "Player")
            {
                playerIn = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!enemy.Detected && !enemy.dead)
        {
            if (other.tag == "Player")
            {
                playerIn = false;
                noObjectsOccluding = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerIn)
        {
            Gizmos.DrawLine(initRayPos.position, FinalRayPos.position);
        }
    }
}
