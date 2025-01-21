using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] protected GunScriptableObject gunData;
    [SerializeField] public GameObject gunMuzzle;

    public GameObject player;
    public Transform playerCameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void Update()
    {
        if (playerCameraTransform)
        {
            Vector3 targetPoint = playerCameraTransform.forward * 25f;
            transform.rotation = Quaternion.LookRotation(targetPoint);
        }
    }
    
    public virtual void Shoot()
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
    }
}
