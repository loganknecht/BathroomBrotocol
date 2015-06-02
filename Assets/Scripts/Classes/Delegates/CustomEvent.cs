using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomEvent<T> {
    //--------------------------------------------------------------------------
    public bool loop = false;
    
    public System.Action customDelegate;
    //--------------------------------------------------------------------------
    public static CustomEvent<T> Create() {
        CustomEvent<T> newDelegateEvent = new CustomEvent<T>();
        return newDelegateEvent;
    }
    //--------------------------------------------------------------------------
    public CustomEvent<T> SetLoop(bool newLoopState) {
        loop = newLoopState;
        return this;
    }
    public bool ShouldLoop() {
        return loop;
    }
    //--------------------------------------------------------------------------
    public CustomEvent<T> SetEvent(System.Action newDelegateEvent) {
        customDelegate = newDelegateEvent;
        return this;
    }
    public System.Action GetEvent() {
        return customDelegate;
    }
    //--------------------------------------------------------------------------
    public void Execute() {
        customDelegate();
    }
}