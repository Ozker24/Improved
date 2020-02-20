using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] ParticleSystem flameParticle;
    [SerializeField] Vector3 halfAreaSize;
    [SerializeField] LayerMask layer;

    [SerializeField] float restStamina;
    [SerializeField] float flameRate;
    public bool fireing;

    [Header("Audio")]
    public AudioSource source;
    public bool playSound;

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
            if (!playSound)
            {
                playSound = true;
                flameParticle.Play();
                source.Play();
            }
            IWM.player.CC.canHit = false;
            fireing = true;
            IWM.usingFlameThrower = true;
        }
    }

    public void ReleaseFire()
    {
        IWM.player.CC.canHit = true;
        fireing = false;
        IWM.usingFlameThrower = false;
        source.Stop();
        flameParticle.Stop();
        playSound = false;
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
