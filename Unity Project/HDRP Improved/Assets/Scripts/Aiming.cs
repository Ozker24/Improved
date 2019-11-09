﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Aiming : MonoBehaviour
{
    Cinemachine.CinemachineVirtualCameraBase vCam;

    Cinemachine.CinemachineFreeLook freeLook;
    public bool aim;

    public GameObject point;
    public GameObject ammo;
    public PlayerController player;
    public WeaponManager WM;

    public float originalLens;
    public float aimLens;

    public float speedAim;

    public void Initialize()
    {
        //vCam = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
        freeLook = GetComponent<Cinemachine.CinemachineFreeLook>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WM = player.GetComponentInChildren<WeaponManager>();

        point.SetActive(false);
        ammo.SetActive(false);
    }

    public void MyUpdate()
    {
        if (freeLook != null)
        {
            if (aim && !player.stop && WM.ableGun)
            {
                //vCam.Priority = 11;
                //freeLook.m_Lens.FieldOfView = Mathf.Lerp(originalLens, aimLens, speedAim);
                freeLook.m_Lens.FieldOfView = aimLens;
            }
            else
            {
                //vCam.Priority = 9;
                //freeLook.m_Lens.FieldOfView = Mathf.Lerp(aimLens, originalLens, speedAim);
                freeLook.m_Lens.FieldOfView = originalLens;
            }
        }

        if (aim && !player.stop && !player.climb && WM.ableGun)
        {
            point.SetActive(true);
            ammo.SetActive(true);
        }
        else
        {
            point.SetActive(false);
            ammo.SetActive(false);
        }
    }

    public void Aim()
    {
        aim = true;
    }

    public void NotAim()
    {
        aim = false;
    }
}