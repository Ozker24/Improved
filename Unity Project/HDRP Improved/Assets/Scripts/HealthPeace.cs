using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPeace : MonoBehaviour
{
    public float health = 100;
    public float addHealth;
    public SpriteRenderer sprite;

    public Color topHealth;
    public Color midHighHealth;
    public Color midLowHealth; 
    public Color lowHealth; 

    public void Initialize()
    {
        sprite.color = topHealth;
    }

    public void MyUpdate()
    {
        CheckLife();
    }

    public void CheckLife()
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
    }

    public void Health()
    {
        health += addHealth;

        if (health >= 100)
        {
            health = 100;
        }
    }
}
