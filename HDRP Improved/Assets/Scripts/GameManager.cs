using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private ImputManager inputs;
    [SerializeField]
    private CamaraAdjustment camera;
    [SerializeField]
    private PlayerAnimations playerAnims;
    [SerializeField]
    private HudManager HUD;
    [SerializeField]
    private Inventory inv;
    [SerializeField]
    private Items Items;
    [SerializeField]
    private SoundManager sound;
    [SerializeField]
    private Aiming Aim;
    [SerializeField]
    private WeaponManager weapon;
    [SerializeField]
    private HealthPeace peace;
    [SerializeField]
    private CloseCombat CC;
    [SerializeField]
    private HitArea Hit;
    [SerializeField]
    private ItemDetector itemDetector;
    [SerializeField]
    private ThrowImpact throwImpact;
    [SerializeField]
    private StealthSystem stealth;
    [SerializeField]
    private PauseManager pauseMn;
    [SerializeField]
    public TransitionWinLose winLose;

    public bool ableToInput = true;
    public bool pause;
    public bool showControls;
    public bool dead;
    public bool win;
    public bool options;

    [Header("Modes")]

    public bool improved;
    public bool godMode;
    //public bool detected = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inputs = GameObject.FindGameObjectWithTag("Managers").GetComponent<ImputManager>();
        camera = GameObject.FindGameObjectWithTag("TPCamera").GetComponent<CamaraAdjustment>();
        playerAnims = player.GetComponentInChildren<PlayerAnimations>();
        HUD = GetComponent<HudManager>();
        weapon = player.GetComponentInChildren<WeaponManager>();
        inv = GameObject.FindGameObjectWithTag("Inventory").GetComponentInChildren<Inventory>();
        Items = player.GetComponentInChildren<Items>();
        sound = GetComponent<SoundManager>();
        peace = player.GetComponentInChildren<HealthPeace>();
        CC = player.GetComponent<CloseCombat>();
        Hit = playerAnims.GetComponentInChildren<HitArea>();
        itemDetector = Items.GetComponent<ItemDetector>();
        throwImpact = player.GetComponentInChildren<ThrowImpact>();
        stealth = player.GetComponent<StealthSystem>();
        pauseMn = gameObject.GetComponent<PauseManager>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        player.Initialize();
        inputs.Initialize();
        weapon.Initialize();
        camera.Initialize();
        HUD.Initialize();
        playerAnims.Initialize();
        inv.Initialize();
        Items.Initialize();
        Aim.Initialize();
        peace.Initialize();
        CC.Initialize();
        Hit.Initialize();
        itemDetector.Initialize();
        stealth.Initialize();
        pauseMn.Initialize();
        //throwImpact.Initialize();
    }

    private void Update()
    {
        inputs.CheckSetPause();
        pauseMn.MyUpdate();

        if (!pause && !showControls && !options)
        {
            inputs.MyUpdate();
            player.MyUpdate();
            weapon.MyUpdate();
            camera.MyUpdate();
            HUD.MyUpdate();
            inv.MyUpdate();
            Items.MyUpdate();
            sound.MyUpdate();
            Aim.MyUpdate();
            peace.MyUpdate();
            CC.MyUpdate();
            itemDetector.MyUpdate();
            stealth.MyUpdate();
            winLose.MyUpdate();
            //throwImpact.MyUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (!pause)
        {
            playerAnims.MyFixedUpdate();
        }
    }
}