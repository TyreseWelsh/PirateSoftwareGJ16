using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] public GameObject gunMuzzle;

    public Transform playerCameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void Update()
    {
        if (playerCameraTransform)
        {
            Vector3 targetPoint = playerCameraTransform.forward * 5f;
            transform.rotation = Quaternion.LookRotation(targetPoint);
        }
    }
    
    public virtual void Shoot()
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
    }
}
