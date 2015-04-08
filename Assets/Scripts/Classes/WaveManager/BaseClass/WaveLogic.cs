using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveLogic : MonoBehaviour, WaveLogicContract {
    public LinkedList<GameObject> waveStatesQueue = new LinkedList<GameObject>();
    public GameObject currentWaveStateGameObject = null;
    public IEnumerator currentWaveStateCoroutine = null;
    public float timer = 0f;
    
    public bool waveLogicFinished = false;
    
    public virtual void Awake() {}
    
    public virtual void Start() {}
    
    public virtual void Update() {}
    
    public virtual void Delay(float newDelayTime) {
        if(currentWaveStateGameObject != null) {
            WaveState currentWaveStateRef = currentWaveStateGameObject.GetComponent<WaveState>();
            currentWaveStateRef.SetDelay(newDelayTime);
        }
    }
    
    public virtual void Initialize() {}
    
    // Think of this as the main update loop
    public virtual void PerformWaveLogic() {
        timer += Time.deltaTime;
        // Debug.Log("Current Timer: " + timer);
        
        PerformLevelFailCheck();
        PerformLevelFinishCheck();
        
        //this assumes that the paused state is already managed by the wave manager
        if(currentWaveStateGameObject != null) {
            // Debug.Log("performing wave state logic");
            WaveState currentWaveStateRef = currentWaveStateGameObject.GetComponent<WaveState>();
            
            // Coroutine start is triggered here for wave state
            if(currentWaveStateCoroutine == null) {
                Debug.Log("starting wave state coroutine");
                currentWaveStateCoroutine = currentWaveStateRef.PerformWaveStateLogic();
                StartCoroutine(currentWaveStateCoroutine);
            }
            
            if(currentWaveStateRef.hasFinished) {
                if(waveStatesQueue.Count > 0) {
                    currentWaveStateGameObject = (GameObject)DequeueWaveState();
                    // cleans up previous wave state's coroutine just in case
                    if(currentWaveStateCoroutine != null) {
                        StopCoroutine(currentWaveStateCoroutine);
                        currentWaveStateCoroutine = null;
                    }
                }
                else {
                    currentWaveStateGameObject = null;
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
    
    public GameObject CreateWaveState(string gameObjectName, WaveState.WaveStateStartLogic startLogic, WaveState.WaveStateLogic performingLogic, WaveState.WaveStateFinishLogic endLogic) {
        GameObject newWaveStateGameObject = ((new GameObject(gameObjectName)).AddComponent<WaveState>()).gameObject;
        newWaveStateGameObject.GetComponent<WaveState>().ConfigureLogic(startLogic, performingLogic, endLogic);
        newWaveStateGameObject.transform.parent = this.gameObject.transform;
        
        return newWaveStateGameObject;
    }
    
    public void InitializeWaveStates(params GameObject[] waveStates) {
        waveStatesQueue = new LinkedList<GameObject>();
        
        foreach(GameObject waveState in waveStates) {
            EnqueueWaveState(waveState);
        }
        if(waveStatesQueue.Count != 0) {
            currentWaveStateGameObject = (GameObject)DequeueWaveState();
        }
    }
    
    public void PerformWaveStateThenReturn(GameObject waveStateToJumpTo) {
        waveStatesQueue.AddFirst(currentWaveStateGameObject);
        currentWaveStateGameObject = waveStateToJumpTo;
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
    
    public void TriggerWaveFinish() {
        currentWaveStateGameObject.GetComponent<WaveState>().TriggerWaveFinish();
    }
}
