using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaWin : MonoBehaviour
{
    public GameObject[] enemies;
    public int enemiesCount = 10;

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(4);
        }
    }
}
