using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowImpact : MonoBehaviour
{
    public float distance;
    public float radius;
    public PlayerController player;
    public Vector3 whereToPoint;

    public void Initialize()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void MyUpdate()
    {
        whereToPoint = player.modelTrans.transform.forward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(player.modelTrans.position.x, player.modelTrans.position.y, player.modelTrans.position.z + distance), radius);
    }

    public Vector3 ReturnDir()
    {
        Vector3 dir = player.camTrans.TransformDirection(whereToPoint);

        dir.Set(dir.x, 0, dir.z);

        return dir.normalized * whereToPoint.magnitude;

    }
}
