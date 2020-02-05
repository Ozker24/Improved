using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptorTrigger : MonoBehaviour
{
    [SerializeField] Interruptor interruptor;

    private void Start()
    {
        interruptor = GetComponentInParent<Interruptor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interruptor.playerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interruptor.playerIn = false;
        }
    }
}
