using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    public HudManager HUD;
    public Items Items;
    public Aiming aim;
    public WeaponManager WM;
    public Inventory inv;
    public GameManager GM;
    public CloseCombat CC;
    public PauseManager pause;
    public GodModeManager god;
    public ImprovedWeaponManager IWM;

    public Vector2 axis;

    public void Initialize()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
        Items = player.GetComponentInChildren<Items>();
        WM = player.GetComponentInChildren<WeaponManager>();
        CC = player.GetComponent<CloseCombat>();
        pause = GameObject.FindGameObjectWithTag("Managers").GetComponent<PauseManager>();
        god = GM.GetComponent<GodModeManager>();
        IWM = player.GetComponentInChildren<ImprovedWeaponManager>();
    }

    public void MyUpdate()
    {
        if (GM.ableToInput)
        {
            MovementImputs();

            if (!GM.improved)
            {
                ItemInputs();
                ChangeGun();
                ActiveItem();
                CancelItem();
                Shot();
                ReleaseShot();
                ChangeItem();
                SetDodge();
                //ShortcutGuns();
            }
            else
            {
                LaserGunInputs();
                FlameThrowerImputs();
                //HyperJumpImputs();
                Fall();
                HyperDash();
                LaunchMisile();
                AbsorbImputs();
            }

            Hit();
            Aim();
            GodMode();
        }
    }

    public void CheckSetPause()// an exclusive Update so when the game is paused the GM still reads the imput
    {
        SetPause();
    }

    public void MovementImputs()
    {
        SetAxis();
        SetRun();
        SetCroach();
        SetClimb();
    }

    public void ItemInputs()
    {
        SetInteract();
    }

    public void SetAxis()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");

        player.GetAxis(this.axis.x, this.axis.y);
    }

    public void SetCroach()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            player.Crouch();
        }
    }

    public void SetRun()
    {
        if (Input.GetButton("Run"))
        {
            player.Run(true);
        }
        else if (Input.GetButtonUp("Run"))
        {
            player.Run(false);
        }
    }

    public void SetClimb()
    {
        if (Input.GetButtonDown("Climb"))
        {
            player.Climb();
        }
    }

    public void SetDodge()
    {
        if (Input.GetButtonDown("Climb"))
        {
            player.Dodge();
        }
    }

    public void SetInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            player.Collect();
        }
    }

    public void ChangeGun()
    {
        if (Input.GetButtonDown("Gun Right"))
        {
            WM.ChangeGunRight();
        }

        if (Input.GetButtonDown("Gun Left"))
        {
            WM.ChangeGunLeft();
        }
    }

    public void ActiveItem()
    {
        if (Input.GetButtonDown("Item"))
        {
            Items.SetButtonPressed();
        }

        if (Input.GetMouseButton(0))
        {
            Items.SetButtonRealised();
        }
    }

    public void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            aim.Aim();
        }
        if (Input.GetMouseButtonUp(1))
        {
            aim.NotAim();
        }
    }

    public void CancelItem()
    {
        if (Input.GetButtonUp("Item"))
        {
            Items.CancelItems();
        }
    }
    public void Shot()
    {
        if (Input.GetMouseButton(0))
        {
            WM.Shot();
        }
        if (Input.GetButtonDown("Reload"))
        {
            WM.Reload();
        }
    }

    public void ReleaseShot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            WM.ReleaseShot();
        }
    }

    public void ChangeItem()
    {
        ItemUp();
        ItemDown();
    }

    public void ItemUp()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            inv.UpInventory();
        }
    }

    public void ItemDown()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            inv.DownInventory();
        }
    }

    public void Hit()
    {
        if (Input.GetButtonDown("Hit"))
        {
            CC.DoHit();
        }
    }

    public void SetPause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            pause.Pause();
        }
    }

    public void GodMode()
    {
        if (Input.GetButtonDown("GodMode"))
        {
            god.SetGodMode();
        }
    }

    public void LaserGunInputs()
    {
        //ShotSemiAutoLaserGun();
        ResetSemiAutoLaserGun();
        ShotChargedLaserGun();
    }

    public void ResetSemiAutoLaserGun()
    {
        if (Input.GetMouseButtonUp(0))
        {
            IWM.laser.ResetLaserGun();
        }
    }

    public void ShotChargedLaserGun()
    {
        if (Input.GetMouseButton(0))
        {
            IWM.laser.ChargingLaserGun();
        }
    }

    public void FireFlameThrower()
    {
        if (Input.GetButton("Flame Thrower"))
        {
            IWM.flameThrower.Fire();
        }
    }

    public void ReleaseFlameThrower()
    {
        if(Input.GetButtonUp("Flame Thrower"))
        {
            IWM.flameThrower.ReleaseFire();
        }
    }

    public void FlameThrowerImputs()
    {
        FireFlameThrower();
        ReleaseFlameThrower();
    }

    public void ChargeHyperJump()
    {
        if(Input.GetButton("HyperJump"))
        {
            IWM.hJump.ChargeHyperJump();
        }
    }

    public void ReleaseHyperJump()
    {
        if (Input.GetButtonUp("HyperJump"))
        {
            IWM.hJump.ReleaseHyperJump();
        }
    }

    public void HyperJumpImputs()
    {
        ChargeHyperJump();
        ReleaseHyperJump();
    }

    public void Fall()
    {
        if (Input.GetButton("HyperJump"))
        {
            IWM.hJump.DoFall();
        }
    }

    public void HyperDash()
    {
        if (Input.GetButtonDown("HyperDash"))
        {
            IWM.hDash.DoDash();
        }
    }

    public void LaunchMisile()
    {
        if (Input.GetButtonDown("Misile"))
        {
            IWM.mLauncher.LauchMisile();
        }
    }

    public void Absorb()
    {
        if (Input.GetButtonDown("Absorb"))
        {
            IWM.absorb.DoAbsorb();
        }
    }

    public void ReleaseAbsorb()
    {
        if (Input.GetButtonUp("Absorb"))
        {
            IWM.absorb.ReleaseAbsorb();
        }
    }

    public void AbsorbImputs()
    {
        Absorb();
        ReleaseAbsorb();
    }

    public void ShortcutGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WM.weaponPreSelected = 0;
            WM.GunShortcuts();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WM.weaponPreSelected = 1;
            WM.GunShortcuts();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WM.weaponPreSelected = 2;
            WM.GunShortcuts();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WM.weaponPreSelected = 3;
            WM.GunShortcuts();
        }
    }
}