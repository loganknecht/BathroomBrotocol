using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorHelper : MonoBehaviour {
    public delegate void OnStateEvent();
    public Dictionary<string, OnStateEvent> onStateEnter = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateUpdate = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateExit = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateMove = new Dictionary<string, OnStateEvent>();
    public Dictionary<string, OnStateEvent> onStateIK = new Dictionary<string, OnStateEvent>();
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
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
