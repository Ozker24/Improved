using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public GameObject actualItem;
    public Transform actualLogo;
    public Inventory Inventory;
    public Items items;
    public GunInventory guns;
    public WeaponManager weapon;

    public Text molotovNum;
    public Text granadeNum;
    public Text soundNum;
    public Text firstAidNum;
    public Text InjectionNum;

    public Text pistolText;
    public Text shotgunText;
    public Text rifleText;
    public Text snipperText;

    public Text showTotalAmmo;
    public Text currentAmmo;

    public int whatItem;

    public void Initialize()
    {
        items = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Items>();
        weapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<WeaponManager>();
    }

    public void MyUpdate()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        UpdateMolotov(items.molotovCount);
        UpdateGrande(items.GranadeCount);
        UpdateSound(items.SoundCount);
        UpdateFirstAid(items.FirstAidCount);
        UpdateInjections(items.InjectionCount);
        UpdatePistol(weapon.weapons[0].ammoReloaded + weapon.weapons[0].currentAmmo);
        UpdateShotgun(weapon.weapons[1].ammoReloaded + weapon.weapons[1].currentAmmo);
        UpdateRifle(weapon.weapons[2].ammoReloaded + weapon.weapons[2].currentAmmo);
        UpdateSnipper(weapon.weapons[3].ammoReloaded + +weapon.weapons[3].currentAmmo);
    }

    public void UpdateMolotov(int molotovs)
    {
        molotovNum.text = molotovs.ToString();
    }

    public void UpdateGrande(int granade)
    {
        granadeNum.text = granade.ToString();
    }

    public void UpdateSound(int sound)
    {
        soundNum.text = sound.ToString();
    }

    public void UpdateFirstAid(int firstAid)
    {
        firstAidNum.text = firstAid.ToString();
    }

    public void UpdateInjections(int injections)
    {
        InjectionNum.text = injections.ToString();
    }

    public void UpdatePistol(int pistol)
    {
        pistolText.text = pistol.ToString();
    }

    public void UpdateShotgun(int shotgun)
    {
        shotgunText.text = shotgun.ToString();
    }

    public void UpdateRifle(int rifle)
    {
        rifleText.text = rifle.ToString();
    }

    public void UpdateSnipper(int snipper)
    {
        snipperText.text = snipper.ToString();
    }

    public void UpdateTotalAmmo(int total)
    {
        showTotalAmmo.text = total.ToString();
    }

    public void UpdateCurrentAmmo(int ammo)
    {
        currentAmmo.text = ammo.ToString();
    }
}
