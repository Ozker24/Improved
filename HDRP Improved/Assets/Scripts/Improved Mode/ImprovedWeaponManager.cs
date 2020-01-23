using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedWeaponManager : MonoBehaviour
{
    public float stamina = 100;
    public LaserGun laser;

    public void Initialize()
    {
        laser = GetComponentInChildren<LaserGun>();

        laser.Initialize();
    }

    public void MyUpdate()
    {
        laser.MyUpdate();
    }
}
