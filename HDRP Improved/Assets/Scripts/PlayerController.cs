using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Dependencies")]
    public CharacterController controler;
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
    public ImprovedWeaponManager IWM;
    public PlayerSave playerSave;

    [Header("States")]
    public bool moving;
    public bool movingForward;
    public bool walking;
    public bool running;
    public bool crouching;
    public bool stop;
    public bool climb;
    public bool aiming;
    public bool graving;

    [Header("Speed Values")]
    public float speed;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float crouchSpeed = 2;

    [Header("Movement")]
    public Vector2 axis = Vector2.zero;
    public Vector3 moveDir = Vector3.zero;
    public float verticalSpeed;
    [SerializeField] float movingForwardXOffset;

    [Header("Rotation")]
    public Camera cam;
    public Transform lookTarget;
    public Transform camTrans;
    public Transform modelTrans;
    public float angleRot;

    [Header("Climb")]
    public Transform climbRayTrans;
    public Vector3 endedClimb;
    public GameObject instantiate;
    [SerializeField] float climbTime;
    public bool canClimb;

    [Header("Crouch")]
    [SerializeField] float crouchHeight;
    [SerializeField] float normalHeight;
    [SerializeField] float charControllercenterOffset;

    [Header("Combat")]
    public int normalModeFistDamage;
    public int improvedModeFistDamage;

    public int normalModeMaxFists;
    public int improvedModeMaxFists;
    public AudioClip deadSound;

    [Header("Execution")]
    public bool canExecute;

    [Header("Stealth")]
    public bool beeingDetected;

    [Header("Dodge")]
    [SerializeField] Vector2 axisOnDodge;
    public float dodgeForce;
    public float dodgeSpeed;
    public float dodgeNormalModeSpeed;
    public float timeOfDodge;
    public float timeOfDodgeNormalMode;
    public float timeForCanDodge;
    public float timeForCanDodgeNormalMode;
    public bool dodgeForGravity;
    public bool canDodge = true;
    public bool dodging = true;

    [Header("Collect")]
    public float gravingTime;

    [Header("Gravity Values")]
    private float forceToGround = Physics.gravity.y;
    public float gravityMagnitude = 1.0f;

    [Header("Sound")]
    public AudioSource baseSource;

    [Header("Respawn")]
    public Vector3 spawnPos;

    public void Initialize()
    {
        //transform.position = spawnPos;

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
        IWM = GetComponentInChildren<ImprovedWeaponManager>();

        speed = walkSpeed;

        cam = Camera.main;
        camTrans = cam.transform;

        dodgeSpeed = dodgeNormalModeSpeed;
        timeOfDodge = timeOfDodgeNormalMode;
        timeForCanDodge = timeForCanDodgeNormalMode;

        normalHeight = controler.height;
        charControllercenterOffset = (normalHeight - crouchHeight) / 2;

        LoadPlayer();
    }

    public void MyUpdate()
    {
        aiming = aim.aim;
        MovePlayer();
        RotateModel();
        SetAnims();
    }

    public void RotateModel()
    {
        if (!stop && !climb && !items.doTrayectoryInDodge)
        {
            if (moving || (items.pressed && items.canDoItem && !items.inv.startCountdown) || aim.aim || CC.point || IWM.usingFlameThrower || IWM.usingLaserGun)
            {
                //if (!items.canDoItem) return;
                //modelTrans.rotation = camTrans.transform.rotation;
                Vector3 newPlayerForward = Vector3.ProjectOnPlane(camTrans.forward, Vector3.up);
                Quaternion newPlayerQuaternion = Quaternion.LookRotation(newPlayerForward, Vector3.up);
                modelTrans.rotation = newPlayerQuaternion;
            }
        }
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

        if (dodging)
        {
            axis = axisOnDodge;
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

        if (axis.y > 0.1f && axis.x <= movingForwardXOffset && axis.x >= -movingForwardXOffset) movingForward = true;
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
        if (controler.isGrounded && !dodgeForGravity && !IWM.hJump.jump)
        {
            verticalSpeed = forceToGround;
        }
        else
        {
            verticalSpeed += forceToGround * gravityMagnitude * Time.deltaTime; // constantemente se le suma acceleracion de la grabedad
            dodgeForGravity = false;
            IWM.hJump.jump = false;
            IWM.hJump.didHyperJump = false;
            IWM.hJump.falling = false;
        }
    }

    public void Dodge()
    {
        if (!dodgeForGravity && controler.isGrounded && canDodge && !dodging && !canClimb && !climb)
        {
            dodgeForGravity = true;
            axisOnDodge = axis;

            crouching = false;

            if (axisOnDodge == Vector2.zero)
            {
                axisOnDodge = Vector2.down;
            }

            Debug.Log(axisOnDodge);

            canDodge = false;
            dodging = true;

            items.doTrayectoryInDodge = true;

            StartCoroutine(StopDodging());
            StartCoroutine(ResetDodge());
        }
    }

    #region Speeds

    public void Crouch()
    {
        crouching = !crouching;

        if (crouching)
        {
            controler.height = crouchHeight;
            controler.center = new Vector3(controler.center.x, controler.center.y - charControllercenterOffset, controler.center.z);
        }
        else
        {
            controler.height = normalHeight;
            controler.center = new Vector3(controler.center.x, controler.center.y + charControllercenterOffset, controler.center.z);
        }
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
        if(!GM.improved)
        {
            if (moving && !crouching && !running) speed = walkSpeed;
            else if (running) speed = runSpeed;
            else if (crouching) speed = crouchSpeed;
            else if (stop) speed = 0;
        }
        else
        {
            speed = IWM.improvedSpeed;
        }

        if (dodging)
        {
            speed = dodgeSpeed;
        }
    }

    #endregion

    public void Climb()
    {
        if (canClimb && !climb && !dodging)
        {
            climb = true;
            GM.ableToInput = false;
            axis = new Vector2(0, 0);
            controler.detectCollisions = false;

            RaycastHit hit = new RaycastHit();// que hemos golpeado primero
            Physics.Raycast(climbRayTrans.position, Vector3.down, out hit);
            Debug.Log(hit.transform.name);

            //Instantiate(instantiate, hit.point, Quaternion.identity);
            anims.SetAnimClimb();

            endedClimb = hit.point;

            StartCoroutine(ResetClimb());
        }
    }

    public void Collect()
    {
        if (itemDetector.closestItem != null && itemDetector.canGrab)
        {
            ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

            graving = true;
            GM.ableToInput = false;
            StartCoroutine(ResetGraving());

            if (itemCollected.type == ItemBase.ItemType.Item)
            {
                if (items.itemsCount[itemCollected.indexValue] < items.maxItems)
                {
                    CheckItems();

                    itemDetector.closestItem.SetActive(false);
                    Destroy(itemDetector.closestItem, 10);
                    itemDetector.closestItem = null;
                    //HUD.actualLogo = null;

                    //PUY HERE SOUND
                }
            }

            else if (itemCollected.type == ItemBase.ItemType.Ammo)
            {
                //if (WM.ammoCollected[itemCollected.indexValue] + itemCollected.bullets < WM.weapons[itemCollected.indexValue].maxAmmo)
                CheckAmmo();

                itemDetector.closestItem.SetActive(false);
                Destroy(itemDetector.closestItem, 10);
                itemDetector.closestItem = null;
                //HUD.actualLogo = null;

                //PUT HERE SOUND
            }

            else if (itemCollected.type == ItemBase.ItemType.Gun)
            {
                
                if(!WM.weaponsType[itemCollected.indexValue].Slot)
                {
                    WM.ChangeStats(0, itemCollected.indexValue);
                }
                else
                {
                    WM.ChangeStats(1, itemCollected.indexValue);
                }

                itemDetector.closestItem.SetActive(false);
                Destroy(itemDetector.closestItem, 10);
                itemDetector.closestItem = null;
            }
        }
    }

    public void CheckItems()
    {
        ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

        if (itemCollected.indexValue == 4)
        {
            items.itemsCount[4]++;
            items.visualItemsCount[4]++;

            items.molotovVisuals[items.visualItemsCount[4] - 1].SetActive(true);
        }
        if (itemCollected.indexValue == 3)
        {
            items.itemsCount[3]++;
            items.visualItemsCount[3]++;

            items.granadeVisuals[items.visualItemsCount[3] - 1].SetActive(true);
        }
        if (itemCollected.indexValue == 2)
        {
            items.itemsCount[2]++;
            items.visualItemsCount[2]++;

            items.soundGranadeVisuals[items.visualItemsCount[2] - 1].SetActive(true);
        }
        if (itemCollected.indexValue == 1)
        {
            items.itemsCount[1]++;
            items.visualItemsCount[1]++;

            items.firstAidVisuals[items.visualItemsCount[1] - 1].SetActive(true);
        }
        if (itemCollected.indexValue == 0)
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

            baseSource.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
            baseSource.PlayOneShot(itemCollected.collectSound);
            //Destroy(source, source.clip.length);
        }
    }

    public void CheckAmmo()
    {
        ItemBase itemCollected = itemDetector.closestItem.GetComponent<ItemBase>();

        //if (itemCollected.ammo) WM.weapons[itemCollected.ForWhatGun].ammoReloaded += itemCollected.bullets;

        WM.ammoCollected[itemCollected.indexValue] += itemCollected.bullets;

        if (itemCollected.collectSound != null)
        {
            baseSource.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
            baseSource.PlayOneShot(itemCollected.collectSound);
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
        anims.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anims.SetAnimDead();
        baseSource.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
        baseSource.ChangePitchAndVolume(0.7f, 1, 0.95f, 1.05f);
        baseSource.PlayOneShot(deadSound);
        //PlaySound();
        GM.winLose.doFadeOut = true;
    }

    IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(timeForCanDodge);
        canDodge = true;
    }

    IEnumerator StopDodging()
    {
        yield return new WaitForSeconds(timeOfDodge);
        dodging = false;
        axisOnDodge = Vector2.zero;
        items.doTrayectoryInDodge = false;
        IWM.usingHyperDash = false;
    }

    IEnumerator ResetClimb()
    {
        yield return new WaitForSeconds(climbTime);
        climb = false;
        controler.detectCollisions = true;
        GM.ableToInput = true;
        transform.position = endedClimb;
    }

    IEnumerator ResetGraving()
    {
        yield return new WaitForSeconds(gravingTime);
        GM.ableToInput = true;
        graving = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(climbRayTrans.position, Vector3.down);
        Gizmos.color = Color.red;
    }

    public void LoadPlayer()
    {
        PlayerSave data = PlayerSaveSystem.LoadPlayer();

        if (data != null && !GM.improved)
        {
            spawnPos.x = data.spawnPos[0];
            spawnPos.y = data.spawnPos[1];
            spawnPos.z = data.spawnPos[2];
            transform.position = spawnPos;

            GM.checkpointsManager.actualCheckPoint = data.actualSpawnPoint;

            life.health = data.health;
            items.itemsCount[4] = data.molotovs;
            items.itemsCount[3] = data.grenades;
            items.itemsCount[2] = data.sounds;
            items.itemsCount[1] = data.kits;
            items.itemsCount[0] = data.EMPs;

            WM.weapons[0].currentAmmo = data.pistolInMagazine;
            WM.weapons[0].ammoReloaded = data.pistolReloaded;
            WM.weapons[1].currentAmmo = data.shotgunInMagazine;
            WM.weapons[1].ammoReloaded = data.shotgunReloaded;
            //WM.weapons[2].currentAmmo = data.subInMagazine;
            //WM.weapons[2].ammoReloaded = data.subReloaded;
            //WM.weapons[3].currentAmmo = data.rifleInMagazine;
            //WM.weapons[3].ammoReloaded = data.rifleReloaded;
        }
    }
}
