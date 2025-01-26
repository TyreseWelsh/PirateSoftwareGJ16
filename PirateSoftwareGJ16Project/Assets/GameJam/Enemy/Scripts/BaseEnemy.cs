using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public int MAX_HEALTH = 100;
    private int health;
    public IDamageable.OnDeath onDeathDelegate;

    
    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int _damage, GameObject _source)
    {
        Debug.Log(gameObject.name + " is damaged!");
        health -= _damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        onDeathDelegate?.Invoke();
        Destroy(gameObject);
    }
}
