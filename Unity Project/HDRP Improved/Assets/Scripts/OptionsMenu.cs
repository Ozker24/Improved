using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public enum Options { Resolution, Graphics, Audio, Controls};
    public Options Settings;

    public string[] qualities;

    public GameObject[] selectors;
    public GameObject[] settings;


    public void Start()
    {
        qualities = QualitySettings.names;

        Settings = Options.Graphics;
    }

    public void Update()
    {
        switch (Settings)
        {
            case Options.Resolution:
            ResolutionUpdate();
            break;

            case Options.Graphics:
            GraphicsUpdate();
            break;

            case Options.Audio:
            AudioUpdate();
            break;

            case Options.Controls:
            ControlsUpdate();
            break;
        }
    }

    #region Updates

    public void ResolutionUpdate()
    {
        selectors[0].SetActive(true);
    }

    public void GraphicsUpdate()
    {
        selectors[1].SetActive(true);
    }

    public void AudioUpdate()
    {
        selectors[2].SetActive(true);
    }

    public void ControlsUpdate()
    {
        selectors[3].SetActive(true);
    }

    #endregion

    #region Buttons

    public void SetResolution()
    {
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);

        Settings = Options.Resolution;
    }

    public void SetGraphics()
    {
        selectors[0].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);

        Settings = Options.Graphics;
    }

    public void SetAudio()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[3].SetActive(false);

        Settings = Options.Audio;
    }

    public void SetControllers()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);

        Settings = Options.Controls;
    }

    #endregion

    #region settings

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    #endregion
}
