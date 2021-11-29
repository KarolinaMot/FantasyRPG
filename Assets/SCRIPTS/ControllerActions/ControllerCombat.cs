using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    AnimatorManager animatorManager;

    [Header ("Attacking timer")]
    public bool isAttacking;
    public float attackTimer;
    public float timerMax;

    public void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
    }
    public void HandleAttack()
    {
        if (attackTimer >= timerMax)
        {
            isAttacking = false;
            attackTimer = 0;
        }
        if (isAttacking)
        {
            attackTimer++;
        }

        animatorManager.SetAnimatorBool("isAttacking", isAttacking);
    }
}
