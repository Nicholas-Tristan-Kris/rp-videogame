using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [Header("Arrow Variables")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed;

    public Bow() : base() {

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Use() {
        base.Use();
        //instantiate arrow
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        //add force to arrow
        arrow.GetComponent<Rigidbody>().AddForce(arrowSpawnPoint.forward * arrowSpeed, ForceMode.Impulse);
        
    }
}
