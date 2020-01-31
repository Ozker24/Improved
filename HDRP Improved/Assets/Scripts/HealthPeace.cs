using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPeace : MonoBehaviour
{
    public float health = 100;
    public float healthToTwinkle;
    public float addHealth;
    public float percentage;
    //public SpriteRenderer sprite;
    public GameObject healthGO;
    public Renderer healthRender;
    public Animator animator;
    //public Material mat;
    public Color color;
    public Color colorBeforeTwinkle;

    /*public Color topHealth;
    public Color midHighHealth;
    public Color midLowHealth; 
    public Color lowHealth;*/

    public void Initialize()
    {
        //sprite.color = topHealth;
        healthRender = healthGO.GetComponent<Renderer>();
        color = Color.red;
        colorBeforeTwinkle = color;
        animator = GetComponent<Animator>();
    }

    public void MyUpdate()
    {
        //CheckLife();
        percentage = Mathf.Clamp01((health - healthToTwinkle) / (100 - healthToTwinkle));

        percentage = (percentage - 1) * (-1); // invertir el valor del porcentaje
        colorBeforeTwinkle.a = percentage;

        //Color testColor = Random.ColorHSV();

        if (health <= healthToTwinkle)
        {
            animator.SetBool("Twinkle", true);
            healthRender.material.SetColor("_BaseColor", colorBeforeTwinkle);
        }
        else
        {
            animator.SetBool("Twinkle", false);
            healthRender.material.SetColor("_BaseColor", colorBeforeTwinkle);
        }
    }

    /*public void CheckLife()
    {
        if (health <= 30)
        {
            sprite.color = lowHealth;
        }

        if (health <= 50 && health > 30)
        {
            sprite.color = midLowHealth;
        }

        if (health <= 80 && health > 50)
        {
            sprite.color = midHighHealth;
        }

        if (health >= 80)
        {
            sprite.color = topHealth;
        }
    }*/

    public void Health()
    {
        health += addHealth;

        if (health >= 100)
        {
            health = 100;
        }
    }
}
