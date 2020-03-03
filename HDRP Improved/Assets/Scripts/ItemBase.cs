using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public enum ItemType { Item, Ammo, Gun};
    public ItemType type;

    public GameObject[] gunVisual;

    public PlayerController player;
    public HudManager HUD;

    public int indexValue;
    public int bullets;
    public bool ammo;
    public AudioClip collectSound;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();

        if (type == ItemType.Gun)
        {
            gunVisual[indexValue].SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HUD.actualItem = gameObject;
            HUD.whatItem = indexValue;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (HUD.actualItem == null)
            {
                HUD.actualItem = gameObject;
                HUD.whatItem = indexValue;
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
