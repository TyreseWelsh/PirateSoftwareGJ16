using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    enum UpgradeTypes
    {
        Health,
        HealthRegen,
        Damage,
        FireRate,
        ReloadSpeed,
        CritRate,
        Range,
        MovementSpeed
    }
    
    [SerializeField] UpgradeTypes currentUpgradeType;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseStat()
    {
        switch (currentUpgradeType)
        {
            case UpgradeTypes.Health:
                IncreaseHealth();
                break;
            case UpgradeTypes.HealthRegen:
                IncreaseHealthRegen();
                break;
            case UpgradeTypes.Damage:
                IncreaseDamage();
                break;
            case UpgradeTypes.FireRate:
                IncreaseFireRate();
                break;
            case UpgradeTypes.ReloadSpeed:
                IncreaseReloadSpeed();
                break;
            case UpgradeTypes.CritRate:
                IncreaseCriticalRate();
                break;
            case UpgradeTypes.Range:
                IncreaseRange();
                break;
            case UpgradeTypes.MovementSpeed:
                IncreaseMovementSpeed();
                break;
        }
    }

    private void IncreaseHealth()
    {
        
    }
    
    private void IncreaseHealthRegen()
    {
        
    }
    
    private void IncreaseDamage()
    {
        
    }
    
    private void IncreaseFireRate()
    {
        
    }
    
    private void IncreaseReloadSpeed()
    {
        
    }
    
    private void IncreaseCriticalRate()
    {
        
    }
    
    private void IncreaseRange()
    {
        
    }
    
    private void IncreaseMovementSpeed()
    {
        
    }
}
