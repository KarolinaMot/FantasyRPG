using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTargetAnimation(string targetAnimation, bool isLocked)
    {
        animator.SetBool("isLocked", isLocked);

        animator.CrossFade(targetAnimation, 0.2f);
    }
}
