using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatusEffectComponent : MonoBehaviour
{
    public GameObject effectSource;

    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Material originalMaterial;
    
    public enum StatusEffectType
    {
        Frost,
        Burn,
        Shock
    };

    private List<StatusEffectType> currentStatusEffects;
    // Frost
    [Header("Frost")]
    [SerializeField] Material frostMaterial;
    private IMobile movementInterface;
    private float frostSlow = 0.5f;
    private Coroutine frostCoroutine;
    private float frostDuration = 2.5f;
    
    // Burn
    [Header("Burn")]
    [SerializeField] ParticleSystem burnParticles;
    private ParticleSystem burnParticlesObject;
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
        if (GetComponent<HealthComponent>())
        {


        }
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
                meshRenderer.material = frostMaterial;
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
        meshRenderer.material = originalMaterial;
        frostCoroutine = null;
        currentStatusEffects.Remove(StatusEffectType.Frost);
        
    }

    private void ApplyBurn(GameObject source)
    {
        currentBurnDuration = MAX_BURN_DURATION;
        if (burnCoroutine == null)
        {
            burnParticlesObject = Instantiate(burnParticles, transform.position, Quaternion.identity);
            burnParticlesObject.transform.parent = transform;
            Vector3 randomSize = new Vector3(Random.Range(2.0f,3.0f), Random.Range(2.0f,3.0f), Random.Range(2.0f,3.0f));
            burnParticlesObject.transform.localScale = randomSize;
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

    public void RemoveBurn()
    {
        burnCoroutine = null;
        burnParticlesObject.Stop();
        Destroy(burnParticlesObject);
        currentStatusEffects.Remove(StatusEffectType.Burn);
    }
}
