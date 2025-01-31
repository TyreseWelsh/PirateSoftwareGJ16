using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : GunScript
{
    [SerializeField] private int numProjectiles = 10;
    [SerializeField] private float spreadOffset = 0.15f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    public override void Shoot(bool isCrit)
    {
        FlashMuzzle();
        
        for (int i = 0; i < numProjectiles; i++)
        {
            Vector3 projectileDirection = transform.forward + new Vector3(Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset), Random.Range(-spreadOffset, spreadOffset));
            Debug.DrawRay(gunMuzzle.transform.position, projectileDirection * 2, Color.green, 0.5f);
            
            gunMuzzle.transform.forward = projectileDirection;
            
            GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, gunMuzzle.transform.rotation);
        }
    }
}
