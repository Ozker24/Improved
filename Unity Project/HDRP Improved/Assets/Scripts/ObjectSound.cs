using System.Collections;
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

    public float radius;
    public LayerMask layer;

    public Vector3 finalPos;

    public void Start()
    {
        sound = GameObject.FindGameObjectWithTag("Managers").GetComponent<SoundManager>();
    }

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
        finalPos = transform.position;

        //Get Objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Enemy")
            {
                EnemyTest enemy = nearbyObject.GetComponent<EnemyTest>();

                enemy.positionWhereSound = finalPos;
            }
        }

        //Remove Granade
        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        //Destroy(gameObject, 5);
    }
}
