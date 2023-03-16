using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField] private float discoverRadius;
    enum DiscoverType { Raycast, Radius }
    [SerializeField] private DiscoverType discoverType;
    [SerializeField] private float attackRad;
    [SerializeField] private Weapon weapon;

    private Transform target;

    protected virtual void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update() {
        if (discoverType == DiscoverType.Radius) {
            if (Vector3.Distance(transform.position, target.position) <= discoverRadius) {
                MoveToTarget();
            }
        } else if (discoverType == DiscoverType.Raycast) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, discoverRadius)) {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    MoveToTarget();
                }
            }
        }
    }

    protected virtual void MoveToTarget() {
        transform.LookAt(target);
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) <= attackRad) {
            Attack();
        }
    }

    public void TakeDamage(int damage) {
        animator.SetTrigger("Hit");
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        animator.SetTrigger("Die");
        float deathAnimationLength = animator.GetCurrentAnimatorClipInfo(0).Length;

        Invoke("DestroyEnemy", deathAnimationLength);
    }

    protected virtual void Attack() {
        animator.SetTrigger("Attack");
    }

    protected virtual void DestroyEnemy() {
        Destroy(gameObject);
    }
}
