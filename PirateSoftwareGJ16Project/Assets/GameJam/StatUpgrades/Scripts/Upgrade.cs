using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private string statType;
    [SerializeField] private int currentPickupTime = 0;
    [SerializeField] private int MAX_PICKUP_TIME = 4;
    
    private SphereCollider pickupRadius;
    private GameObject player;

    private Coroutine pickupCoroutine;
    private Coroutine dropCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        pickupRadius = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStatType(string newType)
    {
        statType = newType;
        Debug.Log("Spawned upgrade of type: " + statType);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            if (pickupCoroutine == null)
            {
                if (dropCoroutine != null)
                {
                    StopCoroutine(dropCoroutine);
                    dropCoroutine = null;
                }
                pickupCoroutine = StartCoroutine(PickupTimer());
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            StopCoroutine(pickupCoroutine);
            pickupCoroutine = null;
            dropCoroutine = StartCoroutine(DropTimer());
        }
    }

    private IEnumerator PickupTimer()
    {
        while (currentPickupTime < MAX_PICKUP_TIME)
        {
            currentPickupTime++;
            yield return new WaitForSeconds(1);
            yield return null;
        }
        
        AddToPlayerStats();
        Destroy(gameObject);
    }

    private IEnumerator DropTimer()
    {

       while (currentPickupTime > 0)
       {
           currentPickupTime--;
           yield return new WaitForSeconds(1);
           yield return null;
       }
       Debug.Log("NO longer ticking down");
       StopCoroutine(dropCoroutine);
    }

    private void AddToPlayerStats()
    {
        StatManagerComponent playerStatManager = player.GetComponent<StatManagerComponent>();
        if (playerStatManager)
        {
            /*switch (upgradeType)
            {
                case UpgradeTypes.Health:
                    playerStatManager.IncrementStatAmount("Health");
                    break;
                case UpgradeTypes.HealthRegen:
                    playerStatManager.IncrementStatAmount("HealthRegen");
                    break;
                case UpgradeTypes.Damage:
                    playerStatManager.IncrementStatAmount("Damage");
                    break;
                case UpgradeTypes.FireRate:
                    playerStatManager.IncrementStatAmount("FireRate");
                    break;
                case UpgradeTypes.ReloadSpeed:
                    playerStatManager.IncrementStatAmount("ReloadSpeed");
                    break;
                case UpgradeTypes.CritRate:
                    playerStatManager.IncrementStatAmount("CritRate");
                    break;
                case UpgradeTypes.Range:
                    playerStatManager.IncrementStatAmount("Range");
                    break;
                case UpgradeTypes.MoveSpeed:
                    playerStatManager.IncrementStatAmount("MoveSpeed");
                    break;*/
            //}
            
            playerStatManager.IncrementStatAmount(statType);
        }
    }
}


