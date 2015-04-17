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
    public delegate void OnStateEvent();
    public Dictionary<string, OnStateEvent> onStateEnter = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateUpdate = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateExit = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateMove = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateIK = new Dictionary<string, OnStateEvent>();
    //--------------------------------------------------------------------------
    public Dictionary<string, OnStateEvent> onAnimationFinish = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, bool> loopAnimationFinishEvent = new Dictionary<string, bool>();
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    public void SetOnAnimationFinish(string stateName, OnStateEvent stateEventFunction, bool loopAnimationEvent = false) {
        if(stateEventFunction != null) {
            onAnimationFinish[stateName] = new OnStateEvent(stateEventFunction);
        }
        else {
            onAnimationFinish[stateName] = null;
        }
        SetLoopAnimationFinishEvent(stateName, loopAnimationEvent);
    }
    public OnStateEvent GetOnAnimationFinish(string stateName) {
        OnStateEvent returnEvent = null;
        onAnimationFinish.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnAnimationFinishDictionary() {
        return onAnimationFinish;
    }
    public void SetLoopAnimationFinishEvent(string stateName, bool loopAnimationEvent) {
        loopAnimationFinishEvent[stateName] = loopAnimationEvent;
    }
    public bool ShouldAnimationFinishEventLoop(string stateName) {
        bool shouldAnimationFinishEventLoop = false;
        loopAnimationFinishEvent.TryGetValue(stateName, out shouldAnimationFinishEventLoop);
        return shouldAnimationFinishEventLoop;
    }
    //--------------------------------------------------------------------------
    public void SetOnStateEnter(string stateName, OnStateEvent stateEventFunction) {
        onStateEnter[stateName] = new OnStateEvent(stateEventFunction);
    }
    public OnStateEvent GetOnStateEnter(string stateName) {
        OnStateEvent returnEvent = null;
        onStateEnter.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnStateEnterDictionary() {
        return onStateEnter;
    }
    
    public void SetOnStateUpdate(string stateName, OnStateEvent stateEventFunction) {
        onStateUpdate[stateName] = new OnStateEvent(stateEventFunction);
    }
    public OnStateEvent GetOnStateUpdate(string stateName) {
        OnStateEvent returnEvent = null;
        onStateUpdate.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnStateUpdateDictionary() {
        return onStateUpdate;
    }
    
    public void SetOnStateExit(string stateName, OnStateEvent stateEventFunction) {
        onStateExit[stateName] = new OnStateEvent(stateEventFunction);
    }
    public OnStateEvent GetOnStateExit(string stateName) {
        OnStateEvent returnEvent = null;
        onStateExit.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnStateExitDictionary() {
        return onStateExit;
    }
    
    public void SetOnStateMove(string stateName, OnStateEvent stateEventFunction) {
        onStateMove[stateName] = new OnStateEvent(stateEventFunction);
    }
    public OnStateEvent GetOnStateMove(string stateName) {
        OnStateEvent returnEvent = null;
        onStateMove.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnStateMoveDictionary() {
        return onStateMove;
    }
    
    public void SetOnStateIK(string stateName, OnStateEvent stateEventFunction) {
        onStateIK[stateName] = new OnStateEvent(stateEventFunction);
    }
    public OnStateEvent GetOnStateIK(string stateName) {
        OnStateEvent returnEvent = null;
        onStateIK.TryGetValue(stateName, out returnEvent);
        return returnEvent;
    }
    public Dictionary<string, OnStateEvent> GetOnStateIKDictionary() {
        return onStateIK;
    }
}
