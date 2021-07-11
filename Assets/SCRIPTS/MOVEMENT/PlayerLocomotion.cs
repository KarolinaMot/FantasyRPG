using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    //this code is responsible for getting information from the input manager and moving the player acordingly

    #region Variables
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidBody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public float minFallingHeight = 3;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;

    [Header("Jump speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    #endregion

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        
    }
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (playerManager.isLocked || isJumping)
          return;
        

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {

        moveDirection = cameraObject.forward * inputManager.verticalImput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalImput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting){
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;
                Debug.Log("Running");
            }
            else if (inputManager.moveAmount > 0 && inputManager.moveAmount < 0.5f)
            {
                moveDirection = moveDirection * walkingSpeed;
                Debug.Log("Walking");
            }
        }

        
        Vector3 movementVelocity = moveDirection;
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

        if (!isGrounded && !isJumping) //checks if player isnt touching the ground i.e. is falling and plays falling animation if he is
        {
            if(!playerManager.isLocked)
                animatorManager.PlayTargetAnimation("FALL", true);

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidBody.AddForce(transform.forward * leapingVelocity);
            playerRigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.1f, -Vector3.up, out hit, 0.3f, groundLayer)) //checks if player is touching the ground 
        {
            if (!isGrounded && playerManager.isLocked) //checks if player was previously falling
            {
                animatorManager.PlayTargetAnimation("LAND", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else //if player isnt touching the ground - he isnt grounded, changes isGrounded bool
        {
            isGrounded = false;
        }

        Debug.Log("Is grounded:" +  isGrounded);
        Debug.Log("Is locked:" +  playerManager.isLocked);
    }

    public void HandleJump() //is called if player presses jump button
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            Vector3 origin = transform.position;
            origin.y = origin.y + rayCastHeightOffset;

            if (Physics.Raycast(origin, -Vector3.up, minFallingHeight))
            {
                animatorManager.animator.SetBool("bigGroundDistance", true);
            }
            else
            {
                animatorManager.animator.SetBool("bigGroundDistance", false);
            }

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidBody.velocity = playerVelocity;
        } 
    }
}
