using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    bool inRange;
    public ChaseState chaseState;
    public Transform enemy;
    public AnimatorManager animatorManager;

    public override State RunCurrentState()
    {
        inRange = ChaseState.CheckIfInRange(player, enemy, animator);

        if (!inRange)
        {
            animator.SetBool("isAttacking", false);
            return chaseState;
        }
        else
        {
            HandleAttacking();
            return this;
        }
            
    }

    private void HandleAttacking()
    {
        if (!animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", true);
        }
        
    }
}
