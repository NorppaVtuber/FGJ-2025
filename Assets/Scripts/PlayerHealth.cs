using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health values")]
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    public UnityEvent OnDamageTaken; //add a listener to this for the UI script (eventually), that hndles updating the UI sprites
    public UnityEvent OnDeath;

    [Header("Audio stuffs")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hurtSound;

    public bool GetIsDead() { return currentHealth <= 0; }

    private void Awake()
    {
        //We want to create the events in awake, as that is called before start, and we're adding any event listeners in start
        if (OnDamageTaken == null)
            OnDamageTaken = new UnityEvent();
        if (OnDeath == null)
            OnDeath = new UnityEvent();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int _damageAmount)
    {
        currentHealth -= _damageAmount;

        OnDamageTaken.Invoke(); //update any UI elements with this
        audioSource.PlayOneShot(hurtSound);

        Debug.Log("Player took damage!");

        if(currentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnDamageTaken.RemoveAllListeners(); //we want to avoid posible issues with memory usage by removing all listeners properly
        OnDeath.RemoveAllListeners();
    }
}
