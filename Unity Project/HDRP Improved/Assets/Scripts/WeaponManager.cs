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
    public bool selecting;

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

        if (selecting)
        {
            if(timeCounter > timeToSelect)
            {
                WeaponSelected = weaponPreSelected;
                timeCounter = 0;
                selecting = false;
                ableGun = false;
                baseSource.PlayOneShot(changeGunClips.clips[WeaponSelected]);
                StartCoroutine(AbleGun(timeAbleGun[WeaponSelected]));
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void Shot()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun) weapons[WeaponSelected].Shot();
        stealth.MakeImportantAudio(distanceToSound[WeaponSelected]);
        //gm.detected = true;
    }

    public void Reload()
    {
        if (!player.climb && !player.stop && items.canDoGun && ableGun)  weapons[WeaponSelected].Reload();
    }

    public void ChangeGunRight()
    {
        inv.RightInventory();

        selecting = true;
        timeCounter = 0;

        if (weaponPreSelected > 0)
        {
            weaponPreSelected--;
        }
    }

    public void ChangeGunLeft()
    {
        inv.LefttInventory();

        selecting = true;
        timeCounter = 0;

        if (weaponPreSelected < maxWeapons - 1)
        {
            weaponPreSelected++;
        }
    }

    #region Corrutines

    IEnumerator AbleGun(float time)
    {
        yield return new WaitForSeconds(time);
        ableGun = true;
    }

    #endregion
}
