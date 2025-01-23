using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public delegate void OnDeath();
    
    public void TakeDamage(int _damage);
    public void Die();

}
