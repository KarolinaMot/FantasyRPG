using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyLocomotion enemyLocomotion;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLocomotion = GetComponentInParent<EnemyLocomotion>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotion.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotion.enemyRigidBody.velocity = velocity;

    }
}
