using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    PlayerAnimatorManager animatorManager;
    PlayerManager playerManager;

    public bool isAttacking;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
    }
    public void handleAttack()
    {
        if(playerLocomotion.isGrounded && !playerLocomotion.isJumping && !isAttacking)
        {
            animatorManager.animator.SetBool("isAttacking", true);
            animatorManager.PlayTargetAnimation("Attack", false);
        }
    }
}
