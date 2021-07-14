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
    public bool hasWeapon;
    public Transform weapon;
    public Transform hand;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        combatManager = GetComponent<CombatManager>();

        weapon = LookForWeapon(hand);
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

    public Transform LookForWeapon( Transform hand)
    {
        foreach (Transform child in hand)
        {
            if (child.gameObject.CompareTag("Weapon"))
            {
                hasWeapon = true;
                return child;
            }
        }
        return null;
    }
}
