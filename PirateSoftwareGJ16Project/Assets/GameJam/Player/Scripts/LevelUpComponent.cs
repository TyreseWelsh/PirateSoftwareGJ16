using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelUpComponent : MonoBehaviour
{
    public int experience = 0;

    public int level = 1;
    public int experienceThreshold = 100;

    [SerializeField] private GameObject levelUpUI;


    private void Update()
    {
        Debug.LogWarning("CURRENT EXPERIENCE= " + experience);
        Debug.LogWarning("CURRENT EXPERIENCETHRESHOLD= " + experienceThreshold);
    }

    // For game jam
    public void AddToExperience()
    {
        Debug.Log("Increased player experience by 20");
        IncreaseExperience(20);
    }
    
    public void IncreaseExperience(int increaseAmount)
    {
        experience += increaseAmount;

        // If we have enough xp, levelup!
        if (experience >= experienceThreshold)
        {
            int oldExperienceThreshold = experienceThreshold;
            int extraExperience = experience - oldExperienceThreshold;

            experienceThreshold *= Mathf.CeilToInt(1.5f);
            LevelUp(1);
            
            experience = 0;
            // Call function again to check if we have enough xp to levelup again
            IncreaseExperience(extraExperience);
        }
    }

    // NOTE: Need to figure out how we can handle UI with multiple level ups at the same time
    public void LevelUp(int levelIncreaseAmount)
    {
        level += levelIncreaseAmount;
        
        Cursor.lockState = CursorLockMode.Confined;
        GameObject newLevelUpMenu = Instantiate(levelUpUI);
        newLevelUpMenu.GetComponent<LevelUpMenuScript>()?.Init(5, gameObject);
    }

    public void IncreaseLevel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelUp(1);
        }
    }
}
