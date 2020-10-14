using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

//////Build upon this and try not to change anything, unless absolutely necessary.//////
public class PlayerMovement : MonoBehaviour
{
    // Public variables in Unity
    public CharacterController controller;  // Calls CharacterController

    public Transform cam; // Calls for Main Camera controlled by Cinemachine
    public Transform groundCheck; // Calls for the ground check
    public LayerMask groundMask; // Calls for the ground layer
    
    // Public floats
    public float speed = 6f;  // Speed of the player
    public float turnSmoothingTime = 0.1f;  // Time of the player turning
    public float groundDistance = 0.4f;  // Ground distance
    public float gravity = -9.81f; // Velocity of gravity
    public float jumpHeight = 3f; // How high the player jumps

    public float WalkSpeed = 6f; //How fast the player accelerates **WHEN WALKING**
    public float RunSpeed = 12f; // How fast the player acelerates **WHEN RUNNING**

    public float CrouchSpeed = 2f;
    public float NormalHeight = 1f;
    public float CrouchHeight = 0.5f;

    public Transform armYPos;//Where the "arms" will cast from << for vaulting and climbing ledges of platforms
    public Transform legYPos;//Where the "feet" will cast from << for vaulting and climbing ledges of platforms

    public float vaultDist = 1;//Distance that the player will move forward onto ledge;

    public bool isGrounded; // Ask is player is on the ground

    // Private variables
    Vector3 velocity; // Calls for velocity of falling
    float turnSmoothVelocity; // Velocity for smoother turning

    [SerializeField] private string vaultableTag = "Vaultable";

    // Update is called once per frame
    void Update()
    {

        speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;//Allows to switch between walk and run
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;//hides Cursor when in play mode. Press ESC t release cursor
        
        // Checks Ground Check to see if player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical"); // W/S or Up/Down
        // Make sures that we don't add upon values (when two keys are pressed) and moves along x- and z-axises
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        
        //Makes sure that the falling velocity does not go under -2.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //This makes it so you can actually get up ledges.
        if (isGrounded)
        {
            controller.slopeLimit = 45;
        }
        else
        {
            controller.slopeLimit = 90;
        }

        
        // Allows the player to fall.
        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);

        // Allows the player to jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        //Added and updated crouching 7/30/2020 ~ Dakota
        if (Input.GetKey(KeyCode.LeftControl))  // Might need to configure in Input Manager 
        {
            controller.height = CrouchHeight;
            speed = Input.GetKey(KeyCode.LeftControl) ? CrouchSpeed : WalkSpeed;
        }
        else
        {
            controller.height = NormalHeight;
            //speed = speed;
        }

        // Detects if WASD/Arrow keys are pressed
        if (direction.magnitude >= 0.1f)
        {
            // Creates a new angle for the player to follow.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothingTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Moves the player in the angle that the camera is facing.
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //Allows for vaulting up an object <<<We should probably tag objects that are vaultable or somehow id them.
        //Right now slopes are kind of weird when vaulting //Camaron 7/31/2020
        
        if (Input.GetButtonDown("Jump") && !isGrounded)
        {

            //The raycasts check if arm height is air and leg height is a platform;
            Vector3 controllerDir = Quaternion.Euler(0f, controller.transform.eulerAngles.y, 0f) * Vector3.forward;

            RaycastHit armHit, legHit;
            Physics.Raycast(armYPos.position, controllerDir, out armHit, 1.5f);
            Physics.Raycast(legYPos.position, controllerDir, out legHit, 1.5f);


            //Read above comment. We can probs change legHit.collider != null to legHit.collider.CompareTag("vaultable") or something like that

            if (armHit.collider == null && legHit.collider != null && legHit.collider.CompareTag("Vaultable"))
            {
                //print("success!!!");

                controller.Move(new Vector3(0, legHit.collider.bounds.max.y - groundCheck.position.y, 0)); //Moves player to top of obj

                controller.Move(controllerDir.normalized * vaultDist);//Moves player forward
            }
        }
    }
}
