using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRestEnemiesCount : MonoBehaviour
{
    [SerializeField] EnemyTest enemy;
    [SerializeField] ArenaWin arenaWin;

    public void Start()
    {
        enemy = GetComponent<EnemyTest>();
    }

    public void RestCount()
    {
        arenaWin.enemiesCount--;
        //enabled = false;
    }
}
