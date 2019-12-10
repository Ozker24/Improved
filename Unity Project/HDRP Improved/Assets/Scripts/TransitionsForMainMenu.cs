using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionsForMainMenu : MonoBehaviour
{
    [SerializeField] ButtonBehaviour button;
    [SerializeField] AnimationsControlMainMenu anim;

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
    public float timeToFadeLogo;

    [Header("States")]
    public bool doFadeIn;
    public bool doFadeOut;

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
        //toFadeOut.color = fadeOutColor;
    }

    public void Update()
    {
        if (doFadeIn)
        {
            FadeIn();
        }

        if (doFadeOut)
        {
            anim.SetButtons(false);

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
            doFadeIn = false;
            StartCoroutine(AppearLogo());
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
            button.GoToGameplay();
        }

        toFadeOut.color = fadeOutColor;
    }

    public void CallFadeOut()
    {
        doFadeOut = true;
    }

    IEnumerator AppearLogo()
    {
        yield return new WaitForSeconds(timeToFadeLogo);
        anim.doFadeIn = true;
    }
        
}
