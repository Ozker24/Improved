using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Header ("Dependencies")]
    public PlayerController player;
    public WeaponManager WM;
    public Inventory inv;
    public Animator healthAnim;
    public HealthPeace peace;
    public Camera cam;
    public AudioPlayer audPlay;
    public Trajectory trajectory;
    public GameObject trajectoryPrefab;

    [Header("Imputs")]
    public int itemSelected;
    public bool pressed;
    public int realised;

    [Header("Time Variables")]
    public float timeFirstAid;
    public float TimeCounter;
    public bool interrupted;
    public bool firstTime;

    [Header("Prefabs")]
    public GameObject GranadePrefab;
    public GameObject MolotovPrefab;
    public GameObject SoundPrefab;
    public GameObject empPrefab;

    [Header("Throw Variables")]
    public Transform throwTrans;
    public float throwForce;

    [Header("Items Count")]
    public int molotovCount;
    public int GranadeCount;
    public int SoundCount;
    public int FirstAidCount;
    public int EmpCount;

    [Header("Cooldown Variables")]
    public bool canDoItem = true;
    public float waitTimeItem; //no ha de ser mayor que waitTimeGun

    public bool canDoGun = true; //Lo llamaremos por animacion
    public float waitTimeGun; //no ha de ser mayor que waitTimeItem
    /*
        4 molotov apuntar y disparar
        3 granade apuntar y disparar
        2 soundobjecct apuntar y disparar
        1 firtsAidKit usar hasta final
        0 injectofnanobots usar hasta final

     */

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        WM = player.GetComponentInChildren<WeaponManager>();

        cam = Camera.main;
        peace = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<HealthPeace>();
        audPlay = GetComponentInChildren<AudioPlayer>();

        trajectoryPrefab.SetActive(false);
        trajectory = trajectoryPrefab.GetComponent<Trajectory>();
    }

    public void MyUpdate()
    {
        itemSelected = inv.actualItem;

        trajectory.direction = cam.transform.forward;
        trajectory.velocity = throwForce;

        if (canDoItem) ActiveItem();

        if (pressed && canDoItem && itemSelected != 1)
        {
            trajectoryPrefab.SetActive(true);
        }
    }

    public void ActiveItem()
    {
        if (!player.stop && !player.climb)
        {
            Molotov();
            Granade();
            SoundItem();
            FirstAid();
            EMP();
        }
    }

    public void SetButtonPressed()
    {
        pressed = true;

        if (canDoItem && itemSelected != 1)
        {
            trajectoryPrefab.SetActive(true);
        }
    }

    public void SetButtonRealised()
    {
        realised = 1;
        StartCoroutine(RealisedFalse());
        pressed = false;

        if (canDoItem)
        {
            trajectoryPrefab.SetActive(false);
            trajectory.drawing = false;
        }
    }

    IEnumerator RealisedFalse()
    {
        yield return new WaitForEndOfFrame();
        realised = 0;
    }

    #region WhatItemsDo
    public void Molotov()
    {
        if (itemSelected == 4 && pressed && molotovCount > 0)
        {
            if (!firstTime)
            {
                Debug.Log("M");
                //audio de apuntando
                firstTime = true;
            }

            Debug.Log("Apunting with Molotov");
        }

        if (itemSelected == 4 && realised == 1 && molotovCount > 0)
        {
            Debug.Log("Launched molotov");

            audPlay.Play(5, 1, Random.Range(0.95f, 1.05f)); // esta antes por temas de velocidad del audio
            InstantiateThings(MolotovPrefab);

            CantGunCantItem(false, false, false);
        }
    }

    public void Granade()
    {
        if (itemSelected == 3 && pressed && GranadeCount > 0)
        {
            if (!firstTime)
            {
                Debug.Log("G");
                //audio de apuntando
                firstTime = true;
            }

            Debug.Log("Apunting with Granade");
        }

        if (itemSelected == 3 && realised == 1 && GranadeCount > 0)
        {
            Debug.Log("Launched Granade");

            InstantiateThings(GranadePrefab);

            audPlay.Play(6, 1, Random.Range(0.95f, 1.05f));

            CantGunCantItem(false, false, false);
        }
    }

    public void SoundItem()
    {
        if (itemSelected == 2 && pressed && SoundCount > 0)
        {
            if (!firstTime)
            {
                Debug.Log("S");
                //audio de apuntando
                firstTime = true;
            }

            Debug.Log("Apunting with Sound");
        }

        if (itemSelected == 2 && realised == 1 && SoundCount > 0)
        {
            Debug.Log("Launched Sound");
            InstantiateThings(SoundPrefab);

            CantGunCantItem(false, false, false);
        }
    }

    public void FirstAid()
    {
        if (itemSelected == 1 && FirstAidCount > 0)
        {
            if (pressed && realised != 1)
            {
                if (!firstTime)
                {
                    Debug.Log("F");
                    audPlay.Play(3, 1, Random.Range(0.95f, 1.05f));
                    firstTime = true;
                }

                if (TimeCounter >= timeFirstAid)
                {
                    Debug.Log("Healed");
                    healthAnim.SetBool("Health", false);
                    TimeCounter = 0;

                    peace.Health();

                    AudioSource source = GetComponentInChildren<AudioSource>();

                    if (source != null)
                    {
                        Destroy(source);
                    }

                    audPlay.Play(4, 1, Random.Range(0.95f, 1.05f));

                    CantGunCantItem(false, false, false);
                }
                else
                {
                    TimeCounter += Time.deltaTime;
                    healthAnim.SetBool("Health", true);
                }
            }

            if (realised == 1)
            {
                Debug.Log("Interrumpted");

                AudioSource source = GetComponentInChildren<AudioSource>();

                if (source != null)
                {
                    Destroy(source);
                }

                healthAnim.SetBool("Health", false);
                TimeCounter = 0;
                firstTime = false;
            }
        }
    }

    public void EMP()
    {
        if (itemSelected == 0 && pressed && EmpCount > 0)
        {
            if (!firstTime)
            {
                Debug.Log("EMP");
                //audio de apuntando
                firstTime = true;
            }

            Debug.Log("Apunting with EMP");
        }

        if (itemSelected == 0 && realised == 1 && EmpCount > 0)
        {
            Debug.Log("Launched EMP");
            InstantiateThings(empPrefab);

            CantGunCantItem(false, false, false);
        }
    }

    #endregion

    public void InstantiateThings(GameObject prefab)
    {
        GameObject grenade = Instantiate(prefab, trajectoryPrefab.transform.position, Quaternion.Euler(0, 0, 0));
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        rb.velocity = trajectory.velocity * trajectory.direction;
    }

    IEnumerator CanDoItem()
    {
        yield return new WaitForSeconds(waitTimeItem);
        canDoItem = true;
    }

    public IEnumerator CanDoGun(float time)
    {
        yield return new WaitForSeconds(time);
        canDoGun = true;
    }

    public void CantGunCantItem(bool gun, bool item, bool first)
    {
        canDoGun = gun;
        StartCoroutine(CanDoGun(WM.itemTimeToGun[WM.WeaponSelected]));
        Debug.Log(WM.itemTimeToGun[WM.WeaponSelected]);

        canDoItem = item;
        firstTime = first;

        StartCoroutine(CanDoItem());
    }
}
