using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject Inv;
    public RectTransform selector;
    public RectTransform selectorGun;
    public bool startCountdown;
    public float timeCounter = 0;
    public float timeToVanish;
    public WeaponManager weapons;

    public int actualItem = 2;
    public int maxItem = 4;

    public int actualGun;
    public int maxGun;

    public Animator anim;

    public void Initialize()
    {
        Inv.SetActive(false);
        maxGun = weapons.maxWeapons;
        //anim = GetComponentInChildren<Animator>();
    }

    public void MyUpdate()
    {
        actualGun = weapons.WeaponSelected;

        if (startCountdown)
        {
            if (timeCounter >= timeToVanish)
            {
                Inv.SetActive(false);
                timeCounter = 0;
                startCountdown = false;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void UpInventory()
    {
        Inv.SetActive(true);
        startCountdown = true;
        timeCounter = 0;
        if (actualItem < maxItem)
        {
            actualItem++;
            anim.SetTrigger("Up");
        }
    }

    public void DownInventory()
    {
        Inv.SetActive(true);
        startCountdown = true;
        timeCounter = 0;
        if (actualItem > 0)
        {
            actualItem--;
            anim.SetTrigger("Down");
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
