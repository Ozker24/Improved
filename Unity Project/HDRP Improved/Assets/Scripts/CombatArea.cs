using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatArea : MonoBehaviour
{
    public GameObject[] enemies;

    public int enemiesNumber;

    public bool inCombat;

    public float secondsToDestroy;

    public bool detected;

    public void Start()
    {
        enemiesNumber = enemies.Length;
    }

    public void Update()
    {
        if (enemiesNumber <= 0)
        {
            inCombat = false;
            Destroy(gameObject, secondsToDestroy);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inCombat = true;
        }
    }
}
