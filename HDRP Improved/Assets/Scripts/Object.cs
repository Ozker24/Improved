﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public float timeCounter;
    public float timeToBurn;

    public void Update()
    {
        if (timeCounter >= timeToBurn)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "FireZone")
        {
            if (gameObject.tag == "Enemy")
            {
                EnemyTest enemy = GetComponent<EnemyTest>();

                if (enemy.dead)
                {
                    return;
                }
            }

            timeCounter += Time.deltaTime;
        }
    }

    /*public void OnTriggerStay(Collider other)
    {
        if (other.tag == "FireZone")
        {
            timeCounter += Time.deltaTime;
        }
    }*/
}
