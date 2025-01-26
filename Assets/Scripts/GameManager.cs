using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("UI stuff")]
    [SerializeField] List<GameObject> healthBubbles;
    [SerializeField] GameObject deathMessageObject;

    [Header("PlayerStuffs")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Gun gun;

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
        deathMessageObject.SetActive(false);
    }

    public PlayerMovement GetPlayerMovement() { return playerMovement; }
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

    public void ResetButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void updateUI()
    {
        for (int i = 0; i < healthBubbles.Count; i++) //I could reverse this, or I could just shove the bubbles in the list in a reverse order
        {
            if (healthBubbles[i].activeSelf == true)
            {
                healthBubbles[i].SetActive(false);

                if (i == healthBubbles.Count - 1)
                {
                    sendDeathMessage();
                }
                return;
            }
        }
    }

    void sendDeathMessage()
    {
        deathMessageObject.SetActive(true);
    }
}
