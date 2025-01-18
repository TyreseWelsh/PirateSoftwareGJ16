using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunScriptableObject", order = 1)]
public class GunScriptableObject : ScriptableObject
{
    // Gun image
    public Sprite image;
    public string name;
    public string description;
    public int shootInterval;
    public GameObject projectilePrefab;
    public GameObject gunPrefab;
}
