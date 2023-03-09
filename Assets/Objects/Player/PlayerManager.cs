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

    [Header("Inventory")]
    [SerializeField] private Weapon activeWeapon;
    [SerializeField] private List<Weapon> weapons;
    
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            Attack();
        }
        
    }

    private void Attack() {
        if (activeWeapon != null) {
            animator.Play(activeWeapon.userAnimation.name);
        }
    }

    public void TakeDamage(float damage) {
        animator.SetTrigger("TakeDamage"); //TODO add take damage animation to animator
        stats.Find(x => x.name == "Health").value -= damage;
    }

}