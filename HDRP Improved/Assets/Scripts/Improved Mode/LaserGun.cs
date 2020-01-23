using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Aiming aim;
    [SerializeField] ImprovedWeaponManager IWM;

    [Header ("General Laser Variables")]
    [SerializeField] float fireRate;
    [SerializeField] float damage;
    [SerializeField] float distance;
    [SerializeField] float restStamina;

    [Header("Semi Auto variables")]
    [SerializeField] float semiAutoLens;
    [SerializeField] float semiAutoDamage;
    [SerializeField] float semiAutoDistance;
    [SerializeField] float semiAutoRestStamina;

    [Header ("Sniper variables")]
    [SerializeField] float sniperLens;
    [SerializeField] float sniperDamage;
    [SerializeField] float sniperDistance;
    [SerializeField] float sniperRestStamina;
    [SerializeField] float sniperFireRate;
    [SerializeField] bool inFireRate;

    [Header("charged variables")]
    [SerializeField] float timeHolding;
    [SerializeField] float timeToCharge;
    [SerializeField] float initChargedDamage;
    [SerializeField] float chargedDamage;
    [SerializeField] float chargedDamageMultiplier;
    [SerializeField] float MaxchargedDamage;
    [SerializeField] bool canCharge;
    [SerializeField] bool charged;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
        aim = GameObject.FindGameObjectWithTag("TPCamera").GetComponent<Aiming>();
        chargedDamage = initChargedDamage;
    }

    public void MyUpdate()
    {
        if (aim.aimed)
        {
            fireRate = sniperFireRate;
            damage = sniperDamage;
            distance = sniperDistance;
            restStamina = sniperRestStamina;
        }
        else
        {
            fireRate = 0;
            damage = semiAutoDamage;
            distance = semiAutoDistance;
            restStamina = semiAutoRestStamina;
            inFireRate = false; // OJO CON TOCAR COSAS QUE TOCA LA CORUTINA
        }
    }

    public void ShotLaserGun()
    {
        Debug.Log("Shot" + damage);

        inFireRate = true;

        IWM.stamina -= restStamina;

        StartCoroutine(FinisedFireRate());
    }

    public void ChargedLaserGun()
    {
        damage = chargedDamage;
        Debug.Log(damage);
    }

    public void ResetLaserGun()
    {
        if (timeHolding < timeToCharge)
        {
            ShotLaserGun();
        }
        else if (timeHolding >= timeToCharge)
        {
            ChargedLaserGun();
        }

        timeHolding = 0;
        chargedDamage = initChargedDamage;
        charged = false;
        canCharge = false;
    }

    public void ChargingLaserGun()
    {
        if (!canCharge)
        {
            if (timeHolding >= timeToCharge)
            {
                timeHolding = timeToCharge;
                canCharge = true;
            }
            else
            {
                timeHolding += Time.deltaTime;
            }
        }
        else
        {
            if(!charged)
            {
                if (chargedDamage >= MaxchargedDamage)
                {
                    charged = true;
                    chargedDamage = MaxchargedDamage;
                }
                else
                {
                    chargedDamage += Time.deltaTime * chargedDamageMultiplier;
                }
            }
        }
    }

    #region

    IEnumerator FinisedFireRate()
    {
        yield return new WaitForSeconds(fireRate);
        inFireRate = false;
    }

    #endregion
}
