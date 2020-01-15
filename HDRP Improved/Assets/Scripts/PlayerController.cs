using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Dependencies")]
    [SerializeField] CharacterController controler;
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
    public StealthSystem stealth;

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
    public float verticalSpeed;

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
    public AudioClip deadSound;

    [Header("Execution")]
    public bool canExecute;

    [Header("Stealth")]
    public bool beeingDetected;

    [Header("Dodge")]
    public float dodgeForce;
    public float dodgeResetTime;
    public bool dodge;
    public bool canDodge = true;

    [Header("Gravity Values")]
    private float forceToGround = Physics.gravity.y;
    public float gravityMagnitude = 1.0f;

    [Header("Sound")]
    public AudioSource CollectItemSource;

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
        stealth = GetComponentInChildren<StealthSystem>();

        speed = walkSpeed;

        cam = Camera.main;
        camTrans = cam.transform;
    }

    public void MyUpdate()
    {
        //GravitySimulation();

        aiming = aim.aim;

        /*if (WM.searchingBullet) stop = true;
        else stop = false;*/

        MovePlayer();

        if (!stop && !climb)
        {
            if (moving || (items.pressed && items.canDoItem && !items.inv.startCountdown) || aim.aim || CC.point)
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
        GravitySimulation();

        Vector3 localRot = axis.x * transform.right + axis.y * transform.forward;

        StablishSpeed();

        CalculateSpeeds();

        moveDir.x = localRot.x * speed;
        moveDir.z = localRot.z * speed;

        moveDir = RotateWithView();

        moveDir.y = verticalSpeed;
        //transform.localRotation = Quaternion.Euler(RotateWithView());

        controler.Move(moveDir * Time.deltaTime);

        anims.ForwardAnimation();

        if (axis.y != 0 || axis.x != 0) moving = true;
        else moving = false;

        if (axis.y > 0.1f && axis.x == 0) movingForward = true;
        else movingForward = false;
    }

    public Vector3 RotateWithView()
    {
        moveDir.y = 0;
        Vector3 dir = camTrans.TransformDirection(moveDir);
        dir.Set(dir.x, 0, dir.z);

        return dir.normalized * moveDir.magnitude;
    }

    void GravitySimulation()
    {
        if (controler.isGrounded && !dodge)
        {
            //Debug.Log(controler.isGrounded);
            verticalSpeed = forceToGround;
        }
        else
        {
            verticalSpeed += forceToGround * gravityMagnitude * Time.deltaTime; // constantemente se le suma acceleracion de la grabedad
            dodge = false;
        }
    }

    public void Dodge()
    {
        if (!dodge && controler.isGrounded && canDodge)
        {
            dodge = true;
            verticalSpeed = dodgeForce;
            StartCoroutine(ResetDodge());
            canDodge = false;
        }
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
        //if (!GM.godMode)
        //{
            if (moving && !crouching && !running) speed = walkSpeed;
            else if (running) speed = runSpeed;
            else if (crouching) speed = crouchSpeed;
            else if (stop) speed = 0;
        //}
    }

    #endregion

    public void Climb()
    {
        if (canClimb && !climb)
        {
            //anims.ClimbAnimation();
        }
    }

    /*public void Dodge()
    {
        Debug.Log("Dodge");
        dodgeDir = new Vector3 (moveDir.x, DodgeForce, moveDir.z);
        rb.AddForce(dodgeDir);
    }*/

    public void Collect()
    {
        if (itemDetector.closestItem != null && itemDetector.canGrab)
        {
            ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

            if (!itemCollected.ammo)
            {
                if (items.itemsCount[itemCollected.WhichItem] < items.maxItems)
                {
                    CheckItems();

                    itemDetector.closestItem.SetActive(false);
                    Destroy(itemDetector.closestItem, 10);
                    itemDetector.closestItem = null;
                    //HUD.actualLogo = null;

                    //PUY HERE SOUND
                }
            }

            else
            {
                if (WM.weapons[itemCollected.ForWhatGun].ammoReloaded + itemCollected.bullets < WM.weapons[itemCollected.ForWhatGun].maxAmmo)
                {
                    CheckAmmo();

                    itemDetector.closestItem.SetActive(false);
                    Destroy(itemDetector.closestItem, 10);
                    itemDetector.closestItem = null;
                    //HUD.actualLogo = null;

                    //PUY HERE SOUND
                }
            }
        }
    }

    public void CheckItems()
    {
        ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

        if (itemCollected.WhichItem == 4)
        {
            items.itemsCount[4]++;
            items.visualItemsCount[4]++;

            items.molotovVisuals[items.visualItemsCount[4] - 1].SetActive(true);
        }
        if (itemCollected.WhichItem == 3)
        {
            items.itemsCount[3]++;
            items.visualItemsCount[3]++;

            items.granadeVisuals[items.visualItemsCount[3] - 1].SetActive(true);
        }
        if (itemCollected.WhichItem == 2)
        {
            items.itemsCount[2]++;
            items.visualItemsCount[2]++;

            items.soundGranadeVisuals[items.visualItemsCount[2] - 1].SetActive(true);
        }
        if (itemCollected.WhichItem == 1)
        {
            items.itemsCount[1]++;
            items.visualItemsCount[1]++;

            items.firstAidVisuals[items.visualItemsCount[1] - 1].SetActive(true);
        }
        if (itemCollected.WhichItem == 0)
        {
            items.itemsCount[0]++;
            items.visualItemsCount[0]++;

            items.EMPVisuals[items.visualItemsCount[0] - 1].SetActive(true);
        }

        if (itemCollected.collectSound != null)
        {
            //AudioSource source = gameObject.AddComponent<AudioSource>();
            //CollectItemSource.clip = itemCollected.collectSound;
            //source.playOnAwake = false;

            CollectItemSource.PlayOneShot(itemCollected.collectSound);
            //Destroy(source, source.clip.length);
        }
    }

    public void CheckAmmo()
    {
        ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

        //if (itemCollected.ammo) WM.weapons[itemCollected.ForWhatGun].ammoReloaded += itemCollected.bullets;

        WM.weapons[itemCollected.ForWhatGun].ammoReloaded += itemCollected.bullets;

        if (itemCollected.collectSound != null)
        {
            CollectItemSource.PlayOneShot(itemCollected.collectSound);
        }
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
        GM.dead = true;
        PlaySound();
        GM.winLose.doFadeOut = true;
    }

    IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(dodgeResetTime);
        canDodge = true;
    }

    public void PlaySound()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = deadSound;
        source.PlayDelayed(0.3f);

        Destroy(source, source.clip.length);
    }
}
