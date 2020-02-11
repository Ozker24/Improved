using System.Collections;
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

    //public bool searchingBullet;

    [Header("Weapons Ammount")]
    public Weapon[] weapons;

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
    public AudioArray changeGunClips;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        stealth = player.stealth;
        items = player.GetComponentInChildren<Items>();
        //audPlay = GetComponent<AudioPlayer>();
        maxWeapons = weapons.Length;
        timeToSelect = inv.timeToVanish;
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
                    ableGun = false;
                    baseSource.PlayOneShot(changeGunClips.clips[WeaponSelected]);
                    StartCoroutine(AbleGun(timeAbleGun[WeaponSelected]));
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
            stealth.MakeImportantAudio(distanceToSound[WeaponSelected]);
        }
        //gm.detected = true;
    }

    public void Reload()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun && !items.pressed && !player.dodging)  weapons[WeaponSelected].Reload();
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

    public void GunShortcuts()
    {
        WeaponSelected = weaponPreSelected;
        ableGun = false;
        baseSource.PlayOneShot(changeGunClips.clips[WeaponSelected]);
        StartCoroutine(AbleGun(timeAbleGun[WeaponSelected]));
    }

    #region Corrutines

    IEnumerator AbleGun(float time)
    {
        yield return new WaitForSeconds(time);
        ableGun = true;
    }

    #endregion
}
