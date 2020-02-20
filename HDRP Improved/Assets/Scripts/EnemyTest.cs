using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    public enum State { Chase, Sound, detect, Patrol, Attack, Stunned, die, Stationary, Look};
    public State states;

    public bool IHaveSight;

    [Header("Dependences")]
    public EnemyTest itsef;
    public NavMeshAgent agent;
    public AudioPlayer audPlay;
    public GameManager gm;
    public StealthSystem stealth;
    public Rigidbody rigid;
    [SerializeField] EnemyPlayerDetector playerDetector;
    [SerializeField] EnemySight sight;
    [SerializeField] ExecutionArea execution;
    [SerializeField] CombatArea combatArea;
    [SerializeField] EnemyAttackArea attack;

    [Header("Chase")]
    public bool Detected;

    [SerializeField] float chaseSpeed;

    public PlayerController player;
    public Vector3 playerPos;

    public Vector3 positionWhereSound;

    public float distToSearch;

    [Header("Detection")]
    public float timeToDetect;
    public float timeCounterDetection;
    public bool sendDetectionInfo;
    public bool callEnemies;
    public bool addedToList;
    public bool wantToHear;
    public bool firstSearch;

    [Header("Look")]
    [SerializeField] float timeLookingCounter;
    [SerializeField] float maxTimeLooking;

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

    [Header("Execution Area")]
    public Collider executionCollider;

    [Header("Patrol")]
    [SerializeField] Transform[] pathPoints;
    [SerializeField] int currentPoint;
    [SerializeField] float patrolStopDist;
    [SerializeField] float patrolSpeed;
    [SerializeField] bool stayThere;

    [Header("Stationary")]
    [SerializeField] float stationaryTimeCounter;
    [SerializeField] float generalWaitOnPointTime;
    [SerializeField] float initGeneralWaitOnPointTime;
    [SerializeField] float waitOnPointTime;
    [SerializeField] float initWaitOnPointTime;
    [SerializeField] float initWaitComeFromSound;
    [SerializeField] float waitComeFromSound;
    [SerializeField] float percentageOfWait;
    [SerializeField] float initPercentageOfWait;
    [SerializeField] int randomNum;
    [SerializeField] bool stopOnThePoints;
    [SerializeField] bool stopOnPoint;
    [SerializeField] bool goToNearPoint;
    public bool comeFromSound;

    [Header("Life")]
    public float currentLife;
    public float maxLife;

    [Header("Stunned")]
    public float timeStunned;
    public float stunnedTimeCounter;

    [Header("Hearing")]
    public float whenSoundAddDist;
    public float whenSoundRestTime;

    [Header("Alert")]
    public float alertSpeed;
    public float alertDistance;
    public float closeAlertTimeToDetect;
    public float alertGeneralWaitOnPointTime;
    public float alertWaitOnPointTime;
    public float alertWaitComeFromSound;
    public float alertWaitPercentage;

    public float alertTimeCounter;
    public float maxAlertTime;

    public bool inAlert;
    public bool endingAlert;
    public bool changeToAlert;

    [Header("Die")]
    public float timeToDie;
    public bool dead;

    [Header("Absorb")]
    public float addStamina;
    public float addLife;
    //public float timeToAddStamina;
    public float absorbTimeCounter;
    public float maxTimeToAbsorb;
    public float absorbPercentage;

    [Header("Sounds")]
    public AudioSource basicSource;
    public AudioArray clips;

    void Start()
    {
        itsef = gameObject.GetComponent<EnemyTest>();

        currentLife = maxLife;

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gm = GameObject.FindGameObjectWithTag("Managers").GetComponent<GameManager>();

        audPlay = GetComponentInChildren<AudioPlayer>();

        stealth = player.GetComponentInChildren<StealthSystem>();

        playerDetector = GetComponentInChildren<EnemyPlayerDetector>();

        sight = GetComponentInChildren<EnemySight>();

        attack = GetComponentInChildren<EnemyAttackArea>();

        execution = GetComponentInChildren<ExecutionArea>();

        rigid = GetComponent<Rigidbody>();

        playerDetector.Initialize();

        sight.Initialize();

        execution.Initialize();

        states = State.Stationary;

        StationarySet();

        generalWaitOnPointTime = initGeneralWaitOnPointTime;
        waitOnPointTime = initWaitOnPointTime;
        waitComeFromSound = initWaitComeFromSound;
        percentageOfWait = initPercentageOfWait;
    }

    void Update()
    {
        playerDetector.MyUpdate();

        if (IHaveSight)
        {
            sight.MyUpdate();
        }

        execution.MyUpdate();

        ImDetectingPlayer();

        CountAlertTime();

        switch (states)
        {
            case State.Sound:
                SoundUpdate();
                break;

            case State.Look:
                LookUpdate();
                break;

            case State.Patrol:
                PatrolUpdate();
                break;

            case State.Stationary:
                StationaryUpdate();
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
            //Debug.Log(agent.remainingDistance);

            if (agent.remainingDistance <= agent.stoppingDistance + 0.01)
            {
                Debug.Log("Arrived" + gameObject.name);
                goToNearPoint = true;
                comeFromSound = true;


                StationarySet();
            }
        }

        else
        {
            //ChaseSet();
            DetectSet();
        }

        if (positionWhereSound != Vector3.zero)
        {
            SetLook(positionWhereSound);
        }
    }

    public void LookUpdate()
    {
        if (timeLookingCounter >= maxTimeLooking)
        {
            timeLookingCounter = 0;
            SoundSet();
        }
        else
        {
            timeLookingCounter += Time.deltaTime;
        }
    }

    public void PatrolUpdate()
    {
        //Detected = gm.detected;

        //positionWhereSound = sound.soundPosition; //cuando ya pase un rato alomejor te interesa que cambien a la nueva posicion

        if (positionWhereSound != Vector3.zero)
        {
            SetLook(positionWhereSound);
            Debug.Log("Heared something");
        }

        if (agent.remainingDistance <= agent.stoppingDistance + 0.01)
        {
            StationarySet();
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
                execution.area.enabled = false;
                //combatArea.detected = true;
            }

            ChaseSet();
        }
        else
        {
            timeCounterDetection += Time.deltaTime;
        }
    }

    public void StationaryUpdate()
    {
        if (!stayThere)
        {
            if (comeFromSound)
            {
                if (stationaryTimeCounter >= waitComeFromSound)
                {
                    if (!firstSearch)
                    {
                        firstSearch = true;
                        //animacion exclusiva para la primera busqueda
                        //Debug.Log("A dormir");
                    }

                    EndOfStationary();
                }
                else
                {
                    stationaryTimeCounter += Time.deltaTime;
                    //Debug.Log(waitComeFromSound);
                }
            }

            else
            {
                if (!stopOnPoint)
                {
                    stopOnPoint = true;
                    randomNum = Random.Range(0, 100);

                    //Debug.Log(randomNum);
                }

                if (randomNum <= percentageOfWait)
                {
                    if (stationaryTimeCounter >= waitOnPointTime)
                    {
                        EndOfStationary();
                    }
                    else
                    {
                        stationaryTimeCounter += Time.deltaTime;
                    }
                }
                else if (randomNum >= percentageOfWait)
                {
                    if (stationaryTimeCounter >= generalWaitOnPointTime)
                    {
                        EndOfStationary();
                    }
                    else
                    {
                        stationaryTimeCounter += Time.deltaTime;
                    }
                }
            }
        }

        if (Detected | combatArea.detected)
        {
            DetectSet();
        }

        if (positionWhereSound != Vector3.zero)
        {
            SetLook(positionWhereSound);
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

        agent.SetDestination(positionWhereSound);
        //Debug.Log(positionWhereSound);

        positionWhereSound = Vector3.zero;
        //Debug.Log(positionWhereSound);

        wantToHear = true;

        stationaryTimeCounter = 0;

        //Debug.Log("SoundSet");

        states = State.Sound;
    }

    public void SetLook(Vector3 pos)
    {
        //positionWhereSound = Vector3.zero;
        //Debug.Log(pos);

        stayThere = false;

        inAlert = true;

        agent.isStopped = true;

        wantToHear = true;

        states = State.Look;
    }

    public void PatrolSet()
    {
        agent.isStopped = false;
        attackTimeCounter = 0;
        agent.stoppingDistance = patrolStopDist;
        agent.speed = patrolSpeed;

        if (goToNearPoint)
        {
            GoToNearestPoint();
        }
        else
        {
            GoToNextPathPoints();
        }

        if (inAlert)
        {
            endingAlert = false;
            changeToAlert = true;
            agent.speed = alertSpeed;
            generalWaitOnPointTime = alertGeneralWaitOnPointTime;
            waitOnPointTime = alertWaitOnPointTime;
            waitComeFromSound = alertWaitComeFromSound;
            percentageOfWait = alertWaitPercentage;
        }
        else
        {
            firstSearch = false;
            agent.speed = patrolSpeed;
            generalWaitOnPointTime = initGeneralWaitOnPointTime;
            waitOnPointTime = initWaitOnPointTime;
            waitComeFromSound = initWaitComeFromSound;
            percentageOfWait = initPercentageOfWait;
        }

        stopOnPoint = false;
        comeFromSound = false;

        wantToHear = true;

        states = State.Patrol;
    }

    public void DetectSet()
    {
        wantToHear = false;
        agent.isStopped = true;
        stealth.detected = true;
        sight.watchingPlayer = false;
        playerDetector.hearingPlayer = false;
        states = State.detect;

        stealth.enemies.Remove(itsef);

        if (stealth.playingDetectingSound && Detected)
        {
            stealth.playingDetectingSound = false;
            stealth.stealthAudioSource.Stop();

            player.baseSource.PlayOneShot(stealth.detectedSound);
        }
    }

    public void StationarySet()
    {
        wantToHear = true;

        agent.isStopped = true;

        positionWhereSound = Vector3.zero;

        stopOnPoint = false;

        states = State.Stationary;
    }

    public void ChaseSet()
    {
        wantToHear = false;
        agent.isStopped = false;
        agent.stoppingDistance = distToAttack;
        attackTimeCounter = 0;
        agent.speed = chaseSpeed;
        states = State.Chase;
    }

    public void AttackSet()
    {
        wantToHear = false;

        agent.isStopped = true;

        attackTimeCounter = timeAttackFinished - restTimeToAttack;

        states = State.Attack;
    }

    public void StunnedSet(float time)
    {
        wantToHear = false;

        agent.isStopped = true;
        timeStunned = time;
        states = State.Stunned;
    }

    public void DieSet()
    {
        wantToHear = false;

        executionCollider.enabled = false;

        agent.isStopped = true;
        //attackArea.enabled = false;

        Detected = false;
        playerInArea = false;
        canDetectPlayer = false;

        sight.watchingPlayer = false;
        playerDetector.hearingPlayer = false;

        combatArea.enemiesNumber--;
        dead = true;

        if (!gm.improved)
        {
            Destroy(gameObject, timeToDie);
        }

        if (addedToList)
        {
            stealth.enemies.Remove(itsef);
            addedToList = false;
        }

        states = State.die;
    }

    #endregion

    void CountAlertTime()
    {
        if (inAlert && changeToAlert && !endingAlert)
        {
            if (alertTimeCounter >= maxAlertTime)
            {
                changeToAlert = false;
                inAlert = false;
                alertTimeCounter = 0;
            }
            else
            {
                alertTimeCounter += Time.deltaTime;
            }
        }
    }

    public void DoAttack()
    {
        Debug.Log("Attacking");

        basicSource.PlayOneShot(clips.clips[0]);

        if (attack.playerInArea)
        {
            Debug.Log("Player hit");
            basicSource.PlayOneShot(clips.clips[1]);
            player.Damage(hitDamage);

            if (player.IWM.absorbing && player.GM.improved)
            {
                player.IWM.absorb.StopAbsorbing();
            }

            if (player.items.healing && !player.GM.improved)
            {
                player.items.CancelHealing();
            }
        }

        else if (!attack.playerInArea)
        {
            Debug.Log("Player Out");
            StunnedSet(stunedTimeWhenMiss);
        }
    }

    public void Damage(float life)
    {
        
        currentLife -= life;

        if (currentLife <= 0)
        {
            currentLife = 0;
            if (!dead)
            {
                basicSource.PlayOneShot(clips.clips[2]);
            }
            DieSet();
        }
    }

    void GoToNextPathPoints()
    {
        //Debug.Log("PassPoint");

        currentPoint++;

        if (currentPoint >= pathPoints.Length)
        {
            currentPoint = 0;
        }

        agent.SetDestination(pathPoints[currentPoint].position);
    }

    void GoToNearestPoint()
    {
        if (pathPoints.Length > 0)
        {
            float minDist = Mathf.Infinity;
            int minPos = 0;

            for (int i = 0; i < pathPoints.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, pathPoints[i].position);

                if (distance < minDist)
                {
                    minDist = distance;
                    minPos = i;
                }
            }

            agent.SetDestination(pathPoints[minPos].position);
            goToNearPoint = false;
        }
    }

    public void ImDetectingPlayer()
    {
        if (sight.watchingPlayer || playerDetector.hearingPlayer)
        {
            if (!addedToList)
            {
                stealth.enemies.Add(itsef);
                addedToList = true;
            }
        }

        if (!sight.watchingPlayer && !playerDetector.hearingPlayer)
        {
            if (addedToList)
            {
                stealth.enemies.Remove(itsef);
                addedToList = false;
            }
        }
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.DrawCube(attackAreaTrans.position, HalfStent * 2);
    }*/

    public void PlaySound(int index, float delay)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.clip = clips.clips[index];
        source.PlayDelayed(delay);

        Destroy(source, source.clip.length);
    }

    void EndOfStationary()
    {
        stationaryTimeCounter = 0;

        stopOnPoint = false;

        agent.isStopped = false;

        PatrolSet();
    }
}
