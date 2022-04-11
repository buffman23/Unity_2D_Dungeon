using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleAttack : StateMachineBehaviour
{
    bool cooldown = false;
    Skeleton skele;
    Vector3 knockback = new Vector3(20, 10, 0);
    Vector3 knockforward = new Vector3(20, 0, 0);

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cooldown = false;
        skele = animator.gameObject.GetComponent<Skeleton>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime > .5 && !cooldown && animator.GetBool("Attack"))
        {
            knockback.x = Mathf.Abs(knockback.x) * (skele.transform.localScale.x > 0 ? 1 : -1);
            CharacterController.instance.Knockback(knockback);
            //skele.Knockback(-knockforward);
            cooldown = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
