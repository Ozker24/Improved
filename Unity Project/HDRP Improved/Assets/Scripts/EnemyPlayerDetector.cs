using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetector : MonoBehaviour
{
    [Header("Dependences")]
    [SerializeField] EnemyTest enemy;
    [SerializeField] PlayerController player;
    [SerializeField] SphereCollider playerDetector;

    [Header("Detector Variables")]
    public float radius;

    public void Initialize()
    {
        enemy = GetComponentInParent<EnemyTest>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerDetector = GetComponent<SphereCollider>();
    }

    public void MyUpdate()
    {
        playerDetector.radius = radius;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && player.stealth.stealthIndicator >= player.stealth.enemyDetection)
        {
            enemy.Detected = true;
        }
    }
}
