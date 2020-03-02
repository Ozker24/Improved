using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LogoBehaviour : MonoBehaviour
{
    public VideoPlayer vidPlayer;
    public bool endWaitSec;

    public void Start()
    {
        vidPlayer = GetComponent<VideoPlayer>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(SetWaitSec());
    }

    public void Update()
    {
        Skiplogo();
        GoToMenu();
    }

    void Skiplogo()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void GoToMenu()
    {
        if(endWaitSec)
        {
            if (!vidPlayer.isPlaying)
            {
                SceneManager.LoadScene(1);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    IEnumerator SetWaitSec()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        endWaitSec = true;
    }
}
