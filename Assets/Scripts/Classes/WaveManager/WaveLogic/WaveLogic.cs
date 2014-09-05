using UnityEngine;
using System.Collections;

public class WaveLogic : MonoBehaviour, WaveLogicContract {
  public Queue waveStatesQueue;
  public GameObject currentWaveStateGameObject;

  public bool waveLogicFinished = false;

	public virtual void Awake() {
	}

	public virtual void Start() {
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
    if(BroGenerator.Instance.HasGeneratorFinished()
       && BroManager.Instance.allBros.Count == 0
       && waveLogicFinished) {
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
}
