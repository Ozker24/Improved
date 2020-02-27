using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] GameObject[] ernest;
    [SerializeField] GameObject[] fase2;
    [SerializeField] int actualCharacter;
    [SerializeField] CinemachineVirtualCamera[] cams;

    [Header("Umbrals")]
    [SerializeField] int characterUmbral;
    [SerializeField] int bigUmbral;
    [SerializeField] int lastBigUmbral;
    [SerializeField] int bigestUmbral;
    [SerializeField] int propsUmbral;
    [SerializeField] int lastPropsUmbral;
    [SerializeField] int gunUmbral;
    [SerializeField] int LastgunUmbral;

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
        if (actualCharacter == 0)
        {
            for (int i = 0; i < ernest.Length; i++)
            {
                ernest[i].SetActive(true);
            }
        }

        else if (actualCharacter == 2)
        {
            for (int i = 0; i < fase2.Length; i++)
            {
                fase2[i].SetActive(true);
            }
        }

        else
        {
            characters[actualCharacter].SetActive(true);
        }
    }

    void ChangeCharacter()
    {
        if (actualCharacter == 0)
        {
            for (int i = 0; i < ernest.Length; i++)
            {
                ernest[i].SetActive(false);
            }
        }

        else if (actualCharacter == 2)
        {
            for (int i = 0; i < fase2.Length; i++)
            {
                fase2[i].SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && actualCharacter < characters.Length - 1) actualCharacter++;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && actualCharacter > 0) actualCharacter--;

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }

        if (actualCharacter == characterUmbral)
        {
            cams[0].gameObject.SetActive(true);
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(false);
        }

        else if (actualCharacter == gunUmbral|| actualCharacter == LastgunUmbral)
        {
            cams[0].gameObject.SetActive(false);
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(true);
            cams[4].gameObject.SetActive(false);
        }

        else if (actualCharacter == propsUmbral || actualCharacter == lastPropsUmbral)
        {
            cams[0].gameObject.SetActive(false);
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(true);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(false);
        }

        else if (actualCharacter == bigUmbral || actualCharacter == lastBigUmbral)
        {
            cams[0].gameObject.SetActive(false);
            cams[1].gameObject.SetActive(true);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(false);
        }
        
        else if (actualCharacter == bigestUmbral)
        {
            cams[0].gameObject.SetActive(false);
            cams[1].gameObject.SetActive(false);
            cams[2].gameObject.SetActive(false);
            cams[3].gameObject.SetActive(false);
            cams[4].gameObject.SetActive(true);
        }

    }
}
