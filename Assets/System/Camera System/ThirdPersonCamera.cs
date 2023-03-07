using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = target.position - new Vector3(transform.position.x, target.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * vertInput + orientation.right * horizInput;

        if (inputDir != Vector3.zero)
        {
            target.forward = Vector3.Slerp(target.forward, inputDir, Time.deltaTime * rotSpeed);
        }
    }
}
