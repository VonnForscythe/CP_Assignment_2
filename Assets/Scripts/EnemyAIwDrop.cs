using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAIwDrop : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public int maxHealth = 2;
    public int currentHealth;
    public HealthBar healthBar;
    Animator animator;

    //Patroling AI
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking AI
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject item;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        
        animator.applyRootMotion = false;

        currentHealth = maxHealth;
       // healthBar.SetMaxHealth(maxHealth);

        //NavMeshHit closestHit;

        //if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
        //    gameObject.transform.position = closestHit.position;
        //else
        //    Debug.LogError("Could not find position on NavMesh!");
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (currentHealth <= 0)
        {
            animator.SetBool("Death", true);
            walkPointRange = 0;
            agent.SetDestination(walkPoint);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        animator.SetBool("Patrol", true);
        animator.SetBool("Attack", false);
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("Attack", true);
    }

    private void AttackPlayer()
    {
        animator.SetBool("Firing", true);
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            Destroy(rb.gameObject, 2.0f);

            

            //FireAtPlayer();
        }
    }

    //private void FireAtPlayer()
    //{
    //    Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

    //    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //    rb.AddForce(transform.up * 8f, ForceMode.Impulse);

    //    alreadyAttacked = true;
    //    Invoke(nameof(ResetAttack), timeBetweenAttacks);
        
    //}


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            TakeDamage(1);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Firing", false);
        
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

       // healthBar.SetHealth(currentHealth);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
        Rigidbody rb = Instantiate(item, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

