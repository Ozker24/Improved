using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float timeCounter;
    public float timeToVanish;

    public Vector3 halfSize;

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

        Burning();
    }

    void Burning()
    {
        Collider[] objects = Physics.OverlapBox(transform.position, halfSize, transform.rotation, layer);
        
        foreach (Collider objectsBurning in objects)
        {
            if (objectsBurning.tag == "Object")
            {
                Fireable fireable = objectsBurning.GetComponent<Fireable>();

                if (fireable != null)
                {
                    fireable.timeCounter += fireable.fireableIndex;
                }
            }
        }
    }
}
