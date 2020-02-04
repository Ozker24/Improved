﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask layer;

    [SerializeField] float nearestDistance = 9999999999;
    [SerializeField] float constantDistToEnemy;
    [SerializeField] float distToReset;
    [SerializeField] EnemyTest nearestEnemy;
    [SerializeField] float mergeOfReset;

    private void Update()
    {
        if (nearestEnemy != null)
        {
            constantDistToEnemy = Vector3.Distance(gameObject.transform.position, nearestEnemy.transform.position);
        }

        DetectNearestAbsorb();
        ResetNearestEnemy();

        CheckReset();
    }

    void DetectNearestAbsorb()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearByObject in  colliders)
        {
            if (nearByObject.tag == "Enemy")
            {
                EnemyTest enemy = nearByObject.GetComponent<EnemyTest>();

                if (enemy.dead)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }
        }
    }

    void ResetNearestEnemy ()
    {
        if (nearestEnemy != null)
        {
            if (Vector3.Distance (transform.position, nearestEnemy.transform.position) >= nearestDistance + mergeOfReset)
            {
                nearestDistance = Mathf.Infinity;
            }
        }
    }

    void CheckReset()
    {
        if (constantDistToEnemy >= distToReset)
        {
            nearestDistance = Mathf.Infinity;
            nearestEnemy = null;
            constantDistToEnemy = 0;
        }
    }
}
