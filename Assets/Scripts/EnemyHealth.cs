using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health values")]
    [SerializeField] int maxHealth;
    int currentHealth;

    public UnityEvent OnEnemyDamageTaken;
    public UnityEvent OnEnemyDeath;

    private void Awake()
    {
        //We want to create the events in awake, as that is called before start, and we're adding any event listeners in start
        if (OnEnemyDamageTaken == null)
            OnEnemyDamageTaken = new UnityEvent();
        if (OnEnemyDeath == null)
            OnEnemyDeath = new UnityEvent();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _damageAmount)
    {
        currentHealth -= _damageAmount;

        OnEnemyDamageTaken.Invoke();

        if (currentHealth <= 0)
        {
            OnEnemyDeath.Invoke();
        }
    }
    private void OnDestroy()
    {
        OnEnemyDamageTaken.RemoveAllListeners(); //we want to avoid posible issues with memory usage by removing all listeners properly
        OnEnemyDeath.RemoveAllListeners();
    }
}
