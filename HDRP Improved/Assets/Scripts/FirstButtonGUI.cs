using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstButtonGUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] DevicesDetector devices;

    [SerializeField] EventSystem ESystem;
    [SerializeField] GameObject firstButtonOnOptions;
    [SerializeField] GameObject firstButtonOnMainMenu;
    [SerializeField] GameObject firstButtonOnPause;
    [SerializeField] GameObject firstButtonOnResolution;
    [SerializeField] GameObject firstButtonOnAudio;
    [SerializeField] GameObject firstButtonOnControllerInput;
    [SerializeField] GameObject firstButtonOnPCInput;
    //[SerializeField] GameObject firstButtonOnControll;

    public void ChangeFBOnOptions()
    {
        ESystem.SetSelectedGameObject(firstButtonOnOptions);
    }
    public void ChangeFBOnMenu()
    {
        ESystem.SetSelectedGameObject(firstButtonOnMainMenu);
    }
    public void ChangeFBOnPause()
    {
        ESystem.SetSelectedGameObject(firstButtonOnPause);
    }
    public void ChangeFBOnResolution()
    {
        ESystem.SetSelectedGameObject(firstButtonOnResolution);
    }
    public void ChangeFBOnAudio()
    {
        ESystem.SetSelectedGameObject(firstButtonOnAudio);
    }

    public void ChangeFBOnInputs()
    {
        if (devices.PS4Controller | devices.XBoxOneController)
        {
            ESystem.SetSelectedGameObject(firstButtonOnControllerInput);
        }
        else
        {
            ESystem.SetSelectedGameObject(firstButtonOnPCInput);
        }
    }
}
