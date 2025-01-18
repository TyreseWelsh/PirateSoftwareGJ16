using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenuScript : MonoBehaviour
{
    /*[SerializeField] private GameObject gunOption0;
    [SerializeField] private GameObject gunOption1;
    [SerializeField] private GameObject gunOption2;*/

    [SerializeField] private GameObject gunOptionPrefab;
    [SerializeField] private List<GameObject> gunOptions;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {


    }

    public void Init(int numGunOptions, GameObject newPlayer)
    {
        SetPlayer(newPlayer);
        
        // NOTE: With multiple gun options we will need to calculate the offset based on the number of options
        for (int i = 0; i < numGunOptions; ++i)
        {
            GameObject newGunOption = Instantiate(gunOptionPrefab, gameObject.transform);
            gunOptions.Add(newGunOption);
        }
        
        if (player)
        {
            foreach (GameObject gun in gunOptions)
            {
                gun.GetComponent<GunOption>()?.SetPlayer(player);
            }
        }
    }
    
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
