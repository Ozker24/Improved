using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControllers : MonoBehaviour
{
    [SerializeField] GameManager GM;

    [SerializeField] DevicesDetector device;
    [SerializeField] GameObject keyboardControls;
    [SerializeField] GameObject PS4Controls;
    [SerializeField] GameObject PS4ControlsImp;
    [SerializeField] GameObject keyboardControlsImp;

    public bool showThem;

    public void Start()
    {
        DisplayControllers();
    }

    public void Update()
    {
        if (showThem)
        {
            DisplayControllers();
        }
    }

    public void DisplayControllers()
    {
        if (GM != null)
        {
            if (!GM.improved)
            {
                AppearNormalControls();
            }
            else
            {
                AppearImprovedControls();
            }
        }
        else
        {
            AppearNormalControls();
        }
    }

    public void AppearNormalControls()
    {
        PS4ControlsImp.SetActive(false);
        keyboardControlsImp.SetActive(false);

        if (device.PS4Controller)
        {
            keyboardControls.SetActive(false);
            PS4Controls.SetActive(true);
        }
        else
        {
            PS4Controls.SetActive(false);
            keyboardControls.SetActive(true);
        }
    }

    public void AppearImprovedControls()
    {
        keyboardControls.SetActive(false);
        PS4Controls.SetActive(false);

        if (device.PS4Controller)
        {
            keyboardControlsImp.SetActive(false);
            PS4ControlsImp.SetActive(true);
        }
        else
        {
            PS4ControlsImp.SetActive(false);
            keyboardControlsImp.SetActive(true);
        }
    }
}
