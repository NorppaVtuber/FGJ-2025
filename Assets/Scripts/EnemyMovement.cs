using UnityEngine;
using UnityEngine.Events;

//TODO: navigation using navmesh, movement, attack, finding the player (FSM?)
public class EnemyMovement : MonoBehaviour
{
    GameManager managerInstance;

    [Header("Movement Values")]
    [SerializeField] float enemySpeed;

    [Header("Attack stuff")]
    [SerializeField] float attackDistance;
    [SerializeField] int attackDamage;

    bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDead = false;
        if (managerInstance == null)
            managerInstance = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
