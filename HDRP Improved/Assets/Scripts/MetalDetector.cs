using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetector : MonoBehaviour
{
    [SerializeField] StealthSystem stealth;
    [SerializeField] float soundEmisionDistance;
    [SerializeField] AudioSource source;

    private void Start()
    {
        stealth = GameObject.FindGameObjectWithTag("Player").GetComponent<StealthSystem>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            stealth.MakeImportantAudio(soundEmisionDistance, gameObject.transform.position);
            if (source.isPlaying)
            {
                source.Stop();
            }
            source.Play();
        }
    }
}
