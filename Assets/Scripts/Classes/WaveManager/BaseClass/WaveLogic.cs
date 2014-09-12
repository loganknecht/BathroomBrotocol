using UnityEngine;
using System.Collections;

public class WaveLogic : MonoBehaviour, WaveLogicContract {
  public Queue waveStatesQueue = new Queue();
  public GameObject currentWaveStateGameObject = null;

  public bool waveLogicFinished = false;

	public virtual void Awake() {
	}

	public virtual void Start() {
    Camera.main.GetComponent<RotateCamera>().RotateBathroomToMatchCamera();
	}

	public virtual void Update() {
		PerformWaveLogic();
    PerformLevelFailCheck();
    PerformLevelFinishCheck();
	}

  public virtual void PerformWaveLogic() {
    //this assumes that the paused state is already managed by the wave manager
    if(currentWaveStateGameObject != null) {
      // Debug.Log("performing wave state logic");
      WaveState currentWaveStateRef = currentWaveStateGameObject.GetComponent<WaveState>();

      if(!currentWaveStateRef.hasBeenTriggered) {
        currentWaveStateRef.hasBeenTriggered = true;
        currentWaveStateRef.isPlaying = true;
        currentWaveStateRef.waveStateStartLogic();
      }
      else if(currentWaveStateRef.hasBeenTriggered
              && !currentWaveStateRef.triggerFinishLogic) {
        currentWaveStateRef.waveStateLogic();
      }
      else if(currentWaveStateRef.triggerFinishLogic) {
        currentWaveStateRef.waveStateFinishLogic();
      }

      if(currentWaveStateRef.hasFinished) {
        if(waveStatesQueue.Count > 0) {
          currentWaveStateGameObject = (GameObject)waveStatesQueue.Dequeue();
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
    waveStatesQueue = new Queue();

    foreach(GameObject waveState in waveStates) {
      waveStatesQueue.Enqueue(waveState);
    }
    if(waveStatesQueue.Count != 0) {
      currentWaveStateGameObject = (GameObject)waveStatesQueue.Dequeue();
    }
  }

  public void PerformWaveStateStartedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().hasBeenTriggered = true;
    currentWaveStateGameObject.GetComponent<WaveState>().isPlaying = true;
  }
  public void PerformWaveStatePlayingFinishedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().triggerFinishLogic = true;
  }
  public void PerformWaveStateHasFinishedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().hasFinished = true;
  }
}
