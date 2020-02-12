using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public enum Options { Resolution, Graphics, Audio, Controls };
    public Options Settings;

    [Header("OptionsManager")]
    public OptionsManager OM;

    [Header("To Hide")]
    public GameObject[] selectors;
    public GameObject[] settings;

    [Header("Qualities")]
    public Dropdown dropQuality;
    public string[] qualities;

    [Header("Resolution")]
    public Resolution[] resolutions;
    public int currentResolution;
    public Dropdown dropdown;
    public bool useActualResolution;
    //public Toggle AutoResToggle;

    [Header("Audio")]
    public AudioMixer master;
    public Slider sliderMasterAud;

    public void Start()
    {
        qualities = QualitySettings.names;

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualityLoad();
        }

        if (PlayerPrefs.HasKey("VolumeMaster"))
        {
            AudioLoad();
        }

        Settings = Options.Graphics;

        InitializeResolutions();
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

    public void GraphicsUpdate()
    {
        selectors[0].SetActive(true);
        settings[0].SetActive(true);
    }

    public void ResolutionUpdate()
    {
        selectors[1].SetActive(true);
        settings[1].SetActive(true);
    }

    public void AudioUpdate()
    {
        selectors[2].SetActive(true);
        settings[2].SetActive(true);
    }

    public void ControlsUpdate()
    {
        selectors[3].SetActive(true);
        settings[3].SetActive(true);
    }

    #endregion

    #region Buttons

    public void SetGraphics()
    {
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);

        settings[1].SetActive(false);
        settings[2].SetActive(false);
        settings[3].SetActive(false);

        Settings = Options.Graphics;
    }

    public void SetResolution()
    {
        selectors[0].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);

        settings[0].SetActive(false);
        settings[2].SetActive(false);
        settings[3].SetActive(false);

        Settings = Options.Resolution;
    }

    public void SetAudio()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[3].SetActive(false);

        settings[0].SetActive(false);
        settings[1].SetActive(false);
        settings[3].SetActive(false);

        Settings = Options.Audio;
    }

    public void SetControllers()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);

        settings[0].SetActive(false);
        settings[1].SetActive(false);
        settings[2].SetActive(false);

        Settings = Options.Controls;
    }

    #endregion

    #region settings

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);

        PlayerPrefs.SetInt("Quality", quality);

        OM.selectedQuality = quality;
    }

    public void SetVolume(float volume)
    {
        //Debug.Log(volume);

        master.SetFloat("MasterVolume", volume);

        PlayerPrefs.SetFloat("VolumeMaster", volume);

        OM.audioValue = volume;
    }

    public void AsignResolution()
    {
        //Debug.Log(dropdown.value);

        Screen.SetResolution(resolutions[dropdown.value].width, resolutions[dropdown.value].height, Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution", dropdown.value);

        OM.selectedResolution = dropdown.value;

        //Debug.Log(dropdown.value);
    }

    public void InitializeResolutions()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        currentResolution = 0;

        List<string> resolutionsList = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + "x" + resolutions[i].height;
            resolutionsList.Add(resolution);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            } //esto calcula cual es tu resolucion actual
        }

        dropdown.AddOptions(resolutionsList);

        if (PlayerPrefs.HasKey("Resolution"))
        {
            dropdown.value = PlayerPrefs.GetInt("Resolution");
        }

        else
        {
            currentResolution = PlayerPrefs.GetInt("Resolution");
            dropdown.value = currentResolution;
        }

        dropdown.RefreshShownValue();

        dropdown.RefreshShownValue();
    }

    #endregion

    #region Load

    public void QualityLoad()
    {
        int initQuality = PlayerPrefs.GetInt("Quality");

        QualitySettings.SetQualityLevel(initQuality);

        dropQuality.value = initQuality;

        dropQuality.RefreshShownValue();
    }

    public void AudioLoad()
    {
        float initialVolume = PlayerPrefs.GetFloat("VolumeMaster");

        master.SetFloat("MasterVolume", initialVolume);

        sliderMasterAud.value = initialVolume;
    }
    #endregion
}
