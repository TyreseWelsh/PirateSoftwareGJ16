using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] protected GameObject gunMuzzle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Shoot()
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
    }
}
