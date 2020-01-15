using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraAdjustment : MonoBehaviour
{
    [Header("Dependences")]
    public CinemachineFreeLook camaraTP;
    public PlayerController player;

    public void Initialize()
    {
        /*camaraTP = GetComponent<CinemachineFreeLook>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();*/
    }

    public void MyUpdate()
    {
        /*Vector3 dir = cameraTransform.TransformDirection(player.moveDir);
        dir.Set(dir.x, 0, dir.z);

        Vector3 finalVector = dir.normalized * player.moveDir.magnitude;

        player.transform.localRotation = Quaternion.FromToRotation();*/
    }
}
