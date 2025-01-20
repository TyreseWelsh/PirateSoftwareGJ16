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
    
    
    public void ApplyStatusEffect(StatusEffectType effectType)
    {
        
    }
}
