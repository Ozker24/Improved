using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInventory : MonoBehaviour
{
    public GameObject Inv;
    public RectTransform selector;
    public bool startCountdown;
    public float timeCounter = 0;
    public float timeToVanish;

    public Animator anim;

    public void Initialize()
    {
        Inv.SetActive(false);
        //anim = GetComponentInChildren<Animator>();
    }

    public void MyUpdate()
    {
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

    public void RightInventory()
    {
        Inv.SetActive(true);
        startCountdown = true;
        timeCounter = 0;
    }
}
