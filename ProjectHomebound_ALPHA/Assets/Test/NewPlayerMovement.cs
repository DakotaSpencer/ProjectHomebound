using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    // Public variables in Unity
    public CharacterController controller;  // Calls Character Controller
    public Transform cam; // Calls for Main Camera controlled by Cinemachine
    public Transform groundCheck; // Calls for the ground check
    public LayerMask groundMask; // Calls for the ground layer
    
    // Public floats
    public float speed;  // Speed of the player
    public float turningTime = 0.1f;  // Time of the player turning
    public float groundDistance = 0.4f;  // Ground distance
    public float gravity = -19.62f; // Velocity of gravity
    public float jumpHeight = 4f; // How high the player jumps
    public float normalSpeed = 10f; // The speed of the player when walking
    public float sprintSpeed = 20f; // Speed when the player is sprinting
    public float crouchingSpeed = 5f; // The speed of the player when running
    public float normalHeight = 2f; // Height when standing
    public float crouchHeight = 1f; // Height when crouching

    // Private variables
    Vector3 velocity; // Calls for velocity of falling
    float turnSmoothVelocity; // Velocity for smoother turning
    bool isGrounded; // Ask is player is on the ground

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical"); // W/S or Up/Down
        // Make sures that we don't add upon values (when two keys are pressed) and moves along x- and z-axises
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Detects if WASD/Arrow keys are pressed
        if (direction.magnitude >= 0.1f)
        {
            // Creates a new angle for the player to follow
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turningTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Moves the player in the angle that the camera is facing AKA moveDir
            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            if (Input.GetKeyDown("left shift")) // Might need to configure in Input Manager 
            {
                speed = sprintSpeed;
            }
            else if (Input.GetKeyDown("left ctrl"))  // Might need to configure in Input Manager 
            {
                controller.height = crouchHeight;
                speed = crouchingSpeed;
            }
            else
            {
                controller.height = normalHeight;
                speed = normalSpeed;
            }
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Checks Ground Check to see if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        //Makes sure that the falling velocity does not go under -2.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Allows the player to jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Allows the player to fall.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
