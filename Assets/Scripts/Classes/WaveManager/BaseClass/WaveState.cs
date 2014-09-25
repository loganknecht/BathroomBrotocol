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

  public WaveStateStartLogic playerWaveStateStartLogic = null;
  public WaveStateLogic playerWaveStateLogic = null;
  public WaveStateFinishLogic playerWaveStateFinishLogic = null;

  public void Awake() {
    hasBeenTriggered = false;
    isPlaying = false;
    triggerFinishLogic = false;
    hasFinished = false;

    waveStateStartLogic = new WaveStateStartLogic(DefaultWaveStateStartedLogic);
    waveStateLogic = new WaveStateLogic(DefaultWaveStatePlayingLogic);
    waveStateFinishLogic = new WaveStateFinishLogic(DefaultWaveStateFinishedLogic);
  }

  public void Start() {
  }

  public void ConfigureLogic(WaveStateStartLogic startLogic, WaveStateLogic performingLogic, WaveStateFinishLogic endLogic) {
    if(startLogic != null) {
      playerWaveStateStartLogic = new WaveStateStartLogic(startLogic);
    }
    else {
      playerWaveStateStartLogic = new WaveStateStartLogic(WaveStateStartedLogic);
    }

    if(performingLogic != null) {
      playerWaveStateLogic = new WaveStateLogic(performingLogic);
    }
    else {
      playerWaveStateLogic = new WaveStateLogic(WaveStatePlayingLogic);
    }

    if(endLogic != null) {
      playerWaveStateFinishLogic = new WaveStateFinishLogic(endLogic);
    }
    else {
      playerWaveStateFinishLogic = new WaveStateFinishLogic(WaveStateFinishedLogic);
    }
  }

  public void Reset() {
    hasBeenTriggered = false;
    isPlaying = false;
    triggerFinishLogic = false;
    hasFinished = false;
  }

  public void DefaultWaveStateStartedLogic() {
    PerformWaveStateStartTrigger();
    playerWaveStateStartLogic();
  }
  public void DefaultWaveStatePlayingLogic() {
    playerWaveStateLogic();
  }
  public void DefaultWaveStateFinishedLogic() {
    PerformWaveStateHasFinishedTrigger();
    playerWaveStateFinishLogic();
  }

  public void WaveStateStartedLogic() {
    Debug.Log("PERFORMING DEFAULT WAVE STATE STARTED LOGIC, PLEASE FIX THIS ISSUE.");
  }

  public void WaveStatePlayingLogic() {
    Debug.Log("PERFORMING DEFAULT WAVE STATE LOGIC, PLEASE FIX THIS ISSUE.");
    triggerFinishLogic = true;
  }

  public void WaveStateFinishedLogic() {
    if(triggerFinishLogic) {
      Debug.Log("PERFORMING DEFAULT WAVE STATE FINISHED LOGIC, PLEASE FIX THIS ISSUE.");
    }
  }

   public void PerformWaveStateStartTrigger() {
    hasBeenTriggered = true;
    isPlaying = true;
  }
  public void PerformWaveStatePlayingFinishedTrigger() {
    triggerFinishLogic = true;
  }
  public void PerformWaveStateHasFinishedTrigger() {
    hasFinished = true;
  }
}
