using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float timeCounter;
    public float timeToVanish;

    public Vector3 halfExtent;

    public LayerMask layer;

    public void Update()
    {
        if (timeCounter >= timeToVanish)
        {
            Destroy(gameObject);
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }

    /*public void OnTriggerStay(Collider other)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtent, transform.rotation, layer);

        foreach (Collider nearbyObject in colliders)
        {
            Debug.Log(nearbyObject.name);

            Object obj = nearbyObject.GetComponent<Object>();

            if (obj != null)
            {
                obj.timeCounter += Time.deltaTime;
            }
        }
    }*/

    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, halfExtent * 2);
        Gizmos.color = Color.red;
    }
}
