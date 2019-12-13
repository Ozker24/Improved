﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public float rotateForce;
    public float radius;
    public LayerMask layer;
    public AudioPlayer audPlay;

    public MeshRenderer mesh;
    public Collider coll;
    public Rigidbody rb;

    public GameObject fireZonePrefab;

    public void Update()
    {
        Rotate();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public void Rotate()
    {
        //transform.Rotate(1 * rotateForce, 0, 0);
    }

    public void Explode()
    {
        Debug.Log("Booom");

        // Show Effects
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        audPlay.Play(0, 1, 1);

        //Get Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearbyObject in colliders)
        {
            //animacion y particula de quemar

            if (nearbyObject.tag == "Enemy")
            {
                EnemyTest enemy = nearbyObject.GetComponent<EnemyTest>();
                enemy.Damage(enemy.currentLife);
                enemy.dead = true;
                Debug.Log("Killed By Molotov");
            }
            else if (nearbyObject.tag == "Object")
            {
                Destroy(nearbyObject.gameObject, 2f);
            }
        }

        GameObject fireZone = Instantiate(fireZonePrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.Euler(0, 0, 0));

        //Damage enemies

        //Remove Molotov
        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        Destroy(gameObject, 5);
    }
}
