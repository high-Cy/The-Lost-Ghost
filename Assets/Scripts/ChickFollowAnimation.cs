using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickFollowAnimation : MonoBehaviour
{
    private ChickFollow chickFollow;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        chickFollow = GetComponent<ChickFollow>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AnimatorPlaying("Base Layer.Jump W Root"))
        {
            if (chickFollow.SelfToTargetDist() > chickFollow.GetDist())
            {
                animator.SetBool("Run", true);
            }
            else if (ToJump())
            {
                animator.SetTrigger("Jump");
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }
    }

    private bool ToJump()
    {
        return Random.Range(0, 10) == 0;
    }

    private bool AnimatorPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    private bool AnimatorPlaying(string stateName)
    {
        return AnimatorPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
