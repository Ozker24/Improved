using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image image;

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
        if (image != null)
        {
            if (GM.improved)
            {
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }

        laser.MyUpdate();
        flameThrower.MyUpdate();
        absorb.MyUpdate();

        percentage = Mathf.Clamp01(stamina / 100);

        if (image != null)
        {
            image.fillAmount = percentage;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }

        staminaMat.SetFloat("Vector1_8DF954C3", percentage);
    }
}
