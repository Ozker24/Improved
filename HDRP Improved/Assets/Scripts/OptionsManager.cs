using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class OptionsManager
{
    public float audioValue;
    public int selectedResolution;
    public int selectedQuality;
    public string cameraYSensitibityPC;
    public string cameraXSensitibityPC;
    public float cameraYSensitibityController;
    public float cameraXSensitibityController;
    public bool invertCameraYPC;
    public bool invertCameraXPC;
    public bool invertCameraYControllers;
    public bool invertCameraXControllers;

    public OptionsManager (OptionsMenu oMenu)
    {
        audioValue = oMenu.audioValue;
        selectedResolution = oMenu.selectedResolution;
        selectedQuality = oMenu.selectedQuality;
        cameraYSensitibityPC = oMenu.cameraYSensitibityPC;
        cameraXSensitibityPC = oMenu.cameraXSensitibityPC;
        cameraYSensitibityController = oMenu.cameraYSensitibityController;
        cameraXSensitibityController = oMenu.cameraXSensitibityController;
        invertCameraYPC = oMenu.invertCameraYPC;
        invertCameraXPC = oMenu.invertCameraXPC;
        invertCameraYControllers = oMenu.invertCameraYControllers;
        invertCameraXControllers = oMenu.invertCameraXControllers;
    }
}
