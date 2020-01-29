using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovedWeaponManager : MonoBehaviour
{
    [Header ("Dependencies")]
    public GameManager GM;
    public LaserGun laser;
    public FlameThrower flameThrower;

    [Header("Stamina")]
    public float stamina = 100;
    public float percentage;
    public Image image;

    [Header ("Using Weapons")]
    public bool usingLaserGun;
    public bool usingFlameThrower;

    public void Initialize()
    {
        laser = GetComponentInChildren<LaserGun>();
        flameThrower = GetComponentInChildren<FlameThrower>();
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        laser.Initialize();
        flameThrower.Initialize();
    }

    public void MyUpdate()
    {
        if (image != null)
        {
            if (GM.improved)
            {
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        laser.MyUpdate();
        flameThrower.MyUpdate();

        percentage = Mathf.Clamp01(stamina / 100);

        if (image != null)
        {
            image.fillAmount = percentage;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }
    }
}
