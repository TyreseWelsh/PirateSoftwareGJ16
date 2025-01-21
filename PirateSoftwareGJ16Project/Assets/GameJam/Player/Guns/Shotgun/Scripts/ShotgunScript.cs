using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : GunScript
{
    private int numProjectiles = 10;
    private float spreadAngle = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        float currentShootAngle = (numProjectiles - 1) * -(spreadAngle / 2);
        for (int i = 0; i < numProjectiles; ++i)
        {
            transform.Rotate(Vector3.up, currentShootAngle);
            Debug.DrawRay(gunMuzzle.transform.position, transform.forward * 2, Color.green, 30f);
            
            GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
            
            currentShootAngle += spreadAngle;
        }
    }

    private void TEMPFlatConeShot()
    {
        /*float currentShootAngle = (numProjectiles - 1) * -(spreadAngle / 2);
        for (int i = 0; i < numProjectiles; ++i)
        {
            //Debug.Log("Shotgun bullet angle = " + currentShootAngle);

            gunMuzzle.transform.Rotate(Vector3.up, currentShootAngle);
            Debug.Log(gunMuzzle.transform.position);
            Debug.DrawRay(gunMuzzle.transform.position, gunMuzzle.transform.forward * 2, Color.green, 30f);

            //firingPoint.transform.rotation = Quaternion.identity;
            currentShootAngle += spreadAngle;
        }*/
    }
}
