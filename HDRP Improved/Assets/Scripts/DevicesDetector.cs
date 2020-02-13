using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicesDetector : MonoBehaviour
{
    [Header("Controllers")]
    public bool PS4Controller;
    public bool XBoxOneController;

    private void Update()
    {
        DetectDevice();
    }

    void DetectDevice()
    {
        string[] names = Input.GetJoystickNames();

        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                PS4Controller = true;
                XBoxOneController = false;
            }
            else if (names[x].Length == 33)
            {
                XBoxOneController = true;
                PS4Controller = false;
            }

            else if (names[x].Length == 0)
            {
                XBoxOneController = false;
                PS4Controller = false;
            }
        }
    }
}
