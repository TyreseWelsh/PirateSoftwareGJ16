using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = Unity.Mathematics.Random;

public class UpgradeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeSpawns;
    [SerializeField]private int upgradeIndex = 0;
    [SerializeField] private int maxUpgrades;
    [SerializeField] private float spawnTimer;
    

    [SerializeField] private GameObject upgradePrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnUpgrades(spawnTimer));
    }

    private IEnumerator SpawnUpgrades(float curSpawnTimer)
    { 
        yield return new WaitForSeconds(curSpawnTimer);
        if (upgradeIndex < maxUpgrades)
        {
            int randomSpawn = UnityEngine.Random.Range(0, upgradeSpawns.Length - 1);
            Debug.Log(randomSpawn);
            StatSpawnPoint spawnPointScript = upgradeSpawns[randomSpawn].GetComponent<StatSpawnPoint>();
            if (!spawnPointScript.upgradeSpawned)
            {
                    Instantiate(upgradePrefab,upgradeSpawns[randomSpawn].transform.position, Quaternion.identity);
                    spawnPointScript.upgradeSpawned = true;
                    upgradeIndex++;
                    yield return StartCoroutine(SpawnUpgrades(curSpawnTimer));
            }
            else
            {
                yield return StartCoroutine(SpawnUpgrades(0f));
                
            }
            
        }
        


    }
}
