using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    public float timeToExplode;
    public float timeCounter;
    public bool exploded;
    public float empTime;

    public float radius;
    //public float force;
    public LayerMask layer;

    public AudioPlayer audPlay;

    public MeshRenderer mesh;
    public Collider coll;
    public Rigidbody rb;

    public GameObject explosionEffect;

    public void Update()
    {
        if (timeCounter >= timeToExplode)
        {
            Explode();
            timeCounter = 0;
            exploded = true;
        }

        if (!exploded)
        {
            timeCounter += Time.deltaTime;
        }
    }

    public void Explode()
    {
        Debug.Log("Booom");

        // Show Effects
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        //audPlay.Play(0, 1, 1);

        //Get Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {
                nearbyObject.transform.SendMessage("StunnedSet", empTime, SendMessageOptions.RequireReceiver);
                Debug.Log("Enemy");
            }
        }
        //Damage enemies

        //Remove Granade
        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        Destroy(gameObject, 5);
    }
}
