using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy1 : MonoBehaviour
{
    public AudioSource[] audio;

    public bool audioPlaying;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        Patrolling();
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInAttackRange && playerInAttackRange) Attack();

        
        
    }

    void Patrolling()
    {
        if (!walkPointSet)
        { 
            SearchWalkPoint(); 
        }

        if (walkPointSet)
        {
            Debug.Log("Walking");
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f)
        { 
            walkPointSet = false;
            Debug.Log("Walkpoint reached");
        }
        if (audioPlaying)
        {
            audio[0].Stop();
            audioPlaying = false;
        }
        
    }

    void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround))
        { 
            walkPointSet = true;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
        if (!audioPlaying)
        {
            audio[0].Play();
            audioPlaying = true;
        }
        walkPointSet = false;
    }

    void Attack()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Insert Attack code here>
            Debug.Log("Attack");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            //<Insert Attack code here

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    { 
        alreadyAttacked = false;
    }
}
