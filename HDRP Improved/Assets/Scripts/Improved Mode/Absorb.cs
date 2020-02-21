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
    [SerializeField] float beforeAbsorbe;
    [SerializeField] float howMuchToAddStamina;
    [SerializeField] float howMuchToAddLife;
    [SerializeField] bool canAbsorb;
    public bool interrumpt;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip absorbing;
    [SerializeField] AudioClip absorbed;
    [SerializeField] bool playSound;

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
            if (!IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingMisileLaucher && !IWM.absorbing && nearestEnemy.addStamina > 0) //&& nearestEnemy.addLife > 0)
            {
                if (canAbsorb)
                {
                    IWM.player.CC.canHit = false;

                    if (!playSound)
                    {
                        source.loop = true;
                        source.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
                        source.PlayOneShot(absorbing);
                        playSound = true;
                    }

                    IWM.player.stop = true;
                    IWM.absorbing = true;
                    IWM.stamina = IWM.stamina - nearestEnemy.addStamina * nearestEnemy.absorbPercentage;
                    IWM.player.life.health = IWM.player.life.health - nearestEnemy.addLife * nearestEnemy.absorbPercentage;
                    IWM.addConstantLife = false;
                    IWM.addConstantStamina = false;
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
            if (nearestEnemy != null && howMuchToAddStamina == 0 && howMuchToAddLife == 0)
            {
                howMuchToAddStamina += nearestEnemy.addStamina * nearestEnemy.absorbPercentage;
                howMuchToAddLife += nearestEnemy.addLife * nearestEnemy.absorbPercentage;

                Debug.Log(howMuchToAddStamina + "FromRelease");
            }

            IWM.player.stop = false;
            IWM.absorbing = false;
            IWM.stamina += howMuchToAddStamina;
            IWM.player.life.health += howMuchToAddLife;
            howMuchToAddStamina = 0;
            howMuchToAddLife = 0;

            IWM.addConstantLife = true;
            IWM.addConstantStamina = true;

            IWM.player.CC.canHit = true;

            if (source.isPlaying)
            {
                source.loop = false;
                source.Stop();
                playSound = false;
            }
        }
    }

    void GainStamina()
    {
        if (nearestEnemy != null && nearestEnemy.addStamina > 0 && nearestEnemy.addLife > 0)
        {
            if (nearestEnemy.absorbTimeCounter >= nearestEnemy.maxTimeToAbsorb)
            {
                nearestEnemy.absorbPercentage = 1;

                if (howMuchToAddStamina == 0 && howMuchToAddLife == 0)
                {
                    howMuchToAddStamina += nearestEnemy.addStamina * nearestEnemy.absorbPercentage;
                    howMuchToAddLife += nearestEnemy.addLife * nearestEnemy.absorbPercentage;

                    Debug.Log(howMuchToAddStamina + "FromGain");
                }

                nearestEnemy.addStamina = 0;
                nearestEnemy.addLife = 0;

                nearestEnemy.absorbTimeCounter = 0;

                IWM.player.stop = false;
                IWM.absorbing = false;
                IWM.stamina += howMuchToAddStamina;
                IWM.player.life.health += howMuchToAddLife;
                howMuchToAddStamina = 0;
                howMuchToAddLife = 0;

                IWM.addConstantLife = true;
                IWM.addConstantStamina = true;

                Destroy(nearestEnemy.gameObject, 5);
                nearestEnemy = null;

                IWM.player.CC.canHit = true;

                if (source.isPlaying)
                {
                    source.loop = false;
                    source.Stop();
                    playSound = false;
                }

                source.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
                //source.PlayOneShot(absorbed);
            }
            else
            {
                nearestEnemy.absorbTimeCounter += Time.deltaTime;
                nearestEnemy.absorbPercentage = Mathf.Clamp01(nearestEnemy.absorbTimeCounter / nearestEnemy.maxTimeToAbsorb);
            }
        }
    }

    /*IEnumerator ResetAbsorb()
    {
        yield return new WaitForSeconds(nearestEnemy.timeToAddStamina);
        IWM.absorbing = false;
    }*/
}
