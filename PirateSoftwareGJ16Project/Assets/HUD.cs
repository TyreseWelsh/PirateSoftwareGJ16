using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour,  IUIUpdate
{
    [SerializeField] private GameObject player;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider experienceBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private GameObject upgradeBar;
    
    private LevelUpComponent playerLevelUpScript;
    private HealthComponent playerHealthScript;
    private ShootComponent playerShootingScript;
    private StatManagerComponent playerStatManagerScript;
    
    [System.Serializable]
    public class StatObject
    {
        public string statName;
        public int amount;
        public Sprite image;

    }

    [SerializeField] private List<StatObject> statObjectIcons;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = player.GetComponent<HealthComponent>();
        playerShootingScript = player.GetComponent<ShootComponent>();
        playerLevelUpScript = player.GetComponent<LevelUpComponent>();
        playerStatManagerScript = player.GetComponent<StatManagerComponent>();
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

        if (playerLevelUpScript)
        {
            experienceBar.maxValue = playerLevelUpScript.experienceThreshold;
            experienceBar.value = playerLevelUpScript.experience;
        }
        
        
    }

    public void updateUI(string _name)
    {
        
    }
    
    
}
