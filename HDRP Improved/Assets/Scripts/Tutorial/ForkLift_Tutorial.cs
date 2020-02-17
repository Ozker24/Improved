using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLift_Tutorial : MonoBehaviour
{
    public enum AudioState {None, Start, idle, Finish}
    public AudioState states;

    [SerializeField] GameObject forkLiftCargo;
    public float cargoInitPos;
    public float finalPos;

    public float timeCounter;
    public float liftJourney;
    public bool liftCargo;
    public bool fallCargo;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip startClip;
    public AudioClip idleClip;
    public AudioClip finishClip;

    [Header("After Lifting")]
    public Collider triggerFall;
    [SerializeField] float fallMultipier;

    public void Start()
    {
        cargoInitPos = forkLiftCargo.transform.position.y;
    }

    public void Update()
    {
        if (liftCargo)
        {
            LiftCargo();
            timeCounter += Time.deltaTime;
            triggerFall.enabled = true;
        }

        if (fallCargo)
        {
            timeCounter += Time.deltaTime * fallMultipier;
            FallCargo();
        }

        SetAudio();
    }

    void LiftCargo()
    {
        if (timeCounter < 1)
        {
            forkLiftCargo.transform.position = new Vector3(forkLiftCargo.transform.position.x, Mathf.Lerp(cargoInitPos, finalPos, timeCounter), forkLiftCargo.transform.position.z);
        }
        else if (timeCounter >= 1)
        {
            liftCargo = false;
            timeCounter = 0;
        }
    }

    void FallCargo()
    {
        if (timeCounter < 1)
        {
            forkLiftCargo.transform.position = new Vector3(forkLiftCargo.transform.position.x, Mathf.Lerp(finalPos, cargoInitPos, timeCounter), forkLiftCargo.transform.position.z);
        }
        else if (timeCounter >= 1)
        {
            fallCargo = false;
            timeCounter = 0;
            Invoke("DestroyCargo", 0.1f);
        }
    }

    void DestroyCargo()
    {
        forkLiftCargo.SetActive(false);
    }

    void SetAudio()
    {
        switch (states)
        {
            case AudioState.Start:
                AudioStartUpdate();
                break;
            case AudioState.idle:
                //AudioIdleUpdate();
                break;
            case AudioState.Finish:
                AudioFinishUpdate();
                break;
        }
    }

    void audioStartSet()
    {
        source.clip = startClip;
        source.Play();
        states = AudioState.Start;
    }

    public void AudioFinishSet()
    {
        states = AudioState.Finish;
        source.Stop();
        source.clip = finishClip;
        source.loop = false;
        source.Play();
    }

    void AudioStartUpdate()
    {
        if (!source.isPlaying)
        {
            source.clip = idleClip;
            source.loop = true;
            source.Play();
            states = AudioState.idle;
        }
    }

    void AudioIdleUpdate()
    {
        if (!source.isPlaying)
        {
            /*states = AudioState.idle;
            source.Stop();
            source.clip = idleClip;
            source.loop = true;
            source.Play();*/
        }
    }

    void AudioFinishUpdate()
    {
        if (!source.isPlaying)
        {
            states = AudioState.None;
            source.Stop();
            source.clip = null;
            source.loop = false;
            SetFallCargo();
        }
    }

    public void SetLiftCargo()
    {
        liftCargo = true;
        audioStartSet();
    }

    public void SetFallCargo()
    {
        fallCargo = true;
    }
}
