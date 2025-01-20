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
    void Update()
    {
        
    }

    public override void Shoot()
    {
        float currentShootAngle = (numProjectiles - 1) * -(spreadAngle / 2);
        for (int i = 0; i < numProjectiles; ++i)
        {
            //Debug.Log("Shotgun bullet angle = " + currentShootAngle);

            firingPoint.transform.Rotate(Vector3.up, currentShootAngle);
            Debug.Log(firingPoint.transform.position);
            Debug.DrawRay(firingPoint.transform.position, firingPoint.transform.forward * 2, Color.green, 30f);
            
            GameObject projectile = Instantiate(gunData.projectilePrefab, firingPoint.transform.position, firingPoint.transform.rotation);

            firingPoint.transform.rotation = Quaternion.identity;
            currentShootAngle += spreadAngle;
        }
    }

    private void TEMPFlatConeShot()
    {
        float currentShootAngle = (numProjectiles - 1) * -(spreadAngle / 2);
        for (int i = 0; i < numProjectiles; ++i)
        {
            //Debug.Log("Shotgun bullet angle = " + currentShootAngle);

            firingPoint.transform.Rotate(Vector3.up, currentShootAngle);
            Debug.Log(firingPoint.transform.position);
            Debug.DrawRay(firingPoint.transform.position, firingPoint.transform.forward * 2, Color.green, 30f);

            //firingPoint.transform.rotation = Quaternion.identity;
            currentShootAngle += spreadAngle;
        }
    }
}
