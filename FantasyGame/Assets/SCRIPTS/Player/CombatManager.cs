using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Animator playerAnimator;
    private StarterAssetsInputs inputs;
    private int attackNum = -1;
    // Start is called before the first frame update
    void Awake()
    {
        playerAnimator = GetComponent<Animator>(); 
        inputs = GetComponent<StarterAssetsInputs>();
        playerAnimator.SetInteger("AttackNum", attackNum);
    }

    // Update is called once per frame
    void Update()
    {
        AttackAnimation();  
    }

    private void AttackAnimation()
    {
        if (!inputs.attack)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            {
                attackNum = -1;
                playerAnimator.SetInteger("AttackNum", attackNum);
            }
            return;
        }
           

        switch (attackNum)
        {
            case -1:
                attackNum++;
                playerAnimator.SetInteger("AttackNum", attackNum);
                inputs.attack = false;
                break;
            case 0:
            case 1:
            case 2:
                if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
                {
                    attackNum++;
                    playerAnimator.SetInteger("AttackNum", attackNum);
                }
                else
                {
                    attackNum = -1;
                    playerAnimator.SetInteger("AttackNum", attackNum);
                }

                inputs.attack = false;
                return;
            case 3:
                if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
                {
                    attackNum=0;
                    playerAnimator.SetInteger("AttackNum", attackNum);
                }
                else
                {
                    attackNum = -1;
                    playerAnimator.SetInteger("AttackNum", attackNum);
                }
                    inputs.attack = false;
                return;
        }
    }
}
