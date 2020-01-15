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
    public bool Canceled;
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

    public int[] itemsCount;
    public int[] visualItemsCount;

    public GameObject[] molotovVisuals;
    public GameObject[] granadeVisuals;
    public GameObject[] soundGranadeVisuals;
    public GameObject[] firstAidVisuals;
    public GameObject[] EMPVisuals;

    public int maxItems = 3;

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
        0 EMP apuntar y disparar
     */

    [Header("Sounds")]
    public AudioSource baseSource;

    public AudioArray ClipsSelected;
    public AudioArray ClipsLaunched;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        WM = player.GetComponentInChildren<WeaponManager>();

        cam = Camera.main;
        peace = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<HealthPeace>();
        audPlay = GetComponentInChildren<AudioPlayer>();

        trajectoryPrefab.SetActive(false);
        trajectory = trajectoryPrefab.GetComponent<Trajectory>();

        InitializeVisuals();
    }

    private void InitializeVisuals()
    {
        for (int i = 0; i < itemsCount[4]; i++)
        {
            molotovVisuals[i].SetActive(true);
            visualItemsCount[4]++;
        }

        for (int i = 0; i < itemsCount[3]; i++)
        {
            granadeVisuals[i].SetActive(true);
            visualItemsCount[3]++;
        }

        for (int i = 0; i < itemsCount[2]; i++)
        {
            soundGranadeVisuals[i].SetActive(true);
            visualItemsCount[2]++;
        }

        firstAidVisuals[itemsCount[1] - 1].SetActive(true);
        visualItemsCount[1] = itemsCount[1];

        for (int i = 0; i < itemsCount[0]; i++)
        {
            EMPVisuals[i].SetActive(true);
            visualItemsCount[0]++;
        }
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

        if (inv.startCountdown)
        {
            trajectoryPrefab.SetActive(false);
        }
    }

    public void ActiveItem()
    {
        if (!player.stop && !player.climb && !inv.startCountdown && !Canceled)
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
        if (itemsCount[itemSelected] > 0)
        {
            pressed = true;

            if (canDoItem && itemSelected != 1)
            {
                trajectoryPrefab.SetActive(true);
            }
        }
    }

    public void CancelItems()
    {
        if (pressed && itemSelected != 1)
        {
            pressed = false;
            trajectoryPrefab.SetActive(false);
            trajectory.drawing = false;
            firstTime = false;
            Canceled = true;

            visualItemsCount[itemSelected]++;

            if (itemSelected == 4)
            {
                molotovVisuals[visualItemsCount[itemSelected] - 1].SetActive(true);
            }

            if (itemSelected == 3)
            {
                granadeVisuals[visualItemsCount[itemSelected] - 1].SetActive(true);
            }

            if (itemSelected == 2)
            {
                soundGranadeVisuals[visualItemsCount[itemSelected] - 1].SetActive(true);
            }

            if (itemSelected == 1)
            {
                firstAidVisuals[visualItemsCount[itemSelected] - 1].SetActive(true);
            }

            if (itemSelected == 0)
            {
                EMPVisuals[visualItemsCount[itemSelected] - 1].SetActive(true);
            }
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

        StartCoroutine(StopCancelling());

    }

    IEnumerator RealisedFalse()
    {
        yield return new WaitForEndOfFrame();
        realised = 0;
    }

    #region WhatItemsDo
    public void Molotov()
    {
        if (itemSelected == 4 && pressed &&  itemsCount [4] > 0)
        {
            if (!firstTime)
            {
                Debug.Log("M");

                if (ClipsSelected.clips[itemSelected] != null)
                {
                    baseSource.PlayOneShot(ClipsSelected.clips[itemSelected]);
                }

                //audio de apuntando

                molotovVisuals[visualItemsCount[itemSelected] - 1].SetActive(false);

                visualItemsCount[4]--;

                firstTime = true;
            }

            Debug.Log("Apunting with Molotov");
        }

        if (itemSelected == 4 && realised == 1 && itemsCount[4] > 0)
        {
            Debug.Log("Launched molotov");

            if (ClipsLaunched.clips[itemSelected] != null)
            {
                baseSource.PlayOneShot(ClipsLaunched.clips[itemSelected]);
            }

            //audPlay.Play(5, 1, Random.Range(0.95f, 1.05f)); // esta antes por temas de velocidad del audio
            InstantiateThings(MolotovPrefab);

            itemsCount[4]--;

            CantGunCantItem(false, false, false);
        }
    }

    public void Granade()
    {
        if (itemSelected == 3 && pressed && itemsCount[3] > 0)
        {
            if (!firstTime)
            {
                Debug.Log("G");
                //audio de apuntando

                if (ClipsSelected.clips[itemSelected] != null)
                {
                    baseSource.PlayOneShot(ClipsSelected.clips[itemSelected]);
                }


                granadeVisuals[visualItemsCount[itemSelected] - 1].SetActive(false);

                visualItemsCount[3]--;

                firstTime = true;
            }

            Debug.Log("Apunting with Granade");
        }

        if (itemSelected == 3 && realised == 1 && itemsCount[3] > 0)
        {
            Debug.Log("Launched Granade");

            InstantiateThings(GranadePrefab);

            //audPlay.Play(6, 1, Random.Range(0.95f, 1.05f));
            if (ClipsLaunched.clips[itemSelected] != null)
            {
                baseSource.PlayOneShot(ClipsLaunched.clips[itemSelected]);
            }

            itemsCount[3]--;

            CantGunCantItem(false, false, false);
        }
    }

    public void SoundItem()
    {
        if (itemSelected == 2 && pressed && itemsCount[2] > 0)
        {
            if (!firstTime)
            {
                Debug.Log("S");

                if (ClipsSelected.clips[itemSelected] != null)
                {
                    baseSource.PlayOneShot(ClipsSelected.clips[itemSelected]);
                }
                //audio de apuntando

                soundGranadeVisuals[visualItemsCount[itemSelected] -1 ].SetActive(false);

                visualItemsCount[2]--;

                firstTime = true;
            }

            Debug.Log("Apunting with Sound");
        }

        if (itemSelected == 2 && realised == 1 && itemsCount[2] > 0)
        {
            Debug.Log("Launched Sound");
            if (ClipsLaunched.clips[itemSelected] != null)
            {
                baseSource.PlayOneShot(ClipsLaunched.clips[itemSelected]);
            }
            InstantiateThings(SoundPrefab);

            itemsCount[2]--;

            CantGunCantItem(false, false, false);
        }
    }

    public void FirstAid()
    {
        if (itemSelected == 1 && itemsCount[1] > 0 && player.life.health < 100)
        {
            if (pressed && realised != 1)
            {
                if (!firstTime)
                {
                    Debug.Log("F");
                    //audPlay.Play(3, 1, Random.Range(0.95f, 1.05f));
                    if (ClipsSelected.clips[itemSelected] != null && baseSource != null)
                    {
                        baseSource.PlayOneShot(ClipsSelected.clips[itemSelected]);
                    }

                    if (itemsCount[1] > 1)
                    {
                        for (int i = 0; i < firstAidVisuals.Length; i++)
                        {
                            firstAidVisuals[i].SetActive(false);
                        }

                        visualItemsCount[1]--;

                        firstAidVisuals[visualItemsCount[1] - 1].SetActive(true);
                    }
                    else
                    {
                        for (int i = 0; i < firstAidVisuals.Length; i++)
                        {
                            firstAidVisuals[i].SetActive(false);
                        }

                        visualItemsCount[1]--;
                    }

                    firstTime = true;
                }

                if (TimeCounter >= timeFirstAid)
                {
                    Debug.Log("Healed");
                    healthAnim.SetBool("Health", false);
                    TimeCounter = 0;

                    peace.Health();

                    if (baseSource != null)
                    {
                        baseSource.Stop();
                    }

                    //audPlay.Play(4, 1, Random.Range(0.95f, 1.05f));
                    if (ClipsLaunched.clips[itemSelected] != null)
                    {
                        baseSource.PlayOneShot(ClipsLaunched.clips[itemSelected]);
                    }

                    pressed = false;

                    itemsCount[1]--;

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

                AudioSource source = GetComponent<AudioSource>();

                if (source != null)
                {
                    source.Stop();
                }

                for (int i = 0; i < firstAidVisuals.Length; i++)
                {
                    firstAidVisuals[i].SetActive(false);
                }

                visualItemsCount[1]++;

                firstAidVisuals[visualItemsCount[1] - 1].SetActive(true);

                healthAnim.SetBool("Health", false);
                TimeCounter = 0;
                firstTime = false;
            }
        }
    }

    public void EMP()
    {
        if (itemSelected == 0 && pressed && itemsCount[0] > 0)
        {
            if (!firstTime)
            {
                Debug.Log("EMP");
                if (ClipsSelected.clips[itemSelected] != null)
                {
                    baseSource.PlayOneShot(ClipsSelected.clips[itemSelected]);
                }
                //audio de apuntando

                EMPVisuals[visualItemsCount[itemSelected] - 1].SetActive(false);

                visualItemsCount[0]--;

                firstTime = true;
            }

            Debug.Log("Apunting with EMP");
        }

        if (itemSelected == 0 && realised == 1 && itemsCount[0] > 0)
        {
            Debug.Log("Launched EMP");
            if (ClipsLaunched.clips[itemSelected] != null)
            {
                baseSource.PlayOneShot(ClipsLaunched.clips[itemSelected]);
            }

            InstantiateThings(empPrefab);

            itemsCount[0]--;

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

    public void PlaySound(int index)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        //source.PlayOneShot

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = ClipsLaunched.clips[index];
        source.Play();

        Destroy(source, source.clip.length);
    }

    IEnumerator StopCancelling()
    {
        yield return new WaitForSeconds(0.3f);

        Canceled = false;
    }
}
