using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public PlayerController player;

    public Weapon[] weapons;

    public HudManager HUD;

    public Inventory inv;

    public AudioPlayer audPlay;

    public AudioPlayer audPlayMag;

    public Aiming aiming;

    public GameManager gm;

    public Items items;

    //public bool searchingBullet;

    public int WeaponSelected = 0;

    public int maxWeapons;

    public bool pressing;

    public int realised = 0;

    public float timeCounter;

    public bool magazineFull;

    public bool fullMainAmmo;

    public bool startSearching;

    public bool playZipSound = true;

    public float timeToStart;

    public float ableToThrow;

    public bool ableToLunch = true;

    public bool ableToRun = true;

    public bool ableToBullet = true;


    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();

        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();

        items = player.GetComponentInChildren<Items>();

        //audPlay = GetComponent<AudioPlayer>();

        maxWeapons = weapons.Length;
    }

    public void MyUpdate()
    {
        HUD.UpdateCurrentAmmo(weapons[WeaponSelected].currentAmmo);

        if (weapons[WeaponSelected].currentAmmo >= weapons[WeaponSelected].magazineAmmo && player.walking) //////////////// falta en el aim!
        {
            ableToBullet = false;
        }
        else
        {
            ableToBullet = true;
        }

        if (!ableToBullet) StartCoroutine(DissapearAmmoCount());

        if (startSearching)
        {
            //ReloadBullet();
            HUD.UpdateTotalAmmo(weapons[WeaponSelected].totalAmmo);
        }
    }

    /*public void ReloadBullet()
    {
        if (pressing && realised != 1 && !magazineFull && weapons[WeaponSelected].totalAmmo > 0)
        {
            if (ableToBullet) HUD.showTotalAmmo.gameObject.SetActive(true);

            if (timeCounter >= weapons[WeaponSelected].timeForBullet)
            {
                if (weapons[WeaponSelected].currentAmmo < weapons[WeaponSelected].magazineAmmo && !player.running)
                {
                    RestBullet();

                    weapons[WeaponSelected].currentAmmo++;
                }
                else if (!player.walking && !player.running && !player.aiming)
                {
                    RestBullet();

                    weapons[WeaponSelected].ammoReloaded++;
                }

                else ableToBullet = false;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        if (realised == 1 || (player.walking && fullMainAmmo))
        {
            timeCounter = 0;

            //searchingBullet = false;

            HUD.showTotalAmmo.gameObject.SetActive(false);

            audPlay.Play(5, 1, 1);

            pressing = false;

            playZipSound = true;

            startSearching = false;

            //searchingBullet = false;
        }

        if (weapons[WeaponSelected].currentAmmo < weapons[WeaponSelected].magazineAmmo)
        {
            fullMainAmmo = false;
        }

        if (weapons[WeaponSelected].currentAmmo >= weapons[WeaponSelected].magazineAmmo)
        {
            if (!fullMainAmmo)
            {
                magazineFull = true;

                StartCoroutine(ChangingFirstMagazine());
            }
        }

        if (weapons[WeaponSelected].ammoReloaded % weapons[WeaponSelected].magazineAmmo == 0 && weapons[WeaponSelected].ammoReloaded != 0)
        {
            magazineFull = true;
            StartCoroutine(SearchingMagazine());
        }
    }*/

    public void SetPressing()
    {
            pressing = true;

            //searchingBullet = true;

            ableToLunch = false;

            ableToRun = false;

        if (ableToBullet)
        {
            if (playZipSound)
            {
                audPlay.Play(4, 1, 1);

                StartCoroutine(StartSearching());

                playZipSound = false;
            }
        }
    }

    public void SetRealised()
    {
        realised = 1;

        StartCoroutine(RealisedFalse());
        StartCoroutine(TrueAbleToLunch());
    }

    public void RestBullet()
    {
        timeCounter = 0;
        weapons[WeaponSelected].totalAmmo--;

        audPlay.Play(WeaponSelected, 1, 1);
    }

    IEnumerator RealisedFalse()
    {
        yield return new WaitForEndOfFrame();

        //yield return new WaitForEndOfFrame();
        realised = 0;
    }

    IEnumerator SearchingMagazine()
    {
        yield return new WaitForSeconds(weapons[WeaponSelected].timeForSearch);
        magazineFull = false;
    }

    IEnumerator ChangingFirstMagazine()
    {
        yield return new WaitForSeconds(weapons[WeaponSelected].timeForFirstMagazine);
        magazineFull = false;
        fullMainAmmo = true;
    }

    public void Shot()
    {
        if (!player.climb && !player.stop && items.canDoGun) weapons[WeaponSelected].Shot();
        gm.detected = true;
    }

    public void Reload()
    {
        if (!player.climb && !player.stop && items.canDoGun)  weapons[WeaponSelected].Reload();
    }

    public void ChangeGunRight()
    {
        inv.RightInventory();

        if (WeaponSelected > 0)
        {
            WeaponSelected--;
        }
    }

    public void ChangeGunLeft()
    {
        inv.LefttInventory();

        if (WeaponSelected < maxWeapons - 1)
        {
            WeaponSelected++;
        }
    }

    IEnumerator StartSearching()
    {
        yield return new WaitForSeconds(timeToStart);
        startSearching = true;
    }

    IEnumerator TrueAbleToLunch()
    {
        yield return new WaitForSeconds(ableToThrow);
        ableToLunch = true;
        ableToRun = true;
    }

    IEnumerator DissapearAmmoCount()
    {
        yield return new WaitForSeconds(0.5f);
        timeCounter = 0;
        HUD.showTotalAmmo.gameObject.SetActive(false);
    }
}
