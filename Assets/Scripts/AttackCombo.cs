using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCombo : StateMachineBehaviour
{
    bool canHitCombo;
    bool audioPlayed;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canHitCombo = true;
        audioPlayed = false;
        animator.SetBool("HitCombo", false);

        if (stateInfo.IsName("NoAttack"))
        {
            CharacterController.instance.setAttack(Attack.None);
        }
        else if (stateInfo.IsName("SlashUp"))
        {
            CharacterController.instance.setAttack(Attack.SlashUp);
        }
        else if (stateInfo.IsName("SlashDown"))
        {
            CharacterController.instance.setAttack(Attack.SlashDown);
        }
        else if (stateInfo.IsName("Stab"))
        {
            CharacterController.instance.setAttack(Attack.Stab);
        }

        CharacterController.instance.allowAttack = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!audioPlayed)
        {
            if (stateInfo.normalizedTime > .1 && stateInfo.IsName("SlashUp"))
            {
                SoundController.instance.PlayAudio("swing2");
                audioPlayed = true;
            }
            else if (stateInfo.normalizedTime > .3 && stateInfo.IsName("SlashDown"))
            {
                SoundController.instance.PlayAudio("swing");
                audioPlayed = true;
            }
            else if (stateInfo.normalizedTime > .2 && stateInfo.IsName("Stab"))
            {
                SoundController.instance.PlayAudio("swing3");
                audioPlayed = true;
            }
        }

        if (canHitCombo)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (stateInfo.normalizedTime >= .40 && CharacterController.instance.HitSkeleton())
                {
                    animator.SetBool("HitCombo", true);
                }
                canHitCombo = false;
            }

            if (stateInfo.IsName("NoAttack"))
                CharacterController.instance.allowAttack = false;
            else if (stateInfo.IsName("SlashUp"))
                CharacterController.instance.allowAttack = (stateInfo.normalizedTime * stateInfo.length > .15f);
            else if (stateInfo.IsName("SlashDown"))
                CharacterController.instance.allowAttack = (stateInfo.normalizedTime * stateInfo.length > .6f && stateInfo.normalizedTime < .45f);
            else if (stateInfo.IsName("Stab"))
                CharacterController.instance.allowAttack = (stateInfo.normalizedTime * stateInfo.length > .3f);


        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

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
