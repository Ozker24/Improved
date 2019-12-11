using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public GameObject OptionMenu;
    public GameObject[] desactiveAtOptions;
    public bool inOptions;
    public Button optionsButton;


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
    
    public void GoToWin()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToLose()
    {
        SceneManager.LoadScene(4);
    }

    public void SetOptions()
    {
        inOptions = !inOptions;
        TransitionOptions(inOptions);
    }

    public void TransitionOptions( bool options)
    {
        for (int i = 0; i < desactiveAtOptions.Length; i++)
        {
            desactiveAtOptions[i].SetActive(!options);
        }

        OptionMenu.SetActive(options);
    }
}
