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

    void Update()
    {
        if (playerCameraTransform)
        {
            Vector3 targetPoint = playerCameraTransform.forward * 50f;
            Quaternion newGunRotation = Quaternion.FromToRotation(transform.position, targetPoint);
            transform.rotation = Quaternion.Euler(transform.rotation.x, newGunRotation.y, transform.rotation.z);
            Debug.Log("Gun rotation = " + transform.rotation.eulerAngles);
        }
    }
    
    public virtual void Shoot()
    {
        GameObject projectile = Instantiate(gunData.projectilePrefab, gunMuzzle.transform.position, transform.rotation);
    }
}
