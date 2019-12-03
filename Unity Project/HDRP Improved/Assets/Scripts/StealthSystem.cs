using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [Header("Dependances")]
    [SerializeField] PlayerController player;

    [Header("Can Be Detected")]
    public bool canBeDetected = false;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void MyUpdate()
    {
        DetectAction();
    }

    public void DetectAction()
    {
        if (player.walking | player.running && !player.crouching)
        {
            canBeDetected = true;
        }

        else if (!player.moving)
        {
            canBeDetected = false;
        }

        else if (player.crouching && !player.walking && !player.running)
        {
            canBeDetected = false;
        }
    }
}
