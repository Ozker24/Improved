using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedWeaponManager : MonoBehaviour
{
    [Header ("Dependencies")]
    public GameManager GM;
    public PlayerController player;
    public LaserGun laser;
    public FlameThrower flameThrower;
    public HyperJump hJump;
    public HyperDash hDash;
    public MisileLauncher mLauncher;
    public Absorb absorb;

    [Header("Stadistics")]
    public float improvedSpeed;

    [Header("Stamina")]
    [SerializeField] Material staminaMat;
    public float stamina = 100;
    public float percentage;
    public float staminaMultiply;
    public float staminaFastMultiply;
    public float staminaSlowMultiply;
    public bool addConstantStamina;
    public bool speedStamina;

    [Header("Life")]
    public float lifeMultiply;
    public float lifeFastMultiply;
    public float lifeSlowMultiply;
    public bool addConstantLife;
    public bool speedLife;

    [Header ("Using Weapons")]
    public bool usingLaserGun;
    public bool usingFlameThrower;
    public bool usingHyperJump;
    public bool usingHyperDash;
    public bool usingMisileLaucher;
    public bool absorbing;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        laser = GetComponentInChildren<LaserGun>();
        flameThrower = GetComponentInChildren<FlameThrower>();
        hJump = GetComponentInChildren<HyperJump>();
        hDash = GetComponentInChildren<HyperDash>();
        mLauncher = GetComponentInChildren<MisileLauncher>();
        absorb = GetComponentInChildren<Absorb>();
        laser.Initialize();
        flameThrower.Initialize();
        hJump.Initialize();
        hDash.Initialize();
        mLauncher.Initialize();
        absorb.Initialize();
    }

    public void MyUpdate()
    {
        laser.MyUpdate();
        flameThrower.MyUpdate();
        hDash.MyUpdate();
        absorb.MyUpdate();

        percentage = Mathf.Clamp01(stamina / 100);

        if (stamina < 0)
        {
            stamina = 0;
        }

        ConstantGainStamina();
        ConstantGainLife();

        staminaMat.SetFloat("Vector1_8DF954C3", percentage);
    }

    void ConstantGainLife()
    {
        if (speedLife)
        {
            lifeMultiply = lifeFastMultiply;
        }
        else
        {
            lifeMultiply = lifeSlowMultiply;
        }

        if (addConstantLife && GM.improved)
        {
            if (player.life.health < 100)
            {
                player.life.health += Time.deltaTime * lifeMultiply;
            }
            else if(player.life.health >= 100)
            {
                stamina = 100;
            }
        }
    }

    void ConstantGainStamina()
    {
        if (speedStamina)
        {
            staminaMultiply = staminaFastMultiply;
        }
        else
        {
            staminaMultiply = staminaSlowMultiply;
        }

        if (addConstantStamina && GM.improved)
        {
            if (stamina < 100)
            {
                stamina += Time.deltaTime * staminaMultiply;
            }
            else if (stamina >= 100)
            {
                stamina = 100;
            }
        }
    }
}
