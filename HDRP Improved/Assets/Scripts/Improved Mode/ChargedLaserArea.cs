using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaserArea : MonoBehaviour
{
    [SerializeField] LaserGun laser;
    [SerializeField] BoxCollider boxColl;
    public bool enemyIn;

    public void Initialize()
    {
        boxColl = GetComponent<BoxCollider>();
        boxColl.enabled = false;
    }

    public void MyUpdate()
    {
        boxColl.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyIn = false;
        }
    }
}
