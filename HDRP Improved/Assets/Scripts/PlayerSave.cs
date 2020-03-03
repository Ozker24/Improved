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
    public int slot1Ref;
    public int slot2Ref;
    public int slot1CurrentAmmo;
    public int slot2CurrentAmmo;
    public int pistolAmmoCollected;
    public int shotgunAmmoCollected;
    public int automaticAmmoCollected;
    public int rifleAmmoCollected;


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
        slot1Ref = player.WM.weapons[0].gunReference;
        slot2Ref = player.WM.weapons[1].gunReference;
        slot1CurrentAmmo = player.WM.weapons[0].currentAmmo;
        slot2CurrentAmmo = player.WM.weapons[1].currentAmmo;
        pistolAmmoCollected = player.WM.ammoCollected[0];
        shotgunAmmoCollected = player.WM.ammoCollected[1];
        automaticAmmoCollected = player.WM.ammoCollected[2];
        rifleAmmoCollected = player.WM.ammoCollected[3];
    }
}
