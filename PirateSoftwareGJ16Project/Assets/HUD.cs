using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Slider HealthBar;
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI AmmoCount;

    private HealthComponent playerHealthScript;

    private ShootComponent playerShootingScript;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = player.GetComponent<HealthComponent>();
        playerShootingScript = player.GetComponent<ShootComponent>();
        Debug.Log(playerHealthScript.GetMaxHealth(false));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerHealthScript != null)
        {
            HealthBar.value = playerHealthScript.currentHealth;
            HealthText.text = playerHealthScript.currentHealth.ToString();
        }
        if (playerShootingScript != null)
        {
            AmmoCount.text = playerShootingScript.currentAmmoCount + "/" + playerShootingScript.MAX_AMMO_COUNT;
        }
        
    }
}
