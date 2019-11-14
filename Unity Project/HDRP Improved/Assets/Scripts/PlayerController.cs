using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Dependencies")]
    [SerializeField]
    private CharacterController controler;
    public PlayerAnimations anims;
    public HudManager HUD;
    public WeaponManager WM;
    public Aiming aim;
    public GameManager GM;
    public GameObject hitArea;
    public HealthPeace life;
    public Items items;
    public CloseCombat CC;
    public ItemDetector itemDetector;

    [Header("States")]
    public bool moving;
    public bool movingForward;
    public bool walking;
    public bool running;
    public bool crouching;
    public bool stop;
    public bool climb;
    public bool aiming;

    [Header("Speed Values")]
    public float speed;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float crouchSpeed = 2;

    [Header("Movement")]
    public Vector2 axis = Vector2.zero;
    public Vector3 moveDir = Vector3.zero;

    [Header("Rotation")]
    public Camera cam;
    public float angleRot;
    public Transform lookTarget;
    public Transform camTrans;
    public Transform modelTrans;

    [Header("Climb")]
    public bool canClimb;
    public Vector3 endedClimb;

    [Header("Combat")]
    public int fistDamage;

    [Header("Dodge")]
    public Rigidbody rb;
    public Vector3 dodgeDir;
    public float DodgeForce;

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
        itemDetector = items.GetComponent<ItemDetector>();
        rb = GetComponent<Rigidbody>();

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
            if (moving || (items.pressed && items.canDoItem) || aim.aim || CC.point)
            {
                //if (!items.canDoItem) return;
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
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climb")
        {
            canClimb = false;
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
            if (!stop)
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

    public void Dodge()
    {
        Debug.Log("Dodge");
        dodgeDir = new Vector3 (moveDir.x, DodgeForce, moveDir.z);
        rb.AddForce(dodgeDir);
    }

    public void Collect()
    {
        if (itemDetector.closestItem != null && itemDetector.canGrab)
        {
            CheckItem();
            itemDetector.closestItem.SetActive(false);
            Destroy(itemDetector.closestItem, 10);
            itemDetector.closestItem = null;
            //HUD.actualLogo = null;
        }
    }

    public void CheckItem()
    {
        if (itemDetector.closestItem.GetComponent<ItemBase>().WhichItem == 4) items.molotovCount++;
        if (itemDetector.closestItem.GetComponent<ItemBase>().WhichItem == 4) items.GranadeCount++;
        if (itemDetector.closestItem.GetComponent<ItemBase>().WhichItem == 4) items.SoundCount++;
        if (itemDetector.closestItem.GetComponent<ItemBase>().WhichItem == 4) items.FirstAidCount++;
        if (itemDetector.closestItem.GetComponent<ItemBase>().WhichItem == 4) items.EmpCount++;
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
