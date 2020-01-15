using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowImpact : MonoBehaviour
{
    public PlayerController player;
    public Items items;
    public float distance;
    public float high;
    public float force;
    [SerializeField] float time;
    public Vector3 parabolVect;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        items = player.items;
    }

    public void MyUpdate()
    {
        high = items.throwTrans.position.y;
        parabolVect = items.cam.transform.forward;
        //force = items.throwForce;

        CalculateTime();
        CalculatePos();

        transform.position = (transform.position + Vector3.forward * distance);
    }

    public void CalculateTime()
    {
        float FSin = -force * parabolVect.y;
        float Fsin2 = ((force * parabolVect.y) * (force * parabolVect.y));
        float FourGrab = -4 * ( -4.9f ) * high;
        float Grab = 2 * (- 4.9f);
        time = (FSin - Mathf.Sqrt(Fsin2 - FourGrab) / Grab);
    }

    public void CalculatePos()
    {
        distance = force * parabolVect.z * time;
    }
}
