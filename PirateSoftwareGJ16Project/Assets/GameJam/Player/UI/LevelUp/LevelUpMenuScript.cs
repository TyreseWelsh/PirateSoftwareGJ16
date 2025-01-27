using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject gunOptionPrefab;
    [SerializeField] private List<GameObject> gunOptions;

    private GameObject player;

    private float gunOptionsDistance = 120f;
    
    // Start is called before the first frame update
    void Start()
    {


    }

    public void Init(int numGunOptions, GameObject newPlayer)
    {
        SetPlayer(newPlayer);
        
        Time.timeScale = 0.1f;
        // Setting first gunOption spawn location
        float gunOptionSpawnLocationX = (numGunOptions - 1) * -gunOptionsDistance;
        // NOTE: With multiple gun options we will need to calculate the offset based on the number of options
        for (int i = 0; i < numGunOptions; ++i)
        {
            GameObject newGunOption = Instantiate(gunOptionPrefab, gameObject.transform);
            newGunOption.transform.localPosition = new Vector3(
                newGunOption.transform.localPosition.x + gunOptionSpawnLocationX,
                newGunOption.transform.localPosition.y, 
                newGunOption.transform.localPosition.z);
            
            gunOptions.Add(newGunOption);

            gunOptionSpawnLocationX += gunOptionsDistance * 2;
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
