using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Collections;

//TODO: navigation using navmesh, movement, attack, finding the player (FSM?)
public class EnemyMovement : MonoBehaviour
{
    GameManager managerInstance;

    [Header("Movement Values")]
    [SerializeField] float enemySpeed;
    [Range(0, 50)] [SerializeField] float sightRange = 20;

    NavMeshAgent thisAgent;

    [Header("Attack stuff")]
    [Range(0, 50)] [SerializeField] float attackRange = 5f;
    [SerializeField] float timeBetweenAttacks = 3f;
    [SerializeField] int attackDamage;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || managerInstance.GetPlayerHealth().GetIsDead())
            return;

        float _distanceFromPlayer = Vector3.Distance(playerPos.position, this.transform.position);

        if (_distanceFromPlayer <= sightRange && _distanceFromPlayer > attackRange)
        {
            isAttacking = false;
            thisAgent.isStopped = false;
            StopAllCoroutines();

            chasePlayer();
        }

        if(_distanceFromPlayer <= attackRange && !isAttacking)
        {
            thisAgent.isStopped = true;
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
        thisAgent.SetDestination(playerPos.position);
    }

    IEnumerator attackPlayer()
    {
        isAttacking = true;

        yield return new WaitForSeconds(timeBetweenAttacks);

        Debug.Log("Hurt player"); //TODO: add actual attacking
        managerInstance.GetPlayerHealth().TakeDamage(attackDamage);

        isAttacking = false;
    }
}
