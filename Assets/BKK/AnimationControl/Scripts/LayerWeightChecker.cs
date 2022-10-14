using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeightChecker : StateMachineBehaviour
{
    public float fadeSpeed = 5f;

    private Coroutine routine;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(routine != null) CoroutineHelper.StopCoroutine(routine);
        animator.SetLayerWeight(layerIndex,1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetLayerWeight(layerIndex,0);

        routine = CoroutineHelper.StartCoroutine(FadeLayer(animator, layerIndex));
    }

    private IEnumerator FadeLayer(Animator animator, int layerIndex)
    {
        var weight = animator.GetLayerWeight(layerIndex);
        
        do
        {
            animator.SetLayerWeight(layerIndex,weight -= Time.deltaTime * fadeSpeed);
            yield return null;
        } while (animator.GetLayerWeight(layerIndex) > 0);
    }
    
    

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
