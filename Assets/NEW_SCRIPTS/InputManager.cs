using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    ControllerLocomotion controllerLocomotion;
    AnimatorManager animatorManager;
    private Vector2 movementInput;

    [HideInInspector]
    public float verticalMovement;
    [HideInInspector]
    public float horizontalMovement;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        controllerLocomotion = GetComponentInParent<ControllerLocomotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            verticalMovement = 0;
            horizontalMovement = 0;

            playerControls.PlayerMovement.Movement.performed += _ => movementInput = _.ReadValue<Vector2>();
            playerControls.PlayerActions.Jump.performed += _ => HandleJumpInput();
            playerControls.PlayerActions.Sprint.performed += _ => animatorManager.anim.SetBool("isSprinting", true);
            playerControls.PlayerActions.Sprint.canceled += _ => animatorManager.anim.SetBool("isSprinting", false);
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
    }

    private void HandleMovementInput()
    {
        verticalMovement = movementInput.y;
        horizontalMovement = movementInput.x;

        animatorManager.horizontal = horizontalMovement;
        animatorManager.vertical = verticalMovement;
    }

    private void HandleJumpInput()
    {
        if (verticalMovement == 0 && horizontalMovement == 0)
            controllerLocomotion.HandleJump(controllerLocomotion.idleJumpForce, 1);
        else
            controllerLocomotion.HandleJump(controllerLocomotion.runJumpForce, 2);
    }
}
