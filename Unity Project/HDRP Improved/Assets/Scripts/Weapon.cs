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

    public bool isShoting;
    public bool isReloading;

    public float fireRate;
    public float reloadTime;
    public float fireDist;
    public int bulletDamage;
    public float stunedTime;

    public int shotGunSpreads;
    public float spread;

    public GameObject bloodParticle;

    public Transform whereToShot;

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

        audPlay.Play(0, 1, Random.Range(0.95f, 1.05f));
        //ammoReloaded--;

        if (weapon.WeaponSelected != 1)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit = new RaycastHit();// que hemos golpeado primero

            if (Physics.Raycast(ray, out hit, fireDist))
            {
                Debug.Log(hit.transform.tag);
                //Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.SendMessage("StunnedSet", stunedTime, SendMessageOptions.RequireReceiver);
                    hit.transform.SendMessage("Damage", bulletDamage, SendMessageOptions.RequireReceiver);
                }
            }
        }

        else
        {
            for (int i = 0; i < shotGunSpreads; i++)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f, 0.2f), 0));
                RaycastHit hit = new RaycastHit();// que hemos golpeado primero

                if (Physics.Raycast(ray, out hit, fireDist))
                {
                    Instantiate(bloodParticle, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
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

        StartCoroutine(ResetShot());
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

            audPlay.Play(1, 1, 1);

            StartCoroutine(ResetReload());
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
}
