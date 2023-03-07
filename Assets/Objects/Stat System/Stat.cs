using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stat
{
    [Tooltip("The name of the stat")]
    public string name;
    [Tooltip("The description of the stat")]
    public string description;
    [Tooltip("The current value of the stat")] 
    public float value;
    [Tooltip("The maximum value of the stat")] 
    public float maxValue;
    [Tooltip("Is the stat able to regen itself back to max value")] 
    public bool canRegen;
    [Tooltip("By default the regen rate is 1 unit per second, but this can be changed to make it regen faster or slower")]
    [Range(1f, 10f)] public float regenMultiplier;
    [Tooltip("The resistance to the stat lowering, 0 means the full change is applied, 1 means the stat doesn't change")]
    [Range(0f, 1f)] public float resistance;

    [Tooltip("The upgrades that can be applied to the stat")]
    public List<Upgrade> upgrades;
    [SerializeField] private int currentUpgrade = 0;

    [Tooltip("The icon that represents the stat")]
    public Image icon;

    void Update() {
        if (canRegen) {
            value += regenMultiplier * Time.deltaTime;
            if (value > maxValue) {
                value = maxValue;
            }
        }
    } 

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
