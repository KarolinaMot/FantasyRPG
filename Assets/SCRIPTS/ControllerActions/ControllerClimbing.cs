using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerClimbing : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRb;
    [HeaderAttribute("Walking up stairs")]
    public GameObject lowStep;
    public GameObject highStep;
    public GameObject handSpot;
    public float stepSmooth = 0.1f;

    [Header("Raycast lengths")]
    public float lowStepRaycast = 0.1f;
    public float highStepRaycast = 0.2f;
    public float handRaycast = 0.1f;
    public float headRaycast = 0.2f;

    public bool isClimbing = false;
    public bool isOnWall = false;
    public float climbingSpeedVertical;
    public float climbingSpeedHorizontal;

    void Awake()
    {
        inputManager = GetComponentInChildren<InputManager>();
        playerRb = GetComponent<Rigidbody>();
    }
    public void HandleClimbing()
    {
        StepClimb();
        ClimbingUpWalls();
    }

    private void StepClimb()
    {
        RaycastHit hitLower;
        RaycastHit hitLower45;
        RaycastHit hitLowerMinus45;

        #region Debug Rays
        Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(Vector3.forward) * lowStepRaycast, Color.green);
        Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(1.5f, 0, 1) * lowStepRaycast, Color.green);
        Debug.DrawRay(lowStep.transform.position, transform.TransformDirection(-1.5f, 0, 1) * lowStepRaycast, Color.green);
        Debug.DrawRay(highStep.transform.position, transform.TransformDirection(Vector3.forward) * highStepRaycast, Color.green);
        Debug.DrawRay(highStep.transform.position, transform.TransformDirection(1.5f, 0, 1) * highStepRaycast, Color.green);
        Debug.DrawRay(highStep.transform.position, transform.TransformDirection(-1.5f, 0, 1) * highStepRaycast, Color.green);
        Debug.DrawRay(handSpot.transform.position, transform.TransformDirection(Vector3.forward) * handRaycast, Color.green);
        #endregion

        if (inputManager.verticalInput != 0 || inputManager.horizontalInput != 0)
        {
            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, lowStepRaycast))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
                else
                {
                    Debug.Log("Hand ray thrown");
                    RaycastHit hitHand;
                    if (Physics.Raycast(handSpot.transform.position, transform.TransformDirection(Vector3.forward), out hitHand, handRaycast) 
                        && inputManager.verticalInput != 0 || Physics.Raycast(handSpot.transform.position, 
                        transform.TransformDirection(Vector3.forward), out hitHand, handRaycast) && inputManager.horizontalInput != 0)
                    {
                        isOnWall = true;
                        isClimbing = true;
                    }
                }
            }
            else
            {
                isOnWall = false;
                isClimbing = false;
            }
            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, lowStepRaycast))
            {
                RaycastHit hitUpper45;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }

            if (Physics.Raycast(lowStep.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, lowStepRaycast))
            {
                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(highStep.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, highStepRaycast))
                {
                    playerRb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }
        }
        else
        {
            isClimbing = false;
        }
            
    }

    private void ClimbingUpWalls()
    {
        if (isClimbing)
        {
            playerRb.position -= new Vector3(-inputManager.horizontalInput*climbingSpeedHorizontal, -inputManager.verticalInput*climbingSpeedVertical, 0f);
        }
        if (isOnWall)
            playerRb.useGravity = false;
        else
            playerRb.useGravity = true;
    }
}
