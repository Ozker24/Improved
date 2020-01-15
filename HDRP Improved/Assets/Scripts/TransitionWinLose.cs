using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionWinLose : MonoBehaviour
{
    [SerializeField] GameManager GM;
    [SerializeField] ButtonBehaviour button;

    [Header("FadeInSources")]
    public Image toFadeOut;
    public Color fadeOutColor;

    [Header("Time Variables")]
    public float timeOfFadeOut;

    [Header("States")]
    public bool doFadeOut;
    public bool PauseEnd;

    public void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        button = GM.GetComponent<ButtonBehaviour>();
    }

    public void MyUpdate()
    {
        if (doFadeOut)
        {
            if (!PauseEnd)
            {
                PauseEnd = true;
                Time.timeScale = 0;
            }

            if (fadeOutColor.a < 1)
            {
                fadeOutColor.a += Time.unscaledDeltaTime / timeOfFadeOut;
            }
            else
            {
                fadeOutColor.a = 1;
                doFadeOut = false;

                if (GM.dead)
                {
                    button.GoToLose();
                }

                else if(GM.win)
                {
                    button.GoToWin();
                }

                Time.timeScale = 1;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
            }

            toFadeOut.color = fadeOutColor;
        }
    }
}
