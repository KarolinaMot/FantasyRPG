using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //this code gets input information

    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    CombatManager combatManager;

    public Vector2 movementInput;

    public float moveAmount;
    public float verticalImput;
    public float horizontalImput;

    public bool sprintInput;
    public bool jumpInput;
    public bool attackInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        combatManager = GetComponent<CombatManager>();
    }
    private void OnEnable()
    {
        if (playerControls == null) {
           playerControls = new PlayerControls();

           playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

           playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
           playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
           playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
           playerControls.PlayerActions.Attack.performed += i => attackInput = true;
        }

        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleAttackInput();

    }

    private void HandleMovementInput()
    {
        verticalImput = movementInput.y;
        horizontalImput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalImput) + Mathf.Abs(verticalImput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
        

    }

    private void HandleSprintingInput()
    {
        if (sprintInput && moveAmount != 0)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
       if (jumpInput)
       {
            jumpInput = false;
            playerLocomotion.HandleJump();
            
        }
    }

    private void HandleAttackInput()
    {
        if (attackInput)
        {
            attackInput = false;
            combatManager.handleAttack();
        }
    }
}
