using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlayerManager : MonoBehaviour
{

    [Header("Stats")]
    [Tooltip("All of the different stats that a player has")]
    [SerializeField] private List<Stat> stats;
    [Tooltip("All of the difference multipliers that a player has")]
    [SerializeField] private List<Mutliplier> mutlipliers;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
