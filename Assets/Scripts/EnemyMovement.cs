using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Animation anim;
    float timer = 0.0f;
    public float health;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttack;
    bool alreadyAttacked;
    public int attackDamage;

    // State
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        health = 6;
        //whatIsPlayer = LayerMask.GetMask("Player");
        attackDamage = 1;
    }
    private void Update()
    {
        // Check if player is in sight/attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerInSightRange && !playerInAttackRange)
        //{
        //    Patroling();
        //}
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            Idle();
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            Walk();
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Arrived
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // Calc a new random point range
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Run();
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Attack1();
            GameObject.Find("Player").GetComponent<PlayerDamage>().PlayerTakeDamage(attackDamage);
            StartCoroutine(AttackCooldown());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(2);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            Dead();
            Invoke(nameof(DestroySelf), 1f);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
    public void Idle()
    {
        anim.Play("Idle");
    }

    public void Walk()
    {
        anim.Play("Walk");
    }

    public void Run()
    {
        anim.Play("Run");
    }

    public void Attack1()
    {
        anim.Play("Attack1");
    }

    public void Attack2()
    {
        anim.Play("Attack2");
    }

    public void Dead()
    {
        anim.Play("Death");
    }
}
