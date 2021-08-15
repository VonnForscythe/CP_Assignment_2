using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turrent : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    Animator anim;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling AI
    //public Vector3 walkPoint;
    //bool walkPointSet;
    //public float walkPointRange;

    //Attacking AI
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
        
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

       // Debug.Log("PlayerinSightRange  " + playerInSightRange + "  attackRange  " + playerInAttackRange);
    
    }

    private void Patroling()
    {
        //if (!walkPointSet) SearchWalkPoint();

        //if (walkPointSet)
        //    agent.SetDestination(walkPoint);

        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if (distanceToWalkPoint.magnitude < 1f)
        //    walkPointSet = false;

        anim.SetBool("isActive", false);

       // Debug.Log("Patroling");
    }

    private void SearchWalkPoint()
    {
        //float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //float randomX = Random.Range(-walkPointRange, walkPointRange);

        //walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        //    walkPointSet = true;

    }


    private void ChasePlayer()
    {
       // agent.SetDestination(player.position);
        anim.SetBool("isActive", true);
        
        transform.LookAt(player);

       // Debug.Log("Chasing");
    }

    private void AttackPlayer()
    {
       // agent.SetDestination(transform.position);

        transform.LookAt(player);
        //anim.SetBool("shoot", true);

       // Debug.Log("Attacking");

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();


            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4.5f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            Destroy(rb.gameObject, 2.0f);
            //Debug.Log("Reset Attack");


        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
      
        // anim.SetBool("shoot", false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}