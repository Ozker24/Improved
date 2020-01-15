using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsControlMainMenu : MonoBehaviour
{
    public Image logo;
    public Color logoColor;
    public Image toFadebuttons;
    public Color ButtonsColor;
    public GameObject pressAnyKey;
    public Button playButton;
    public Button optionButton;
    public Button exitButton;

    public float timeOfFadeIn;
    public float timeOfFadeInButtons;
    public float timeTofadePressKey;
    public bool doFadeIn;
    public bool doFadeInButtons;
    public bool canPressAnyKey;

    public void Start()
    {
        pressAnyKey.SetActive(false);
        logoColor.a = 0;
        logo.color = logoColor;
        ButtonsColor.a = 1;
        toFadebuttons.color = ButtonsColor;

        SetButtons(false);
    }

    public void Update()
    {
        AnyKey();

        if (doFadeIn)
        {
            FadeInLogo();
        }

        if (doFadeInButtons)
        {
            FadeInButtons();
        }
    }

    public void AnyKey()
    {
        if (canPressAnyKey)
        {
            if (Input.anyKeyDown)
            {
                pressAnyKey.SetActive(false);
                canPressAnyKey = false;
                doFadeInButtons = true;
            }
        }
    }

    public void FadeInLogo()
    {
        if (logoColor.a < 1)
        {
            logoColor.a += Time.deltaTime / timeOfFadeIn;
        }
        else
        {
            logoColor.a = 1;
            doFadeIn = false;
            StartCoroutine(AppearPressStart());
        }

        logo.color = logoColor;
    }

    public void FadeInButtons()
    {
        if (ButtonsColor.a > 0)
        {
            ButtonsColor.a -= Time.deltaTime / timeOfFadeInButtons;
        }
        else
        {
            ButtonsColor.a = 0;
            doFadeInButtons = false;

            SetButtons(true);
        }

        toFadebuttons.color = ButtonsColor;
    }

    public void SetButtons(bool active)
    {
        playButton.interactable = active;
        optionButton.interactable = active;
        exitButton.interactable = active;
    }

    IEnumerator AppearPressStart()
    {
        yield return new WaitForSeconds(timeTofadePressKey);
        pressAnyKey.SetActive(true);
        canPressAnyKey = true;
    }
}
