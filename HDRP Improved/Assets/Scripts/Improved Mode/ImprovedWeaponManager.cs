using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedWeaponManager : MonoBehaviour
{
    public GameManager GM;

    public float stamina = 100;
    public LaserGun laser;

    public void Initialize()
    {
        laser = GetComponentInChildren<LaserGun>();
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        laser.Initialize();
    }

    public void MyUpdate()
    {
        laser.MyUpdate();
    }
}
