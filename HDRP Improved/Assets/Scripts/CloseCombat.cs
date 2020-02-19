using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombat : MonoBehaviour
{
    public PlayerController player;
    public HitArea normalModeHit;
    public HitArea improvedModeHit;

    public bool canHit = true;
    public bool Hited = false;
    public bool point = false;
    public bool triggerAnim = false;

    public int ActualHit = 1;
    public int maximumHits;
    public float TimeCounter = 0;
    public float TimeToChangeHit;

    public float secondsAbleToImput;
    public float initialSecondsAbleToInput;
    public float finalSecondsAbleToInput;

    [Header("Sounds")]
    public AudioSource basicSource;
    public AudioClip hitClips;
    public AudioClip executionClip;

    public EnemyTest enemyToExecute;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //normalModeHit = player.GetComponentInChildren<HitArea>();
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

        if (player.GM.improved)
        {
            maximumHits = player.improvedModeMaxFists;
        }
        else
        {
            maximumHits = player.normalModeMaxFists;
        }
    }

    public void DoHit()
    {
        if (canHit)
        {
            player.anims.SetAnimFist();
        }

        if (canHit && !player.canExecute)
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

            //ejecutar animcion
            player.anims.HitAnimations();

            if (!player.GM.improved)
            {
                normalModeHit.AppearHit();
            }
            else
            {
                improvedModeHit.AppearHit();
            }

            basicSource.PlayOneShot(hitClips);
            ActualHit++;

            if (ActualHit < maximumHits)
            {
                secondsAbleToImput = initialSecondsAbleToInput;
            }
            else if (ActualHit >= maximumHits)
            {
                secondsAbleToImput = finalSecondsAbleToInput;
            }

            if (ActualHit > maximumHits)
            {
                ActualHit = 1;
            }
            if (ActualHit == 1)
            {
                Hited = false;
            }

            Debug.Log(ActualHit);
            //canHit = true; // se ha de hacer al acabar la animacion.

            StartCoroutine(SetAbleToImput());
        }

        if (player.canExecute)
        { 
            if (enemyToExecute != null)
            {
                enemyToExecute.Damage(enemyToExecute.currentLife);
                basicSource.PlayOneShot(executionClip);
                enemyToExecute.executionCollider.enabled = false;
                enemyToExecute = null;
                player.canExecute = false;
            }
        }
    }

    IEnumerator TurnOffPoint()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        point = false;
    }



    IEnumerator SetAbleToImput()
    {
        yield return new WaitForSeconds(secondsAbleToImput);
        canHit = true;
        player.GM.ableToInput = true;
    }

    public void PlaySound()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = hitClips;
        source.Play();

        Destroy(source, source.clip.length);
    }
}
