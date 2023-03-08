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
    [SerializeField] private AnimationClip attackAnimation;
    [SerializeField] private AnimationClip deathAnimation;
    [SerializeField] private AnimationClip takeDamage;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip walkAnimation;

    public void TakeDamage(int damage) {
        animator.Play(takeDamage.name);
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        animator.Play(deathAnimation.name);
        Destroy(gameObject);
    }

    private void Attack() {
        animator.Play(attackAnimation.name);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }



}
