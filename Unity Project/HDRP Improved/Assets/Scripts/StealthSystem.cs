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
    public float restValue;
    public float walkValue;
    public bool resting;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        stealthIndicator = initialStealth;
    }

    public void MyUpdate()
    {
        if (player.moving)
        {
            if (resting)
            {
                timeCounter = 0;
                resting = false;
            }
            AddStealth();
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
        }
    }

    public void AddStealth()
    {
        if (timeCounter >= limitTime)
        {
            timeCounter = 0;
            stealthIndicator += walkValue;
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
