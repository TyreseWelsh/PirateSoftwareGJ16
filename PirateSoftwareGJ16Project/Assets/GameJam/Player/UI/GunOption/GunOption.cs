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
    public GunScriptableObject GunData;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        gunImage.sprite = GunData.image;
        gunName.text = GunData.gunName;
        gunDescription.text = GunData.description;
    }

    public void SelectOption()
    {
        Debug.Log("Selected " + GunData.gunName + "!");
        if (player)
        {
            player.GetComponent<ShootComponent>()?.AddGun(GunData);
            Cursor.lockState = CursorLockMode.Locked;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
