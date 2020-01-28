﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public SoundManager sound;

    //public AudioPlayer audPlay;

    public MeshRenderer mesh;
    public Collider coll;
    public Rigidbody rb;

    public float timeToExplode;
    public float timeCounter;
    public bool exploded;

    public AudioSource source;
    public AudioClip explosionClip;

    public float radius;
    public LayerMask layer;

    public Vector3 finalPos;

    public void Start()
    {
        sound = GameObject.FindGameObjectWithTag("Managers").GetComponent<SoundManager>();
    }

    public void Update()
    {
        if (!exploded)
        {
            if (timeCounter >= timeToExplode)
            {
                Explode();
                timeCounter = 0;
                exploded = true;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void Explode()
    {
        finalPos = transform.position;

        //Get Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        source.PlayOneShot(explosionClip);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {
                EnemyTest enemy = nearbyObject.GetComponent<EnemyTest>();

                enemy.positionWhereSound = finalPos;

                Debug.Log("Vector Set");
            }
        }

        //Remove Granade
        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        //Destroy(gameObject, 5);
    }
}
