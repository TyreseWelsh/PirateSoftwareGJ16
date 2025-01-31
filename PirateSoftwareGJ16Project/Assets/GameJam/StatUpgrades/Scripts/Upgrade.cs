using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private string statType;
    [SerializeField] private int currentPickupTime = 0;
    [SerializeField] private int MAX_PICKUP_TIME = 4;

    [SerializeField] private List<SO_UpgradeBar> upgradeDataList;
    private SO_UpgradeBar selectedUpgrade;
    [SerializeField] private Image upgradeImage;
    
    private SphereCollider pickupRadius;
    [HideInInspector] public GameObject player;

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
        if (player)
        {
            // Rotate to look in player direction
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(playerPosition);
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
    
    public void SetStatType(string newType)
    {
        statType = newType;
        foreach (SO_UpgradeBar upgradeData in upgradeDataList)
        {
            if (statType == upgradeData._name)
            {
                selectedUpgrade = upgradeData;
            }
        }

        if (selectedUpgrade)
        {
            upgradeImage.sprite = selectedUpgrade._icon;
        }
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
            playerStatManager.IncrementStatAmount(statType);
        }
    }
}


