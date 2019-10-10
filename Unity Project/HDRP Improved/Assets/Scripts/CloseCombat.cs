using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombat : MonoBehaviour
{
    public PlayerController player;

    public bool canHit = true;
    public bool Hited = false;
    public bool point = false;
    public bool triggerAnim = false;

    public int ActualHit = 1;
    public int maximumHits;
    public float TimeCounter = 0;
    public float TimeToChangeHit;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void MyUpdate()
    {
        if (Hited)
        {
            if (TimeCounter >= TimeToChangeHit)
            {
                Debug.Log("Restart");
                TimeCounter = 0;
                ActualHit = 1;
                canHit = true;
                Hited = false;
                triggerAnim = false;

                player.anims.HitAnimations();
            }
            else
            {
                if (ActualHit != 1)
                {
                    TimeCounter += Time.deltaTime;
                }
            }
        }
    }

    public void DoHit()
    {
        if (canHit)
        {
            player.crouching = false;

            player.GM.ableToInput = false;
            player.axis = Vector2.zero;

            point = true;
            StartCoroutine(TurnOffPoint());

            TimeCounter = 0;
            Hited = true;
            triggerAnim = true;

            canHit = false;

            player.anims.HitAnimations();
            //ejecutar animcion
            ActualHit++;
            if (ActualHit > maximumHits)
            {
                ActualHit = 1;
            }
            if (ActualHit == 1)
            {
                Hited = false;
            }
            //canHit = true; // se ha de hacer al acabar la animacion.
        }
    }

    IEnumerator TurnOffPoint()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        point = false;
    }
}
