using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileScriptableObject", order = 1)]
public class ProjectileScriptableObject : ScriptableObject
{
    public float moveSpeed;
    public int damage;
    public float range;
}
