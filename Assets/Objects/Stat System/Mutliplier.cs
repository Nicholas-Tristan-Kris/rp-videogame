using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mutliplier
{
    [Tooltip("The name of the multiplier")]
    public string name;
    [Tooltip("The description of the multiplier")]
    public string description;
    [Tooltip("The value of the multiplier")]
    public float value;

    [Tooltip("The upgrades that can be applied to the multiplier")]
    public List<Upgrade> upgrades;
    [SerializeField] private int currentUpgrade = 0;


    public void UpgradeStat() {
        if (currentUpgrade < upgrades.Count) {
            Upgrade upgrade = upgrades[currentUpgrade];
            if (upgrade.type == Upgrade.UpgradeType.Add) {
                value += upgrade.amount;
            } else if (upgrade.type == Upgrade.UpgradeType.Multiply) {
                value *= upgrade.amount;
            }
            currentUpgrade++;
        }
    }
}
