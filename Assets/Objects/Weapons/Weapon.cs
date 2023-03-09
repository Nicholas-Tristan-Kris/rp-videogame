using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("User Animations")]
    [SerializeField] public AnimationClip userAnimation;
    [SerializeField] public AnimationClip sheath;
    [SerializeField] public AnimationClip unsheath;

    protected Collider weaponCollider;

    protected virtual void Start() {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

}
