using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class expoSceneEntrance : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(5);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(1);
        }
    }
}
