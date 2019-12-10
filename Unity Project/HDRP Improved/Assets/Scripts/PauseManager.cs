using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameManager GM;
    public GameObject pauseMenu;
    public GameObject previousControls;

    public bool ableToPause;

    public void Initialize()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        pauseMenu.SetActive(false);
        previousControls.SetActive(true);

        Time.timeScale = 0;
    }

    public void MyUpdate()
    {
        if (GM.showControls)
        {
            if ( Input.anyKeyDown)
            {
                Time.timeScale = 1;

                GM.showControls = false;
                previousControls.SetActive(false);
                StartCoroutine(AbleToPause());
            }
        }
    }

    public void Pause()
    {
        if (ableToPause)
        {
            GM.pause = !GM.pause;
            CheckPause();
        }
    }

    public void CheckPause()
    {
        if (!GM.pause)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    IEnumerator AbleToPause()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        ableToPause = true;
    }
}
