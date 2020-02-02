﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misil : MonoBehaviour
{
    [Header ("Dependencies")]
    [SerializeField] MisileLauncher launcher;
    [SerializeField] MisileInpact inpact;
    public ConstantForce constForce;
    public Rigidbody rigBod;

    [Header ("Fly")]
    public Vector3 placeToFly;
    public Vector3 flySpeed;
    [SerializeField] float flyTimeCounter;
    [SerializeField] float maxTimeToFly;
    [SerializeField] float tallestPos;
    [SerializeField] float actualTall;
    [SerializeField] float latestTall;
    public float percentageToGround;
    [SerializeField] bool goToPos;
    public bool setPos;

    [Header("Explosion Effects")]
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject explosionArea;
    [SerializeField] GameObject explosionAreaPrefab;

    private void Start()
    {
        constForce = GetComponent<ConstantForce>();
        rigBod = GetComponent<Rigidbody>();
        constForce.relativeForce = flySpeed;
        launcher = GameObject.FindGameObjectWithTag("Improved").GetComponentInChildren<MisileLauncher>();
        placeToFly = launcher.nextMisilePos;

        explosionArea = Instantiate(explosionAreaPrefab, placeToFly, Quaternion.identity);
        inpact = explosionArea.GetComponent<MisileInpact>();
        inpact.misil = GetComponent<Misil>();
    }

    private void Update()
    {
        CountTime();
    }

    private void CountTime()
    {
        if (!goToPos)
        {
            if (flyTimeCounter >= maxTimeToFly)
            {
                flySpeed = Vector3.zero;
                constForce.relativeForce = flySpeed;
                flyTimeCounter = 0;
                goToPos = true;
            }
            else
            {
                flyTimeCounter += Time.deltaTime;
            }
        }
        else
        {
            Vector3.MoveTowards(transform.position, placeToFly, Time.deltaTime);
            Vector3 tallestVect = new Vector3(transform.position.x, tallestPos, transform.position.z);

            if(setPos)
            {
                //distanceToGround = Vector3.Distance(placeToFly, transform.position);
                float float1 = transform.position.y - placeToFly.y;
                float float2 = tallestPos - placeToFly.y;
                percentageToGround = Mathf.Clamp01(float1 / float2);
            }

            actualTall = transform.position.y;

            if (latestTall > actualTall && !setPos)
            {
                tallestPos = latestTall;
                setPos = true;
            }

            latestTall = actualTall;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;
        explosionArea.GetComponent<Renderer>().enabled = false;
    }
}
