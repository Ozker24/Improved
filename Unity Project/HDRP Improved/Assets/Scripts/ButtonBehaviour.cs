using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Debug.Log("Exit");

        Application.Quit();
    }
    
}
