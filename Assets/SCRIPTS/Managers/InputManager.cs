using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    ControllerLocomotion controllerLocomotion;
    ControllerCombat controllerCombat;
    AnimatorManager animatorManager;
    private Vector2 movementInput;

    [HideInInspector]
    public float verticalMovement;
    [HideInInspector]
    public float horizontalMovement;
    private float moveAmount;
    public bool isSprinting = false;
    public bool isJumping;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        controllerLocomotion = GetComponentInParent<ControllerLocomotion>();
        controllerCombat = GetComponentInParent<ControllerCombat>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            verticalMovement = 0;
            horizontalMovement = 0;

            playerControls.PlayerMovement.Movement.performed += _ => movementInput = _.ReadValue<Vector2>();
            playerControls.PlayerActions.Sprint.performed += _ => animatorManager.SetAnimatorBool("isSprinting", true);
            playerControls.PlayerActions.Sprint.canceled += _ => animatorManager.SetAnimatorBool("isSprinting", false);
            playerControls.PlayerActions.Jump.performed += _ => isJumping = true;
            playerControls.PlayerActions.Attack.performed += _ => controllerCombat.isAttacking = true;
            playerControls.PlayerActions.Attack.performed += _ => controllerCombat.attackTimer = 0;

        }
        playerControls.Enable();   
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleJumpInput();
    }

    private void HandleMovementInput()
    {
        verticalMovement = movementInput.y;
        horizontalMovement = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));
        animatorManager.UpdateLocomotionValues(0, moveAmount);

        isSprinting = animatorManager.GetAnimatorBool("isSprinting");
    }

    private void HandleJumpInput(){
        if(isJumping){
            playerControls.PlayerActions.Jump.performed += _ => Debug.Log("jUMP INPUT");
            isJumping = false;
            controllerLocomotion.HandleJumping();
        }
    }
}
