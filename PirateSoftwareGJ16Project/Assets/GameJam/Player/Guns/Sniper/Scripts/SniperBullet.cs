using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : PistolBulletScript
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            int newDamage = GetDamage(true);
            if (isCrit)
            {
                newDamage *= 2;
            }

            other.gameObject.GetComponent<StatusEffectComponent>()?.ApplyStatusEffect(StatusEffectComponent.StatusEffectType.Frost, gameObject);
            
            if (playerLevelUpComponent)
            {
                other.gameObject.GetComponent<HealthComponent>().onDeathDelegate -= playerLevelUpComponent.AddToExperience;
                other.gameObject.GetComponent<HealthComponent>().onDeathDelegate += playerLevelUpComponent.AddToExperience;
            }
            other.gameObject.GetComponent<IDamageable>().TakeDamage(newDamage, gameObject);
            
            Destroy(gameObject);
        }
        
        Destroy(gameObject);
    }
}
