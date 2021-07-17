using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public bool canSeeThePlayer;
    public ChaseState chaseState;
    public Transform player;
    RaycastHit hit;
    public float maxRange = 14;
    public Animator animator;

    public override State RunCurrentState()
    {
        if (canSeeThePlayer)
        {
            return chaseState;
        }
        else
            return this;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player") && Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange))
        {
            if (hit.transform == player)
            {
                canSeeThePlayer = true;
                animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            else
            {
                canSeeThePlayer = false;
            }
        }
        else
            canSeeThePlayer = false;
    }

    private void OnTriggerExit(Collider other)
    {
        canSeeThePlayer = false;
    }
}
