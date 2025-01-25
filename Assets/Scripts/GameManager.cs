using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("PlayerStuffs")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Gun gun;

    [Header("EnemyStuffs")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int amountOfEnemies;
    [SerializeField] List<Transform> enemySpawnPoints;

    List<EnemyMovement> enemyMovement;
    List<EnemyHealth> enemyHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerHealth.OnDamageTaken.AddListener(updateUI);

        spawnEnemies();
    }

    public PlayerMovement GetPlayerMovement() {  return playerMovement; }
    public PlayerHealth GetPlayerHealth() { return playerHealth; }
    public Gun GetPlayerGun() { return gun; }
    public EnemyMovement GetEnemyMovement() 
    {
        EnemyMovement _neededMovement = null;
        return _neededMovement; 
    }
    public EnemyHealth GetEnemyHealth() 
    {
        EnemyHealth _neededEnemyHealth = null;
        return _neededEnemyHealth; 
    }

    void spawnEnemies()
    {
        int _amountOfEnemiesPerTransform = amountOfEnemies / enemySpawnPoints.Count;
        int _amountAtCurrentSpawn = 0;
        int _currentSpawnPoint = 0;
        for (int i = 0; i < amountOfEnemies; i++)
        {
            if(_amountAtCurrentSpawn < _amountOfEnemiesPerTransform)
            {
                randomizeSpawnpoints(enemySpawnPoints[_currentSpawnPoint]); 
            }
            else if(_amountAtCurrentSpawn >= _amountOfEnemiesPerTransform && _currentSpawnPoint == enemySpawnPoints.Count && i < amountOfEnemies)
            {
                //since we're dividing ints there might be enemies "left over" so we need to spawn the rest at the last spawn point
                randomizeSpawnpoints(enemySpawnPoints[_currentSpawnPoint - 1]);
            }
            else
            {
                _amountAtCurrentSpawn = 0;
                _currentSpawnPoint++;
                i--;
            }
        }
    }

    void randomizeSpawnpoints(Transform _currentSpawnPoint)
    {
        //TODO: some logic about the enemy spawns being lightly randomized around the "big" spawn point so they aren't all in the exac same point
        //It needs some logic to make sure they don't end up outside the navmesh though. But I remember there was some check for that, I'm just too tired to try and figure it out rn
        //also add all their scripts to a List
    }

    void updateUI()
    {
        //code to update health bubbles hereeee
    }
}
