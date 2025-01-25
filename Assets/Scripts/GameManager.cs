using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("PlayerStuffs")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Gun gun;

    [Header("EnemyStuffs")]
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyHealth enemyHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public PlayerMovement GetPlayerMovement() {  return playerMovement; }
    public PlayerHealth GetPlayerHealth() { return playerHealth; }
    public Gun GetPlayerGun() { return gun; }
    public EnemyMovement GetEnemyMovement() { return enemyMovement; }
    public EnemyHealth GetEnemyHealth() { return enemyHealth; }
}
