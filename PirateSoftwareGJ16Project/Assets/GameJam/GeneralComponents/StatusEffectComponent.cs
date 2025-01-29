using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectComponent : MonoBehaviour
{
    public enum StatusEffectType
    {
        Frost,
        Burn,
        Shock
    };

    private List<StatusEffectType> currentStatusEffects;
    //
    private IMobile movementInterface;
    private float frostSlow = 0.5f;
    private Coroutine frostCoroutine;
    private float frostDuration = 2.5f;

    private void Awake()
    {
        movementInterface = GetComponent<IMobile>();
    }

    public void ApplyStatusEffect(StatusEffectType effectType)
    {
        switch (effectType)
        {
            case StatusEffectType.Frost:
                ApplyFrost();
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
}
