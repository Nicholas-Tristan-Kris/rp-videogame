using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private string weaponName;
    [SerializeField] private string description;
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    [SerializeField] private Image icon;

    [Header("User Animations")]
    [SerializeField] public AnimationClip userAnimation;
    [SerializeField] public AnimationClip sheath;
    [SerializeField] public AnimationClip unsheath;

    private Collider weaponCollider;

    private void Start() {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

    //deal damage to the target on collision with a tagged enemy
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

}
