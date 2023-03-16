using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerController : MonoBehaviour
{

    [Header("Stats")]
    [Tooltip("All of the different stats that a player has")]
    [SerializeField] private List<Stat> stats;
    [Tooltip("All of the difference multipliers that a player has")]
    [SerializeField] private List<Mutliplier> mutlipliers;

    [Header("Inventory")]
    [SerializeField] public int activeWeapon; //index of active weapon
    [SerializeField] public List<Weapon> weapons;

    [Header("Animation")]
    [SerializeField] private AnimatorController animatorController;
    // [SerializeField] private Transform rightHand;
    // [SerializeField] private Transform leftHand;
    // [SerializeField] private Transform back;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private float sprintMultiplier;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight; 
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDrag;

    [Header("Jump Control")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDrag;
    [SerializeField] private float jumpCooldown;

    private AnimatorStateMachine attackState;
    private Animator animator;
    private AnimatorState attack;
    private AnimatorState sheath;

    private bool isShieldEquipped = false;
    private bool isWeaponEquipped = false;
    private bool canJump;
    private bool isGrounded;
    private float horizInput, vertInput;
    private Vector3 moveDir;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        animator = GetComponent<Animator>();

        //TODO shield maybe?
        foreach (ChildAnimatorStateMachine casm in animatorController.layers[0].stateMachine.stateMachines) {
            if (casm.stateMachine.name == "Attack") {
                attackState = casm.stateMachine;
            }
        }

        foreach (ChildAnimatorState animState in attackState.states) {
            if (animState.state.name == "Attack") {
                attack = animState.state;
            } else if (animState.state.name == "Sheath") {
                sheath = animState.state;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        GetInput();
    }

    void FixedUpdate() {
        MovePlayer();
    }

    private void GetInput() {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
        if (Input.GetAxis("Sprint") > 0) { 
            vertInput *= sprintMultiplier;
        }

        if (Input.GetAxis("Jump") > 0 && canJump && isGrounded) {
            canJump = false;
            Jump();
        }

        if (Input.GetAxis("Sheath") > 0) {
            Sheath();
        }

        if (Input.GetAxis("Fire1") > 0 && isWeaponEquipped)
        {
            Attack();
        }
    
        if (Input.GetAxis("Fire2") > 0)
        {
            Block();
        }

        //if less than 0 then switch to previous weapon else switch to next weapon
        if (Input.GetAxis("Switch Weapon") != 0) {
            SwitchWeapon(Input.GetAxis("Switch Weapon"));
        }
    }

    private void SwitchWeapon(float val) {
        Sheath();

        if (val < 0) {
            if (activeWeapon == 0) {
                    activeWeapon = weapons.Count - 1;
            } else {
                activeWeapon--;
            }
        } else {
            if (activeWeapon == weapons.Count - 1) {
                activeWeapon = 0;
            } else {
                activeWeapon++;
            }
        }

        attack.motion = weapons[activeWeapon].attack;
        sheath.motion = weapons[activeWeapon].sheath;

        Sheath();
    }

    private void MovePlayer() {
        //calc move dir
        moveDir = orientation.forward * vertInput + orientation.right * horizInput;

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * jumpDrag * Time.deltaTime, ForceMode.VelocityChange);
        
        animator.SetFloat("MoveSpeed", rb.velocity.magnitude);
        
        SpeedControl();
    }

    private void CheckGround() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        animator.SetBool("Grounded", isGrounded);
        if (isGrounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0f;
        }
    }
    
    private void SpeedControl() {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        animator.SetTrigger("Jump");
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke("ResetJump", jumpCooldown);
    }

    private void ResetJump() {
        canJump = true;
    }

    private void Sheath() {
        animator.SetFloat("AnimDirection", animator.GetFloat("AnimDirection") * -1f);
        animator.SetTrigger("Sheath");
        isWeaponEquipped = !isWeaponEquipped;
        weapons[activeWeapon].gameObject.SetActive(isWeaponEquipped);
    }

    private void Attack() {
        animator.SetTrigger("Attack");
    }

    private void Block() {
        animator.SetTrigger("Block");
    }

    public void TakeDamage(float damage) {
        animator.SetTrigger("Hit"); 
        stats.Find(x => x.name == "Health").value -= damage;
    }
}