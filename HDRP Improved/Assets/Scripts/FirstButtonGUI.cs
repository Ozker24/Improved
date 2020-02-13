using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstButtonGUI : MonoBehaviour
{
    [SerializeField] EventSystem ESystem;
    [SerializeField] GameObject firstButtonOnOptions;
    [SerializeField] GameObject firstButtonOnMainMenu;
    [SerializeField] GameObject firstButtonOnPause;

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
        ESystem.SetSelectedGameObject(firstButtonOnOptions);
    }
}
