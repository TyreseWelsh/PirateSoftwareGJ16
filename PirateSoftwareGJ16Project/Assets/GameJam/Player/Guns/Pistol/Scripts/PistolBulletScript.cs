using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PistolBulletScript : MonoBehaviour
{
    [SerializeField] private ProjectileScriptableObject projectileData;
    public GameObject player;
    protected LevelUpComponent playerLevelUpComponent;
    protected StatManagerComponent playerStatManager;
    protected Transform bulletTransform;

    private float maxRange;
    private Vector3 startPosition;
    public bool isCrit = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        bulletTransform = gameObject.GetComponent<Transform>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    public void Update()
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

        if (player)
        {
            playerLevelUpComponent = player.GetComponent<LevelUpComponent>();
            playerStatManager = player.GetComponent<StatManagerComponent>();
        }
    }

    public float GetRange(bool modified)
    {
        if (playerStatManager)
        {
            if (modified)
            {
                return playerStatManager.ApplyStatIncrease("Range", projectileData.range);
            }
        }
        
        return projectileData.range;
    }
    
    public int GetDamage(bool modified)
    {
        if (playerStatManager)
        {
            if (modified)
            {
                return Mathf.CeilToInt(playerStatManager.ApplyStatIncrease("Damage", projectileData.damage));
            }
        }
        
        return projectileData.damage;
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

            if (playerLevelUpComponent)
            {
                other.gameObject.GetComponent<HealthComponent>().onDeathDelegate -= playerLevelUpComponent.AddToExperience;
                other.gameObject.GetComponent<HealthComponent>().onDeathDelegate += playerLevelUpComponent.AddToExperience;
            }
            other.gameObject.GetComponent<IDamageable>().TakeDamage(newDamage, gameObject);
            
            Destroy(gameObject);
        }
        
        Destroy(gameObject);
    }
}
