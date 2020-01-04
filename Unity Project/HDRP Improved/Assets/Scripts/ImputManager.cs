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
    public WeaponManager weapon;
    public Inventory inv;
    public GameManager GM;
    public CloseCombat CC;
    public PauseManager pause;
    public GodModeManager god;

    public Vector2 axis;

    public void Initialize()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
        Items = player.GetComponentInChildren<Items>();
        //aim = player.GetComponent<Aiming>();
        weapon = player.GetComponentInChildren<WeaponManager>();
        CC = player.GetComponent<CloseCombat>();
        pause = GameObject.FindGameObjectWithTag("Managers").GetComponent<PauseManager>();
        god = GM.GetComponent<GodModeManager>();
    }

    public void MyUpdate()
    {
        if (GM.ableToInput)
        {
            MovementImputs();
            ItemInputs();
            ChangeGun();
            ActiveItem();
            CancelItem();
            Aim();
            Shot();
            ReleaseShot();
            ChangeItem();
            Hit();
            SetDodge();
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
            weapon.ChangeGunRight();
        }

        if (Input.GetButtonDown("Gun Left"))
        {
            weapon.ChangeGunLeft();
        }
    }

    public void ActiveItem()
    {
        if (Input.GetButtonDown("Item"))
        {
            Items.SetButtonPressed();
        }

        if (Input.GetButtonUp("Item"))
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
        if (Input.GetMouseButton(1))
        {
            Items.CancelItems();
        }
    }
    public void Shot()
    {
        if (Input.GetMouseButton(0))
        {
            weapon.Shot();
        }
        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }
    }

    public void ReleaseShot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            weapon.ReleaseShot();
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
}