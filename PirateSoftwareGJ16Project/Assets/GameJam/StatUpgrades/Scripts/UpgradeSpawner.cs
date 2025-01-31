using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = Unity.Mathematics.Random;

public class UpgradeSpawner : MonoBehaviour
{
    public enum StatUpgradeTypes
    {
        MaxHealth,
        HealthRegen,
        Damage,
        FireRate,
        ReloadSpeed,
        CritRate,
        Range,
        MoveSpeed
    }
    
    [SerializeField] private GameObject[] upgradeSpawns;
    [SerializeField]private int upgradeIndex = 0;
    [SerializeField] private int maxUpgrades;
    [SerializeField] private float spawnTimer;
    [SerializeField] private GameObject upgradePrefab;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnUpgrades(spawnTimer));
        player = GameObject.FindWithTag("Player");
    }

    private IEnumerator SpawnUpgrades(float curSpawnTimer)
    { 
        yield return new WaitForSeconds(curSpawnTimer);
        if (upgradeIndex < maxUpgrades)
        {
            int randomSpawn = UnityEngine.Random.Range(0, upgradeSpawns.Length - 1);
            Debug.Log(randomSpawn);
            
            StatSpawnPoint spawnPointScript = upgradeSpawns[randomSpawn].GetComponent<StatSpawnPoint>();
            if (!spawnPointScript.occupied)
            {
                    SpawnStatUpgrade(upgradeSpawns[randomSpawn].transform.position);
                    spawnPointScript.occupied = true;
                    upgradeIndex++;
                    yield return StartCoroutine(SpawnUpgrades(curSpawnTimer));
            }
            else
            {
                yield return StartCoroutine(SpawnUpgrades(0f));
                
            }
        }
    }

    private void SpawnStatUpgrade(Vector3 spawnPosition)
    {
        int rangeMax = Enum.GetNames(typeof(StatUpgradeTypes)).Length;
        int randNum = UnityEngine.Random.Range(0, rangeMax);
        string statType = Enum.GetName(typeof(StatUpgradeTypes), randNum);
        
        GameObject upgradeObject = Instantiate(upgradePrefab,spawnPosition, Quaternion.identity);
        
        upgradeObject?.GetComponent<Upgrade>()?.SetStatType(statType);
        //upgradeObject?.GetComponent<Upgrade>()?.SetPlayer(player);
    }
}
