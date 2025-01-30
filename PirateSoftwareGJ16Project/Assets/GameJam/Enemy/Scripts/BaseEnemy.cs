using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IMobile
{
    [SerializeField] private float attackRange = 2f;
    [HideInInspector] public NavMeshAgent agent;

    [SerializeField] private GameObject player;

    [SerializeField] private AttackComponent attackComponent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            Debug.Log("Following");
            agent.destination = player.transform.position;
        }
        else
        {
            if (attackComponent)
            {
                attackComponent.StartAttack();
            }
        }
    }

    public float GetMoveSpeed(bool modified)
    {
        return agent.speed;
    }

    public void SetMoveSpeed(float _speed)
    {
        agent.speed = _speed;
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
