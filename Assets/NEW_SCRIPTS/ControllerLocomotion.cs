using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Animator anim;
    Transform cameraObject;
    Rigidbody playerRb;
    Vector3 movementDirection;

    [Header("Locomotion Stats")]
    public float sprintingSpeed;
    public float runningSpeed;
    public float rotationSpeed;

    [HeaderAttribute("Walking up stairs")]
    public GameObject lowStep;
    public GameObject highStep;


    [Header("Gravity Stats")]
    public float leapingVelocity;
    public float fallingVelocity;
    public float stepHeight;
    float stepSmooth = 0.1f;

    [HeaderAttribute("Jumping stats")]
    public float runJumpForce;
    public float sprintJumpForce;
    public float idleJumpForce = 2;
    public float hardFallDistance;

    [Header("Flags")]
    public bool isGrounded;
    public bool isJumping;
    public bool isFalling;
    public bool isLanding;
    public bool isSprinting;
    public float inAir = 0;



    private void Awake()
    {
        inputManager = GetComponentInChildren<InputManager>();
        anim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

        stepHeight = highStep.transform.position.y;
    }

    public void HandleAllMovement()
    {
        HandleGravityStuff();

        if (!isFalling)
        {
            Locomotion();
            Rotation();
        }
    }

    private void Locomotion()
    {
        HandleSteppingUp();
        movementDirection = cameraObject.forward * inputManager.verticalMovement;
        movementDirection = movementDirection + cameraObject.right * inputManager.horizontalMovement;
        movementDirection.Normalize();
        movementDirection.y = 0;

        if(!isSprinting)
            movementDirection = movementDirection * runningSpeed;
        else
            movementDirection = movementDirection * sprintingSpeed;


        Vector3 movementVelocity = movementDirection;
        playerRb.velocity = movementVelocity;
    }
    private void Rotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalMovement;
        targetDirection = movementDirection + cameraObject.right * inputManager.horizontalMovement;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;        

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        transform.rotation = playerRotation;
    }
    private void HandleGravityStuff()
    {
        UpdateFlags();
        CheckVerticalPlace();

        if(isGrounded)
            HandleGrounded();
        else
            HandleFalling();
    }
    private void HandleSteppingUp()
    {
        if(inputManager.verticalMovement != 0 || inputManager.horizontalMovement != 0)
        {   
            RaycastHit hitLower;
            if(Physics.Raycast(lowStep.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
            {
                RaycastHit hitUpper;
                if(!Physics.Raycast(highStep.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }


            }
        }


    }

    #region Gravity Functions
    private void HandleFalling()
    {
            inAir += Time.deltaTime;
            playerRb.AddForce(transform.forward * leapingVelocity);
            playerRb.AddForce(-Vector3.up * fallingVelocity * inAir);        
    }
    private void HandleGrounded()
    {
        if (isFalling)
        {
            if (inAir >= hardFallDistance)
                anim.SetInteger("landingType", 2);
            else
                anim.SetInteger("landingType", 1);

            anim.SetBool("isLanding", true);
        }

        anim.SetBool("isFalling", false); 
        anim.SetBool("isJumping", false);
        inAir = 0;
    }
    private void CheckVerticalPlace()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);

        switch (hit.distance)
        {
            case float n when n <= 0.05f:
                isGrounded = true;
                break;
            case float n when n >= 0.05f:
                isGrounded = false;
                if (!isJumping)
                    anim.SetBool("isFalling", true);
                break;
        }     
    }
    #endregion    
    public void HandleJump(float jumpForce, int JumpType)
    {
        if (isGrounded)
        {
            anim.SetBool("isJumping", true);
            anim.SetInteger("jumpType", JumpType);
            playerRb.AddForce(0, jumpForce * 100, 0);            
        }
    }

    private void UpdateFlags()
    {
        isLanding = anim.GetBool("isLanding");
        isJumping = anim.GetBool("isJumping");
        isFalling = anim.GetBool("isFalling");
        isSprinting = anim.GetBool("isSprinting");
    }
}
