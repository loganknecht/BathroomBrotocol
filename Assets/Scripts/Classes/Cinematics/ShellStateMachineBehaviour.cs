using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellStateMachineBehaviour : StateMachineBehaviour {

    public string stateName = "";
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AnimatorHelper animatorHelper = animator.gameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            AnimatorHelper.StateEvent stateEvent = animatorHelper.GetOnStateEnter(stateName);
            if(stateEvent != null) {
                stateEvent();
            }
        }
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AnimatorHelper animatorHelper = animator.gameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            AnimatorHelper.StateEvent updateStateEvent = animatorHelper.GetOnStateUpdate(stateName);
            if(updateStateEvent != null) {
                updateStateEvent();
            }
            
            //------------------------------------------------------------------
            // OnAnimationFinish
            //------------------------------------------------------------------
            // Checks to see if animation finished
            if(stateInfo.normalizedTime >= 0.99) {
                // Debug.Log(stateName + ": Animation finished");
                AnimatorHelper.StateEvent animationFinishStateEvent = animatorHelper.GetOnAnimationFinish(stateName);
                if(animationFinishStateEvent != null) {
                    // Debug.Log("Performing animation: " + stateName + " finish event");
                    animationFinishStateEvent();
                    if(!animatorHelper.ShouldAnimationFinishEventLoop(stateName)) {
                        // Debug.Log("only performing once!");
                        animatorHelper.SetOnAnimationFinish(stateName, null);
                        if(animatorHelper.GetDestroyOnFinish()) {
                            Destroy(animator.gameObject);
                        }
                    }
                }
            }
        }
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AnimatorHelper animatorHelper = animator.gameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            AnimatorHelper.StateEvent stateEvent = animatorHelper.GetOnStateExit(stateName);
            if(stateEvent != null) {
                stateEvent();
            }
        }
    }
    
    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AnimatorHelper animatorHelper = animator.gameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            AnimatorHelper.StateEvent stateEvent = animatorHelper.GetOnStateMove(stateName);
            if(stateEvent != null) {
                stateEvent();
            }
        }
    }
    
    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AnimatorHelper animatorHelper = animator.gameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            AnimatorHelper.StateEvent stateEvent = animatorHelper.GetOnStateIK(stateName);
            if(stateEvent != null) {
                stateEvent();
            }
        }
    }
    
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    }
}

