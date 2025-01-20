using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelUpComponent : MonoBehaviour
{
    private int experience = 0;
    private int level = 1;
    private int experienceThreshold = 100;

    [SerializeField] private GameObject levelUpUI;

    public void IncreaseExperience(int increaseAmount)
    {
        experience += increaseAmount;

        // If we have enough xp, levelup!
        if (experience >= experienceThreshold)
        {
            int oldExperienceThreshold = experienceThreshold;
            LevelUp(1);
            experience = 0;
            
            // Call function again to check if we have enough xp to levelup again
            IncreaseExperience(experience - oldExperienceThreshold);
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
