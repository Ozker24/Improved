using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform newSpawnPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void SetNewSpawnPoint()
    {
        player.spawnPos = newSpawnPos.position;

        PlayerSaveSystem.SavePlayer(player);
    }
}
