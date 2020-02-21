using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HealthPeace : MonoBehaviour
{
    public float health = 100;
    public float healthToLow;
    public float healthToHigh;
    public float addHealth;
    public float mediumLifePercentage;
    public float highLifePercentage;
    public GameObject healthGO;
    public Renderer healthRender;

    [Header("Colors")]
    public Color redLowLife;
    public Color redMediumLife;
    public Color greenHighLife;

    [Header ("TwinkleLife")]
    [SerializeField] float brightTimeCounter;
    [SerializeField] float brightMaxTime;
    [SerializeField] float brightPercentage;
    [SerializeField] float littleBrightTime;
    [SerializeField] bool bright;
    [SerializeField] bool wait;
    [SerializeField] bool doOnce;

    [Header("Audio")]
    [SerializeField] AudioMixer master;
    [SerializeField] AudioMixer SFX;
    [SerializeField] AudioMixerSnapshot lowSnap;
    [SerializeField] AudioMixerSnapshot normalSnap;
    [SerializeField] bool transite;


    public void Initialize()
    {
        healthRender = healthGO.GetComponent<Renderer>();
        redLowLife = Color.red;
        redMediumLife = redLowLife;
        greenHighLife = Color.green;
        lowSnap = SFX.FindSnapshot("LowHealth");
        normalSnap = master.FindSnapshot("Snapshot");
    }

    public void MyUpdate()
    {
        if (health <= healthToLow)
        {
            AnimateHealth();
            if (!transite)
            {
                lowSnap.TransitionTo(0.1f);
                transite = true;
            }
        }
        else if (health > healthToLow && health < healthToHigh)
        {
            if (transite)
            {
                normalSnap.TransitionTo(2);
                transite = false;
            }

            MediumLife();
            brightPercentage = 1;
            doOnce = false;
            wait = false;
            bright = false;
            brightTimeCounter = 0;

            highLifePercentage = 0;
            greenHighLife.a = highLifePercentage;
        }
        else if (health >= healthToHigh)
        {
            mediumLifePercentage = 0;
            redMediumLife.a = mediumLifePercentage;

            HighLife();
        }
    }

    void MediumLife()
    {
        mediumLifePercentage = Mathf.Clamp01((health - healthToLow) / (healthToHigh - healthToLow));

        mediumLifePercentage = (mediumLifePercentage - 1) * (-1); // invertir el valor del porcentaje
        redMediumLife.a = mediumLifePercentage;

        healthRender.sharedMaterial.SetColor("_BaseColor", redMediumLife);
        //healthRender.sharedMaterial.EnableKeyword("_EMISSION");
        //DynamicGI.UpdateEnvironment();
        redMediumLife.a = 1f;
        healthRender.sharedMaterial.SetColor("_EmissiveColorLDR", redMediumLife);
    }

    void HighLife()
    {
        highLifePercentage = Mathf.Clamp01((health - healthToHigh) / (100 - healthToHigh));

        greenHighLife.a = highLifePercentage;

        greenHighLife.r = Random.Range(0f, 3f);
        greenHighLife.g = Random.Range(0f, 3f);
        greenHighLife.b = Random.Range(0f, 3f);
        greenHighLife.a = 1f;
        healthRender.sharedMaterial.SetColor("_BaseColor", greenHighLife);
        //healthRender.material.EnableKeyword("_EMISSION");
        //DynamicGI.UpdateEnvironment();

        //greenHighLife.r = Random.Range(0f, 1f);
        //greenHighLife.g = Random.Range(0f, 1f);
        //greenHighLife.b = Random.Range(0f, 1f);
        //greenHighLife.a = Random.Range(0f, 1f);
        healthRender.sharedMaterial.SetColor("_EmissiveColor", greenHighLife);
        healthRender.sharedMaterial.SetColor("_EmissiveColorLDR", greenHighLife);
        //healthRender.sharedMaterial.SetFloat("_EmissiveIntensity", 10000f);

    }

    void AnimateHealth()
    {
        if (bright)
        {
            if (brightTimeCounter >= brightMaxTime)
            {
                bright = false;
                brightTimeCounter = 0;
            }
            else
            {
                brightTimeCounter += Time.deltaTime;
                brightPercentage = Mathf.Clamp01(brightTimeCounter / brightMaxTime);
            }
        }
        else
        {
            if (!wait)
            {
                if (brightTimeCounter >= brightMaxTime)
                {
                    wait = true;
                    brightTimeCounter = 0;
                }
                else
                {
                    brightTimeCounter += Time.deltaTime;
                    brightPercentage = Mathf.Clamp01(brightTimeCounter / brightMaxTime);
                }

                if (!wait)
                {
                    brightPercentage = (brightPercentage - 1) * (-1);
                }
                else
                {
                    brightPercentage = 0;
                }
            }
        }

        if (wait)
        {
            brightPercentage = 1;

            if (!doOnce)
            {
                StartCoroutine(LittleBright());
                doOnce = true;
            }
        }

        redLowLife.a = brightPercentage;

        healthRender.sharedMaterial.SetColor("_BaseColor", redLowLife);
        //healthRender.material.EnableKeyword("_EMISSION");
        //DynamicGI.UpdateEnvironment();
        redLowLife.a = 1f;
        healthRender.sharedMaterial.SetColor("_EmissiveColorLDR", redLowLife);
    }

    public void Health()
    {
        health += addHealth;

        if (health >= 100)
        {
            health = 100;
        }
    }

    IEnumerator LittleBright()
    {
        yield return new WaitForSeconds(littleBrightTime);
        wait = false;
        bright = true;
        doOnce = false;
    }
}
