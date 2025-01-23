using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private GameObject enemy;

    [SerializeField] private float spawnRange = 30;
    [SerializeField] private int maxEnemies = 50;
    private int currentEnemies = 0;

    [SerializeField] private bool bIsEnabled;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsEnabled)
        {
            Vector3 endPoint1 = Random.insideUnitSphere * spawnRange;
            Vector2 thisTransform = new Vector2(transform.position.x, transform.position.z);
            Vector2 thatTransform = new Vector2(endPoint1.x, endPoint1.z);
            if (Vector2.Distance(thisTransform, thatTransform) > spawnRange / 1.75f && endPoint1.y > transform.position.y + 2)
            {
                if (currentEnemies < maxEnemies)
                {
                    Debug.DrawLine(transform.position, endPoint1, Color.green, 0.1f);

                    RaycastHit hit;
                    if (Physics.Raycast(endPoint1, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                    {
                        Vector3 endPoint2 = hit.point;
                        GameObject enemyObject = Instantiate(enemy, endPoint2, Quaternion.identity);
                        enemyObject.GetComponent<AIDestinationSetter>().target = playerTarget.transform;
                        if (enemyObject.GetComponent<BaseEnemy>()?.onDeathDelegate != null)
                        {
                            enemyObject.GetComponent<BaseEnemy>().onDeathDelegate += ReduceCurrentEnemyCount;
                        }
                        currentEnemies++;
                    }
                }
            }
        }
    }

    public void ReduceCurrentEnemyCount()
    {
        currentEnemies--;
        Debug.Log("Enemy DEAD!");
    }
}
