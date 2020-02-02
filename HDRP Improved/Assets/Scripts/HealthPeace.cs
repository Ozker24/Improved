using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPeace : MonoBehaviour
{
    public float health = 100;
    public float healthToTwinkle;
    public float addHealth;
    public float percentage;
    public GameObject healthGO;
    public Renderer healthRender;
    [Header("Colors")]
    public Color colorAfterTwinkle;
    public Color colorBeforeTwinkle;

    [Header ("TwinkleLife")]
    [SerializeField] float brightTimeCounter;
    [SerializeField] float brightMaxTime;
    [SerializeField] float brightPercentage;
    [SerializeField] float littleBrightTime;
    [SerializeField] bool bright;
    [SerializeField] bool wait;
    [SerializeField] bool doOnce;


    public void Initialize()
    {
        healthRender = healthGO.GetComponent<Renderer>();
        colorAfterTwinkle = Color.red;
        colorBeforeTwinkle = colorAfterTwinkle;
    }

    public void MyUpdate()
    {
        if (health <= healthToTwinkle)
        {
            AnimateHealth();
        }
        else
        {
            ShowLife();
            brightPercentage = 1;
            doOnce = false;
            wait = false;
            bright = false;
            brightTimeCounter = 0;
        }
    }

    void ShowLife()
    {
        percentage = Mathf.Clamp01((health - healthToTwinkle) / (100 - healthToTwinkle));

        percentage = (percentage - 1) * (-1); // invertir el valor del porcentaje
        colorBeforeTwinkle.a = percentage;

        healthRender.material.SetColor("_BaseColor", colorBeforeTwinkle);
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

        colorAfterTwinkle.a = brightPercentage;

        healthRender.material.SetColor("_BaseColor", colorAfterTwinkle);
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
