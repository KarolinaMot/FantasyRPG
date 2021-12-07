using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator anim;
    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }
    public void SetAnimatorBool(string animatorBool, bool value)
    {
        anim.SetBool(animatorBool, value);
    }

    public bool GetAnimatorBool(string animatorBool){
        
        return anim.GetBool(animatorBool);
    }

    public void UpdateLocomotionValues(float horizontalMovement, float verticalMovement){

        float snappedHorizontal = 0;
        float snappedVertical = 0;

        #region Snapped variables
            if(horizontalMovement > 0 && horizontalMovement < 0.5f)
                snappedHorizontal = 0.33f;
            else if(horizontalMovement > 0 && horizontalMovement>0.5f)
                snappedHorizontal = 1;
            else if(horizontalMovement < 0 && horizontalMovement >-0.5f)
                snappedHorizontal = -0.33f;
            else if(horizontalMovement < 0 && horizontalMovement < -0.5f)
                snappedHorizontal = -1;
            
            if(verticalMovement > 0 && verticalMovement < 0.5f)
                snappedVertical = 0.33f;
            else if(verticalMovement > 0 && verticalMovement>0.5f)
                snappedVertical = 1;
            else if(verticalMovement < 0 && verticalMovement >-0.5f)
                snappedVertical = -0.33f;
            else if(verticalMovement < 0 && verticalMovement < -0.5f)
                snappedVertical = -1;
        #endregion

        anim.SetFloat("horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        anim.SetFloat("vertical", snappedVertical, 0.1f, Time.deltaTime);       
    }

    public void PlayTargetAnimation(string targetAnimation, bool isLockedInAnimation){

        anim.SetBool("isInteracting", isLockedInAnimation);
        anim.CrossFade(targetAnimation, 0.2f);
    }    
}
