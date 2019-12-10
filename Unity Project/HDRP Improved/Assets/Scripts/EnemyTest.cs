using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    public enum State { Chase, Sound, detect, Patrol, Attack, Stunned, die};
    public State states;

    [Header ("Dependences")]
    public NavMeshAgent agent;
    public AudioPlayer audPlay;
    public GameManager gm;
    [SerializeField] EnemyPlayerDetector playerDetector;
    [SerializeField] CombatArea combatArea;
    [SerializeField] EnemyAttackArea attack;

    [Header ("Sound")]
    public SoundManager sound;

    public Vector3 positionWhereSound;

    public float distToSearch;

    [Header("Chase")]
    public bool Detected;

    public PlayerController player;
    public Vector3 playerPos;


    [Header("Detection")]
    public float timeToDetect;
    public float timeCounterDetection;
    public bool callEnemies;

    [Header("Attack")]
    public float distToAttack;
    public float distToScape;

    public float restTimeToAttack;

    public int hitDamage;

    public BoxCollider attackArea;

    public float attackTimeCounter;
    public float timeAttackFinished;

    public float stunedTimeWhenMiss;

    public bool canDetectPlayer;
    public bool playerInArea;

    [Header("Attack Area")]
    public Transform attackAreaTrans;
    public Vector3 HalfStent;
    public LayerMask layer;

    [Header("Life")]
    public int currentLife;
    public int maxLife;

    [Header("Stunned")]
    public float timeStunned;
    public float stunnedTimeCounter;

    [Header("Die")]
    public float timeToDie;
    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        sound = GameObject.FindGameObjectWithTag("Managers").GetComponent<SoundManager>();

        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();

        audPlay = GetComponentInChildren<AudioPlayer>();

        playerDetector = GetComponentInChildren<EnemyPlayerDetector>();

        attack = GetComponentInChildren<EnemyAttackArea>();

        //attackArea.enabled = false;

        playerDetector.Initialize();

        states = State.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        playerDetector.MyUpdate();

        switch (states)
        {
            case State.Sound:
                SoundUpdate();
                break;

            case State.Patrol:
                PatrolUpdate();
                break;

            case State.detect:
                DetectUpdate();
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
        if(!Detected | !combatArea.detected)
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
            //ChaseSet();
            DetectSet();
        }
    }

    public void PatrolUpdate()
    {
        //Detected = gm.detected;

        positionWhereSound = sound.soundPosition; //cuando ya pase un rato alomejor te interesa que cambien a la nueva posicion

        if (positionWhereSound != Vector3.zero)
        {
            SoundSet();
        }

        if (Detected | combatArea.detected)
        {
            //ChaseSet();
            DetectSet();
        }
    }

    public void DetectUpdate()
    {
        if (timeCounterDetection >= timeToDetect)
        {
            timeCounterDetection = 0;

            if (!combatArea.detected)
            {
                combatArea.detected = true;
            }

            ChaseSet();
        }
        else
        {
            timeCounterDetection += Time.deltaTime;
        }
    }

    public void ChaseUpdate()
    {
        if (Detected | combatArea.detected)
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
            //DetectSet();
            ChaseSet();
        }
    }

    public void StunnedUpdate()
    {
        if (stunnedTimeCounter >= timeStunned)
        {
            stunnedTimeCounter = 0;

            if (Detected | combatArea.detected)
            {
                DetectSet();
                //ChaseSet(); // poner antes del chase el detector set.
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

    public void DetectSet()
    {
        agent.isStopped = true;
        states = State.detect;
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

    public void StunnedSet(float time)
    {
        agent.isStopped = true;
        timeStunned = time;
        states = State.Stunned;
    }

    public void DieSet()
    {
        agent.isStopped = true;
        //attackArea.enabled = false;

        Detected = false;
        playerInArea = false;
        canDetectPlayer = false;

        combatArea.enemiesNumber--;
        dead = true;
        Destroy(gameObject, timeToDie);

        states = State.die;
    }

    #endregion

    public void DoAttack()
    {
        audPlay.Play(0, 1, 1);

        //attackArea.enabled = true;
        //canDetectPlayer = true;
        Debug.Log("Attacking");
        
        if (attack.playerInArea)
        {
            Debug.Log("Player hit");
            player.Damage(hitDamage);
        }

        else if (!attack.playerInArea)
        {
            Debug.Log("Player Out");
            StunnedSet(stunedTimeWhenMiss);
        }

        //StartCoroutine(DissableAttackArea());
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

    public void nDrawGizmos()
    {
        Gizmos.DrawCube(attackAreaTrans.position, HalfStent * 2);
    }

    /*IEnumerator DissableAttackArea()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //canDetectPlayer = false;
    }*/
}
