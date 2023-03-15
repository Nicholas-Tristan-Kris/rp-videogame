using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] protected string weaponName;
    [SerializeField] protected string description;
    [SerializeField] protected int damage;
    public int Damage() {
        return damage;
    }
    [SerializeField] protected float cooldown;
    [SerializeField] protected Sprite icon;

    [Header("Player Animations")]
    [SerializeField] public Motion attack;
    [SerializeField] public Motion sheath;
    [SerializeField] public Motion unsheath;

    protected Collider weaponCollider;

    protected virtual void Start() {
        weaponCollider = GetComponent<Collider>();
        if (weaponCollider == null) {
            weaponCollider = GetComponentInChildren<Collider>();
        }
        weaponCollider.enabled = false;
    }

    protected virtual void Use() {

    }

}
