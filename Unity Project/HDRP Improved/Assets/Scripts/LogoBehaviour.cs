using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoBehaviour : MonoBehaviour
{
    public enum State { FadeIn, FadeOut, Waiting};
    public State states;

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
    public float timeOfWait;

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fadeInColor.a = 0;
        toFadeIn.color = fadeInColor;

        fadeOutColor.a = 0;
        toFadeOut.color = fadeOutColor;
    }

    public void Update()
    {
        Skiplogo();

        switch (states)
        {
            case State.FadeIn:
            FadeIn();
            break;
            case State.Waiting:
            Waiting();
            break;
            case State.FadeOut:
            FadeOut();
            break;
        }
    }

    public void FadeIn()
    {
        if (fadeInColor.a < 1)
        {
            fadeInColor.a += Time.deltaTime / timeOfFadeIn;
        }
        else
        {
            fadeInColor.a = 1;
            states = State.Waiting;
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
            SceneManager.LoadScene(1);
            Cursor.lockState = CursorLockMode.None;
        }

        toFadeOut.color = fadeOutColor;
    }

    public void Waiting()
    {
        if (timeCounter >= timeOfWait)
        {
            timeCounter = 0;
            states = State.FadeOut;
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }

    void Skiplogo()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
