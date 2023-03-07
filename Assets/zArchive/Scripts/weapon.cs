using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private int uses = 500;
    [SerializeField] private GameObject player = null;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] public bool alreadyAttacked = false;
    private GameObject enemy = null;
    private Animator m_animator = null;

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = player.GetComponent<Fighting>().isAttacking;
        m_animator = player.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking && enemy.tag == "enemy") {
            enemy.GetComponent<Health>().setHealth(enemy.GetComponent<Health>().getHealth() - damage);
            enemy.GetComponent<Animator>().SetTrigger("Take Damage");
            uses--;
            isAttacking = false;
        }
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (uses <= 0)
        {
            //TODO - make it break instead of destroy it
            Destroy(this);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        enemy = collision.gameObject.transform.root.gameObject;
        
    }

    void OnTriggerExit(Collider collision) {
        enemy = null;
    }

    public void setPlayer(GameObject player) {
        this.player = player;
        Debug.Log("Set Player");
    }

    public void equipped() {
        Debug.Log("Inside Equipped Method");
        GameObject player = GetComponentInParent<Fighting>().gameObject;
        Debug.Log("Player is: " + player.name);
        setPlayer(player);
    }

    public void setAttacked(bool isAttacking) {
        this.isAttacking = isAttacking;
    }
}
