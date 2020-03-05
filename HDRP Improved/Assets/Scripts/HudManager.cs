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
    public Text EMPNum;

    public Text pistolText;
    public Text shotgunText;
    public Text rifleText;
    public Text snipperText;

    //public Text showTotalAmmo;
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
        //UpdateMolotov(items.itemsCount[4]);
        //UpdateGrande(items.itemsCount[3]);
        //UpdateSound(items.itemsCount[2]);
        //UpdateFirstAid(items.itemsCount[1]);
        //UpdateEMP(items.itemsCount[0]);
        UpdatePistol(weapon.ammoCollected[weapon.WeaponSelected] + weapon.weapons[0].currentAmmo);
        UpdateShotgun(weapon.ammoCollected[weapon.WeaponSelected] + weapon.weapons[1].currentAmmo);
        //UpdateRifle(weapon.weapons[2].ammoReloaded + weapon.weapons[2].currentAmmo);
        //UpdateSnipper(weapon.weapons[3].ammoReloaded + weapon.weapons[3].currentAmmo);
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

    public void UpdateEMP(int EMP)
    {
        EMPNum.text = EMP.ToString();
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

    public void UpdateCurrentAmmo(int ammo)
    {
        currentAmmo.text = ammo.ToString();
    }
}
