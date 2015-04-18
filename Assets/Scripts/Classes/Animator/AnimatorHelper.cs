using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Aaaaaalrighty. This is something that hopefully you'll have to look at very
/// This is a manager/helper class that serves to facilitate adding events
/// dynamically. The reason this is created is because there isn't an easy way
/// within Unity itself to hook into these events.
///
/// Basically this class gets attached to the same class the animator is attached
/// to, then when the animator has the ShellStateMachineBehaviour is attached to
/// each state in the animator, and all you have to do is set the animation name
/// equal to the state machine names.
/// Then, for the animation events will be fired with respect to their occurrence
///
/// The basic events are covered in regards to StateMachineBehaviour functions.
///
/// However addition functions have been created in order to facilitate one off
/// functionality. Things like onAnimationFinish have been added. On top of that
/// there is another dictionary that tracks if the function added should be
/// be performed more than once.
///
/// This isn't super robust, but compared to Unity's default bullshit this will
/// at least get you going. If you want to add multiple events per an animation
/// state, that isn't currrently supported.
///
/// TODO: Add functionality to support multiple functions being called per an
///       event
/// </summary>
public class AnimatorHelper : MonoBehaviour {
    public delegate void StateEvent();
    public Dictionary<string, StateEvent> onStateEnter = new Dictionary<string, StateEvent>();
    public Dictionary<string, StateEvent> onStateUpdate = new Dictionary<string, StateEvent>();
    public Dictionary<string, StateEvent> onStateExit = new Dictionary<string, StateEvent>();
    public Dictionary<string, StateEvent> onStateMove = new Dictionary<string, StateEvent>();
    public Dictionary<string, StateEvent> onStateIK = new Dictionary<string, StateEvent>();
    //--------------------------------------------------------------------------
    public Dictionary<string, StateEvent> onAnimationFinish = new Dictionary<string, StateEvent>();
    public Dictionary<string, bool> loopAnimationFinishEvent = new Dictionary<string, bool>();
    public bool destroyOnFinish = false;
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    //--------------------------------------------------------------------------
    // Simple Retrieval
    //--------------------------------------------------------------------------
    public Dictionary<string, StateEvent> GetOnStateEnterDictionary() {
        return onStateEnter;
    }
    public Dictionary<string, StateEvent> GetOnStateUpdateDictionary() {
        return onStateUpdate;
    }
    public Dictionary<string, StateEvent> GetOnStateExitDictionary() {
        return onStateExit;
    }
    public Dictionary<string, StateEvent> GetOnStateMoveDictionary() {
        return onStateMove;
    }
    public Dictionary<string, StateEvent> GetOnStateIKDictionary() {
        return onStateIK;
    }
    public Dictionary<string, StateEvent> GetOnAnimationFinishDictionary() {
        return onAnimationFinish;
    }
    
    //--------------------------------------------------------------------------
    // Custom Animation Methods
    //--------------------------------------------------------------------------
    public void SetOnAnimationFinish(string stateName, StateEvent stateEventFunction, bool destroyOnFinish = false, bool loopAnimationEvent = false) {
        if(stateEventFunction != null) {
            onAnimationFinish[stateName] = new StateEvent(stateEventFunction);
        }
        else {
            onAnimationFinish[stateName] = null;
        }
        SetLoopAnimationFinishEvent(stateName, loopAnimationEvent);
    }
    public StateEvent GetOnAnimationFinish(string stateName) {
        StateEvent returnEvent = null;
        onAnimationFinish.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    //----------------------------
    public void SetDestroyOnFinish(bool newDestroyOnFinish) {
        destroyOnFinish = newDestroyOnFinish;
    }
    public bool GetDestroyOnFinish() {
        return destroyOnFinish;
    }
    //----------------------------
    /// <summary>
    /// Sets the state for animation that it should loop
    /// </summary>
    public void SetLoopAnimationFinishEvent(string stateName, bool loopAnimationEvent) {
        loopAnimationFinishEvent[stateName] = loopAnimationEvent;
    }
    /// <summary>
    /// Returns a bool indicating if the animation name provided should loop
    /// </summary>
    public bool ShouldAnimationFinishEventLoop(string stateName) {
        bool shouldAnimationFinishEventLoop = false;
        loopAnimationFinishEvent.TryGetValue(stateName, out shouldAnimationFinishEventLoop);
        return shouldAnimationFinishEventLoop;
    }
    //--------------------------------------------------------------------------
    // Default Unity State Machine Hook Ins
    //--------------------------------------------------------------------------
    public void SetOnStateEnter(string stateName, StateEvent stateEventFunction) {
        onStateEnter[stateName] = new StateEvent(stateEventFunction);
    }
    public StateEvent GetOnStateEnter(string stateName) {
        StateEvent returnEvent = null;
        onStateEnter.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    //----------------------------
    public void SetOnStateUpdate(string stateName, StateEvent stateEventFunction) {
        onStateUpdate[stateName] = new StateEvent(stateEventFunction);
    }
    public StateEvent GetOnStateUpdate(string stateName) {
        StateEvent returnEvent = null;
        onStateUpdate.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    //----------------------------
    public void SetOnStateExit(string stateName, StateEvent stateEventFunction) {
        onStateExit[stateName] = new StateEvent(stateEventFunction);
    }
    public StateEvent GetOnStateExit(string stateName) {
        StateEvent returnEvent = null;
        onStateExit.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    //----------------------------
    public void SetOnStateMove(string stateName, StateEvent stateEventFunction) {
        onStateMove[stateName] = new StateEvent(stateEventFunction);
    }
    public StateEvent GetOnStateMove(string stateName) {
        StateEvent returnEvent = null;
        onStateMove.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    //----------------------------
    public void SetOnStateIK(string stateName, StateEvent stateEventFunction) {
        onStateIK[stateName] = new StateEvent(stateEventFunction);
    }
    public StateEvent GetOnStateIK(string stateName) {
        StateEvent returnEvent = null;
        onStateIK.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
}
