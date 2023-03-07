using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollided : MonoBehaviour
{
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        enemy.GetComponent<EnemyAttack>().Attack(collider.gameObject);
    }
}
