using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerSave
{
    public float[] spawnPos;
    public int actualSpawnPoint;
    public float health;
    public int molotovs;
    public int grenades;
    public int sounds;
    public int kits;
    public int EMPs;
    public int pistolInMagazine;
    public int pistolReloaded;
    public int shotgunInMagazine;
    public int shotgunReloaded;
    public int subInMagazine;
    public int subReloaded;
    public int rifleInMagazine;
    public int rifleReloaded;


    public PlayerSave(PlayerController player)
    {
        spawnPos = new float[3];

        spawnPos[0] = player.spawnPos.x;
        spawnPos[1] = player.spawnPos.y;
        spawnPos[2] = player.spawnPos.z;

        actualSpawnPoint = player.GM.checkpointsManager.actualCheckPoint;

        health = player.life.health;
        molotovs = player.items.itemsCount[4];
        grenades = player.items.itemsCount[3];
        sounds = player.items.itemsCount[2];
        kits = player.items.itemsCount[1];
        EMPs = player.items.itemsCount[0];

        pistolInMagazine = player.WM.weapons[0].currentAmmo;
        pistolReloaded = player.WM.weapons[0].ammoReloaded;
        shotgunInMagazine = player.WM.weapons[1].currentAmmo;
        shotgunReloaded = player.WM.weapons[1].ammoReloaded;
        subInMagazine = player.WM.weapons[2].currentAmmo;
        subReloaded = player.WM.weapons[2].ammoReloaded;
        rifleInMagazine = player.WM.weapons[3].currentAmmo;
        rifleReloaded = player.WM.weapons[3].ammoReloaded;
    }
}
