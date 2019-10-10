using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    public enum State { Chase, Sound, Patrol, Attack, Stunned, die};
    public State states;

    public NavMeshAgent agent;

    public AudioPlayer audPlay;

    public GameManager gm;

    [Header ("Sound")]
    public SoundManager sound;

    public Vector3 positionWhereSound;

    public float distToSearch;

    [Header("Chase")]
    public bool Detected;

    public PlayerController player;
    public Vector3 playerPos;

    [Header("Attack")]
    public float distToAttack;
    public float distToScape;

    public float restTimeToAttack;

    public int hitDamage;

    public BoxCollider attackArea;

    public float attackTimeCounter;
    public float timeAttackFinished;

    [Header("Life")]
    public int currentLife;
    public int maxLife;

    [Header("Stunned")]
    public float timeStunned;
    public float stunnedTimeCounter;

    [Header("Die")]

    public float timeToDie;

    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        sound = GameObject.FindGameObjectWithTag("Managers").GetComponent<SoundManager>();

        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();

        audPlay = GetComponentInChildren<AudioPlayer>();

        attackArea.enabled = false;

        states = State.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case State.Sound:
                SoundUpdate();
                break;

            case State.Patrol:
                PatrolUpdate();
                break;

            case State.Chase:
                ChaseUpdate();
                break;

            case State.Attack:
                AttackUpdate();
                break;

            case State.Stunned:
                StunnedUpdate();
                break;

            case State.die:
                DieUpdate();
                break;

            default:
                break;
        }
    }

    #region Updates

    public void SoundUpdate()
    {
        if(!Detected)
        {
            agent.SetDestination(positionWhereSound);

            if (Vector3.Distance(transform.position, positionWhereSound) <= distToSearch)
            {
                Debug.Log("Arrived");
                PatrolSet();
            }
        }

        else
        {
            ChaseSet();
        }
    }

    public void PatrolUpdate()
    {
        Detected = gm.detected;

        positionWhereSound = sound.soundPosition; //cuando ya pase un rato alomejor te interesa que cambien a la nueva posicion

        if (positionWhereSound != Vector3.zero)
        {
            SoundSet();
        }

        if (Detected)
        {
            ChaseSet();
        }
    }

    public void ChaseUpdate()
    {
        if (Detected)
        {
            playerPos = player.transform.position;

            agent.SetDestination(playerPos);
        }
        else
        {
            PatrolSet();
        }


        if ((Vector3.Distance(transform.position, playerPos) <= distToAttack))
        {
            AttackSet();
        }
    }

    public void AttackUpdate()
    {
        if (attackTimeCounter >= timeAttackFinished)
        {
            attackTimeCounter = 0;
            DoAttack();
        }
        else
        {
            attackTimeCounter += Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= distToScape)
        {
            ChaseSet();
        }
    }

    public void StunnedUpdate()
    {
        if (stunnedTimeCounter >= timeStunned)
        {
            stunnedTimeCounter = 0;

            if (Detected)
            {
                ChaseSet();
            }
            else
            {
                PatrolSet();
            }
        }
        else
        {
            stunnedTimeCounter += Time.deltaTime;
        }
    }

    public void DieUpdate()
    {
        Destroy(gameObject, timeToDie);
    }

    #endregion

    #region Sets
    
    public void SoundSet()
    {
        agent.isStopped = false;
        attackTimeCounter = 0;
        agent.stoppingDistance = distToSearch;
        states = State.Sound;
    }

    public void PatrolSet()
    {
        agent.isStopped = true;
        sound.soundPosition = Vector3.zero;
        attackTimeCounter = 0;
        states = State.Patrol;
    }

    public void ChaseSet()
    {
        agent.isStopped = false;
        agent.stoppingDistance = distToAttack;
        attackTimeCounter = 0;
        states = State.Chase;
    }

    public void AttackSet()
    {
        agent.isStopped = true;

        attackTimeCounter = timeAttackFinished - restTimeToAttack;

        states = State.Attack;
    }

    public void StunnedSet()
    {
        agent.isStopped = true;
        states = State.Stunned;
    }

    public void DieSet()
    {
        agent.isStopped = true;
        attackArea.enabled = false;
        Detected = false;
        states = State.die;
    }

    #endregion

    public void DoAttack()
    {
        audPlay.Play(0, 1, 1);

        attackArea.enabled = true;
        //Debug.Log("Active");

        StartCoroutine(DissableAttackArea());
    }

    public void Damage(int life)
    {
        
        currentLife -= life;

        if (currentLife <= 0)
        {
            currentLife = 0;
            DieSet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("DAMAGE PLAYER");

            player.Damage(hitDamage);

            //Debug.Log(player.life.health);
        }
    }

    IEnumerator DissableAttackArea()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        attackArea.enabled = false;
    }
}
