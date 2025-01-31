using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerScript : GunScript
{
    [SerializeField] private int numProjectiles = 10;
    [SerializeField] private float spreadAngle = 0.3f;
    
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
        PlayAltGunSounds();

        float currentShootAngle = (numProjectiles - 1) * -(spreadAngle / 2);
        for (int i = 0; i < numProjectiles; ++i)
        {
            gunMuzzle.transform.Rotate(Vector3.up, currentShootAngle);
            Debug.DrawRay(gunMuzzle.transform.position, transform.forward * 2, Color.green, 50f);
            
            GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, gunMuzzle.transform.rotation);
            
            currentShootAngle += spreadAngle;
        }
    }
    
    
}
