using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireable : MonoBehaviour
{
    public float timeCounter;
    public float timeToBurn;

    public EnemyTest enemy;
    //public float fireableIndex;

    public void Start()
    {
        enemy = GetComponent<EnemyTest>();
    }

    public void Update()
    {
        if (timeCounter >= timeToBurn)
        {
            Destroy(gameObject);

            enemy.dead = true;

            Debug.Log("Dead by Fireable");
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "FireZone")
        {
            if (gameObject.tag == "Enemy")
            {
                EnemyTest enemy = GetComponent<EnemyTest>();

                if (enemy.dead)
                {
                    return;
                }
            }

            timeCounter += Time.deltaTime;
        }
    }*/

    /*public void OnTriggerStay(Collider other)
    {
        if (other.tag == "FireZone")
        {
            timeCounter += Time.deltaTime;
        }
    }*/
}
