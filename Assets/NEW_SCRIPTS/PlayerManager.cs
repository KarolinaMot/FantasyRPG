using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ControllerLocomotion controllerLocomotion;
    InputManager inputManager;
    AnimatorManager animatorManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        controllerLocomotion = GetComponentInParent<ControllerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    void Update()
    {
        inputManager.HandleAllInput();
        animatorManager.SetAllValues();
    }

    private void FixedUpdate()
    {
        controllerLocomotion.HandleAllMovement();        
    }
}
