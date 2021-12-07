using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    ControllerClimbing controllerClimbing;
    PlayerManager playerManager;
    AnimatorManager animatorManager;

    Transform cameraObject;
    Rigidbody playerRb;
    Vector3 movementDirection;
    
    #region Locomotion variables
    [Header("Locomotion Stats")]
    public float sprintingSpeed;
    public float runningSpeed;
    public float rotationSpeed;
    #endregion

    #region Falling Stats
    [Header("Falling Stats")]
    public float leapingVelocity;
    public float fallingVelocity;
    private float fallingVelocity2;
    public float rayCastHeighOffset;
    public LayerMask groundLayer;
    float groundRaycast = 0.6f;
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
        controllerClimbing = GetComponent<ControllerClimbing>();

        cameraObject = Camera.main.transform;
        fallingVelocity2 = fallingVelocity;
    }
    public void HandleMovement()
    {
        UpdateFlags();
        HandleFallingAndLanding();


        if (!controllerClimbing.isOnWall)
        {
            Locomotion();
            Rotation();
        }
    }
    private void Locomotion()
    {
        float sk;

        if (playerManager.isLockedInAnimation || controllerClimbing.isOnWall)
            return;

        Vector3 normalizedCamera = Vector3.ProjectOnPlane(cameraObject.forward, Vector3.up);
        movementDirection = normalizedCamera * inputManager.verticalInput;
        movementDirection = movementDirection + cameraObject.right * inputManager.horizontalInput;
        movementDirection.Normalize();

        if(!isSprinting)
            movementDirection = movementDirection * runningSpeed;
        else
            movementDirection = movementDirection * sprintingSpeed;

        sk = playerRb.velocity.y;
        Vector3 movementVelocity = movementDirection;
        movementVelocity.y = sk;
        playerRb.velocity = movementVelocity;
        
       
    }
    private void Rotation()
    {
        if (playerManager.isLockedInAnimation || controllerClimbing.isOnWall)
            return;

        Vector3 targetDirection;

        targetDirection = movementDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;        

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
        playerRb.angularVelocity = Vector3.zero;
    }
    private void HandleFallingAndLanding(){
       
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeighOffset;
        Debug.DrawRay(rayCastOrigin, Vector3.down*groundRaycast, Color.red);

        if(Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out RaycastHit hit, 10, groundLayer)){

            if (hit.distance <= groundRaycast)
            {
                if (playerManager.isLockedInAnimation && isFalling)
                {
                    animatorManager.PlayTargetAnimation("Land", true);
                    movementDirection.y = 0;
                    isFalling = false;
                }

                inAirTimer = 0;
                isGrounded = true;
            }
            else if(!controllerClimbing.isOnWall)
            {
                if (!playerManager.isLockedInAnimation && hit.transform.gameObject.tag != "Stairs" && !isJumping)
                {
                    animatorManager.PlayTargetAnimation("Fall", true);
                    isFalling = true;
                }

                if (hit.transform.gameObject.tag == "Stairs" && !isJumping)
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

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * runJumpForce);
            Vector3 playerVelocity = movementDirection;
            playerVelocity.y = jumpingVelocity;
            playerRb.velocity = playerVelocity;
            Debug.Log(playerVelocity);
        }
    }
 
    private void UpdateFlags()
    {
        isSprinting = inputManager.isSprinting;
    }
}
