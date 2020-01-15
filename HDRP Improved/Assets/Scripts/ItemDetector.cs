using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    [Header ("Overlap")]
    public float radius;
    public LayerMask layer;

    [Header("Closest Item")]
    public GameObject closestItem;
    public float distanceClosestItem;
    public float constantDisToItem;

    [Header("Restart")]
    public float distanceToReset;
    public float mergeOfReset;

    [Header("Grab")]
    public float distanceToGrab;
    public bool canGrab;

    public void Initialize()
    {
        distanceClosestItem = Mathf.Infinity;
    }

    public void MyUpdate()
    {
        if (closestItem != null)
        {
            constantDisToItem = Vector3.Distance(gameObject.transform.position, closestItem.transform.position);

            if (Vector3.Distance(gameObject.transform.position, closestItem.transform.position) <= distanceToGrab)
            {
                canGrab = true;
            }
            else
            {
                canGrab = false;
            }
        }

        Collider[] items = Physics.OverlapSphere(gameObject.transform.position, radius, layer);

        foreach (Collider nearbyObject in items)
        {
            Debug.Log(nearbyObject.name);

            float distanceToItem = Vector3.Distance(nearbyObject.transform.position, gameObject.transform.position);
            if (distanceToItem <= distanceClosestItem)
            {
                distanceClosestItem = distanceToItem;
                closestItem = nearbyObject.gameObject;
            }
        }

        if (closestItem != null)
        {
            if (Vector3.Distance(gameObject.transform.position, closestItem.transform.position) >= distanceClosestItem + mergeOfReset)
            {
                distanceClosestItem = distanceClosestItem = Mathf.Infinity;
            }
        }

        if (constantDisToItem >= distanceToReset)
        {
            distanceClosestItem = Mathf.Infinity;
            closestItem = null;
            constantDisToItem = 0;
        }

        if (closestItem == null)
        {
            canGrab = false;
        }
    }
}
