using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{ 
    public NavMeshAgent agent;

public Transform player;

public LayerMask whatIsGround, whatIsPlayer;
    public UnityEvent attack;
    public UnityEvent go;
    public UnityEvent getDamage;
    public UnityEvent die;
public float health;

//Patroling
public Vector3 walkPoint;
bool walkPointSet;
public float walkPointRange;

//Attacking
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
}

private void Update()
{
    //Check for sight and attack range
    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    if (!playerInSightRange && !playerInAttackRange) Patroling();
    if (playerInSightRange && !playerInAttackRange) ChasePlayer();
    if (playerInAttackRange && playerInSightRange) AttackPlayer();
}

private void Patroling()
{
    if (!walkPointSet) SearchWalkPoint();

    if (walkPointSet)
        agent.SetDestination(walkPoint);

    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //Walkpoint reached
    if (distanceToWalkPoint.magnitude < 1f)
        walkPointSet = false;
}
private void SearchWalkPoint()
{
    //Calculate random point in range
    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    float randomX = Random.Range(-walkPointRange, walkPointRange);

    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
}

private void ChasePlayer()
{
    agent.SetDestination(player.position);
        if (go != null) go.Invoke();
}

private void AttackPlayer()
{
        if (attack != null) attack.Invoke();

}

private void ResetAttack()
{
    alreadyAttacked = false;
}

    public void TakeDamage(int damage)
    {

        health -= damage;
        if (getDamage != null) {getDamage.Invoke(); }
        if (health <= 0) { if (die != null) { die.Invoke(); } Invoke(nameof(DestroyEnemy), 0.5f);  }
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
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            TakeDamage(other.transform.GetComponent<BulletSetup>().damage);
        }
    }
}
