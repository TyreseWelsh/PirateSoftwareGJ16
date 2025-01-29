using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectComponent : MonoBehaviour
{
    public GameObject effectSource;
    
    public enum StatusEffectType
    {
        Frost,
        Burn,
        Shock
    };

    private List<StatusEffectType> currentStatusEffects;
    // Frost
    private IMobile movementInterface;
    private float frostSlow = 0.5f;
    private Coroutine frostCoroutine;
    private float frostDuration = 2.5f;
    
    // Burn
    private IDamageable damageableInterface;
    private int burnDamage = 3;
    private int burnTicks = 4;
    private Coroutine burnCoroutine;
    private float MAX_BURN_DURATION = 3f;
    private float currentBurnDuration = 0f;

    private void Awake()
    {
        movementInterface = GetComponent<IMobile>();
        damageableInterface = GetComponentInParent<IDamageable>();
    }

    public void ApplyStatusEffect(StatusEffectType effectType, GameObject source)
    {
        switch (effectType)
        {
            case StatusEffectType.Frost:
                ApplyFrost();
                break;
            case StatusEffectType.Burn:
                ApplyBurn(source);
                break;
        }
    }

    private void ApplyFrost()
    {
        // Currently only applies to enemies
        if (frostCoroutine == null)
        {
            if (movementInterface != null)
            {
                movementInterface.SetMoveSpeed(movementInterface.GetMoveSpeed(false) * frostSlow );
                frostCoroutine = StartCoroutine(FrostSlowTimer());
            }
        }
        else
        {
            // If frost already applied, reset timer
            StopCoroutine(frostCoroutine);
            frostCoroutine = StartCoroutine(FrostSlowTimer());
        }
    }

    IEnumerator FrostSlowTimer()
    {
        yield return new WaitForSeconds(frostDuration);
        
        RemoveFrost();
    }
    
    private void RemoveFrost()
    {
        movementInterface.SetMoveSpeed(movementInterface.GetMoveSpeed(false) * frostSlow );
        frostCoroutine = null;
        currentStatusEffects.Remove(StatusEffectType.Frost);
        
    }

    private void ApplyBurn(GameObject source)
    {
        currentBurnDuration = MAX_BURN_DURATION;
        if (burnCoroutine == null)
        {
            burnCoroutine = StartCoroutine(BurnTimer(source));
        }
    }

    IEnumerator BurnTimer(GameObject source)
    {
        while (currentBurnDuration > 0)
        {
            currentBurnDuration -= MAX_BURN_DURATION / burnTicks;
            yield return new WaitForSeconds(MAX_BURN_DURATION / burnTicks);
            
            damageableInterface?.TakeDamage(burnDamage, source);
            yield return null;
        }
        
        RemoveBurn();
    }

    private void RemoveBurn()
    {
        burnCoroutine = null;
        currentStatusEffects.Remove(StatusEffectType.Burn);
    }
}
