using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] int actualCharacter;

    private void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
    }

    void Update()
    {
        ChangeCharacter();
        ShowCharacter() ;
    }

    void ShowCharacter()
    {
        characters[actualCharacter].SetActive(true);
    }

    void ChangeCharacter()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && actualCharacter < characters.Length - 1) actualCharacter++;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && actualCharacter > 0) actualCharacter--;

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
    }
}
