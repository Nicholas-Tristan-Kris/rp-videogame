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
    //everything below will be implemented in an animation controller
    // [SerializeField] private AnimationClip attackAnimation;
    // [SerializeField] private AnimationClip deathAnimation;
    // [SerializeField] private AnimationClip takeDamage;
    // [SerializeField] private AnimationClip idleAnimation;
    // [SerializeField] private AnimationClip walkAnimation;

    public void TakeDamage(int damage) {
        animator.SetTrigger("TakeDamage");
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        animator.SetTrigger("Die");

        //TODO wait until death animation is finished to destroy the object
        Destroy(gameObject);
    }

    private void Attack() {
        animator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }



}
