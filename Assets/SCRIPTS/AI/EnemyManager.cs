using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isPerformingAction;
    EnemyLocomotion enemyLocomotion;

    //[Header("A.I. Settings")]
    

    private void Awake()
    {
        enemyLocomotion = GetComponent<EnemyLocomotion>();
    }

    private void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (enemyLocomotion.currentTarget != null)
        {
            enemyLocomotion.HandleMoveToTarget();
        }
    }
}
