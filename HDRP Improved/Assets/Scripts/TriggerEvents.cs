using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    [Header("AppearVictoryDoor")]
    [SerializeField] GameObject VictoryDoor;

    public void Test()
    {
        Debug.Log("HelloThere");
    }

    public void TestTwo()
    {
        Debug.Log("GeneralKenobi");
    }

    public void AppearVictoryDoor()
    {
        VictoryDoor.SetActive(true);
    }
}
