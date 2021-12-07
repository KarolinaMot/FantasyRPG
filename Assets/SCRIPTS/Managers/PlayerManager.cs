using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ControllerLocomotion controllerLocomotion;
    ControllerClimbing controllerClimbing;
    ControllerCombat controllerCombat;
    InputManager inputManager;
    AnimatorManager animatorManager;

    public bool isLockedInAnimation;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        controllerLocomotion = GetComponentInParent<ControllerLocomotion>();
        controllerClimbing = GetComponentInParent<ControllerClimbing>();
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
        animatorManager.SetAnimatorBool("isClimbing", controllerClimbing.isClimbing);
    }
    private void FixedUpdate()
    {
        controllerLocomotion.HandleMovement();
        controllerClimbing.HandleClimbing();
    }
}
