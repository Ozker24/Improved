using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    public BoxCollider coll;
    public PlayerController player;

    public void Initialize()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void AppearHit()
    {
        coll.enabled = true;

        Debug.Log("Active");

        StartCoroutine(DesappearHit());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyTest enemy = other.GetComponent<EnemyTest>();
            enemy.StunnedSet();
            enemy.Damage(player.fistDamage);
        }
    }

    IEnumerator DesappearHit()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        coll.enabled = false;
    }
}
