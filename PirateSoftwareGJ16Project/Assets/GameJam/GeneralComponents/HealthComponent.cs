using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    private StatManagerComponent statManager;
    [SerializeField] private GameObject player;
    [Header("Health Stats")]
    [SerializeField] public int MAX_HEALTH = 120;
    [HideInInspector] public float currentHealth;
    
    [Header("Regen")]
    [SerializeField] private bool canRegen = false;
    [SerializeField] private float regenDelayTime = 3f;
    [SerializeField] private float baseRegenAmount = 0.5f;
    [SerializeField] private float regenRate = 1f;
    private Coroutine delayCoroutine;
    private Coroutine regenCoroutine;

    
    public IDamageable.OnDeath onDeathDelegate { get; set; }
    
    [Header("Debug")]
    [SerializeField] private bool debugEnabled = false;

    private void Awake()
    {
        statManager = player.GetComponent<StatManagerComponent>();
    }

    private void Start()
    {
        currentHealth = GetMaxHealth(false);
        
    }

    public void StartRegenDelay(float delay)
    {
        if (debugEnabled)
        {
            print("Health is now = " + currentHealth);
        }
        
        // Stopping previous running coroutines
        StopRegen();

        if (delayCoroutine == null)
        {
            delayCoroutine = StartCoroutine(RegenDelay(delay));
        }
    }
    
    IEnumerator RegenDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenHealth());
        }
    }
    
    IEnumerator RegenHealth()
    {
        while (currentHealth < GetMaxHealth(true))
        {
            GainHealth(GetHealthRegen(true));
            if (debugEnabled)
            {
                print("Health = " + currentHealth);
            }
            
            yield return new WaitForSeconds(regenRate);
            yield return null;
        }
        
        currentHealth = GetMaxHealth(true);
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
        regenCoroutine = null;
    }

    void StopRegen()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }
    }

    public void GainHealth(float addedHealth)
    {
        currentHealth += addedHealth;

        int currentMaxHealth = GetMaxHealth(true);
        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
    }
    
    public int GetMaxHealth(bool modified)
    {
        if (!modified)
        {
            return MAX_HEALTH;
        }
        
        return Mathf.CeilToInt(statManager.ApplyStatIncrease("MaxHealth", MAX_HEALTH));
    }

    public float GetHealthRegen(bool modified)
    {
        if (!modified)
        {
            return baseRegenAmount;
        }
        
        return statManager.ApplyStatIncrease("HealthRegen", baseRegenAmount);
    }
    
    // IDamageable functions
    public void TakeDamage(int _damage, GameObject _source)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        if (canRegen)
        {
            StartRegenDelay(regenDelayTime);
        }
    }

    public void Die()
    {
        Debug.Log("PLAYER DEAD");
        onDeathDelegate?.Invoke();
        Destroy(gameObject);
    }
}
