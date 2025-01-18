using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] private GunScriptableObject gunData;
    [SerializeField] private GameObject firingPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot()
    {
        //GameObject projectile = Instantiate(gunData.projectilePrefab, firingPoint.transform.position, transform.rotation);
    }
}
