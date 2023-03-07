using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour
{

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private GameObject weapon = null;
    [SerializeField] private bool currentlyAttacking;
    public bool isAttacking = false;
    // Update is called once per frame
    void Update()
    {
        isAttacking = m_animator.GetCurrentAnimatorStateInfo(1).IsName("Sword Slash");

        if(weapon == null) {
            if(GetComponentInChildren<weapon>()) {
                GameObject equippedWeapon = GetComponentInChildren<weapon>().gameObject;
                equippedWeapon.GetComponent<weapon>().equipped();
                this.weapon = equippedWeapon;
            }
        }
        if(weapon != null) {
            if(!GetComponentInChildren<weapon>()) {
                weapon = null;
            }
            currentlyAttacking = weapon.GetComponent<weapon>().alreadyAttacked;
        }

        if (weapon != null && Input.GetButton("Fire1") && !isAttacking)
        {
            Attack();
            weapon.GetComponent<weapon>().setAttacked(true);
        }

    }

    private void Attack()
    {

        m_animator.SetTrigger("Attack");

    }

    public void setWeapon(GameObject weapon) {
        this.weapon = weapon;
    }
}
