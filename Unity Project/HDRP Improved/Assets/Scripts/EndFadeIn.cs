using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndFadeIn : MonoBehaviour
{
    [SerializeField] ButtonBehaviour button;

    [Header("FadeInSources")]
    public Image toFadeIn;
    public Color fadeInColor;

    [Header("FadeOutSources")]
    public Image toFadeOut;
    public Color fadeOutColor;

    [Header("Time Variables")]
    public float timeCounter;
    public float timeOfFadeIn;
    public float timeOfFadeOut;

    [Header("States")]
    public bool doFadeIn = true;
    public bool doFadeOut;

    public Button menuButton;
    public Button gameplayButton;

    public bool toMenu;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        doFadeIn = true;
        doFadeOut = false;

        fadeInColor.a = 1;
        toFadeIn.color = fadeInColor;

        fadeOutColor.a = 0;
        toFadeOut.color = fadeOutColor;

        menuButton.interactable = false;
    }

    public void Update()
    {
        if (doFadeIn)
        {
            FadeIn();
        }

        if (doFadeOut)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        if (fadeInColor.a > 0)
        {
            fadeInColor.a -= Time.deltaTime / timeOfFadeIn;
        }
        else
        {
            fadeInColor.a = 0;
            menuButton.interactable = true;

            doFadeIn = false;
        }

        toFadeIn.color = fadeInColor;
    }

    public void FadeOut()
    {
        if (fadeOutColor.a < 1)
        {
            fadeOutColor.a += Time.deltaTime / timeOfFadeOut;
        }
        else
        {
            fadeOutColor.a = 1;

            if (toMenu)
            {
                button.GoToMainMenu();
            }

            else
            {
                button.GoToGameplay();
            }
        }

        toFadeOut.color = fadeOutColor;
    }

    public void CallFadeOut()
    {
        doFadeOut = true;
        menuButton.interactable = false;
    }

    public void SetMenu()
    {
        toMenu = true;
    }
}
