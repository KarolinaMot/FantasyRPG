using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public bool canSeeThePlayer;
    public ChaseState chaseState;
    RaycastHit hit;
    public float maxRange = 14;


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
        //if (other.CompareTag("Player"))
        //{
        //    Debug.Log(other.gameObject.name);
        //}
        
        if (other.CompareTag("Player"))
        {
            Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange);

            if (!hit.collider.CompareTag("Building"))
            {
                canSeeThePlayer = true;
                Debug.Log("Player detected");

            }
            else
            {
                Debug.Log("Player not detected");
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
