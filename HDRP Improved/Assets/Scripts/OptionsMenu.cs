﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public enum Options { Resolution, Graphics, Audio, Controls };
    public Options Settings;

    [Header("StorageVariables")]
    public float audioValue;
    public int selectedResolution;
    public int selectedQuality;

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
    public Dropdown resolutionDropdown;
    public bool useActualResolution;
    //public Toggle AutoResToggle;

    [Header("Audio")]
    public AudioMixer master;
    public Slider sliderMasterAud;

    public void Start()
    {
        qualities = QualitySettings.names;

        Settings = Options.Graphics;

        InitializeResolutions();

        LoadOptions();
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

    #region SetOptions

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);

        selectedQuality = quality;
    }

    public void SetVolume(float volume)
    {
        master.SetFloat("MasterVolume", volume);

        audioValue = volume;
    }

    public void AsignResolution()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);

        selectedResolution = resolutionDropdown.value;
    }

    public void InitializeResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

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

        resolutionDropdown.AddOptions(resolutionsList);

        resolutionDropdown.value = selectedResolution;

        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.RefreshShownValue();
    }

    #endregion

    public void SaveOptions()
    {
        OptionSaveSystem.SaveOptions(this);
    }

    public void LoadOptions()
    {
        OptionsManager data = OptionSaveSystem.LoadOptions();

        sliderMasterAud.value = data.audioValue;
        resolutionDropdown.value = data.selectedResolution;
        dropQuality.value = data.selectedQuality;
    }
}
