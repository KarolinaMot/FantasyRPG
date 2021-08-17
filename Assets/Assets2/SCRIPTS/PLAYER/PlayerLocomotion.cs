using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    //this code is responsible for getting information from the input manager and moving the player acordingly

    #region Variables
    PlayerManager playerManager;
    InputManager inputManager;
    PlayerAnimatorManager animatorManager;
    CombatManager combatManager;

    Vector3 moveDirection;
    Transform cameraObject;
    [HideInInspector]
    public Rigidbody playerRigidBody;

    [Header("Falling")]
    [HideInInspector]
    public float inAirTimer;
    [HideInInspector]
    public float leapingVelocity;
    public float fallingVelocity = 1;
    [HideInInspector]
    public float rayCastHeightOffset = 0.5f;
    [HideInInspector]
    public float distanceToGround = 0;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isClimbing;

    [Header("Movement Speeds")]
    [SerializeField] float walkingSpeed = 1.5f;
    [SerializeField] float runningSpeed = 6;
    [SerializeField] float sprintingSpeed = 10;
    [SerializeField] float rotationSpeed = 12;

    [Header("Jump speeds")]
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float gravityIntensity = -3;

    [Header("Going up steps")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepSmooth = 0.1f;
    #endregion

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        combatManager = GetComponent<CombatManager>();
        cameraObject = Camera.main.transform;



    }
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isLocked || combatManager.isAttacking)
            return;


        HandleMovement();
        HandleRotation();
        HandleStepClimb();
    }

    private void HandleMovement()
    {

        moveDirection = cameraObject.forward * inputManager.verticalImput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalImput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (!isJumping)
        {
            walkingSpeed = 1.5f;
            runningSpeed = 6;
            sprintingSpeed = 12;
        }

        if (isSprinting){
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
            }
            else if (inputManager.moveAmount > 0 && inputManager.moveAmount < 0.5f)
            {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        
        Vector3 movementVelocity = moveDirection;
        if (isJumping)
        {
            playerRigidBody.velocity += movementVelocity;
        }
        else
            playerRigidBody.velocity = movementVelocity;


    }

    private void HandleRotation()
    {

        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalImput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalImput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;

    }

    private void HandleFallingAndLanding() //checks if player is falling or landing and plays animation
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if (!isGrounded) //checks if player isnt touching the ground i.e. is falling and plays falling animation if he is
        {
            Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, groundLayer);

            distanceToGround = hit.distance;
            animatorManager.animator.SetFloat("distanceToGround", distanceToGround);

            if (!playerManager.isLocked && distanceToGround > jumpHeight)
                animatorManager.PlayTargetAnimation("FALL", true);

            inAirTimer = inAirTimer + Time.deltaTime;

            if (!isJumping && !isClimbing)
            {
                fallingVelocity = 1500;
            }

            if (isJumping || isClimbing)
            {
                fallingVelocity = 100;
            }

            playerRigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.Raycast(rayCastOrigin, -Vector3.up, 0.2f, groundLayer)) //checks if player is touching the ground 
        {

            if (!isGrounded && playerManager.isLocked) //checks if player was previously falling
            {
                animatorManager.PlayTargetAnimation("LAND", true);
            }

            isGrounded = true;

            inAirTimer = 0;            
        }
        else //if player isnt touching the ground - he isnt grounded, changes isGrounded bool
        {
            isGrounded = false;
        }
    }

    public void HandleJump() //is called if player presses jump button
    {
        if (isGrounded && !isJumping)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            
            if(inputManager.moveAmount > 0)
            {
                walkingSpeed = 1;
                runningSpeed = 4;
                sprintingSpeed = 7;
            }

            Vector3 origin = transform.position;
            origin.y = origin.y + rayCastHeightOffset;


            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);

            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidBody.velocity = playerVelocity;
        } 
    }

    private void HandleStepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.3f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 1) && inputManager.moveAmount > 0)
            {
                playerRigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                isClimbing = true;
            }
            else
                isClimbing = false;
        }
        else
            isClimbing = false;

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.3f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 1) && inputManager.moveAmount > 0)
            {
                playerRigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                isClimbing = true;
            }
            else
                isClimbing = false;
        }
        else
            isClimbing = false;

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.3f))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 1) && inputManager.moveAmount > 0)
            {
                playerRigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                isClimbing = true;
            }
            else
                isClimbing = false;
        }
        else
            isClimbing = false;

    }
}
