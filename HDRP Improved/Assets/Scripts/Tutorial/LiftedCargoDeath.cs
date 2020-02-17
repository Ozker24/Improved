using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftedCargoDeath : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            player.Damage(player.life.health);
        }
    }
}
