using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryGate : MonoBehaviour
{
    [SerializeField] GameManager GM;
    [SerializeField] TransitionWinLose winLose;
    [SerializeField] PauseManager pause;

    public void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();
        pause = GM.GetComponent<PauseManager>();
    }

    public void GameEnded()
    {
        GM.win = true;
        winLose.doFadeOut = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameEnded();
        }
    }
}
