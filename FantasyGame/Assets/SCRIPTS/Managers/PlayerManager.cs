using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerLocomotion controllerLocomotion;
    PlayerClimbing controllerClimbing;
    ControllerCombat controllerCombat;
    InputManager inputManager;
    AnimatorManager animatorManager;

    public bool isLockedInAnimation;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        controllerLocomotion = GetComponentInParent<PlayerLocomotion>();
        controllerClimbing = GetComponentInParent<PlayerClimbing>();
        controllerCombat = GetComponentInParent<ControllerCombat>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    void Update()
    {
        inputManager.HandleAllInput();
        controllerCombat.HandleAttack();
    }

    private void LateUpdate(){
        isLockedInAnimation = animatorManager.GetAnimatorBool("isInteracting");
        controllerLocomotion.isJumping =  animatorManager.GetAnimatorBool("isJumping");
        animatorManager.SetAnimatorBool("isGrounded", controllerLocomotion.isGrounded);
    }
    private void FixedUpdate()
    {
        controllerLocomotion.HandleMovement();
        controllerClimbing.HandleClimbing();
    }
}
