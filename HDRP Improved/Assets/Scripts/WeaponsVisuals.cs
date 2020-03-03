using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsVisuals : MonoBehaviour
{
    [SerializeField] WeaponManager WM;
    [SerializeField] GameObject[] handGuns;
    [SerializeField] GameObject[] bagGuns;

    public void Initialize()
    {
        WM = GetComponent<WeaponManager>();

        ActiveAllGunsBack();
        handGuns[WM.weapons[WM.WeaponSelected].gunReference].SetActive(true);
        bagGuns[WM.weapons[WM.WeaponSelected].gunReference].SetActive(false);
    }

    public void ChangeGunsVisuals()
    {
        SetGunVisuals();
    }

    public void StartChangingGunVisuals()
    {
        DesactiveAllGunsHand();
        ActiveAllGunsBack();
    }

    void SetGunVisuals()
    {
        for (int i = 0; i < handGuns.Length; i++)
        {
            handGuns[i].SetActive(false);
        }

        for (int i = 0; i < bagGuns.Length; i++)
        {
            bagGuns[i].SetActive(false);
        }


        handGuns[WM.weapons[WM.WeaponSelected].gunReference].SetActive(true);
        bagGuns[WM.weapons[WM.WeaponSelected].gunReference].SetActive(false);

        if (WM.WeaponSelected == 0)
        {
            handGuns[WM.weapons[1].gunReference].SetActive(false);
            bagGuns[WM.weapons[1].gunReference].SetActive(true);
        }

        else if (WM.WeaponSelected == 1)
        {
            handGuns[WM.weapons[0].gunReference].SetActive(false);
            bagGuns[WM.weapons[0].gunReference].SetActive(true);
        }
    }

    public void DesactiveAllGunsHand()
    {
        for (int i = 0; i < handGuns.Length; i++)
        {
            handGuns[i].SetActive(false);
        }
    }

    void ActiveAllGunsBack()
    {
        bagGuns[WM.weapons[0].gunReference].SetActive(true);
        bagGuns[WM.weapons[1].gunReference].SetActive(true);
    }

    void ActiveBagGuns()
    {
        for (int i = 0; i < bagGuns.Length; i++)
        {
            bagGuns[i].SetActive(true);
        }

        bagGuns[WM.weapons[WM.WeaponSelected].gunReference].SetActive(false);
    }
}
