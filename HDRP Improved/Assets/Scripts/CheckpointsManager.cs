using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public GameObject[] checkPoints;
    public int actualCheckPoint;

    public void SetNewCheckPoint()
    {
        if (actualCheckPoint < checkPoints.Length)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                checkPoints[i].SetActive(false);
            }

            checkPoints[actualCheckPoint].SetActive(true);
        }
        else if (actualCheckPoint >= checkPoints.Length)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                checkPoints[i].SetActive(false);
            }

            Debug.LogWarning("There Are no More Check Points");
        }
    }
}
