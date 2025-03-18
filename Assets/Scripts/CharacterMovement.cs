using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform CameraRotation;
    private Rigidbody rb;// Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isGrounded;
    private bool jumpCooldown=true;
    [SerializeField] private float Speed=10;
    [SerializeField] private float JumpForce=10;
    [SerializeField] private float CayoteLength=0.5f;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Jump()
    {
        if (Input.GetButton("Jump")&&isGrounded)
        {
            rb.AddForce(0,JumpForce,0,ForceMode.Impulse);
            StartCoroutine(JumpCooldown());
        }
    }

    void Move()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        
        transform.rotation=Quaternion.Euler(0,CameraRotation.rotation.eulerAngles.y,0);
        rb.AddRelativeForce(inputVector*Speed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (jumpCooldown)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(CayoteTime());
    }

    IEnumerator CayoteTime()
    {
        yield return new WaitForSeconds(CayoteLength);
        isGrounded = false;
    }

    IEnumerator JumpCooldown()
    {
        jumpCooldown = false;
        isGrounded = false;
        yield return new WaitForSeconds(1);
        jumpCooldown = true;
    }
}
