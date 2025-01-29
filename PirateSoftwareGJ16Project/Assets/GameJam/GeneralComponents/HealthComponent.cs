using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    private StatManagerComponent statManager;
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

    [Header("Damaged")] 
    [SerializeField] private Material damageFlashMaterial;
    [SerializeField] private Material originalMaterial;
    private float damageFlashDuration = 0.1f;
    [SerializeField] private MeshRenderer[] damageableMeshes;

    public IDamageable.OnDeath onDeathDelegate;
    
    [Header("Debug")]
    [SerializeField] private bool debugEnabled = false;

    private void Awake()
    {
        statManager = GetComponent<StatManagerComponent>();
        damageableMeshes = GetComponentsInChildren<MeshRenderer>();
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
        if (statManager)
        {
            if (modified)
            {
                return Mathf.CeilToInt(statManager.ApplyStatIncrease("MaxHealth", MAX_HEALTH));
            }
        }
        
        return MAX_HEALTH;
    }

    public float GetHealthRegen(bool modified)
    {
        if (statManager)
        {
            if (modified)
            {
                return statManager.ApplyStatIncrease("HealthRegen", baseRegenAmount);
            }
        }
        
        return baseRegenAmount;
    }
    
    // IDamageable functions
    public void TakeDamage(int _damage, GameObject _source)
    {
        currentHealth -= _damage;
        // Damage flash
        foreach (MeshRenderer meshRenderer in damageableMeshes)
        {
            StartCoroutine(DamageFlash(meshRenderer, originalMaterial, damageFlashMaterial,damageFlashDuration));
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        if (canRegen)
        {
            StartRegenDelay(regenDelayTime);
        }
    }

    public IEnumerator DamageFlash(MeshRenderer meshRender, Material startingMaterial,
        Material flashMaterial, float flashTime)
    {
        meshRender.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        
        meshRender.material = startingMaterial;
    }

    
    public void Die()
    {
        Debug.Log(gameObject.name + " DEAD");
        onDeathDelegate?.Invoke();
        Destroy(gameObject);
    }
    
}
