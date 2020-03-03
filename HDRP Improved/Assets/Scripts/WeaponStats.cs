using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public int magazineAmmo; //capacidad del cargador

    public int currentAmmo; //municion en el cargador

    public int maxAmmo;

    public int gunReference;

    public float fireRate;
    public float reloadTime;
    public float fireDist;
    public float bulletDamage;
    public float bulletMetalDamage;
    public float stunedTime;

    public int shotGunSpreads;
    public float spread;

    public float timeAbleGun;
    public float itemTimeToGun;
    public float timeToAim;
    public float distanceToSound;

    public bool isAutomatic;
    public bool Slot;

    public AudioClip shotClips;
    public AudioClip reloadClips;
}
