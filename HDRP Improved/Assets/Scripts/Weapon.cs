using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponManager weapon;

    public AudioPlayer audPlay;

    public int totalAmmo; //municion total

    //public float timeForBullet;

    //public float timeForSearch;

    //public float timeForFirstMagazine;

    public int ammoReloaded; //municion recargada total

    public int magazineAmmo; //capacidad del cargador

    public int currentAmmo; //municion en el cargador

    public int maxAmmo;

    public bool isShoting;
    public bool isReloading;
    public bool isAutomatic;
    public bool canSHootAgain;

    public float fireRate;
    public float reloadTime;
    public float fireDist;
    public float bulletDamage;
    public float stunedTime;

    public int shotGunSpreads;
    public float spread;

    public GameObject bloodParticle;
    public ParticleSystem muzzleParticle;
    public GameObject sparklePartcile;

    [Header("Sounds")]
    public AudioSource basicSource;
    public AudioArray ShotClips;
    public AudioClip MetalClip;
    public AudioArray ReloadClips;

    public void Start()
    {
        bloodParticle = weapon.bloodParticle;
        muzzleParticle = weapon.muzzleParticle;
        sparklePartcile = weapon.sparklePartcile;
        ShotClips = weapon.shotClips;
        ReloadClips = weapon.ReloadClips;
        MetalClip = weapon.MetalClip;
    }

    public void Shot()
    {
        if (isReloading || isShoting) return;
        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        isShoting = true;
        currentAmmo--;

        //audPlay.Play(0, 1, Random.Range(0.95f, 1.05f));
        //ammoReloaded--;

        basicSource.PlayOneShot(ShotClips.clips[weapon.WeaponSelected]);

        muzzleParticle.Play();

        if (weapon.WeaponSelected != 1)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit = new RaycastHit();// que hemos golpeado primero

            if (Physics.Raycast(ray, out hit, fireDist))
            {
                Debug.Log(hit.transform.tag);

                if (hit.transform.tag == "Enemy")
                {
                    GameObject particle = (GameObject)Instantiate(bloodParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    Destroy(particle, 10);
                    hit.transform.SendMessage("StunnedSet", stunedTime, SendMessageOptions.RequireReceiver);
                    hit.transform.SendMessage("Damage", bulletDamage, SendMessageOptions.RequireReceiver);

                    EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
                    if (enemy != null)
                    {
                        enemy.Detected = true;
                    }
                }

                else if (hit.transform.tag == "Metal")
                {
                    GameObject particle = (GameObject)Instantiate(sparklePartcile, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    basicSource.PlayOneShot(MetalClip);
                    Destroy(particle, 10);
                }
            }
        }

        else
        {
            for (int i = 0; i < shotGunSpreads; i++)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(Random.Range(0.3f,0.7f), Random.Range(0.3f, 0.7f), 0));
                RaycastHit hit = new RaycastHit();// que hemos golpeado primero
                Debug.DrawRay(ray.origin, ray.direction * fireDist, Color.red, 0.5f);

                if (Physics.Raycast(ray, out hit, fireDist))
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        GameObject particle = (GameObject)Instantiate(bloodParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                        Destroy(particle, 10);

                        hit.transform.SendMessage("StunnedSet", stunedTime, SendMessageOptions.RequireReceiver);
                        hit.transform.SendMessage("Damage", bulletDamage, SendMessageOptions.RequireReceiver);

                        EnemyTest enemy = hit.transform.GetComponent<EnemyTest>();
                        if (enemy != null)
                        {
                            enemy.Detected = true;
                        }
                    }

                    else if (hit.transform.tag == "Metal")
                    {
                        GameObject particle = (GameObject)Instantiate(sparklePartcile, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                        basicSource.PlayOneShot(MetalClip);
                        Destroy(particle, 10);
                    }
                }

                Debug.Log(hit.point);


                /*Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit = new RaycastHit();// que hemos golpeado primero

                whereToShot.transform.position = ray.origin;
                whereToShot.transform.rotation = Quaternion.Euler(Random.Range(-7, 7), Random.Range(-7, 7), whereToShot.localEulerAngles.z);

                if (Physics.Raycast(whereToShot.position, whereToShot.forward, out hit, fireDist))
                {
                    Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }*/
            }
        }

        if (isAutomatic)
        {
            StartCoroutine(ResetShot());
        }
    }

    public void Reload()
    {
        if (isShoting || isReloading) return;
        if (currentAmmo >= magazineAmmo) return;

        if (ammoReloaded <= 0)
        {
            //sound of no ammo
        }
        else
        {
            isReloading = true;

            basicSource.PlayOneShot(ReloadClips.clips[weapon.WeaponSelected]);

            StartCoroutine(ResetReload());
        }
    }

    public void ReleaseShot()
    {
        if (!isAutomatic)
        {
            if (isShoting)
            {
                isShoting = false;
            }
        }
    }

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(fireRate);

        isShoting = false;
    }

    IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;

        int diff = magazineAmmo - currentAmmo;
        if (diff <= ammoReloaded)
        {
            ammoReloaded -= diff;

            currentAmmo += diff;
        }
        else
        {
            currentAmmo += ammoReloaded;

            ammoReloaded = 0;
        }
    }

    public void PlaySound()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = MetalClip;
        source.PlayDelayed(0.3f);

        Destroy(source, source.clip.length);
    }

    public void PlayShotSound(int index)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = ShotClips.clips[index];
        source.PlayDelayed(0.0f);

        Destroy(source, source.clip.length);
    }

    public void PlayReloadSound(int index)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = ShotClips.clips[index];
        source.PlayDelayed(0.0f);

        Destroy(source, source.clip.length);
    }
}
