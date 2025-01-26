using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] public GameObject gunMuzzle;

    public Transform playerCameraTransform;
    protected Quaternion projectileRotation;
    
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

    protected void CalculateProjectileRotation()
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
    }
    
    // TODO: Because actual gun is being rotated in correct aim direction, calculating projectile rotation is not needed. Can just use transform.rotation instead 
    public virtual void Shoot()
    {
        //CalculateProjectileRotation();        
        
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
    }
}
