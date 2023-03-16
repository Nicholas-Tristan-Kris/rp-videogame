using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slingshot : Weapon
{
    [Header("Slingshot Stats")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform rockSpawnPoint;
    [SerializeField] private float rockSpeed;

    public Slingshot() : base() {
        
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Use() {
        base.Use();
        GameObject rock = Instantiate(rockPrefab, rockSpawnPoint.position, rockSpawnPoint.rotation);
        rock.GetComponent<Rigidbody>().AddForce(rockSpawnPoint.forward * rockSpeed, ForceMode.Impulse);
    }
}