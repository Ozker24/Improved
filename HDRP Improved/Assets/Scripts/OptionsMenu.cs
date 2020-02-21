using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    public enum Options { Resolution, Graphics, Audio, Controls, InputSettings };
    public Options Settings;

    [Header("Dependencie")]
    public OptionsManager OM;
    public ShowControllers showControllers;
    public FirstButtonGUI FBGui;
    public GameManager GM;
    public DevicesDetector devices;
    public LiftGammaGain LGG;
    public Volume volume;

    [Header("StorageVariables")]
    public float audioValue;
    public float effectsValue;
    public float musicValue;
    public int selectedResolution;
    public int selectedQuality;
    public float cameraYSensitibityPC;
    public float cameraXSensitibityPC;
    public float cameraYSensitibityController;
    public float cameraXSensitibityController;
    public bool invertCameraYPC;
    public bool invertCameraXPC;
    public bool invertCameraYControllers;
    public bool invertCameraXControllers;
    public float gammaValue;

    [Header("To Hide")]
    public GameObject[] selectors;
    public GameObject[] settings;

    [Header("Qualities")]
    public Dropdown dropQuality;
    public string[] qualities;

    [Header("Resolutions")]
    public int currentResolution;
    public Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public bool useActualResolution;

    public Slider gammaSlider;
    //public Toggle AutoResToggle;

    [Header("Audio")]
    public AudioMixer master;
    public AudioMixer effects;
    public AudioMixer music;
    public Slider sliderMasterAud;
    public Slider effectsSlider;
    public Slider musicSlider;

    [Header("Input Settings")]
    public GameObject pCInputSettings;
    public GameObject controllerInputSettings;

    public Slider ySensitibitySliderPC;
    public Slider xSensitibitySliderPC;

    public Slider ySensitibitySlider;
    public Slider xSensitibitySlider;

    public Text ySliderValue;
    public Text xSliderValue;

    public Text ySliderValuePC;
    public Text xSliderValuePC;

    public Toggle invertXPC;
    public Toggle invertYPC;
    public Toggle invertXController;
    public Toggle invertYController;

    [Header("GUI")]
    public int currentOption;
    public int maxOptions;

    [Header("States")]
    public bool inOptions;
    public bool inInputSettings;

    public void Start()
    {
        devices = GetComponent<DevicesDetector>();

        qualities = QualitySettings.names;

        Settings = Options.Graphics;

        ChangeResolution();
        //InitializeResolutions();

        if (volume != null)
        {
            LiftGammaGain tmp;
            if (volume.profile.TryGet<LiftGammaGain>(out tmp))
            {
                LGG = tmp;
            }
        }

        LoadOptions();

        SetGamma();

        SetYSensitibityPC();
        SetXSensitibityPC(); //comprobar para la primera vez si los sets van bien tras el load

        SetXSensitibityController();
        SetYSensitibityController();

        SetMusicVolume(musicSlider.value);
        SetEffectsVolume(effectsSlider.value);

        //SetToggleInvertYPC();
        //SetToggleInvertXPC();
        //SetToggleInvertYController();
        //SetToggleInvertXController();

        ChangeSlideTextValue();
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

            case Options.InputSettings:
                InputsUpdate();
                break;
        }

        if (GM != null)
        {
            inOptions = GM.options;

            if (!GM.options)
            {
                inInputSettings = false;
            }
        }

        DebugGamma();

        SetCurrentOption();

        ChangeInputSettings();
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

    public void InputsUpdate()
    {
        selectors[4].SetActive(true);
        settings[4].SetActive(true);
    }

    #endregion

    #region Buttons

    public void SetGraphics()
    {
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);
        selectors[4].SetActive(false);

        settings[1].SetActive(false);
        settings[2].SetActive(false);
        settings[3].SetActive(false);
        settings[4].SetActive(false);

        showControllers.showThem = false;

        inInputSettings = false;
        
        Settings = Options.Graphics;
    }

    public void SetResolution()
    {
        selectors[0].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);
        selectors[4].SetActive(false);

        settings[0].SetActive(false);
        settings[2].SetActive(false);
        settings[3].SetActive(false);
        settings[4].SetActive(false);

        showControllers.showThem = false;

        inInputSettings = false;

        Settings = Options.Resolution;
    }

    public void SetAudio()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[3].SetActive(false);
        selectors[4].SetActive(false);

        settings[0].SetActive(false);
        settings[1].SetActive(false);
        settings[3].SetActive(false);
        settings[4].SetActive(false);

        showControllers.showThem = false;

        inInputSettings = false;

        Settings = Options.Audio;
    }

    public void SetControllers()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);
        selectors[4].SetActive(false);

        settings[0].SetActive(false);
        settings[1].SetActive(false);
        settings[2].SetActive(false);
        settings[4].SetActive(false);

        showControllers.showThem = true;

        inInputSettings = false;

        Settings = Options.Controls;
    }

    public void SetInputs()
    {
        selectors[0].SetActive(false);
        selectors[1].SetActive(false);
        selectors[2].SetActive(false);
        selectors[3].SetActive(false);

        settings[0].SetActive(false);
        settings[1].SetActive(false);
        settings[2].SetActive(false);
        settings[3].SetActive(false);

        showControllers.showThem = false;

        inInputSettings = true;

        Settings = Options.InputSettings;
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

    public void SetEffectsVolume(float volume)
    {
        effects.SetFloat("EffectsVolume", volume);

        effectsValue = volume;
    }

    public void SetMusicVolume(float volume)
    {
        music.SetFloat("MusicVolume", volume);

        musicValue = volume;
    }

    public void ChangeResolution()
    {
        if (resolutionDropdown.value == 0)
        {
            Screen.SetResolution(1280, 720, true);
            Debug.Log("1280");
        }
        else if (resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1600, 1200, true);
            Debug.Log("1600");
        }
        else if (resolutionDropdown.value == 2)
        {
            Screen.SetResolution(1920, 1080, true);
            Debug.Log("1920");
        }
    }

    public void ChangeInputSettings()
    {
        if (inInputSettings)
        {
            if (devices.PS4Controller | devices.XBoxOneController)
            {
                ChangeSlideTextValue();
                controllerInputSettings.SetActive(true);
                pCInputSettings.SetActive(false);
            }
            
            else if (!devices.PS4Controller && !devices.XBoxOneController)
            {
                controllerInputSettings.SetActive(false);
                pCInputSettings.SetActive(true);
            }
        }
    }

    public void SetYSensitibityPC()
    {
        cameraYSensitibityPC = ySensitibitySliderPC.value;
    }

    public void SetXSensitibityPC()
    {
        cameraXSensitibityPC = xSensitibitySliderPC.value;
    }

    public void SetYSensitibityController()
    {
        cameraYSensitibityController = ySensitibitySlider.value;
    }

    public void SetXSensitibityController()
    {
        cameraXSensitibityController = xSensitibitySlider.value;
    }

    public void ChangeSlideTextValue()
    {
        ySliderValue.text = ySensitibitySlider.value.ToString();

        xSliderValue.text = xSensitibitySlider.value.ToString();
    }

    public void ChangeSlideTextValuePC()
    {
        ySliderValuePC.text = ySensitibitySliderPC.value.ToString();

        xSliderValuePC.text = xSensitibitySliderPC.value.ToString();
    }

    public void SetToggleInvertYPC()
    {
        invertCameraYPC = invertYPC.isOn;
    }

    public void SetToggleInvertXPC()
    {
        invertCameraXPC = invertXPC.isOn;
    }

    public void SetToggleInvertYController()
    {
        invertCameraYControllers = invertYController.isOn;
    }

    public void SetToggleInvertXController()
    {
        invertCameraXControllers = invertXController.isOn;
    }

    public void AsignResolution()
    {
        //Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);

        selectedResolution = resolutionDropdown.value;
        ChangeResolution();

        //teclear por codigo propiamente las resoluciones.
    }

    public void SetGamma()
    {
        gammaValue = gammaSlider.value;

        if (LGG != null)
        {
            LGG.gamma.value = new Vector4(1, 1, 1, gammaValue - 1);
        }
    }

    public void DebugGamma()
    {
        //Debug.Log(LGG.gamma.value.w);
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

    #region GUI

    public void SetCurrentOption()
    {
        if (inOptions)
        {
            if (Input.GetButtonDown("Pos Horizontal GUI"))
            {
                if (currentOption < maxOptions)
                {
                    currentOption++;
                    ChangeOption();
                }
            }

            if (Input.GetButtonDown("Neg Horizontal GUI"))
            {
                if (currentOption > 0)
                {
                    currentOption--;
                    ChangeOption();
                }
            }
        }
    }

    public void ChangeOption()
    {
        if (currentOption == 0)
        {
            SetGraphics();
            FBGui.ChangeFBOnOptions();
        }

        if (currentOption == 1)
        {
            SetResolution();
            FBGui.ChangeFBOnResolution();
        }

        if (currentOption == 2)
        {
            SetAudio();
            FBGui.ChangeFBOnAudio();
        }

        if (currentOption == 3)
        {
            SetControllers();
        }

        if (currentOption == 4)
        {
            SetInputs();
            FBGui.ChangeFBOnInputs();
            ChangeSlideTextValuePC();
            ChangeSlideTextValue();
        }
    }

    public void SetInOptions()
    {
        inOptions =! inOptions;
    }

    public void SetFalseInInputs()
    {
        inInputSettings = false;
    }

    public void ResetCurrentOption()
    {
        currentOption = 0;
    }

    #endregion

    public void SaveOptions()
    {
        OptionSaveSystem.SaveOptions(this);
    }

    public void LoadOptions()
    {
        OptionsManager data = OptionSaveSystem.LoadOptions();

        if (data != null)
        {
            sliderMasterAud.value = data.audioValue;
            effectsSlider.value = data.effectsValue;
            musicSlider.value = data.musicValue;
            resolutionDropdown.value = data.selectedResolution;
            dropQuality.value = data.selectedQuality;
            ySensitibitySliderPC.value = data.cameraYSensitibityPC;
            xSensitibitySliderPC.value = data.cameraXSensitibityPC;
            ySensitibitySlider.value = data.cameraYSensitibityController;
            xSensitibitySlider.value = data.cameraXSensitibityController;
            invertYPC.isOn = data.invertCameraYPC;
            invertXPC.isOn = data.invertCameraXPC;
            invertYController.isOn = data.invertCameraYControllers;
            invertXController.isOn = data.invertCameraXControllers;
            gammaSlider.value = data.gammaValue;
            //LGG.gamma.value = new Vector4(1,1,1, data.gammaValue);
        }
    }
}
