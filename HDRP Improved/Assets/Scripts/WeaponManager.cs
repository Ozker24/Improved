﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header ("Game Objects")]
    public PlayerController player;
    public HudManager HUD;
    public Inventory inv;
    //public AudioPlayer audPlay;
    //public AudioPlayer audPlayMag;
    public Aiming aiming;
    public GameManager gm;
    public Items items;
    public StealthSystem stealth;
    public WeaponsVisuals WVisuals;

    //public bool searchingBullet;

    [Header("Weapons")]
    public Weapon[] weapons;
    public WeaponStats[] weaponsType;
    public GameObject weaponItem;
    public Transform posReleasedGun;

    [Header("Ammo Supply")]
    public int[] ammoCollected;

    [Header("Weapon Selection")]
    public int weaponPreSelected;
    public int WeaponSelected = 0;
    public float timeCounter;
    public float timeToSelect;
    public bool selectingGun;

    public int maxWeapons;

    public float[] timeAbleGun;
    public bool ableGun = true;
    public float[] itemTimeToGun;
    public float[] timeToAim;
    public float[] distanceToSound;

    public GameObject bloodParticle;
    public ParticleSystem muzzleParticle;
    public GameObject sparklePartcile;

    public AudioClip MetalClip;
    public AudioArray shotClips;
    public AudioArray ReloadClips;

    [Header("Sound")]
    public AudioSource baseSource;
    public AudioSource UiSource;
    public AudioArray changeGunClips;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        WVisuals = GetComponent<WeaponsVisuals>();
        WVisuals.Initialize();
        stealth = player.stealth;
        items = player.GetComponentInChildren<Items>();
        //audPlay = GetComponent<AudioPlayer>();
        maxWeapons = weapons.Length;

        ChangeStats(0, weapons[0].gunReference);
        ChangeStats(1, weapons[1].gunReference);
        //timeToSelect = inv.timeToVanish;
    }

    public void MyUpdate()
    {
        HUD.UpdateCurrentAmmo(weapons[WeaponSelected].currentAmmo);

        if (selectingGun)
        {
            if(timeCounter > timeToSelect)
            {
                if (weaponPreSelected != WeaponSelected && selectingGun)
                {
                    WeaponSelected = weaponPreSelected;

                    WVisuals.StartChangingGunVisuals();

                    ableGun = false;
                    UiSource.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
                    UiSource.PlayOneShot(changeGunClips.clips[WeaponSelected]);
                    StartCoroutine(AbleGun(weapons[WeaponSelected].timeAbleGun));
                }

                timeCounter = 0;
                selectingGun = false;
                //si hay audio que suena al salir la ui aqui.
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void Shot()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun && !items.pressed && !player.dodging)
        {
            weapons[WeaponSelected].Shot();
            stealth.MakeImportantAudio(weapons[WeaponSelected].distanceToSound,player.transform.position);
        }
        //gm.detected = true;
    }

    public void Reload()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun && !items.pressed && !player.dodging)
        {
            weapons[WeaponSelected].Reload();
        }
    }

    public void ReleaseShot()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun && !items.pressed && !player.dodging) weapons[WeaponSelected].ReleaseShot();
    }

    public void ChangeGunRight()
    {
        if (!items.pressed && player.GM.ableToInput && !player.dodging)
        {
            inv.RightInventory();

            selectingGun = true;
            timeCounter = 0;

            if (weaponPreSelected > 0)
            {
                weaponPreSelected--;
            }
        }
    }

    public void ChangeGunLeft()
    {
        if (!items.pressed && player.GM.ableToInput && !player.dodging)
        {
            inv.LefttInventory();

            selectingGun = true;
            timeCounter = 0;

            if (weaponPreSelected < maxWeapons - 1)
            {
                weaponPreSelected++;
            }
        }
    }

    public void ChangeStats(int weaponIndex, int typeIndex)
    {
        weapons[weaponIndex].magazineAmmo = weaponsType[typeIndex].magazineAmmo;
        //weapons[weaponIndex].currentAmmo = weaponsType[typeIndex].currentAmmo;
        weapons[weaponIndex].maxAmmo = weaponsType[typeIndex].maxAmmo;
        weapons[weaponIndex].fireRate = weaponsType[typeIndex].fireRate;
        weapons[weaponIndex].reloadTime = weaponsType[typeIndex].reloadTime;
        weapons[weaponIndex].fireDist = weaponsType[typeIndex].fireDist;
        weapons[weaponIndex].bulletDamage = weaponsType[typeIndex].bulletDamage;
        weapons[weaponIndex].bulletMetalDamage = weaponsType[typeIndex].bulletMetalDamage;
        weapons[weaponIndex].stunedTime = weaponsType[typeIndex].stunedTime;
        weapons[weaponIndex].shotGunSpreads = weaponsType[typeIndex].shotGunSpreads;
        weapons[weaponIndex].spread = weaponsType[typeIndex].spread;
        weapons[weaponIndex].timeAbleGun = weaponsType[typeIndex].timeAbleGun;
        weapons[weaponIndex].itemTimeToGun = weaponsType[typeIndex].itemTimeToGun;
        weapons[weaponIndex].timeToAim = weaponsType[typeIndex].timeToAim;
        weapons[weaponIndex].distanceToSound = weaponsType[typeIndex].distanceToSound;
        weapons[weaponIndex].isAutomatic = weaponsType[typeIndex].isAutomatic;
        weapons[weaponIndex].slot = weaponsType[typeIndex].Slot;
        weapons[weaponIndex].gunReference = weaponsType[typeIndex].gunReference;
    }

    /*public void GunShortcuts()
    {
        WeaponSelected = weaponPreSelected;
        ableGun = false;
        UiSource.PlayOneShot(changeGunClips.clips[WeaponSelected]);
        StartCoroutine(AbleGun(timeAbleGun[WeaponSelected]));
    }*/

    #region Corrutines

    IEnumerator AbleGun(float time)
    {
        yield return new WaitForSeconds(time);
        WVisuals.ChangeGunsVisuals();
        ableGun = true;
    }

    #endregion
}
