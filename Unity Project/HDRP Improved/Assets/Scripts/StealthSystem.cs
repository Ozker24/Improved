using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Dependances")]
    [SerializeField] PlayerController player;

    [Header("Time Values")]
    public float timeCounter;
    public float limitTime;
    public float limitTimeToRest;
    //public float walkLimitTime;

    [Header("Stealth")]
    public float initialStealth;
    public float stealthIndicator;

    [Header("Stealth Values")]
    public float addStealth;
    public float walkValue;
    public float runValue;
    public float restValue;
    public float enemyDetection;
    public bool resting;


    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        stealthIndicator = initialStealth;
    }

    public void MyUpdate()
    {
        /*if (player.moving)
        {
            DetectAction();

            if (resting)
            {
                timeCounter = 0;
                resting = false;
            }

            if (stealthIndicator < enemyDetection)
            {
                AddStealth();
            }

            else if (stealthIndicator >= enemyDetection)
            {
                stealthIndicator = enemyDetection;
            }
        }
        else
        {
            if (stealthIndicator > initialStealth)
            {
                if (!resting)
                {
                    timeCounter = 0;
                    resting = true;
                }

                RestStealth();
            }
            else if (stealthIndicator <= initialStealth)
            {
                stealthIndicator = initialStealth;
            }
        }*/
    }

    public void DetectAction()
    {
        if (player.walking)
        {
            addStealth = walkValue;
        }

        else if (player.running)
        {
            addStealth = runValue;
        }

        else if  (player.crouching)
        {

        }
    }

    public void AddStealth()
    {
        if (timeCounter >= limitTime)
        {
            timeCounter = 0;
            stealthIndicator += addStealth;
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }

    public void RestStealth()
    {
        if (timeCounter >= limitTimeToRest)
        {
            timeCounter = 0;
            stealthIndicator -= restValue;
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }
}
