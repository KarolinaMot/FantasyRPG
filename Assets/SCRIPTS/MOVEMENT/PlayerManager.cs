using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //This code handles all inputs and movement
    Animator animator;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    CombatManager combatManager;


    public bool isLocked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        combatManager = GetComponent<CombatManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isLocked = animator.GetBool("isLocked");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        combatManager.isAttacking = animator.GetBool("isAttacking");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);

    }
}
