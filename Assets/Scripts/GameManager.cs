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
    [SerializeField] float spawnDistance;

    Dictionary<EnemyMovement, EnemyHealth> enemies;

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
        enemies = new Dictionary<EnemyMovement, EnemyHealth>();

        spawnEnemies();
    }

    public PlayerMovement GetPlayerMovement() {  return playerMovement; }
    public PlayerHealth GetPlayerHealth() { return playerHealth; }
    public Gun GetPlayerGun() { return gun; }
    public EnemyMovement GetEnemyMovement(GameObject _enemy)  //TODO: go through the Dictionary and find the correct script to reference in these two thingamajigs
    {
        EnemyMovement _neededMovement = _enemy.GetComponent<EnemyMovement>();
        return _neededMovement; 
    }
    public EnemyHealth GetEnemyHealth(GameObject _enemy) 
    {
        EnemyHealth _neededEnemyHealth = _enemy.GetComponent<EnemyHealth>();
        return _neededEnemyHealth; 
    }

    void spawnEnemies()
    {
        int _amountOfEnemiesPerTransform = 0;
        if (enemySpawnPoints.Count <= 0)
            _amountOfEnemiesPerTransform = amountOfEnemies / 1;
        else
            _amountOfEnemiesPerTransform = amountOfEnemies / enemySpawnPoints.Count;
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
        GameObject _newEnemy = Instantiate(enemyPrefab, _currentSpawnPoint);
        EnemyMovement _savedMovement = _newEnemy.GetComponent<EnemyMovement>();

        enemies.Add(_savedMovement, _newEnemy.GetComponent<EnemyHealth>());

        _savedMovement.RandomizeStartPosition(_currentSpawnPoint, spawnDistance);
    }

    void updateUI()
    {
        //code to update health bubbles hereeee
    }
}
