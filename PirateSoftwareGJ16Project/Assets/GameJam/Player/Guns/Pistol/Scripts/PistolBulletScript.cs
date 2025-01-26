using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PistolBulletScript : MonoBehaviour
{
    [SerializeField] private ProjectileScriptableObject projectileData;
    public GameObject player;
    private StatManagerComponent playerStatManager;
    private Transform bulletTransform;

    private float range;
    private Vector3 startPosition;
    public bool isCrit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletTransform = gameObject.GetComponent<Transform>();
        range = projectileData.range;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRange();
        Move();
    }

    private void CheckRange()
    {
        float currentDistance = Vector3.Distance(transform.position, startPosition);

        if (currentDistance > GetRange(true))
        {
            Destroy(gameObject);
        }
    }
    
    // Note: Solution used to fix bullets not following correct direction is to add Space.World (instead of local space)
    private void Move()
    {
        bulletTransform.Translate(projectileData.moveSpeed * Time.deltaTime * bulletTransform.forward, Space.World);
    }

    public void InitOwner(GameObject newPlayer)
    {
        player = newPlayer;
        
        playerStatManager = player.GetComponent<StatManagerComponent>();
    }

    public float GetRange(bool modified)
    {
        if (!modified)
        {
            return projectileData.range;
        }
        
        return playerStatManager.ApplyStatIncrease("Range", projectileData.range);
    }
    
    public int GetDamage(bool modified)
    {
        if (!modified)
        {
            return projectileData.damage;
        }
        
        return Mathf.CeilToInt(playerStatManager.ApplyStatIncrease("Damage", projectileData.damage));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            int newDamage = GetDamage(true);
            if (isCrit)
            {
                newDamage *= 2;
            }
            
            other.gameObject.GetComponent<IDamageable>().TakeDamage(newDamage, gameObject);
            Destroy(gameObject);
        }
    }
}
