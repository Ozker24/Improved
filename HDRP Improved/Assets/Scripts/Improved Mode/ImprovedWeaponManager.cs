using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovedWeaponManager : MonoBehaviour
{
    public GameManager GM;

    public float stamina = 100;
    public float percentage;
    public LaserGun laser;
    public Image image;

    public void Initialize()
    {
        laser = GetComponentInChildren<LaserGun>();
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        laser.Initialize();
    }

    public void MyUpdate()
    {
        if (GM.improved)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }

        laser.MyUpdate();
        percentage = Mathf.Clamp01(stamina / 100);

        image.fillAmount = percentage;

        if (stamina < 0)
        {
            stamina = 0;
        }
    }
}
