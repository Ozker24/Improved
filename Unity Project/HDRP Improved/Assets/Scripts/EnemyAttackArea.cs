using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour
{
    public EnemyTest enemy;

    public bool playerInArea;

    public void Start()
    {
        enemy = GetComponentInParent<EnemyTest>();
    }

    private void OnTriggerStay(Collider other) // no pregunto por el tag puesto que la layer solo afecta al player
    {
        Debug.Log("Enter");

        if (enemy.canDetectPlayer)
        {
            Debug.Log("In");

            if (other != null)
            {
                Debug.Log("something");
                enemy.player.Damage(enemy.hitDamage);
                playerInArea = true;  
            }

            if (!playerInArea)
            {
                enemy.StunnedSet(enemy.stunedTimeWhenMiss);
            }

            other = null;
            playerInArea = false;
            enemy.canDetectPlayer = false;
        }
    }
}
