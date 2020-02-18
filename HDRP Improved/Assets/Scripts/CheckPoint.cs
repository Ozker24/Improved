using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] CheckpointsManager checkManager;
    [SerializeField] Transform newSpawnPos;
    [SerializeField] Collider col;

    private void Start()
    {
        checkManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<CheckpointsManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        col = GetComponent<Collider>();
    }

    public void SetNewSpawnPoint()
    {
        player.spawnPos = newSpawnPos.position;
        checkManager.actualCheckPoint++;

        checkManager.SetNewCheckPoint();

        PlayerSaveSystem.SavePlayer(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (col != null && other.tag == ("Player")) 
        {
            SetNewSpawnPoint();
        }
    }
}
