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

    [SerializeField] private GameObject levelUpUI;
    private GameObject currentLevelUpMenu;
 
    // How many levelups we have in the backlog to show ui for
    private int levelUpBacklog;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = player.GetComponent<HealthComponent>();
        playerShootingScript = player.GetComponent<ShootComponent>();
        playerLevelUpScript = player.GetComponent<LevelUpComponent>();
        playerStatManagerScript = player.GetComponent<StatManagerComponent>();
        playerStatManagerScript.onStatUpdate += updateUpgradeUI;
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

    public void updateUpgradeUI(string _name, int _amount)
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

    // Called when something (mainly LevelUpComponent) wants to add a level up menu to the screen
    public void AddLevelUpMenu()
    {
        Debug.Log("Add to levelup backlog...");
        levelUpBacklog++;

        CheckLevelUpBacklog();
    }

    // Will create a new Level Up Menu if there is none already, if there is one present, do nothing (we still incremented the "backlog" so we can check after the current one is destroyed
    private void CheckLevelUpBacklog()
    {
        if (!currentLevelUpMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            currentLevelUpMenu = Instantiate(levelUpUI);
            LevelUpMenuScript levelUpMenuScript = currentLevelUpMenu.GetComponent<LevelUpMenuScript>();
            levelUpMenuScript.Init(3, player);
            levelUpMenuScript.onDestroyed += DecrementNumLevelUps;
            return;
        }
        
        Debug.Log("Level up menu already present!");
    }

    private void DecrementNumLevelUps()
    {
        Debug.Log("Reducing level up backlog!");
        levelUpBacklog--;
        currentLevelUpMenu = null;

        if (levelUpBacklog > 0)
        {
            CheckLevelUpBacklog();
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}
