using UnityEngine;
using System.Collections;

public class WaveState : MonoBehaviour {
  public bool hasBeenTriggered;
  public bool isPlaying;
  public bool triggerFinishLogic;
  public bool hasFinished;

  public delegate void WaveStateStartLogic();
  public delegate void WaveStateLogic();
  public delegate void WaveStateFinishLogic();
  public WaveStateStartLogic waveStateStartLogic = null;
  public WaveStateLogic waveStateLogic = null;
  public WaveStateFinishLogic waveStateFinishLogic = null;

  public void Awake() {
  }

  public void Start() {
    hasBeenTriggered = false;
    isPlaying = false;
    triggerFinishLogic = false;
    hasFinished = false;

    // waveStateStartLogic = new WaveStateStartLogic(WaveStateStartedLogic);
    // waveStateLogic = new WaveStateLogic(WaveStatePerformingLogic);
    // waveStateFinishLogic = new WaveStateFinishLogic(WaveStateFinishedLogic);
  }

  public void ConfigureLogic(WaveStateStartLogic startLogic, WaveStateLogic performingLogic, WaveStateFinishLogic endLogic) {
    if(startLogic != null) {
      waveStateStartLogic = new WaveStateStartLogic(startLogic);
    }
    else {
      waveStateStartLogic = new WaveStateStartLogic(WaveStateStartedLogic);
    }

    if(performingLogic != null) {
      waveStateLogic = new WaveStateLogic(performingLogic);
    }
    else {
      waveStateLogic = new WaveStateLogic(WaveStatePerformingLogic);
    }

    if(endLogic != null) {
      waveStateFinishLogic = new WaveStateFinishLogic(endLogic);
    }
    else {
      waveStateFinishLogic = new WaveStateFinishLogic(WaveStateFinishedLogic);
    }
  }

  public void WaveStateStartedLogic() {
    Debug.Log("PERFORMING DEFAULT WAVE STATE STARTED LOGIC, PLEASE FIX THIS ISSUE.");
    hasBeenTriggered = true;
    isPlaying = true;
  }

  public void WaveStatePerformingLogic() {
    Debug.Log("PERFORMING DEFAULT WAVE STATE LOGIC, PLEASE FIX THIS ISSUE.");
    triggerFinishLogic = true;
  }

  public void WaveStateFinishedLogic() {
    if(triggerFinishLogic) {
      Debug.Log("PERFORMING DEFAULT WAVE STATE FINISHED LOGIC, PLEASE FIX THIS ISSUE.");
      isPlaying = false;
      hasFinished = true;
    }
  }
}
