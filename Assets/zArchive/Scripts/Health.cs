using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float regenPerSec = 1;
    [SerializeField] private bool canRegen = false;
    private float nextActionTime = 0.0f;
    private Animator anim = null;
    private bool died;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime && canRegen)
        {
            nextActionTime = Time.time + 1f;
            health += (int)regenPerSec;
        }
        if (health <= 0)
        {
            Death();
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(this.gameObject.GetComponent<Controller>());
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Destroy(this.gameObject.GetComponent<Collider>(), anim.GetCurrentAnimatorStateInfo(0).length);
            foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
            {
                Destroy(c);
            }
            Destroy(this.gameObject, anim.GetCurrentAnimatorStateInfo(0).length+2f);
        }
    }

    private void Death()
    {
        if (died == false) {anim.SetTrigger("Die"); died = true;}
        
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int health)
    {
        this.health = health;
    }
}