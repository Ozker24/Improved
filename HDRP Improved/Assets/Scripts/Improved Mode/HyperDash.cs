using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperDash : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] float timeUsingDash;
    [SerializeField] float restStamina;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void DoDash()
    {
        if(IWM.stamina > 0 && !IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingLaserGun && !IWM.usingMisileLaucher && !IWM.absorbing)
        {
            IWM.usingHyperDash = true;
            IWM.stamina -= restStamina;
            StartCoroutine(SetUsingHyperDash());
        }
    }

    IEnumerator SetUsingHyperDash()
    {
        yield return new WaitForSeconds(timeUsingDash);
        IWM.usingHyperDash = false;
    }
}
