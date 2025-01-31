using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPointToPlayer : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        {
            if (player)
            {
                // Rotate to look in player direction
                Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                transform.LookAt(playerPosition);
            }
        }
    }

    //public void SetPlayer(GameObject newPlayer)
    //{
    //    player = newPlayer;
    //}


}
