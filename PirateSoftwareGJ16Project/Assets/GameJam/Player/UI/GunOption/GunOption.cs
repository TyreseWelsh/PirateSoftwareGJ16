using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GunOption : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image gunImage;
    [SerializeField] TMPro.TextMeshProUGUI gunName;
    [SerializeField] TMPro.TextMeshProUGUI gunDescription;

    [Header("Data")] 
    public List<GunScriptableObject> gunDataList;
    private GunScriptableObject gunData;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        InitOption();
    }

    private void InitOption()
    {
        if (gunDataList.Count > 0)
        {
            int randGunDataIndex = Random.Range(0, gunDataList.Count);
            gunData = gunDataList[randGunDataIndex];
            if (gunData)
            {
                gunImage.sprite = gunData.image;
                gunName.text = gunData.gunName;
                gunDescription.text = gunData.description;
            }
        }
    }
    
    public void SelectOption()
    {
        if (player && gunData)
        {
            player.GetComponent<ShootComponent>()?.AddGun(gunData);
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            Debug.LogError("Missing player or Gun Data", gameObject);
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
