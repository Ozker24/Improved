using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControllers : MonoBehaviour
{
    [SerializeField] DevicesDetector device;
    [SerializeField] GameObject keyboardControls;
    [SerializeField] GameObject PS4Controls;

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
}
