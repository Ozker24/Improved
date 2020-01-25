using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Aiming aim;
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] ChargedLaserArea chargedArea;

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

    [Header("charged Area")]
    [SerializeField] Transform areaPos;
    [SerializeField] Vector3 halfAreaSize;
    [SerializeField] LayerMask layer;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
        //chargedArea = gameObject.GetComponentInChildren<ChargedLaserArea>();
        aim = GameObject.FindGameObjectWithTag("TPCamera").GetComponent<Aiming>();
        chargedDamage = initChargedDamage;
        //chargedArea.Initialize();
    }

    public void MyUpdate()
    {
        if (IWM.GM.improved)
        {
            //chargedArea.MyUpdate();
        }

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
        if (!inFireRate)
        {
            inFireRate = true;

            IWM.stamina -= restStamina;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit = new RaycastHit();// que hemos golpeado primero

            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.tag == "Enemy")
                {
                    Debug.Log("Hit");
                    hit.transform.SendMessage("Damage", damage, SendMessageOptions.RequireReceiver);
                }
            }

            StartCoroutine(FinisedFireRate());
        }
    }

    public void ChargedLaserGun()
    {
        Debug.Log("DO");
        damage = chargedDamage;

        Collider[] enemiesInArea = Physics.OverlapBox(areaPos.position, halfAreaSize, transform.rotation, layer);

        foreach (Collider nearbyObject in enemiesInArea)
        {
            if (nearbyObject.tag == ("Enemy"))
            {
                Debug.Log("Enemy");
                EnemyTest enemy = nearbyObject.GetComponent<EnemyTest>();
                enemy.Damage(damage);
            }
        }
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
        if (!inFireRate)
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
                if (!charged)
                {
                    if (chargedDamage >= MaxchargedDamage)
                    {
                        charged = true;
                        chargedDamage = MaxchargedDamage;
                    }
                    else
                    {
                        chargedDamage += Time.deltaTime;
                    }
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
