using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public PlayerController player;
    public WeaponManager WM;

    public Inventory inv;
    public int itemSelected;
    public bool pressed;
    public int realised;
    public float timeFirstAid;
    public float timeInject;
    public float TimeCounter;
    public bool interrupted;
    public bool firstTime;

    public Animator healthAnim;
    public HealthPeace peace;

    public Camera cam;
    public GameObject GranadePrefab;
    public GameObject MolotovPrefab;
    public GameObject SoundPrefab;
    public Transform throwTrans;
    public float throwForce;

    public int molotovCount;
    public int GranadeCount;
    public int SoundCount;
    public int FirstAidCount;
    public int InjectionCount;

    public bool canDoItem = true;
    public float waitTimeItem;

    public bool canDoGun = true;
    public float waitTimeGun;

    public TimeManager time;

    public AudioPlayer audPlay;

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
        time = GameObject.FindGameObjectWithTag("Managers").GetComponent<TimeManager>();
        peace = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<HealthPeace>();
        audPlay = GetComponentInChildren<AudioPlayer>();

    }

    public void MyUpdate()
    {
        itemSelected = inv.actualItem;

        if (canDoItem) ActiveItem();
    }

    public void ActiveItem()
    {
        if (!player.stop && !player.climb && WM.ableToLunch)
        {
            Molotov();
            Granade();
            SoundItem();
            FirstAid();
            Inject();
        }
    }

    public void SetButtonPressed()
    {
        pressed = true;
    }

    public void SetButtonRealised()
    {
        realised = 1;
        StartCoroutine(RealisedFalse());
        pressed = false;
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

    public void Inject()
    {
        if (itemSelected == 0 && InjectionCount > 0)
        {
            if (pressed && realised != 1)
            {
                if (!firstTime)
                {
                    //Debug.Log("I");
                    audPlay.Play(2, 1, Random.Range(0.95f, 1.05f));
                    firstTime = true;
                }

                if (TimeCounter >= timeInject)
                {
                    Debug.Log("Injected");
                    TimeCounter = 0;

                    AudioSource source = GetComponentInChildren<AudioSource>();

                    if (source != null)
                    {
                        Destroy(source);
                    }

                    audPlay.PlayIgnoringTime(0, 1);
                    time.ZaWarudo();

                    StartCoroutine(PlayBackZaWarudo());

                    CantGunCantItem(false, false, false);
                }
                else
                {
                    TimeCounter += Time.deltaTime;
                }
            }

            if (realised == 1)
            {
                Debug.Log("Interrumpted");
                TimeCounter = 0;

                AudioSource source = GetComponentInChildren<AudioSource>();

                if (source != null)
                {
                    Destroy(source);
                }

                firstTime = false;
            }
        }
    }

    #endregion

    public void InstantiateThings(GameObject prefab)
    {
        GameObject granade = Instantiate(prefab, throwTrans.position, throwTrans.rotation);
        Rigidbody rb = granade.GetComponent<Rigidbody>();
        rb.AddForce(cam.transform.forward * throwForce);
    }

    IEnumerator CanDoItem()
    {
        yield return new WaitForSeconds(waitTimeItem);
        canDoItem = true;
    }

    public IEnumerator PlayBackZaWarudo()
    {
        yield return new WaitForSeconds(time.injectedTime);
        audPlay.PlayIgnoringTime(1, 1);
    }

    public IEnumerator CanDoGun()
    {
        yield return new WaitForSeconds(waitTimeGun);
        canDoGun = true;
    }

    public void CantGunCantItem(bool gun, bool item, bool first)
    {
        canDoGun = gun;
        StartCoroutine(CanDoGun());

        canDoItem = item;
        firstTime = first;

        StartCoroutine(CanDoItem());
    }
}
