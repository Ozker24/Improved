using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public PlayerController player;
    public HudManager HUD;

    public int WhichItem;
    public int bullets;
    public int ForWhatGun;
    public bool ammo;
    public AudioClip collectSound;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HUD.actualItem = gameObject;
            HUD.whatItem = WhichItem;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (HUD.actualItem == null)
            {
                HUD.actualItem = gameObject;
                HUD.whatItem = WhichItem;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            HUD.actualItem = null;
            HUD.whatItem = -1;
        }
    }
}
