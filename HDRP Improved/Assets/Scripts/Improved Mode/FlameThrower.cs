using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] BoxCollider boxCol;

    [SerializeField] float restStamina;
    public bool fireing;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();

        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    public void MyUpdate()
    {
        if (IWM.GM.improved)
        {
            boxCol.enabled = true;
        }
        else
        {
            boxCol.enabled = false;
        }

        if (fireing)
        {
            IWM.stamina -= restStamina;
        }
    }

    public void Fire()
    {
        if (IWM.stamina > 0 && !IWM.usingLaserGun)
        {
            fireing = true;
            IWM.usingFlameThrower = true;
        }
    }

    public void ReleaseFire()
    {
        fireing = false;
        IWM.usingFlameThrower = false;
    }
}
