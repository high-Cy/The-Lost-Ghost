using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyFollow enemyFollow;
    private Animator animator;
    private string animState;
    // Start is called before the first frame update
    void Start()
    {
        enemyFollow = GetComponent<EnemyFollow>();
        animator = GetComponentInChildren<Animator>();
        animState = "Base Layer.IdleNormal";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Enemy dont needa follow player, just play idle
        if (!enemyFollow.GetToFollow())
        {
            // random between idle and no anim
            if (EarlyReturn()) return;
            animator.Play("Base Layer.IdleNormal");
            return;
        }

        // change state when finish taunting
        if (!AnimatorPlaying("Base Layer.Taunt"))
        {
            if (enemyFollow.SelfToTargetDist() < 10)
            {
                animState = "Base Layer.Attack02";
            }
            else if (!enemyFollow.InRange() && ToTaunt())
            {
                animState = "Base Layer.Taunt";
            }
            else if (enemyFollow.InRange())
            {
                animState = "Base Layer.SenseSomethingST";
            }
            else
            {
                animState = "Base Layer.IdleNormal";

            }
            animator.Play(animState);
        }
    }

    private bool ToTaunt()
    {
        return Random.Range(0, 50) == 0;
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

    private bool EarlyReturn() {
        return Random.Range(0,9) == 1;
    }
}
