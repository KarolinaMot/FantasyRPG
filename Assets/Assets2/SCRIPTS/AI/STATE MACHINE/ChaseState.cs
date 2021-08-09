using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public AttackState attackState;
    bool isInRange;
    public NavMeshAgent navMeshAgent;
    public Transform enemy;

    public override State RunCurrentState()
    {
        HandleChasing();
        isInRange = CheckIfInRange(player, enemy, animator);

        if (isInRange)
        {
            return attackState;
            
        }
        else
        {
            return this;
        }
    }

    private void HandleChasing()
    {
        navMeshAgent.SetDestination(player.position);
    }


    public static bool CheckIfInRange(Transform player, Transform enemy, Animator animator)
    {
        if (Vector3.Distance(player.position, enemy.position) <= 1.6)
        {
            animator.SetFloat("Blend", 0);
            return true;
        }
        else
        {
            animator.SetFloat("Blend", 1);
            return false;
        }
            
    }
 
}
