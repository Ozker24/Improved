using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionArea : MonoBehaviour
{
    [SerializeField] EnemyTest enemy;
    public Collider area;

    public void Initialize()
    {
        enemy = GetComponentInParent< EnemyTest>();
        area = GetComponent<BoxCollider>();
    }

    public void MyUpdate()
    {
        
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            enemy.player.canExecute = true;
            enemy.player.CC.enemyToExecute = enemy;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemy.player.canExecute = true;
            enemy.player.CC.enemyToExecute = null;
        }
    }
}
