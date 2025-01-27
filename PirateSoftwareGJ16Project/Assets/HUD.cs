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
    [SerializeField] private Slider experienceBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private GameObject upgradeBar;
    [SerializeField] private Image[] upgradeIcons;
    [SerializeField] private TextMeshProUGUI[] upgradeAmounts;
    
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

    [SerializeField] private List<SO_UpgradeBar> statObjects;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = player.GetComponent<HealthComponent>();
        playerShootingScript = player.GetComponent<ShootComponent>();
        playerLevelUpScript = player.GetComponent<LevelUpComponent>();
        playerStatManagerScript = player.GetComponent<StatManagerComponent>();
        Debug.Log(playerHealthScript.GetMaxHealth(true));
        playerStatManagerScript.onStatUpdate += updateUI;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthScript)
        {
            healthBar.maxValue = playerHealthScript.GetMaxHealth(true);
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

    public void updateUI(string _name, int _amount)
    {
        
        for (int i = 0; i < statObjects.Count; i++)
        {
            
            if (statObjects[i]._name == _name)
            {
                
                if (!upgradeIcons[i].GameObject().activeSelf)
                {
                    Debug.Log(i + "YOUR MUM YEAH");
                    upgradeIcons[i].GameObject().SetActive(true);
                    upgradeAmounts[i].GameObject().SetActive(true);
                    
                }
                upgradeIcons[i].sprite = statObjects[i]._icon;
                upgradeAmounts[i].text = _amount.ToString();

                    
                
            }
        }
    }
    
    
}
