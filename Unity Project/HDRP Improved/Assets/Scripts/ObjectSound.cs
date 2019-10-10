using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public SoundManager sound;

    public AudioPlayer audPlay;

    public MeshRenderer mesh;
    public Collider coll;
    public Rigidbody rb;

    public void Start()
    {
        sound = GameObject.FindGameObjectWithTag("Managers").GetComponent<SoundManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        sound.soundPosition = transform.position;

        audPlay.Play(Random.Range(0 , 2), 1, 1);

        coll.enabled = false;
        rb.detectCollisions = false;
        mesh.enabled = false;

        Destroy(gameObject, 5);
    }
}
