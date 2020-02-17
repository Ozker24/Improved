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
    [SerializeField] ParticleSystem laserShotPartcile;
    [SerializeField] ParticleSystem chargedLaserShotParticle;
    [SerializeField] GameObject laserHitPartcile;

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
    [SerializeField] float initRestStamina;
    [SerializeField] float maxRestStamina;


    [SerializeField] float timeCounter;
    [SerializeField] float maxTimeCharged;
    [SerializeField] float perceentage;

    [SerializeField] bool canCharge;
    [SerializeField] bool charged;

    [Header("charged Area")]
    [SerializeField] Transform areaPos;
    [SerializeField] Vector3 halfAreaSize;
    [SerializeField] LayerMask layer;

    [Header("Sounds")]
    [SerializeField] AudioSource baseSource;
    [SerializeField] AudioClip shotSingleShot;
    [SerializeField] AudioClip shotSniperShot;
    [SerializeField] AudioClip shotChargedShot;
    [SerializeField] AudioClip chargingChargedShot;
    [SerializeField] AudioClip finalyCharged;
    [SerializeField] bool playChargingSound;

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
            //inFireRate = false; // OJO CON TOCAR COSAS QUE TOCA LA CORUTINA
        }
    }

    public void ShotLaserGun()
    {
        if (!inFireRate)
        {
            inFireRate = true;

            IWM.stamina -= restStamina;

            laserShotPartcile.Play();

            if(damage == semiAutoDamage)
            {
                baseSource.PlayOneShot(shotSingleShot);
            }
            else if (damage == sniperDamage)
            {
                baseSource.PlayOneShot(shotSniperShot);
            }

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit = new RaycastHit();// que hemos golpeado primero

            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.tag == "Enemy")
                {
                    Debug.Log("Hit");
                    GameObject particle = Instantiate(laserHitPartcile, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    Destroy(particle, 10);
                    hit.transform.SendMessage("Damage", damage, SendMessageOptions.RequireReceiver);
                }
            }

            Debug.Log(restStamina);

            StartCoroutine(FinisedFireRate());
        }
    }

    public void ChargedLaserGun()
    {
        damage = MaxchargedDamage * perceentage;

        IWM.stamina -= maxRestStamina * perceentage;

        timeCounter = 0;
        perceentage = 0;

        chargedLaserShotParticle.Play();

        if (baseSource.isPlaying)
        {
            baseSource.Stop();
        }

        baseSource.PlayOneShot(shotChargedShot);

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
        if (IWM.stamina > 0 && !IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingMisileLaucher && !IWM.absorbing)
        {
            baseSource.loop = false;

            if (timeHolding < timeToCharge)
            {
                ShotLaserGun();
            }
            else if (timeHolding >= timeToCharge)
            {
                ChargedLaserGun();
            }

            IWM.usingLaserGun = false;

            timeHolding = 0;
            //chargedDamage = initChargedDamage;
            charged = false;
            canCharge = false;

            IWM.player.CC.canHit = true;
        }
    }

    public void ChargingLaserGun()
    {
        if (IWM.stamina > 0 && !IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingMisileLaucher && !IWM.absorbing)
        {
            IWM.player.CC.canHit = false;

            if (!inFireRate)
            {
                IWM.usingLaserGun = true;

                if (!canCharge) //antes de empezar realmente a cargar
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
                        if (timeCounter >= maxTimeCharged)
                        {
                            charged = true;
                            //chargedDamage = MaxchargedDamage;
                            timeCounter = maxTimeCharged;
                            if (baseSource.isPlaying)
                            {
                                baseSource.Stop();
                            }

                            playChargingSound = false;

                            baseSource.PlayOneShot(finalyCharged);
                        }
                        else
                        {
                            timeCounter += Time.deltaTime;
                            perceentage = Mathf.Clamp01(timeCounter / maxTimeCharged);

                            if (!playChargingSound)
                            {
                                baseSource.loop = true;
                                baseSource.PlayOneShot(chargingChargedShot);
                                playChargingSound = true;
                            }
                        }
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
