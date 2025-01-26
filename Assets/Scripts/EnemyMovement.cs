using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//TODO: navigation using navmesh, movement, attack, finding the player (FSM?)
public class EnemyMovement : MonoBehaviour
{
    GameManager managerInstance;

    [Header("Movement Values")]
    //[SerializeField] float enemySpeed;
    [Range(0, 50)] [SerializeField] float sightRange = 20;

    NavMeshAgent thisAgent;

    [Header("Attack stuff")]
    [Range(0, 50)] [SerializeField] float attackRange = 5f;
    [SerializeField] float timeBetweenAttacks = 3f;
    [SerializeField] int attackDamage;

    [Header("VFX stuffs")]
    [SerializeField] ParticleSystem enemyDeathParticle;
    [SerializeField] AudioSource audioSource;

    bool isDead = false;
    bool isAttacking = false;

    Transform playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDead = false;
        if (managerInstance == null)
            managerInstance = GameManager.Instance;

        thisAgent = GetComponent<NavMeshAgent>();
        playerPos = managerInstance.GetPlayerMovement().GetPlayerTransform();
        thisAgent.isStopped = false;

        managerInstance.GetEnemyHealth(gameObject).OnEnemyDeath.AddListener(onDeath);
        audioSource.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || managerInstance.GetPlayerHealth().GetIsDead())
            return;

        float _distanceFromPlayer = Vector3.Distance(playerPos.position, transform.position);

        if (_distanceFromPlayer <= sightRange && _distanceFromPlayer > attackRange)
        {
            //Debug.Log("Player seen");
            isAttacking = false;
            thisAgent.isStopped = false;
            StopAllCoroutines();

            chasePlayer();
        }

        if(_distanceFromPlayer <= attackRange && !isAttacking)
        {
            thisAgent.isStopped = true;
            audioSource.mute = true;
            StartCoroutine(attackPlayer());
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif

    void chasePlayer()
    {
        //Debug.Log("Chasing player");
        audioSource.mute = false;
        thisAgent.SetDestination(playerPos.position);
    }

    IEnumerator attackPlayer()
    {
        isAttacking = true;

        Debug.Log("Hurt player");
        managerInstance.GetPlayerHealth().TakeDamage(attackDamage);

        yield return new WaitForSeconds(timeBetweenAttacks);

        isAttacking = false;
    }

    void onDeath()
    {
        thisAgent.isStopped = true;
        Instantiate(enemyDeathParticle, transform.position, Quaternion.identity);
    }
}
