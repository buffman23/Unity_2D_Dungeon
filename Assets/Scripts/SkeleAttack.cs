using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleAttack : StateMachineBehaviour
{
    bool cooldown = false;
    Skeleton skele;
    Vector3 knockbackTemplate = new Vector3(10, 5, 0);
    Vector3 knockback;

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
            knockback.x = knockbackTemplate.x * (skele.transform.localScale.x > 0 ? 1 : -1);
            Vector3 playerVelocity = CharacterController.instance.GetComponent<Rigidbody2D>().velocity;
            knockback.y = Mathf.Abs(playerVelocity.y) < .1f ? knockbackTemplate.y : 0f;
            //knockback.y = Mathf.Max(knockbackTemplate.y - CharacterController.instance.GetComponent<Rigidbody2D>().velocity.y);

            CharacterController.instance.Knockback(knockback);
            CharacterController.instance.Damage(Skeleton.attackDamage);
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
