using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UpgradeTile : MonoBehaviour 
{
  [SerializeField] private string statName;

  private Stat stat;
  private Button button;


  void Start() {
    button = GetComponent<Button>();
    button.onClick.AddListener(UpgradeStat); 
    stat = FindObjectOfType<PlayerController>().GetStat(statName);
    if (stat == null) {
      Debug.LogError("Stat: " + statName + " not found");
    }

    //setup button graphics
    button.targetGraphic = stat.icon;
  }

  void UpgradeStat() {
    //TODO: add check to see if player has enough money/points/whatever to upgrade and return error if not
    
    stat.UpgradeStat();
    button.targetGraphic = stat.icon;
  } 

}