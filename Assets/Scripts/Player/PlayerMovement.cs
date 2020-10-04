using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private float gravity = -9.81f;
    private float jumpHeight = 1.25f;
    private float groundDistance = 0.2f;

    public Transform groundCheck;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool grounded;

    private CharacterController controller;

    private float stuckTime = 1;
    private float time;

    void Start() 
    {
        time = stuckTime;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time > 0) return;

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1.0f);

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
