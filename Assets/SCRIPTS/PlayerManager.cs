using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ControllerLocomotion controllerLocomotion;
    InputManager inputManager;
    AnimatorManager animatorManager;

    public bool isLockedInAnimation;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        controllerLocomotion = GetComponentInParent<ControllerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    void Update()
    {
        inputManager.HandleAllInput();
    }

    private void LateUpdate(){
        isLockedInAnimation = animatorManager.GetAnimatorBool("isInteracting");
        controllerLocomotion.isJumping =  animatorManager.GetAnimatorBool("isJumping");
        animatorManager.SetAnimatorBool("isGrounded", controllerLocomotion.isGrounded);
    }
    private void FixedUpdate()
    {
        controllerLocomotion.HandleAllMovement();        
    }
}
