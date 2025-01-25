using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health values")]
    [SerializeField] int maxHealth;
    int currentHealth;

    public UnityEvent OnEnemyDeath;

    private void Awake()
    {
        //We want to create the events in awake, as that is called before start, and we're adding any event listeners in start
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

        if (currentHealth <= 0)
        {
            OnEnemyDeath.Invoke();
        }
    }
    private void OnDestroy()
    {
        OnEnemyDeath.RemoveAllListeners();
    }
}
