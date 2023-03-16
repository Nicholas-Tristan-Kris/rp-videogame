using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : Weapon
{
    [Header("Throwing Knife Stats")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private Transform knifeSpawnPoint;
    [SerializeField] private float knifeSpeed;

    public ThrowingKnife() : base() {

    }

    protected override void Start() {
        base.Start();
    }

    protected override void Use() {
        base.Use();
        GameObject knife = Instantiate(knifePrefab, knifeSpawnPoint.position, knifeSpawnPoint.rotation);
        knife.GetComponent<Rigidbody>().AddForce(knifeSpawnPoint.forward * knifeSpeed, ForceMode.Impulse);
    }
}

