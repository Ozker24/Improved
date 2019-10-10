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
    }

    public void MyUpdate()
    {
        if (GM.ableToInput)
        {
            MovementImputs();
            ItemInputs();
            ChangeGun();
            ActiveItem();
            Aim();
            ReloadBullet();
            Shot();
            ChangeItem();
            Hit();
        }
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

    public void SetInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            player.Collect();
        }
    }

    public void ChangeGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            weapon.ChangeGunRight();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
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

    public void ReloadBullet()
    {
        if (Input.GetButton("Bullet"))
        {
            weapon.SetPressing();
        }

        if (Input.GetButtonUp("Bullet"))
        {
            weapon.SetRealised();
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

    public void ChangeItem()
    {
        ItemUp();
        ItemDown();
    }

    public void ItemUp()
    {
        if (Input.GetButtonDown("Item Up"))
        {
            inv.UpInventory();
        }
    }

    public void ItemDown()
    {
        if (Input.GetButtonDown("Item Down"))
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
}