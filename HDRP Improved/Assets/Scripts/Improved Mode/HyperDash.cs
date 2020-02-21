using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperDash : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] float timeOfDodgeImprovedMode;
    [SerializeField] float timeForCanDodgeImprovedMode;
    [SerializeField] float dodgeImprovedModeSpeed;
    [SerializeField] float restStamina;
    [SerializeField] AudioSource source;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void MyUpdate()
    {
        if (IWM.GM.improved)
        {
            IWM.player.dodgeSpeed = dodgeImprovedModeSpeed;
            IWM.player.timeOfDodge = timeOfDodgeImprovedMode;
            IWM.player.timeForCanDodge = timeForCanDodgeImprovedMode;
        }
    }


    public void DoDash()
    {
        if(IWM.stamina > 0 && !IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingLaserGun && !IWM.usingMisileLaucher && !IWM.absorbing)
        {
            source.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
            //source.Play();
            Debug.LogWarning("HyperDash sound is desactive");
            IWM.usingHyperDash = true;
            IWM.stamina -= restStamina;
            IWM.player.Dodge();
        }
    }
}
