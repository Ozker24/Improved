using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsVisuals : MonoBehaviour
{
    [SerializeField] WeaponManager WM;
    [SerializeField] GameObject[] handGuns;
    [SerializeField] GameObject[] bagGuns;

    public void Start()
    {
        WM = GetComponent<WeaponManager>();

        ChangeGunsVisuals();
    }

    public void ChangeGunsVisuals()
    {
        DesactiveHandsGuns();
        ActiveBagGuns();
    }

    public void StartChangingGunVisuals()
    {
        DesactiveAllGunsHand();
        ActiveAllGunsBack();
    }

    void DesactiveHandsGuns()
    {
        for (int i = 0; i < handGuns.Length; i++)
        {
            handGuns[i].SetActive(false);
        }

        handGuns[WM.WeaponSelected].SetActive(true);
    }

    void DesactiveAllGunsHand()
    {
        for (int i = 0; i < handGuns.Length; i++)
        {
            handGuns[i].SetActive(false);
        }
    }

    void ActiveAllGunsBack()
    {
        for (int i = 0; i < bagGuns.Length; i++)
        {
            bagGuns[i].SetActive(true);
        }
    }

    void ActiveBagGuns()
    {
        for (int i = 0; i < bagGuns.Length; i++)
        {
            bagGuns[i].SetActive(true);
        }

        bagGuns[WM.WeaponSelected].SetActive(false);
    }
}
