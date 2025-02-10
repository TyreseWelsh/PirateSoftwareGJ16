using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private float spawnRange = 70;
    [SerializeField] private int maxEnemies = 150;
    private int currentEnemies = 0;

    [SerializeField] private bool isEnabled;

    [Header("Weighting")] 
    private Vector3 totalWeighting;

    [SerializeField] private GameObject enemyTypeX;
    [SerializeField] private GameObject enemyTypeY;
    [SerializeField] private GameObject enemyTypeZ;
    private List<GameObject> enemyTypes;
    
    [Header("Weight Affectors")]
    [SerializeField] private float maxAffectorRange = 210f;
    [SerializeField] private GameObject weightAffectorX;
    [SerializeField] private GameObject weightAffectorY;
    [SerializeField] private GameObject weightAffectorZ;

    [Header("Spawn Timer")] 
    private Coroutine spawnCoroutine;
    [SerializeField] private float spawnDelay = 5f;
    private int numToSpawn = 6;
    private float existanceTime = 0f;
    private float spawnIncreaseInterval = 40f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        InitialiseEnemyTypes();

        if (isEnabled)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemyTimer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        Debug.DrawLine(transform.position, weightAffectorX.transform.position, Color.red);
        Debug.DrawLine(transform.position, weightAffectorY.transform.position, Color.green);
        Debug.DrawLine(transform.position, weightAffectorZ.transform.position, Color.blue);
        
        
        /*if (isEnabled)
        {
            if (currentEnemies < maxEnemies)
            {
                // TODO: Make sure enemies keep spawning as others die, and every so often spawn a new enemy anyway
                //CalculateWeighting();
            }
        }*/
    }

    private void InitialiseEnemyTypes()
    {
        enemyTypes = new List<GameObject>();
        enemyTypes.Add(enemyTypeX);
        enemyTypes.Add(enemyTypeY);
        enemyTypes.Add(enemyTypeZ);
    }

    IEnumerator SpawnEnemyTimer()
    {
        while (currentEnemies < maxEnemies)
        {
            yield return new WaitForSeconds(spawnDelay);
            for (int i = 0; i < numToSpawn; i++)
            {
                CalculateWeighting();
            }

            existanceTime += Time.deltaTime;
            if (existanceTime >= spawnIncreaseInterval)
            {
                spawnIncreaseInterval *= 1.3f;
                numToSpawn += 2;
                maxEnemies = Mathf.CeilToInt(maxEnemies * 1.6f);
            }
        }
    }
    
    public void CalculateWeighting()
    {
        float distanceToAffector = 0;
            
        if (weightAffectorX
            && weightAffectorY
            && weightAffectorZ)
        {
            // First affector
            distanceToAffector = Vector3.Distance(transform.position, weightAffectorX.transform.position);
            totalWeighting.x = maxAffectorRange - distanceToAffector;

            // Second affector
            distanceToAffector = Vector3.Distance(transform.position, weightAffectorY.transform.position);
            totalWeighting.y =  maxAffectorRange - distanceToAffector;

            // Third affector
            distanceToAffector = Vector3.Distance(transform.position, weightAffectorZ.transform.position);
            totalWeighting.z = maxAffectorRange - distanceToAffector;
        }
        else
        {
            Debug.Log("ERROR: MISSING ENEMY SPAWN WEIGHTING AFFECTOR");
        }
        
        List<float> enemyWeights = new List<float>();
        enemyWeights.Add(totalWeighting.x);
        enemyWeights.Add(totalWeighting.y);
        enemyWeights.Add(totalWeighting.z);
        
        FindEnemyToSpawn(enemyWeights);
    }

    private void FindEnemyToSpawn(List<float> weights)
    {
        // Temp
        List<string> teststrings = new List<string>();
        teststrings.Add("sword");
        teststrings.Add("hammer");
        teststrings.Add("axe");
        

        float weightTotal = 0;
        foreach (float weight in weights)
        {
            weightTotal += weight;
        }
        
        float randNum = Random.Range(1, weightTotal);
        float cursor = 0;

        for (int i = 0; i < 3; i++)
        {
            cursor += weights[i];

            if (cursor >= randNum)
            {
                //Debug.Log("Chosen enemy = " + teststrings[i] + " with weight= " + weights[i]);
                SpawnEnemy(enemyTypes[i]);
                return;
            }
        }
    }

    private void SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnPoint2D = player.transform.position + Random.insideUnitSphere * spawnRange;
        spawnPoint2D.y = player.transform.position.y + 5;
        
        //Debug.DrawLine(transform.position, endPoint1, Color.yellow, 100f);

        Vector2 spawnerPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 spawnPosition2D = new Vector2(spawnPoint2D.x, spawnPoint2D.z);
        // Finding spawn X/Y
        if (Vector2.Distance(spawnerPosition2D, spawnPosition2D) > spawnRange / 3f && spawnPoint2D.y > transform.position.y + 2)
        {
            Debug.DrawLine(transform.position, spawnPoint2D, Color.yellow, 8f);

            // Finding spawn Z from line trace downwards at spawnPoint2D
            RaycastHit hit;
            if (Physics.Raycast(spawnPoint2D, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(spawnPoint2D, transform.TransformDirection(Vector3.down) * 100f, Color.cyan, 8f);
                
                //Debug.DrawLine(transform.position, hit.point, Color.green, 30f);
                // Line trace to potential spawn point was not blocked by "OuterEnvironment" (environment object that would lead out of map
                NavMeshHit navHit;
                if (NavMesh.SamplePosition(hit.point, out navHit, 3f, NavMesh.AllAreas))
                {
                    // Spawn point is inside map
                    Debug.DrawLine(transform.position, navHit.position, Color.red, 30f);
                    
                    Vector3 spawnPoint = hit.point;
                    GameObject enemyObject = Instantiate(enemyType, spawnPoint, Quaternion.identity);
                    enemyObject.GetComponent<BaseEnemy>()?.SetPlayer(player);
                    if (enemyObject.GetComponent<HealthComponent>())
                    {
                        enemyObject.GetComponent<HealthComponent>().onDeathDelegate += ReduceCurrentEnemyCount;
                        enemyObject.GetComponent<HealthComponent>().onDeathDelegate += CalculateWeighting;
                        Debug.Log("Bind onDeath delegates");

                    }
                    
                    currentEnemies++;
                }
            }
            else
            {
                // Try and spawn an enemy again
                SpawnEnemy(enemyType);
            }
        }
    }
    
    public void ReduceCurrentEnemyCount()
    {
        currentEnemies--;
        Debug.Log("Enemy DEAD!");
    }
}
