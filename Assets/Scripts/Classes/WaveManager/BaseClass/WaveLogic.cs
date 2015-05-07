using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveLogic : MonoBehaviour, WaveLogicContract {
    public LinkedList<GameObject> waveStatesQueue = new LinkedList<GameObject>();
    public GameObject currentWaveStateGameObject = null;
    public float delayTimer = 0f;
    
    public bool waveLogicFinished = false;
    
    public virtual void Awake() {}
    
    public virtual void Start() {}
    
    public virtual void Update() {
        PerformDelayTimerLogic();
    }
    
    public virtual void Finished() {
        waveLogicFinished = true;
    }
    
    public virtual void Initialize() {
        //
    }
    
    public virtual void PerformDelayTimerLogic() {
        if(delayTimer > 0) {
            delayTimer -= Time.deltaTime;
        }
    }
    
    public virtual bool HasDelayFinished() {
        if(delayTimer <= 0) {
            return true;
        }
        else {
            return false;
        }
    }
    
    public virtual void Delay(float newTimer) {
        delayTimer = newTimer;
    }
    
    public virtual void Completed() {
        if(currentWaveStateGameObject != null) {
            WaveState currentWaveStateRef = currentWaveStateGameObject.GetComponent<WaveState>();
            currentWaveStateRef.Completed();
        }
        // Debug.Log("WaveLogic Completed");
    }
    
    public virtual bool HasFinished() {
        return false;
    }
    
    // Think of this as the main update loop
    public virtual void PerformWaveLogic() {
        // Debug.Log("Current Timer: " + delayTimer);
        
        if(HasDelayFinished()) {
            PerformLevelFailCheck();
            PerformLevelFinishCheck();
            
            //this assumes that the paused state is already managed by the wave manager
            if(currentWaveStateGameObject != null) {
                // Debug.Log("performing wave state logic");
                WaveState currentWaveStateRef = currentWaveStateGameObject.GetComponent<WaveState>();
                currentWaveStateRef.PerformWaveStateLogic();
                
                if(currentWaveStateRef.HasFinished()) {
                    if(waveStatesQueue.Count > 0) {
                        currentWaveStateGameObject = (GameObject)DequeueWaveState();
                    }
                    else {
                        currentWaveStateGameObject = null;
                    }
                }
            }
        }
    }
    
    public virtual void PerformLevelFailCheck() {
        if(AllBathroomObjectsBroken()) {
            LevelManager.Instance.TriggerFailedLevel();
        }
    }
    
    public virtual void PerformLevelFinishCheck() {
        if(waveLogicFinished) {
            LevelManager.Instance.TriggerFinishedLevel();
        }
    }
    
    public bool AllBathroomObjectsBroken() {
        bool foundBathroomObjectThatIsNotBroken = false;
        foreach(GameObject gameObj in BathroomObjectManager.Instance.allBathroomObjects) {
            BathroomObject bathObjRef = gameObj.GetComponent<BathroomObject>();
            if(bathObjRef != null) {
                if(bathObjRef.type != BathroomObjectType.Exit) {
                    if(bathObjRef.state != BathroomObjectState.Broken
                        && bathObjRef.state != BathroomObjectState.BrokenByPee
                        && bathObjRef.state != BathroomObjectState.BrokenByPoop) {
                        foundBathroomObjectThatIsNotBroken = true;
                    }
                }
            }
        }
        
        return !foundBathroomObjectThatIsNotBroken;
    }
    
    public GameObject CreateWaveState(string gameObjectName, WaveState.WaveStateLogic waveStateLogic) {
        GameObject newWaveStateGameObject = ((new GameObject(gameObjectName)).AddComponent<WaveState>()).gameObject;
        newWaveStateGameObject.GetComponent<WaveState>().ConfigureLogic(waveStateLogic);
        newWaveStateGameObject.transform.parent = this.gameObject.transform;
        
        return newWaveStateGameObject;
    }
    
    public GameObject CreateDelayState(string gameObjectName, float delayTime) {
        return CreateWaveState("Delay", () => {
            Delay(delayTime);
            Completed();
        });
    }
    
    public void InitializeWaveStates(params GameObject[] waveStates) {
        // waveStatesQueue = new LinkedList<GameObject>();
        
        // foreach(GameObject waveState in waveStates) {
        //     EnqueueWaveState(waveState);
        //     // Debug.Log(waveState);
        // }
        if(waveStatesQueue.Count != 0) {
            currentWaveStateGameObject = (GameObject)DequeueWaveState();
        }
    }
    
    public void PerformWaveStatesThenReturn(params GameObject[] waveStatesToJumpTo) {
        // waveStatesQueue.AddFirst(currentWaveStateGameObject);
        // currentWaveStateGameObject = waveStateToJumpTo;
        System.Array.Reverse(waveStatesToJumpTo);
        foreach(GameObject waveStateToJumpTo in waveStatesToJumpTo) {
            waveStatesQueue.AddFirst(waveStateToJumpTo);
        }
    }
    
    public void PerformWaveStates(params GameObject[] waveStatesToJumpTo) {
        foreach(GameObject waveStateToJumpTo in waveStatesToJumpTo) {
            waveStatesQueue.AddLast(waveStateToJumpTo);
        }
    }
    
    public void PerformWaveStatesAndWait(params GameObject[] waveStatesToJumpTo) {
        foreach(GameObject waveStateToJumpTo in waveStatesToJumpTo) {
            waveStatesQueue.AddLast(waveStateToJumpTo);
        }
        waveStatesQueue.AddLast(CreateWaveState("Wait", delegate() { /* wait */ }));
        
    }
    
    public void EnqueueWaveState(GameObject waveStateGameObjectToEnqueue) {
        waveStatesQueue.AddLast(waveStateGameObjectToEnqueue);
    }
    
    public void EnqueueWaveStateAtFront(GameObject waveStateGameObjectToEnqueue) {
        waveStatesQueue.AddFirst(waveStateGameObjectToEnqueue);
    }
    
    public GameObject DequeueWaveState() {
        GameObject waveStateDequeued = waveStatesQueue.First.Value;
        waveStatesQueue.RemoveFirst();
        return waveStateDequeued;
    }
    
    public void AddWaveStateToFrontOfQueue(GameObject waveStateGameObjectToAdd) {
        waveStatesQueue.AddFirst(waveStateGameObjectToAdd);
    }
    
    public void AddWaveStateToEndOfQueue(GameObject waveStateGameObjectToAdd) {
        waveStatesQueue.AddLast(waveStateGameObjectToAdd);
    }
}
