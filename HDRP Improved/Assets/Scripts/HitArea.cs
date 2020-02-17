using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    public BoxCollider coll;
    public PlayerController player;

    public float StunedTime;

    [Header("Sounds")]
    public AudioSource basicSource;
    public AudioClip HitSound;

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
            Debug.Log("Enemy");
            EnemyTest enemy = other.GetComponent<EnemyTest>();
            enemy.StunnedSet(StunedTime);

            if (!player.GM.improved)
            {
                enemy.Damage(player.normalModeFistDamage);
            }
            else
            {
                enemy.Damage(player.improvedModeFistDamage);
            }

            enemy.Detected = true;
            if (!enemy.dead)
            {
                basicSource.PlayOneShot(HitSound);
            }
        }
    }

    IEnumerator DesappearHit()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        coll.enabled = false;
    }

    public void PlaySound()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        // Configurar audiosource
        source.playOnAwake = false;
        source.clip = HitSound;
        source.PlayDelayed(0.3f);

        Destroy(source, source.clip.length);
    }
}
