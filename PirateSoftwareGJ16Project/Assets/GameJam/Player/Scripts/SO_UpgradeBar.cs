using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeBarData", menuName = "ScriptableObjects/UpgradeBarScriptableObject", order = 1)]
public class SO_UpgradeBar : ScriptableObject
{
    public string _name;
    public int _amount;
    public Sprite _icon;

}
