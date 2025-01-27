using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoCount;

    private HealthComponent playerHealthScript;

    private ShootComponent playerShootingScript;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = player.GetComponent<HealthComponent>();
        playerShootingScript = player.GetComponent<ShootComponent>();
        Debug.Log(playerHealthScript.GetMaxHealth(true));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthScript)
        {
            healthBar.maxValue = playerHealthScript.GetMaxHealth(true);
            Debug.Log("HEALTH MAX = " + healthBar.maxValue);
            healthBar.value =  playerHealthScript.currentHealth;
            healthText.text = playerHealthScript.currentHealth.ToString();
        }
        
        if (playerShootingScript)
        {
            ammoCount.text = playerShootingScript.currentAmmoCount + "/" + playerShootingScript.MAX_AMMO_COUNT;
        }
        
    }
}
