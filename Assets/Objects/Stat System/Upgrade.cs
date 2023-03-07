using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    [Tooltip("The name of the upgrade")]
    public string name;
    [Tooltip("The description of the upgrade")]
    public string description;
    [Tooltip("The cost of the upgrade, -1 if free")]
    public int cost = -1;
    [Tooltip("Does the upgrade multiply the current value or add/subtract from it")]
    public enum UpgradeType { Add, Multiply };
    public UpgradeType type;
    [Tooltip("The amount of the upgrade")]
    public float amount;

    
}
