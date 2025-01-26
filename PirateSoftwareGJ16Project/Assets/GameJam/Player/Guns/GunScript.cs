using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] public GameObject gunMuzzle;

    public GameObject player;
    public ShootComponent shootComponent;
    public Transform playerCameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void Update()
    {
        if (playerCameraTransform)
        {
            Vector3 targetPoint = playerCameraTransform.position + playerCameraTransform.forward * 50f;
            Quaternion gunRotation = Quaternion.LookRotation(targetPoint - gunMuzzle.transform.position);
            transform.rotation = gunRotation;
        }
    }

    public void InitOwner(GameObject newPlayer)
    {
        player = newPlayer;
        
        shootComponent = player.GetComponent<ShootComponent>();
    }
    
    /*protected void CalculateProjectileRotation()
    {
        Vector3 targetPoint = playerCameraTransform.position + playerCameraTransform.forward * 50f;
        Vector3 lookDirection = targetPoint - gunMuzzle.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        
        projectileRotation = lookRotation;

        
        RaycastHit hit;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward * 100, out hit))
        {
            lookDirection = hit.point - gunMuzzle.transform.position;
            Debug.DrawRay(gunMuzzle.transform.position, lookDirection * 100, Color.red, 1f);
            projectileRotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }*/
    
    public virtual void Shoot(bool isCrit)
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
        // Change script to base projectile script (not PistolBulletScript)
        PistolBulletScript projectileScript = projectile.GetComponent<PistolBulletScript>();
        if (projectileScript != null)
        {
            projectileScript.InitOwner(player);
            projectileScript.isCrit = isCrit;
        }
    }
}
