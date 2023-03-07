using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    }

    private void MovePlayer() {
        //calc move dir
        moveDir = orientation.forward * vertInput + orientation.right * horizInput;

        if (isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * jumpDrag * Time.deltaTime, ForceMode.VelocityChange);
        
        SpeedControl();
    }

    private void CheckGround() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

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

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke("ResetJump", jumpCooldown);
    }

    private void ResetJump() {
        canJump = true;
    }
}

