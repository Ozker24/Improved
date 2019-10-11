using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controler;
    public PlayerAnimations anims;
    public HudManager HUD;
    public WeaponManager WM;
    public Aiming aim;
    public GameManager GM;

    [Header("Movement")]
    public bool moving;
    public bool movingForward;
    public bool walking;
    public bool running;
    public bool crouching;
    public bool stop;
    public bool climb;
    public bool aiming;

    public float speed;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float crouchSpeed = 2;

    public Vector2 axis = Vector2.zero;
    public Vector3 moveDir = Vector3.zero;

    [Header("Rotation")]
    public float angleRot;

    public Transform lookTarget;

    public Camera cam;
    public Transform camTrans;

    public Transform modelTrans;

    [Header("Climb")]
    public bool canClimb;
    public Vector3 endedClimb;

    [Header("Item")]
    public bool canCollect;
    public Items items;

    [Header("Combat")]
    public CloseCombat CC;

    public GameObject hitArea;

    public int fistDamage;

    public HealthPeace life;

    public void Initialize()
    {
        controler = GetComponent<CharacterController>();
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        anims = GetComponentInChildren<PlayerAnimations>();
        HUD = GameObject.FindGameObjectWithTag("Managers").GetComponent<HudManager>();
        items = GetComponentInChildren<Items>();
        WM = GetComponentInChildren<WeaponManager>();
        CC = GetComponent<CloseCombat>();
        life = GetComponentInChildren<HealthPeace>();


        speed = walkSpeed;

        cam = Camera.main;
        camTrans = cam.transform;
    }

    public void MyUpdate()
    {
        aiming = aim.aim;

        /*if (WM.searchingBullet) stop = true;
        else stop = false;*/

        MovePlayer();

        if (!stop && !climb)
        {
            if (moving || items.pressed || aim.aim || CC.point)
            {
                //modelTrans.rotation = camTrans.transform.rotation;
                Vector3 newPlayerForward = Vector3.ProjectOnPlane(camTrans.forward, Vector3.up);
                Quaternion newPlayerQuaternion = Quaternion.LookRotation(newPlayerForward, Vector3.up);
                modelTrans.rotation = newPlayerQuaternion;
            }
        }

        SetAnims();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climb")
        {
            canClimb = true;
        }
        if (other.tag == "Item")
        {
            canCollect = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climb")
        {
            canClimb = false;
        }
        if (other.tag == "Item")
        {
            canCollect = false;
        }
    }

    public void GetAxis(float x, float y)
    {
        if (stop)
        {
            axis.x = 0;
            axis.y = 0;
        }
        else
        {
            axis.x = x;
            axis.y = y;
        }
    }

    public void MovePlayer()
    {
        Vector3 localRot = axis.x * transform.right + axis.y * transform.forward;

        StablishSpeed();

        CalculateSpeeds();

        moveDir.x = localRot.x * speed;
        moveDir.z = localRot.z * speed;

        moveDir = RotateWithView();

        controler.Move(moveDir * Time.deltaTime);

        anims.ForwardAnimation();

        if (axis.y != 0 || axis.x != 0) moving = true;
        else moving = false;

        if (axis.y > 0.1f && axis.x == 0) movingForward = true;
        else movingForward = false;
    }

    public Vector3 RotateWithView()
    {
        Vector3 dir = camTrans.TransformDirection(moveDir);
        dir.Set(dir.x, 0, dir.z);

        return dir.normalized * moveDir.magnitude;
    }

    #region Speeds

    public void Crouch()
    {
        crouching = !crouching;
    }

    public void Run(bool state)
    {
        if (movingForward)
        {
            if (!stop && WM.ableToRun)
            {
                running = state;
            }
        }
        else
        {
            running = false;
        }
    }

    public void StablishSpeed()
    {
        if (stop && moving)
        {
            moving = false;
        }

        if (stop && running)
        {
            running = false;
        }

        if (running && aiming)
        {
            running = false;
        }

        if (moving && !running && !crouching && !stop) walking = true;
        else walking = false;

        if (crouching && running && moving)
        {
            crouching = false;
        }
    }

    public void CalculateSpeeds()
    {
        if (moving && !crouching && !running) speed = walkSpeed;
        else if (running) speed = runSpeed;
        else if (crouching) speed = crouchSpeed;
        else if (stop) speed = 0;
    }

    #endregion

    public void Climb()
    {
        if (canClimb && !climb)
        {
            anims.ClimbAnimation();
        }
    }

    public void Collect()
    {
        if (HUD.actualItem != null && canCollect)
        {
            CheckItem();
            HUD.actualItem.SetActive(false);
            Destroy(HUD.actualItem, 10);
            HUD.actualItem = null;
            HUD.actualLogo = null;
        }
    }

    public void CheckItem()
    {
        if (HUD.whatItem == 4) items.molotovCount++;
        if (HUD.whatItem == 3) items.GranadeCount++;
        if (HUD.whatItem == 2) items.SoundCount++;
        if (HUD.whatItem == 1) items.FirstAidCount++;
        if (HUD.whatItem == 0) items.InjectionCount++;
    }

    public void SetAnims()
    {
        anims.CrouchAnimation(crouching);
        anims.RunAnimation(running);
        anims.WalkAnimation(walking);
    }

    public void Damage(float damage)
    {
        life.health -= damage;

        if (life.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
