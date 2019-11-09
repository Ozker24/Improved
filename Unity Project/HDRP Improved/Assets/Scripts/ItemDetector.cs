using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public float radius;
    public LayerMask layer;
    public float distanceClosestItem;
    public GameObject closestItem;

    public void Initialize()
    {
        distanceClosestItem = Mathf.Infinity;
    }

    public void MyUpdate()
    {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);
        ItemBase[] items = GameObject.FindObjectsOfType<ItemBase>();
        foreach (ItemBase nearbyObject in items)
        {
            float distanceToItem = (nearbyObject.transform.position - gameObject.transform.position).sqrMagnitude;
            if (distanceToItem <= distanceClosestItem)
            {
                distanceClosestItem = distanceToItem;
                closestItem = nearbyObject.gameObject;
            }
        }
    }
}
