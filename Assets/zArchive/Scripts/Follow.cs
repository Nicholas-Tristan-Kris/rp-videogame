using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] int moveSpeed = 1;
    [SerializeField] int moveDist = 1;
    [SerializeField] int attackDist = 1;

    private Transform pTransform = null;
    private EnemyAttack attackScript = null;
    private Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pTransform = player.GetComponent<Transform>();
        attackScript = this.GetComponent<EnemyAttack>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))) { cast(); }
    }

    private void cast()
    {
        RaycastHit hit;
        Vector3 rayDirection = pTransform.position - transform.position;
        Vector3 rayStart = transform.position + new Vector3(0, 2, 0);
        Debug.DrawRay(rayStart, rayDirection, Color.green);
        if (Physics.Raycast(rayStart, rayDirection, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == ("Player"))
            {
                moveToPlayer();
            }

        }
    }

    void moveToPlayer()
    {

        transform.LookAt(pTransform);

        if (Vector3.Distance(transform.position, pTransform.position) <= moveDist)
        {
            
            if (Vector3.Distance(transform.position, pTransform.position) <= attackDist)
            {
                attackScript.isAttacking = true;
                anim.SetTrigger("Attack");
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                attackScript.isAttacking = false;
            }
        }
        else
        {
           anim.SetBool("isWalking", false);
        }
        }
}
