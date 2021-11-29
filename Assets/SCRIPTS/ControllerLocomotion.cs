using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    Transform cameraObject;
    Rigidbody playerRb;
    Vector3 movementDirection;

    [Header("Raycast lengths")]
    public float groundRaycast =0.6f;
    public float lowStepRaycast = 0.1f;
    public float highStepRaycast = 0.2f;
    
    #region Locomotion variables
    [Header("Locomotion Stats")]
    public float sprintingSpeed;
    public float runningSpeed;
    public float rotationSpeed;
    
    [HeaderAttribute("Walking up stairs")]
    public GameObject lowStep;
    public GameObject highStep;
    public float stepSmooth = 0.1f;
    #endregion

    #region Falling Stats
    [Header("Falling Stats")]
    public float leapingVelocity;
    public float fallingVelocity;
    private float fallingVelocity2;
    public float rayCastHeighOffset;
    public LayerMask groundLayer;
    #endregion

    #region Jumping Stats
    [HeaderAttribute("Jumping stats")]
    public float runJumpForce = 3;
    public float sprintJumpForce = 5;
    public float idleJumpForce = 2;
    public float gravityIntensity = -15;
    public float hardFallDistance;
    #endregion

    #region Flag variables
    [Header("Flags")]
    public bool isJumping;
    public bool isGrounded;
    public bool isFalling;
    public bool isSprinting;
    public float inAirTimer = 0;
    #endregion

    private void Awake()
    {

        playerManager = GetComponentInChildren<PlayerManager>();
        inputManager = GetComponentInChildren<InputManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerRb = GetComponent<Rigidbody>();

        cameraObject = Camera.main.transform;
        fallingVelocity2 = fallingVelocity;
    }

    public void HandleAllMovement()
    {
        UpdateFlags();
        StepClimb();
        HandleFallingAndLanding();

        Locomotion();
        Rotation();       
    }

    private void Locomotion()
    {
        if (playerManager.isLockedInAnimation)
            return;
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
        if (playerManager.isLockedInAnimation)
            return;

        Vector3 targetDirection;

        targetDirection = movementDirection + cameraObject.right * inputManager.horizontalMovement;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;        

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding(){
       
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeighOffset;
        Debug.DrawRay(rayCastOrigin, Vector3.down*groundRaycast, Color.red);

        if(Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out RaycastHit hit, 10, groundLayer)){
            
            if(hit.distance <= groundRaycast)
            {
                if (playerManager.isLockedInAnimation && isFalling)
                {
                    animatorManager.PlayTargetAnimation("Land", true);
                    isFalling = false;
                }

                inAirTimer = 0;
                isGrounded = true;
            }
            else
            {
                if (!playerManager.isLockedInAnimation && hit.transform.gameObject.tag!="Stairs")
                {
                    animatorManager.PlayTargetAnimation("Fall", true);
                    isFalling = true;
                }

                if (hit.transform.gameObject.tag == "Stairs")
                    fallingVelocity = fallingVelocity2 + 10000;   
                else
                    fallingVelocity = fallingVelocity2;
                     
                inAirTimer = inAirTimer + Time.deltaTime;
                playerRb.AddForce(transform.forward * leapingVelocity);
                playerRb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
                isGrounded = false;
            }
        }

    }

    public void HandleJumping(){

        if(isGrounded){
            animatorManager.SetAnimatorBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);


            movementDirection = cameraObject.forward * inputManager.verticalMovement;
            movementDirection = movementDirection + cameraObject.right * inputManager.horizontalMovement;
            movementDirection.Normalize();
            movementDirection.y = 0;

            if (!isSprinting)
                movementDirection = movementDirection * runningSpeed;
            else
                movementDirection = movementDirection * sprintingSpeed;
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * runJumpForce);
            Vector3 playerVelocity = movementDirection;
            playerVelocity.y = jumpingVelocity;
            playerRb.velocity = playerVelocity;
        }
    }

    private void StepClimb()
    {
        RaycastHit hitLower;
        RaycastHit hitLower45;
        RaycastHit hitLowerMinus45;

        #region Debug Rays
            Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(Vector3.forward)*0.1f, Color.green);
            Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(1.5f, 0, 1) *0.1f, Color.green);
            Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(-1.5f, 0, 1) *0.1f, Color.green);
            Debug.DrawRay(highStep.transform.position, transform.TransformDirection(Vector3.forward)*0.2f, Color.green);
            Debug.DrawRay(highStep.transform.position, transform.TransformDirection(1.5f, 0, 1) *0.2f, Color.green);
            Debug.DrawRay(highStep.transform.position, transform.TransformDirection(-1.5f, 0, 1) *0.2f, Color.green);
        #endregion

        if(inputManager.verticalMovement != 0 || inputManager.horizontalMovement!=0)
        {
            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, lowStepRaycast))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }

            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, lowStepRaycast))
            {
                RaycastHit hitUpper45;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }

            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, lowStepRaycast))
            {
                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }
        }
        

    }
    private void UpdateFlags()
    {
        isSprinting = inputManager.isSprinting;
    }
}
