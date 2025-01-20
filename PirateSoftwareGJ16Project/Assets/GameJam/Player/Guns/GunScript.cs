using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] protected GameObject firingPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Shoot()
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, firingPoint.transform.position, transform.rotation);
    }
}
