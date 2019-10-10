using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraAdjustment : MonoBehaviour
{
    public CinemachineFreeLook camaraTP;

    [Header ("PlayerSettings")]
    public PlayerController player;

    public float camaraAxisX;
    public float CamaraLocalRotY;
    public float CamaraRotY;

    public void Initialize()
    {
        camaraTP = GetComponent<CinemachineFreeLook>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void MyUpdate()
    {
        camaraAxisX = camaraTP.m_XAxis.Value;
        CamaraLocalRotY = camaraTP.transform.localRotation.y;
        CamaraRotY = camaraTP.transform.rotation.y;
    }
}
