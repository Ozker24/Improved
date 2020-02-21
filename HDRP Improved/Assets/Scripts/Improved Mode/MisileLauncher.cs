using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisileLauncher : MonoBehaviour
{
    [SerializeField] ImprovedWeaponManager IWM;
    [SerializeField] float restStamina;
    [SerializeField] float launchFireRange;
    [SerializeField] bool canLaunch;

    [Header("Misile Parameters")]
    [SerializeField] GameObject misile;
    [SerializeField] float misileDist;
    public Vector3 nextMisilePos;
    public Transform detectCelling;
    [SerializeField] LayerMask roofLayer;
    [SerializeField] float rayRoofDist;
    [SerializeField] bool underRoof;

    [Header("Audio")]
    [SerializeField] AudioSource source;

    public void Initialize()
    {
        IWM = GetComponentInParent<ImprovedWeaponManager>();
    }

    public void MyUpdate()
    {
        CanILaunchMisile();
    }

    public void LauchMisile()
    {
        if (!underRoof)
        {
            if (IWM.stamina > 0 && !IWM.usingFlameThrower && !IWM.usingHyperJump && !IWM.usingHyperDash && !IWM.usingLaserGun && !IWM.usingMisileLaucher && canLaunch && !IWM.absorbing)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit = new RaycastHit();// que hemos golpeado primero

                if (Physics.Raycast(ray, out hit, misileDist))
                {
                    IWM.usingMisileLaucher = true;
                    IWM.stamina -= restStamina;
                    canLaunch = false;
                    IWM.usingMisileLaucher = false;
                    StartCoroutine(ResetCanLaunch());

                    source.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
                    source.Play();

                    nextMisilePos = hit.point;

                    Instantiate(misile, transform.position, Quaternion.identity);
                }
            }
        }
    }

    void CanILaunchMisile()
    {
        RaycastHit hit = new RaycastHit();// que hemos golpeado primero
        if (Physics.Raycast(detectCelling.position, Vector3.up, out hit, rayRoofDist, roofLayer))
        {
            if (hit.transform.tag == "Roof")
            {
                underRoof = true;
            }
        }
        else
        {
            underRoof = false;
        }
    }

    IEnumerator ResetCanLaunch()
    {
        yield return new WaitForSeconds(launchFireRange);
        canLaunch = true;
    }
}
