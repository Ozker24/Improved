using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperJump : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;

    [Header("Time")]
    [SerializeField] float timeCounter;
    [SerializeField] float timeToStopCounting;
    [SerializeField] float timePercentage;
    public bool didHyperJump; //cargando salto

    [Header("Jump")]
    [SerializeField] float force;
    [SerializeField] float maxForce;
    public bool jump; //estoy en salto

    [Header("HyperFall")]
    public bool falling;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void ChargeHyperJump()
    {
        if (IWM.stamina > 0 && !didHyperJump && !IWM.usingFlameThrower && !IWM.usingLaserGun && !jump)
        {
            if (timeCounter >= timeToStopCounting)
            {
                timeCounter = 0;
                didHyperJump = true;
            }
            else
            {
                IWM.player.stop = true;
                IWM.usingHyperJump = true;
                timeCounter += Time.deltaTime;
                timePercentage = Mathf.Clamp01(timeCounter/timeToStopCounting);
            }
        }
    }

    public void ReleaseHyperJump()
    {
        IWM.player.stop = false;

        IWM.usingHyperJump = false;

        force = maxForce * timePercentage;

        timePercentage = 0;

        if (IWM.player.controler.isGrounded)
        {
            IWM.player.verticalSpeed = force;
            jump = true;
        }
    }

    public void DoFall()
    {
        if (jump)
        {
            falling = true;
        }
    }
}
