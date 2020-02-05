using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerEvents))]

public class Interruptor : MonoBehaviour
{
    public UnityEvent activeEvent;
    public UnityEvent desactiveEvent;

    public bool activate;
    public bool playerIn;
    public bool isASwitch;
    public bool activatedBefore;

    private void Update()
    {
        Interact();
    }

    void Interact()
    {
        if (playerIn)
        {
            if (Input.GetButtonDown("Interact"))
            {
                DoInteraction();
            }
        }
    }

    public void DoInteraction()
    {
        if (isASwitch)
        {
            if (activate)
            {
                if (desactiveEvent != null)
                {
                    desactiveEvent.Invoke();
                    activate = false;
                }
            }
            else
            {
                if (activeEvent != null)
                {
                    activeEvent.Invoke();
                    activate = true;
                }
            }
        }
        else
        {
            if (!activatedBefore)
            {
                if (desactiveEvent != null && activate)
                {
                    desactiveEvent.Invoke();
                    activate = false;
                }

                else if (activeEvent != null && !activate)
                {
                    activeEvent.Invoke();
                    activate = true;
                }

                activatedBefore = true;
            }
        }

    }
}
