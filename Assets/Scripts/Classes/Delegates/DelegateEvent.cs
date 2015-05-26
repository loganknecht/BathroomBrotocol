using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--------------------------------------------------------------------------
// Delegate event types go here
//--------------------------------------------------------------------------
public delegate void CustomEvent();

public class DelegateEvent<T> {
    //--------------------------------------------------------------------------
    public bool loop = false;
    
    public System.Func<T> customDelegate;
    
    //--------------------------------------------------------------------------
    public static DelegateEvent<T> Create(System.Func<T> newOnDelegateEvent, bool loop = false) {
        DelegateEvent<T> newDelegateEvent = new DelegateEvent<T>();
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
    public void SetEvent(System.Func<T> newDelegateEvent) {
        customDelegate = newDelegateEvent;
    }
    public System.Func<T> GetEvent() {
        return customDelegate;
    }
    //--------------------------------------------------------------------------
    public void Execute() {
        Execute(customDelegate);
    }
    public void Execute(System.Func<T> customDelegateFunction) {
        customDelegateFunction();
        // new System.Func<T>();
        // customDelegate();
    }
}

public class DelegateEvents<T> {
    public List<DelegateEvent<T>> allDelegateEvents;
    
    public DelegateEvents() {
        allDelegateEvents = new List<DelegateEvent<T>>();
    }
    
    public void Add(System.Func<T> newOnDelegateEvent, bool loop = false) {
        Add(DelegateEvent<T>.Create(newOnDelegateEvent, loop));
    }
    
    public void Add(DelegateEvent<T> newDelegateEvent) {
        allDelegateEvents.Add(newDelegateEvent);
    }
    
    public void Execute() {
        List<DelegateEvent<T>> loopingDelegates = new List<DelegateEvent<T>>();
        foreach(DelegateEvent<T> delegateEvent in allDelegateEvents) {
            delegateEvent.Execute();
            if(delegateEvent.ShouldLoop()) {
                loopingDelegates.Add(delegateEvent);
            }
        }
        allDelegateEvents = loopingDelegates;
    }
}
