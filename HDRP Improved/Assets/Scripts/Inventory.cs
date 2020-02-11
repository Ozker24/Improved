using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject Inv;
    public RectTransform selector;
    public RectTransform selectorGun;
    public float timeCounter = 0;
    public float timeToVanish;
    public WeaponManager weapons;
    public Items items;
    public PlaySoundRemote soundRemote;

    public AudioSource baseSource;
    public AudioClip[] selectionSound;

    public int actualItem = 2;
    public int maxItem = 4;
    public int itemPreSelected = 2;

    public int actualGun;
    public int maxGun;

    public Animator anim;
    public AudioArray selectionSounds;

    public bool startCountdown;
    [SerializeField] bool selectingItem;

    public void Initialize()
    {
        Inv.SetActive(false);
        maxGun = weapons.maxWeapons;
        items = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Items>();
        //anim = GetComponentInChildren<Animator>();
    }

    public void MyUpdate()
    {
        actualGun = weapons.weaponPreSelected;

        if (startCountdown)
        {
            if (timeCounter >= timeToVanish)
            {
                Debug.Log(selectingItem);

                if (items.itemSelected != itemPreSelected)
                {
                    actualItem = itemPreSelected;
                    baseSource.PlayOneShot(selectionSound[actualItem]);
                    Debug.Log("************************Playing Sound");
                }

                Inv.SetActive(false);
                timeCounter = 0;
                startCountdown = false;
                selectingItem = false;

                Debug.Log(items.itemSelected);
                Debug.Log( itemPreSelected);
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void UpInventory()
    {
        if (!items.pressed && !items.player.dodging)
        {
            Inv.SetActive(true);
            startCountdown = true;
            timeCounter = 0;
            selectingItem = true;

            if (itemPreSelected < maxItem)
            {
                itemPreSelected++;
                anim.SetTrigger("Up");
            }
        }
    }

    public void DownInventory()
    {
        if (!items.pressed && !items.player.dodging)
        {
            Inv.SetActive(true);
            startCountdown = true;
            timeCounter = 0;
            selectingItem = true;

            if (itemPreSelected > 0)
            {
                itemPreSelected--;
                anim.SetTrigger("Down");
            }
        }
    }

    public void RightInventory()
    {
        Inv.SetActive(true);
        startCountdown = true;
        timeCounter = 0;

        if (actualGun == 2)
        {
            anim.SetTrigger("JumpLeft"); // estan cambiados de direccion #dislexia
        }

        else if (actualGun > 0 && actualGun != 2)
        {
            anim.SetTrigger("Right");
        }
    }

    public void LefttInventory()
    {
        Inv.SetActive(true);
        startCountdown = true;
        timeCounter = 0;

        if (actualGun == 1)
        {
            anim.SetTrigger("JumpRight"); // estan cambiados de direccion #dislexia
        }

        else if (actualGun < maxGun - 1 && actualGun != 1)
        {
            anim.SetTrigger("Left");
        }
    }

    public void CalculatePosition()
    {
        selector.parent.position = selector.position;
        selector.transform.localPosition = Vector3.zero;
    }

    public void CalculatePositionGun()
    {
        selectorGun.parent.position = selectorGun.position;
        selectorGun.transform.localPosition = Vector3.zero;
    }
}
