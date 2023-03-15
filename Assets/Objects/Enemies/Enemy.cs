using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float damage;
    [SerializeField] private float speed;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    public void TakeDamage(int damage) {
        animator.SetTrigger("Hit");
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        animator.SetTrigger("Die");
        float deathAnimationLength = animator.GetCurrentAnimatorClipInfo(0).Length;

        Invoke("DestroyEnemy", deathAnimationLength);
    }

    private void Attack() {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void DestroyEnemy() {
        Destroy(gameObject);
    }

}
