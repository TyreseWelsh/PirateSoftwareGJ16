using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthComponent : HealthComponent, IDamageable
{
    
    public override void Die()
    {
        SceneManager.LoadScene(2);
        base.Die();
    }
}
