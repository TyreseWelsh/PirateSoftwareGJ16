 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManagerComponent : MonoBehaviour
{
    // Both increase additively.
    // Flat will add the current "increase value" onto the base value
    // Percentage will find the "increase value" percent of the base value, and then add that new number back onto the base value.
    public enum IncreaseType
    {
        Flat,
        Percentage
    }

    [System.Serializable]
    public class StatObject
    {
        public string statName;
        public int increaseValue;
        public IncreaseType increaseType;
    }

    [SerializeField] private List<StatObject> statObjects;
    public Dictionary<string, int> currentStats = new Dictionary<string, int>();

    
    // Start is called before the first frame update
    void Start()
    {
        InitBaseStatUpgrades();
    }


    private void InitBaseStatUpgrades()
    {
        foreach (StatObject statObject in statObjects)
        {
            currentStats.Add(statObject.statName, 0);
        }
    }

    // NOTE: Issue with how this applies to reload speed and fire rate stats: because these stats wants to be lower, doing addition here to get the final stat doesnt make sense
    // FIX: Where these stats are applied, make their stat want to be bigger to be better
    public float ApplyStatIncrease(string newStatName, float baseNumber)
    {
        StatObject currentStatObject = FindStatObject(newStatName);
        if (currentStatObject != null)
        {
            switch (currentStatObject.increaseType)
            {
                case IncreaseType.Flat:
                    return baseNumber + currentStatObject.increaseValue * currentStats[newStatName];
                
                case IncreaseType.Percentage:
                    return baseNumber + (baseNumber / 100) * (currentStatObject.increaseValue * currentStats[newStatName]);
                
                default:
                    //Debug.Log("ERROR: NO SET INCREASE TYPE");
                    return -1;
            }
        }
        
        //Debug.Log("ERROR: INVALID STAT NAME");
        return -1;
    }

    // Note: May somehow become expensive depending on how often we are applying stat increases
    private StatObject FindStatObject(string _statName)
    {
        foreach (StatObject statObject in statObjects)
        {
            if (statObject.statName == _statName)
            {
                return statObject;
            }
        }
        
        return null;
    }

    public void IncrementStatAmount(string _statName)
    {
        Debug.Log("Applying Stat Increase to: " + _statName);
        currentStats[_statName]++;
        BroadcastMessage("updateUI", _statName);
    }
    
    int GetStatAmount(string newStatName)
    {
        int statValue;
        currentStats.TryGetValue(newStatName, out statValue);
        
        return -statValue;
    }
    
    // OLD "APPLY" FUNCTIONS (KEEP JUST IN CASE)
    /*public int ApplyHealthIncrease(int baseHealth)
    {
        int healthIncrease = 10;
        healthIncrease *= stats["Health"];
        
        return baseHealth + healthIncrease * stats["Health"];
    }

    public int ApplyHealthRegenIncrease(int baseHealthRegen)
    {
        int healthRegenIncrease = 3;
        healthRegenIncrease *= stats["Health"];
        
        return baseHealthRegen + healthRegenIncrease;
    }

    public int ApplyDamageIncerase(int baseDamage)
    {
        int damageIncrease = 10;
        damageIncrease *= stats["Damage"];
        
        return baseDamage + (baseDamage / 100) * damageIncrease;
    }

    public float ApplyFireRateIncrease(float baseFireRate)
    {
        float fireRateIncrease = 8;
        fireRateIncrease *= stats["FireRate"];
        
        return baseFireRate + (baseFireRate / 100) * fireRateIncrease;
    }
    
    public float ApplyReloadSpeedIncrease(float baseReloadSpeed)
    {
        float reloadSpeedIncrease = 8;
        reloadSpeedIncrease *= stats["FireRate"];
        
        return baseReloadSpeed + (baseReloadSpeed / 100) * reloadSpeedIncrease;
    }
    
    public float ApplyCritRateIncrease(float baseCritRate)
    {
        float critRateIncrease = 8;
        critRateIncrease *= stats["CritRate"];
        
        return baseCritRate + (baseCritRate / 100) * critRateIncrease;
    }
    
    public float ApplyRangeIncrease(float baseRangeRate)
    {
        float rangeIncrease = 5;
        rangeIncrease *= stats["Range"];
        
        return baseRangeRate + (baseRangeRate / 100) * rangeIncrease;
    }
    
    public float ApplyMoveSpeedIncrease(float baseMoveSpeedRate)
    {
        float moveSpeedIncrease = 8;
        moveSpeedIncrease *= stats["MoveSpeed"];
        
        return baseMoveSpeedRate + (baseMoveSpeedRate / 100) * moveSpeedIncrease;
    }*/
}
