using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy_Sound_AI : MonoBehaviour
{
    //Teleporting player
    public GameObject player;
    public GameObject playerSpawn;

    //AI sight
    public bool playerIsInLOS = false;
    public float fieldOfViewAngle = 160f;
    public float losRadius = 30f;

    //AI memory
    private bool aiMemorizesPlayer = false;
    public float memoryStartTime = 10f;
    private float increasingMemoryTime;

    //Stun Values
    public float stunDuration = 5f;
    private float timeStunned;

    //AI hearing
    Vector3 noisePosition;
    private bool aiHeardPlayer = false;
    public float noiseTravelDistance = 50f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime = 3f;
    public float walkNoiseMult = 0.5f;

    private float noiseVolume = 0f;

    //Patrolling
    public Transform[] moveSpots;
    private int randomSpot;

    public LayerMask SafeZone, EnemyBlock;

    //Wait time at waypoint when patrolling
    private float waitTime;
    public float startWaitTime = 1f;

    NavMeshAgent nav;

    //Chase
    public float chaseRadius = 20f;
    public float aggroRadius = 8f;
    private bool chasing;

    public float facePlayerFactor = 20f;

    //Attack Distance
    public float distToPlayer = 1f;

    //On Awake
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        /*float distance = Vector3.Distance(PlayerControl.playerPos, transform.position);

        if (distance > chaseRadius)
        {
            Patrol();
        }
        else if (distance <= chaseRadius)
        {
            ChasePlayer();
            FacePlayer();
        }
        */
        float distance = Vector3.Distance(PlayerControl.playerPos, transform.position);
        if (distance <= losRadius)
        {
            CheckLOS();
        }

        if (nav.isActiveAndEnabled)
        {
            if (playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false)
            {
                
                Patrol();
                NoiseCheck();
                StopCoroutine(AiMemory());
            }
            else if (aiHeardPlayer == true && playerIsInLOS == false && aiMemorizesPlayer == false)
            {
                canSpin = true;
                GoToNoisePosition();
                
            }
            else if (playerIsInLOS == true)
            {
                aiMemorizesPlayer = true;
                FacePlayer();
                ChasePlayer();
            }
            else if (aiMemorizesPlayer == true && playerIsInLOS == false)
            {
                ChasePlayer();
                StartCoroutine(AiMemory());
            }
        }
        if (PlayerPickup.objects >= 2)
        {
            nav.speed = 10f;
        }
        else if (PlayerPickup.objects >= 3)
        {
            nav.speed = 16f;
        }
        else if (PlayerPickup.objects >= 1)
        {
            nav.speed = 6f;
        }
        else
        {
            nav.speed = 4f;
        }
    }

    void NoiseCheck()
    {
        float distance = Vector3.Distance(PlayerControl.playerPos, transform.position);

        if (distance <= noiseTravelDistance * walkNoiseMult)
        {
            if (PlayerControl.walkSilent == false && Input.GetButton("Horizontal") || PlayerControl.walkSilent == false && Input.GetButton("Vertical"))
            {
                noisePosition = PlayerControl.playerPos;
                noiseVolume += 1f;

                if (Physics.Raycast(noisePosition, -transform.up, 5f, SafeZone))
                {
                    aiHeardPlayer = false;
                    
                }
                else if (Physics.Raycast(noisePosition, -transform.up, 5f, EnemyBlock))
                {
                    aiHeardPlayer = false;
                    
                }
                else 
                {
                    aiHeardPlayer = true;
                }

                
            }
        }
        else if (distance <= noiseTravelDistance)
        {
            if (PlayerControl.silent == false && Input.GetButton("Horizontal") || PlayerControl.silent == false && Input.GetButton("Vertical"))
            {
                noisePosition = PlayerControl.playerPos;
                noiseVolume += 1f;

                if (Physics.Raycast(noisePosition, -transform.up, 5f, SafeZone))
                {
                    aiHeardPlayer = false;
                    
                }
                else if (Physics.Raycast(noisePosition, -transform.up, 5f, EnemyBlock))
                {
                    aiHeardPlayer = false;
                    
                }
                else 
                {
                    aiHeardPlayer = true;
                }

                
            }
            else if (Projectile.makeNoise == true)
            {
                noisePosition = Projectile.impactPos;
                noiseVolume += 1f;

                
                if (Physics.Raycast(noisePosition, -transform.up, 5f, SafeZone))
                {
                    aiHeardPlayer = false;
                    canSpin = false;
                }
                else if (Physics.Raycast(noisePosition, -transform.up, 5f, EnemyBlock))
                {
                    aiHeardPlayer = false;
                    canSpin = false;
                }
                else
                {
                    aiHeardPlayer = true;
                }

            }
            else 
            {
                
                aiHeardPlayer = false;
                canSpin = false;
            }
        }
    }

    void GoToNoisePosition()
    {
        
        nav.SetDestination(noisePosition);
        

        if (Vector3.Distance(transform.position, noisePosition) <= 5f && canSpin == true)
        {
            
            isSpinningTime += Time.deltaTime;

            transform.Rotate(Vector3.up * spinSpeed, Space.World);

            if(isSpinningTime >= spinTime)
            {
                canSpin = false;
                aiHeardPlayer = false;
                isSpinningTime = 0f;
                noiseVolume = 0f;
                Projectile.makeNoise = false;
            }
        }
        
    }

    IEnumerator AiMemory()
    {
        increasingMemoryTime = 0f;

        while(increasingMemoryTime < memoryStartTime)
        {
            increasingMemoryTime += Time.deltaTime;
            aiMemorizesPlayer = true;
            yield return null; 
        }

        aiHeardPlayer = false;
        aiMemorizesPlayer = false;
    }

    void CheckLOS()
    {
        Vector3 direction = PlayerControl.playerPos - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, direction.normalized, out hit, losRadius))
            {
                if (hit.collider.tag == "Player")
                { 
                    playerIsInLOS = true;
                    aiMemorizesPlayer = true;
                }
                else
                {
                    playerIsInLOS = false;
                }
            }
        }
    }

    void Patrol()
    {
        Debug.Log("Patrolling");

        chasing = false;
        nav.SetDestination(moveSpots[randomSpot].position);

        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void ChasePlayer()
    {
        float distance = Vector3.Distance(PlayerControl.playerPos, transform.position);

        if (distance <= aggroRadius && distance > distToPlayer)
        {
            chasing = true;
            nav.SetDestination(PlayerControl.playerPos);
        }
        if (chasing == true && distance <= chaseRadius)
        {
            Debug.Log("In Aggro Range");
            nav.SetDestination(PlayerControl.playerPos);
        }
        if (chasing == true && distance >= chaseRadius - 1f)
        {
            Debug.Log("Out of Aggro");
            chasing = false;
            playerIsInLOS = false;
        }
        if (distance <= aggroRadius && distance < distToPlayer)
        {
            Attack();
        }

        /*  else if (nav.isActiveAndEnabled && distance <= distToPlayer)
          { 
                AttackPLayer();
          }
        */
    }

    void FacePlayer()
    {
        Vector3 direction = (PlayerControl.playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
    }

    void OnCollisionEnter(Collision other)
    { 
        if(other.gameObject.tag == "Projectile")
        {
            Stun();
        }
    }

    void Attack()
    {
        Debug.Log("Attack");
        player.transform.position = playerSpawn.transform.position;
        player.SendMessage("LoseObject");
    }

    void Stun()
    {
        Debug.Log("Hit");
        StartCoroutine(StunEffect());
    }
    IEnumerator StunEffect()
    {
        timeStunned = 0f;

        while (timeStunned <= stunDuration)
        {
            timeStunned += Time.deltaTime;
            nav.speed = 0f;
            yield return null;
        }

    }
}
