using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    public float vertical;
    public float horizontal;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void SetAllValues()
    {
        SetMovingValues();
    }
   
    private void SetMovingValues()
    {
        if (vertical!=0 || horizontal!=0 && !anim.GetBool("isJumping"))
            anim.SetBool("onLocomotion", true);
        else
            anim.SetBool("onLocomotion", false);
    }
}
