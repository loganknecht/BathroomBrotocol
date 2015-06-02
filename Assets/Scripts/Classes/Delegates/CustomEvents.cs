using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomEvents<T> {
    public List<CustomEvent<T>> allDelegateEvents;
    
    public CustomEvents() {
        allDelegateEvents = new List<CustomEvent<T>>();
    }
    
    public static CustomEvents<T> Create() {
        CustomEvents<T> newCustomEvents = new CustomEvents<T>();
        return newCustomEvents;
    }
    
    public void Add(System.Action newOnDelegateEvent, bool loop = false) {
        CustomEvent<T> newEvent = CustomEvent<T>.Create()
                                  .SetEvent(newOnDelegateEvent)
                                  .SetLoop(loop);
        Add(newEvent);
    }
    
    public void Add(CustomEvent<T> newDelegateEvent) {
        allDelegateEvents.Add(newDelegateEvent);
    }
    
    public void Execute() {
        List<CustomEvent<T>> loopingDelegates = new List<CustomEvent<T>>();
        foreach(CustomEvent<T> delegateEvent in allDelegateEvents) {
            delegateEvent.Execute();
            if(delegateEvent.ShouldLoop()) {
                loopingDelegates.Add(delegateEvent);
            }
        }
        allDelegateEvents = loopingDelegates;
    }
}
