using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeManager : MonoBehaviour
{
    public PlayerController player;
    public GameManager GM;
    public WeaponManager WM;
    public HudManager Hud;
    public Items items;
    public HealthPeace health;

    [Header("Old Variables")]
    public float oldSpeed;
    public float oldDodgeForce;
    public float[] oldChangeGunTime;
    public float[] oldGunsDistance;
    public int oldHitDamage;
    public float oldDodgeResetTime;

    [Header ("new Variables")]
    public float godSpeed;
    public float godDodgeForce;
    public int addBulletsCount;
    public float godGunsDistance;

    [Header("God States")]
    public bool imposibleToDetect;

    public void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WM = player.GetComponentInChildren<WeaponManager>();
        Hud = GetComponent<HudManager>();
        items = player.GetComponentInChildren<Items>();
        health = player.GetComponentInChildren<HealthPeace>();

        for (int i = 0; i < WM.weapons.Length; i++)
        {
            oldGunsDistance[i] = WM.weapons[i].fireDist;
            oldChangeGunTime[i] = WM.timeAbleGun[i];
        }

        oldSpeed = player.runSpeed;
        oldDodgeForce = player.dodgeForce;
        oldDodgeResetTime = player.timeOfDodge;
        oldHitDamage = player.normalModeFistDamage;
}

    public void Update()
    {
        if (GM.godMode)
        {
            SecretCommands();

            if (imposibleToDetect)
            {
                player.stealth.canBeDetected = false;
            }
        }
    }

    public void SetGodMode()
    {
        GM.godMode =! GM.godMode;

        if (GM.godMode)
        {
            SetNewInfo();
        }
        else
        {
            SetOldInfo();
        }
    }

    public void SetNewInfo()
    {
        player.runSpeed = godSpeed;
        player.dodgeForce = godDodgeForce;
        //player.fistDamage = 3;
        player.timeOfDodge = 0;
        player.normalModeFistDamage = 3;

        for (int i = 0; i < WM.weapons.Length; i++)
        {
            WM.weapons[i].fireDist = godGunsDistance;
            WM.weapons[i].bulletDamage = 3;
            WM.timeAbleGun[i] = 0;
        }
    }

    public void SetOldInfo()
    {
        player.dodgeForce = oldDodgeForce;
        player.runSpeed = oldSpeed;
        //player.fistDamage = oldHitDamage;
        player.timeOfDodge = oldDodgeResetTime;
        player.normalModeFistDamage = oldHitDamage;
        for (int i = 0; i < WM.weapons.Length; i++)
        {
            WM.weapons[i].fireDist = oldGunsDistance[i];
            WM.weapons[i].bulletDamage = 1;
            WM.timeAbleGun[i] = oldChangeGunTime[i];
        }
    }

    public void SecretCommands()
    {
        AddBullets();
        AddItems();
        SetImposibleToDetect();
        AddLife();
    }

    public void AddBullets()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            WM.weapons[WM.WeaponSelected].ammoReloaded += addBulletsCount;

            Hud.UpdateInventory();
        }
    }

    public void AddItems()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (items.itemSelected == 4)
            {
                items.itemsCount[4]++;
            }
            else if (items.itemSelected == 3)
            {
                items.itemsCount[3]++;
            }
            else if (items.itemSelected == 2)
            {
                items.itemsCount[2]++;
            }
            else if (items.itemSelected == 1)
            {
                items.itemsCount[1]++;
            }
            else if (items.itemSelected == 0)
            {
                items.itemsCount[0]++;
            }

            Hud.UpdateInventory();
        }
    }

    public void SetImposibleToDetect()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            imposibleToDetect =! imposibleToDetect;
        }
    }

    public void AddLife()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            health.Health();
        }
    }
}
