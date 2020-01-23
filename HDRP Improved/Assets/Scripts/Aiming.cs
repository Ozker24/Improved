using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Aiming : MonoBehaviour
{
    [Header ("Dependences")]
    public GameObject point;
    public GameObject ammo;
    public PlayerController player;
    public WeaponManager WM;
    public Items items;

    [Header("Cinemachine")]
    Cinemachine.CinemachineVirtualCameraBase vCam;
    Cinemachine.CinemachineFreeLook freeLook;

    [Header("Zooms")]
    public bool aim;
    public float actualLens;
    public float originalLens;
    public float aimLens;

    [Header ("Zoom Interpolation")]
    public bool direction = false;
    public bool dontDo = true;
    public float timeCounter;
    public float timeOfZoom;
    //public float speed;

    public void Initialize()
    {
        //vCam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
        freeLook = GetComponent<Cinemachine.CinemachineFreeLook>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WM = player.GetComponentInChildren<WeaponManager>();
        items = player.GetComponentInChildren<Items>();

        point.SetActive(false);
        ammo.SetActive(false);
    }

    public void MyUpdate()
    {
        if (freeLook != null)
        {
            actualLens = freeLook.m_Lens.FieldOfView; // it is always calculating the actual lens, it is the begining point of the lerp

            if (aim && !player.stop && WM.ableGun && !items.pressed) //if i am aiming and other bools dont interrupt me
            {
                if (!direction) // and i am actually aming
                {
                    dontDo = false;
                    timeCounter = 0;
                    direction = true;
                }
                else if (timeCounter < timeOfZoom) //while the time counter does not arrive to the limit
                {
                    freeLook.m_Lens.FieldOfView = Mathf.Lerp(actualLens, aimLens, timeCounter);
                    timeCounter += Time.deltaTime * WM.timeToAim[WM.WeaponSelected];
                }
                else if (timeCounter >= timeOfZoom) // when the time counter aarives to the limit
                {
                    freeLook.m_Lens.FieldOfView = aimLens;
                }
            }
            else
            {
                if (!dontDo) // this avoids to do the function becouse at the begining you aren't aming.
                {
                    if (direction) // and i am not aming
                    {
                        timeCounter = 0;
                        direction = false;
                    }
                    else if (timeCounter < timeOfZoom) //while the time counter does not arrive to the limit
                    {
                        freeLook.m_Lens.FieldOfView = Mathf.Lerp(actualLens, originalLens, timeCounter);
                        timeCounter += Time.deltaTime * WM.timeToAim[WM.WeaponSelected];
                    }
                    else if (timeCounter >= timeOfZoom) // when the time counter aarives to the limit
                    {
                        freeLook.m_Lens.FieldOfView = originalLens;
                    }
                }
            }
        }

        if (aim && !player.stop && !player.climb && WM.ableGun && !items.pressed)
        {
            point.SetActive(true);
            ammo.SetActive(true);
        }
        else
        {
            point.SetActive(false);
            ammo.SetActive(false);
        }

        if (player.GM.improved)
        {
            point.SetActive(true);
        }
    }

    public void Aim() //Returns true while the imput is pressed.
    {
        aim = true;
    }

    public void NotAim() //Returns false while the imput is pressed.
    {
        aim = false;
    }
}