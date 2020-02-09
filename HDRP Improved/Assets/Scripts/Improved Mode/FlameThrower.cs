using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] Vector3 halfAreaSize;
    [SerializeField] LayerMask layer;

    [SerializeField] float restStamina;
    [SerializeField] float flameRate;
    public bool fireing;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void MyUpdate()
    {
        if (fireing)
        {
            IWM.stamina -= restStamina;

            DetectOnFire();
        }
    }

    public void Fire()
    {
        if (IWM.stamina > 0 && !IWM.usingLaserGun && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingMisileLaucher && !IWM.absorbing)
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

    void DetectOnFire()
    {
        Collider[] enemiesInArea = Physics.OverlapBox(transform.position, halfAreaSize, transform.rotation, layer);

        foreach(Collider nearbyObjects in enemiesInArea)
        {
            if (nearbyObjects.tag == ("Enemy"))
            {
                Fireable toFire = nearbyObjects.GetComponent<Fireable>();

                if (toFire != null)
                {
                    toFire.timeCounter += flameRate;
                }
            }
        }
    }
}
