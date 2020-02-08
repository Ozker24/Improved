using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ImprovedWeaponManager IWM;

    [SerializeField] float radius;
    [SerializeField] LayerMask layer;

    [SerializeField] float nearestDistance = 9999999999;
    [SerializeField] float constantDistToEnemy;
    [SerializeField] float distToReset;
    [SerializeField] EnemyTest nearestEnemy;
    [SerializeField] float mergeOfReset;
    [SerializeField] float absorbDistance;
    [SerializeField] float howMuchToAdd;
    [SerializeField] bool canAbsorb;
    public bool interrumpt;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void MyUpdate()
    {
        if (nearestEnemy != null)
        {
            constantDistToEnemy = Vector3.Distance(gameObject.transform.position, nearestEnemy.transform.position);
        }

        DetectNearestAbsorb();
        ResetNearestEnemy();

        CheckReset();

        SetCanAbsorb();

        if (Input.GetKeyDown(KeyCode.J))
        {
            StopAbsorbing();
        }

        if (IWM.absorbing) //&& !interrumpt)// para cuando te goolpeen otro booleano y asi no suma
        {
            GainStamina();
        }
    }

    void DetectNearestAbsorb()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);

        foreach (Collider nearByObject in  colliders)
        {
            if (nearByObject.tag == "Enemy")
            {
                EnemyTest enemy = nearByObject.GetComponent<EnemyTest>();

                if (enemy.dead && enemy.addStamina > 0)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }
        }
    }

    void ResetNearestEnemy ()
    {
        if (nearestEnemy != null)
        {
            if (Vector3.Distance (transform.position, nearestEnemy.transform.position) >= nearestDistance + mergeOfReset)
            {
                nearestDistance = Mathf.Infinity;
            }
        }
    }

    void CheckReset()
    {
        if (constantDistToEnemy >= distToReset)
        {
            nearestDistance = Mathf.Infinity;
            nearestEnemy = null;
            constantDistToEnemy = 0;
        }
    }

    void SetCanAbsorb()
    {
        if (constantDistToEnemy <= absorbDistance)
        {
            canAbsorb = true;
        }
        else if (constantDistToEnemy > absorbDistance)
        {
            canAbsorb = false;
        }

        if (nearestEnemy == null)
        {
            canAbsorb = false;
        }
    }

    public void DoAbsorb()
    {
        if (nearestEnemy != null)
        {
            if (!IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingMisileLaucher && !IWM.absorbing && nearestEnemy.addStamina > 0)
            {
                if (canAbsorb)
                {
                    IWM.player.stop = true;
                    IWM.absorbing = true;
                    IWM.stamina = IWM.stamina - nearestEnemy.addStamina * nearestEnemy.absorbPercentage;
                    //StartCoroutine(ResetAbsorb());
                }
            }
        }
    }

    public void ReleaseAbsorb()
    {
        StopAbsorbing();
        interrumpt = false;
    }

    public void StopAbsorbing()
    {
        if (IWM.absorbing && !interrumpt)
        {
            if (nearestEnemy != null && howMuchToAdd == 0)
            {
                howMuchToAdd += nearestEnemy.addStamina * nearestEnemy.absorbPercentage;

                Debug.Log(howMuchToAdd + "FromRelease");
            }

            IWM.player.stop = false;
            IWM.absorbing = false;
            IWM.stamina += howMuchToAdd;
            howMuchToAdd = 0;
        }
    }

    void GainStamina()
    {
        if (nearestEnemy != null && nearestEnemy.addStamina > 0)
        {
            if (nearestEnemy.staminaTimeCounter >= nearestEnemy.maxTimeToStamina)
            {
                nearestEnemy.absorbPercentage = 1;

                if (howMuchToAdd == 0)
                {
                    howMuchToAdd += nearestEnemy.addStamina * nearestEnemy.absorbPercentage;

                    Debug.Log(howMuchToAdd + "FromGain");
                }

                nearestEnemy.addStamina = 0;
                nearestEnemy.staminaTimeCounter = 0;

                IWM.player.stop = false;
                IWM.absorbing = false;
                IWM.stamina += howMuchToAdd;
                howMuchToAdd = 0;

                Destroy(nearestEnemy.gameObject, 5);
                nearestEnemy = null;
            }
            else
            {
                nearestEnemy.staminaTimeCounter += Time.deltaTime;
                nearestEnemy.absorbPercentage = Mathf.Clamp01(nearestEnemy.staminaTimeCounter / nearestEnemy.maxTimeToStamina);
            }
        }
    }

    IEnumerator ResetAbsorb()
    {
        yield return new WaitForSeconds(nearestEnemy.timeToAddStamina);
        IWM.absorbing = false;
    }
}
