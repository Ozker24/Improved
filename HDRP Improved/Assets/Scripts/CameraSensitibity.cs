using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitibity : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook playerCam;
    [SerializeField] OptionsMenu OM;
    [SerializeField] DevicesDetector devices;

    [Header("PC")]
    public float initYSpeedPC;
    public float YSensitibityPC;
    public float finalYSpeedPC;
    public float initXSpeedPC;
    public float XSensitibityPC;
    public float finalXSpeedPC;

    [Header("Controllers")]
    public float initYSpeedControllers;
    public float YSensitibityControllers;
    public float finalYSpeedControllers;
    public float initXSpeedControllers;
    public float XSensitibityControllers;
    public float finalXSpeedControllers;

    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("TPCamera").GetComponent<CinemachineFreeLook>();

        //OM = GameObject.FindGameObjectWithTag("Options Menu").GetComponent<OptionsMenu>();

        if (devices.PS4Controller && devices.XBoxOneController)
        {
            playerCam.m_YAxis.m_MaxSpeed = initYSpeedControllers;
            playerCam.m_XAxis.m_MaxSpeed = initXSpeedControllers;

            InvertXControllers();
            InvertYControllers();
        }
        else
        {
            playerCam.m_YAxis.m_MaxSpeed = initYSpeedPC;
            playerCam.m_XAxis.m_MaxSpeed = initXSpeedPC;

            InvertXPC();
            InvertYPC();
        }
    }

    public void Update()
    {
        if (!devices.PS4Controller && !devices.XBoxOneController)
        {
            SetYSensitivityPc();
            SetXSensitivityPC();

            InvertXPC();
            InvertYPC();
        }
        else
        {
            SetYSensitivityControllers();
            SetXSensitivityControllers();

            InvertXControllers();
            InvertYControllers();
        }
    }

    #region Sensitivity

    public void SetYSensitivityPc()
    {
        YSensitibityPC = OM.cameraYSensitibityPC;
        finalYSpeedPC = initYSpeedPC * YSensitibityPC;
        playerCam.m_YAxis.m_MaxSpeed = finalYSpeedPC;
    }

    public void SetXSensitivityPC()
    {
        XSensitibityPC = OM.cameraXSensitibityPC;
        finalXSpeedPC = initXSpeedPC * XSensitibityPC;
        playerCam.m_XAxis.m_MaxSpeed = finalXSpeedPC;
    }

    public void SetYSensitivityControllers()
    {
        YSensitibityControllers = OM.cameraYSensitibityController;
        finalYSpeedControllers = initYSpeedControllers * YSensitibityControllers;
        playerCam.m_YAxis.m_MaxSpeed = finalYSpeedControllers;
    }

    public void SetXSensitivityControllers()
    {
        XSensitibityControllers = OM.cameraXSensitibityController;
        finalXSpeedControllers = initXSpeedControllers * XSensitibityControllers;
        playerCam.m_XAxis.m_MaxSpeed = finalXSpeedControllers;
    }

    #endregion

    #region invert

    public void InvertYPC()
    {
        playerCam.m_YAxis.m_InvertInput = OM.invertCameraYPC;
    }

    public void InvertXPC()
    {
        playerCam.m_XAxis.m_InvertInput = OM.invertCameraXPC;
    }

    public void InvertYControllers()
    {
        playerCam.m_YAxis.m_InvertInput = OM.invertCameraYControllers;
    }

    public void InvertXControllers()
    {
        playerCam.m_XAxis.m_InvertInput = OM.invertCameraXControllers;
    }

    #endregion
}
