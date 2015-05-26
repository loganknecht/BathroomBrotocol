using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--------------------------------------------------------------------------
// Delegate event types go here
//--------------------------------------------------------------------------
// public delegate void DelegateEvent();
// public delegate void DelegateEvent(int x, int y);

public class CustomEvent<T> {
    //--------------------------------------------------------------------------
    public bool loop = false;
    
    public System.Action customDelegate;
    
    //--------------------------------------------------------------------------
    public static CustomEvent<T> Create(System.Action newOnDelegateEvent, bool loop = false) {
        CustomEvent<T> newDelegateEvent = new CustomEvent<T>();
        newDelegateEvent.SetEvent(newOnDelegateEvent);
        newDelegateEvent.SetLoop(loop);
        return newDelegateEvent;
    }
    //--------------------------------------------------------------------------
    public void SetLoop(bool newLoopState) {
        loop = newLoopState;
    }
    public bool ShouldLoop() {
        return loop;
    }
    //--------------------------------------------------------------------------
    public void SetEvent(System.Action newDelegateEvent) {
        customDelegate = newDelegateEvent;
    }
    public System.Action GetEvent() {
        return customDelegate;
    }
    //--------------------------------------------------------------------------
    public void Execute() {
        customDelegate();
        // Execute(customDelegate);
        // customDelegate();
    }
    // public void Execute(DelegateEvent customDelegateFunction) {
    // customDelegateFunction();
    // new System.Func<T>();
    // customDelegate();
    // }
}