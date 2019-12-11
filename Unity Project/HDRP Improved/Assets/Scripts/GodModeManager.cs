using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeManager : MonoBehaviour
{
    public PlayerController player;
    public GameManager GM;
    public WeaponManager WM;
    public HudManager Hud;

    [Header("Old Variables")]
    public float oldSpeed;
    public float oldDodgeForce;

    [Header ("new Variables")]
    public float godSpeed;
    public float godDodgeForce;
    public int addBulletsCount;

    public void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WM = player.GetComponentInChildren<WeaponManager>();
        Hud = GetComponent<HudManager>();

        oldSpeed = player.runSpeed;
        oldDodgeForce = player.dodgeForce;
    }

    public void Update()
    {
        SecretCommands();
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
    }

    public void SetOldInfo()
    {
        player.dodgeForce = oldDodgeForce;
        player.runSpeed = oldSpeed;
    }

    public void SecretCommands()
    {
        AddBullets();
    }

    public void AddBullets()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            WM.weapons[WM.WeaponSelected].ammoReloaded += addBulletsCount;

            Hud.UpdateInventory();
        }
    }
}
