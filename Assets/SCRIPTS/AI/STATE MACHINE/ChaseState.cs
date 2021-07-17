using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    public AttackState attackState;
    public bool isInRange;
    public NavMeshAgent navMeshAgent;
    public Transform player;

    public override State RunCurrentState()
    {
        HandleChasing();

        if (isInRange)
        {
            return attackState;
        }
        else
             return this;
    }

    private void HandleChasing()
    {
        navMeshAgent.SetDestination(player.position);
    }

 
}
