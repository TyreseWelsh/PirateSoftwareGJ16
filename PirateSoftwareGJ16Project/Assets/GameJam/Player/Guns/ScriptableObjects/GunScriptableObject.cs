using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunScriptableObject", order = 1)]
public class GunScriptableObject : ScriptableObject
{
    // Gun image
    public Sprite image;
    public string gunName;
    public string description;
    public int shootInterval;
    public int projectileDamage;
    public GameObject projectilePrefab;
    public GameObject gunPrefab;
}
