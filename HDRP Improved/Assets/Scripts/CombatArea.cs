using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatArea : MonoBehaviour
{
    [Header ("Dependences")]
    public PlayerController player;
    public SphereCollider coll;

    [Header("Player Location")]
    public Vector3 playerPos;

    [Header("Enemies")]
    public GameObject[] enemies;
    public int enemiesNumber;

    [Header("Combat Variables")]
    public float radious;
    public float distToAvoidCombat;
    public float secondsToDestroy;
    public bool inCombat;
    public bool detected;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerPos = player.transform.position;
        enemiesNumber = enemies.Length;
        coll = GetComponent<SphereCollider>();
        coll.radius = radious;
    }

    public void Update()
    {
        if (inCombat)
        {
            playerPos = player.transform.position;

            if (Vector3.Distance (transform.position, playerPos) >= distToAvoidCombat)
            {
                inCombat = false;
            }
        }

        if (enemiesNumber <= 0)
        {
            inCombat = false;
            detected = false;
            player.stealth.detected = false;
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
