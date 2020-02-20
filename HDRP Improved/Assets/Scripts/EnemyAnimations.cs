using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public Animator anim;
    public EnemyTest enemy;

    public void Initialize()
    {
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<EnemyTest>();
    }

    public void MyUpdate()
    {
        SetAnimWalk();
        SetAnimRun();
    }

    #region Animation Methods

    public void SetAnimWalk()
    {
        if (enemy.walk)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }

    public void SetAnimRun()
    {
        if (enemy.run)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    public void SetAnimSurprise()
    {
        anim.SetTrigger("Surprise");
    }

    public void SetAnimDetection()
    {
        anim.SetTrigger("Detection");
    }

    public void SetAnimHit()
    {
        anim.SetTrigger("Hit");
    }

    public void SetAnimDead()
    {
        anim.SetTrigger("Die");
    }

    public void SetAnimAttack()
    {
        anim.SetTrigger("Attack");
    }

    #endregion
}
