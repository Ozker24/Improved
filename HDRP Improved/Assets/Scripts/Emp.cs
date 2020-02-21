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
    public LayerMask layer;

    public AudioSource source;
    public AudioClip explosionClip;

    [Header ("When Explode")]
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

        GameObject EMPExplosion = Instantiate(explosionEffect, transform.position, transform.rotation);

        //Debug.Log(source + "AAAAAAAAAAAAAAAAAAAAA");

        //Common.common.ChangePitchAndVolume(source);
        source.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
        source.PlayOneShot(explosionClip);

        Destroy(EMPExplosion, 10);

        //Get Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {
                nearbyObject.transform.SendMessage("StunnedSet", empTime, SendMessageOptions.RequireReceiver);
                Debug.Log("Enemy");
            }
            
            else if (nearbyObject.tag == "Interruptor")
            {
                Interruptor interruptor = nearbyObject.GetComponentInParent<Interruptor>();

                if (interruptor != null)
                {
                    interruptor.DoInteraction();
                }
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
