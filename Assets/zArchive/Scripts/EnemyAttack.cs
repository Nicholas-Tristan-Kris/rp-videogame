using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool isAttacking = false;
    [SerializeField] private int damage = 50;
    [SerializeField] private GameObject hand = null;
    private Collider handCollider = null;
    private Animator m_animator = null;
    private bool alreadyAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        handCollider = hand.GetComponent<Collider>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))) {alreadyAttacked = false;}
    }
    public void Attack(GameObject player)
    {
        if (!alreadyAttacked && isAttacking && (this.gameObject.GetComponent<Health>().getHealth() > 0))
        {
            alreadyAttacked = true;
            if (player.tag == "Player")
            {
                player.GetComponent<Health>().setHealth(player.GetComponent<Health>().getHealth() - damage);
            }
        }
    }
}
