using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PistolBulletScript : MonoBehaviour
{
    [SerializeField] private ProjectileScriptableObject projectileData;
    
    private Transform bulletTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // Note: Solution used to fix bullets not following correct direction is to add Space.World (instead of local space)
    private void Move()
    {
        bulletTransform.Translate(projectileData.moveSpeed * Time.deltaTime * bulletTransform.forward, Space.World);
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(20);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().TakeDamage(30, gameObject);
            Destroy(gameObject);
        }
    }
}
