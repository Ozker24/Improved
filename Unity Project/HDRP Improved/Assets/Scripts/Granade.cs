using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public bool startExplode;
    public float timeToExplode;
    public float timeCounter;
    public bool exploded;

    public float radius;
    public float force;
    public LayerMask layer;

    public AudioPlayer audPlay;

    public MeshRenderer mesh;
    public Collider coll;
    public Rigidbody rb;

    public GameObject explosionEffect;
    public ParticleSystem grenadeParticle;
    public ParticleSystem.Particle[] grenadePart;

    public void Start()
    {
        grenadePart = new ParticleSystem.Particle[grenadeParticle.main.maxParticles];
    }

    public void Update()
    {
        if(startExplode)
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
            //Add Force to objects
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if (nearbyObject.tag == "Enemy")
            {
                EnemyTest enemy = nearbyObject.GetComponent<EnemyTest>();

                //rb.AddExplosionForce(force, transform.position, radius);

                enemy.Damage(enemy.maxLife);
            }
        }
        //Damage enemies

        //Remove Granade
        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        Destroy(gameObject, 5);
    }

    public void OnParticleSystemStopped()
    {
        Debug.Log(grenadePart[0].position);
    }

}
