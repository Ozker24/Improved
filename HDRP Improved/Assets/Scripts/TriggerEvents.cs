using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    [Header("AppearVictoryDoor")]
    public GameObject VictoryDoor;
    public AudioSource source;

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
        source.Play();
        VictoryDoor.SetActive(true);
    }
}
