using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class OptionsManager
{
    public float audioValue;
    public int selectedResolution;
    public int selectedQuality;

    public OptionsManager (OptionsMenu oMenu)
    {
        audioValue = oMenu.audioValue;
        selectedResolution = oMenu.selectedResolution;
        selectedQuality = oMenu.selectedQuality;
    }
}
