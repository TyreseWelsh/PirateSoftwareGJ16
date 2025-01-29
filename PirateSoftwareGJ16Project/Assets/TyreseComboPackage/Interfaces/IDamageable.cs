using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public delegate void OnDeath();
    
    public void TakeDamage(int _damage, GameObject _source);
    
    public IEnumerator DamageFlash(MeshRenderer meshRender, Material startingMaterial, Material damageFlashMaterial, float flashTime);
    
    //public void DamagedKnockback(GameObject knockbackSource);
    
    public void Die();
}
