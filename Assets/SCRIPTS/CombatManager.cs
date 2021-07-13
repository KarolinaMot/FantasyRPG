using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public bool isAttacking;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
    }
    public void handleAttack()
    {
        if(playerLocomotion.isGrounded && !playerLocomotion.isJumping)
        {
            animatorManager.animator.SetBool("isAttacking", true);
            animatorManager.PlayTargetAnimation("Attack", false);
        }
    }
}
