using UnityEngine;
using System.Collections;

public class WaveState : MonoBehaviour {
    public bool hasBeenTriggered = false;
    public bool isPlaying = false;
    public bool triggerFinishLogic = false;
    public bool hasFinished = false;
    public bool isPaused = false;

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
        waveStateStartLogic = new WaveStateStartLogic(DefaultWaveStateStartedLogic);
        waveStateLogic = new WaveStateLogic(DefaultWaveStatePlayingLogic);
        waveStateFinishLogic = new WaveStateFinishLogic(DefaultWaveStateFinishedLogic);
    }

    public void Start() {
    }

    public void Update() {
    }

    public void ConfigureLogic(WaveStateStartLogic startLogic, WaveStateLogic performingLogic, WaveStateFinishLogic endLogic) {
        playerWaveStateStartLogic = new WaveStateStartLogic(startLogic);
        playerWaveStateLogic = new WaveStateLogic(performingLogic);
        playerWaveStateFinishLogic = new WaveStateFinishLogic(endLogic);
    }

    public void PerformWaveStateLogic() {
        // playerWaveStateStartLogic();
        if(!hasBeenTriggered) {
            hasBeenTriggered = true;
            isPlaying = true;
            waveStateStartLogic();
        }
        else if(hasBeenTriggered
              && !triggerFinishLogic) {
            waveStateLogic();
        }
        else if(triggerFinishLogic) {
            waveStateFinishLogic();
        }
    }

    public void Reset() {
        hasBeenTriggered = false;
        isPlaying = false;
        triggerFinishLogic = false;
        hasFinished = false;
    }

    public void DefaultWaveStateStartedLogic() {
        // Debug.Log("PERFORMING DEFAULT WAVE STATE STARTED LOGIC, PLEASE FIX THIS ISSUE.");
        TriggerWaveStart();
        playerWaveStateStartLogic();
    }
    public void DefaultWaveStatePlayingLogic() {
        // Debug.Log("PERFORMING DEFAULT WAVE STATE LOGIC, PLEASE FIX THIS ISSUE.");
        playerWaveStateLogic();
    }
    public void DefaultWaveStateFinishedLogic() {
        // Debug.Log("PERFORMING DEFAULT WAVE STATE FINISHED LOGIC, PLEASE FIX THIS ISSUE.");
        WaveHasFinished();
        playerWaveStateFinishLogic();
    }

    public void TriggerWaveStart() {
        hasBeenTriggered = true;
        isPlaying = true;
    }
    public void TriggerWaveFinish() {
        triggerFinishLogic = true;
    }
    public void WaveHasFinished() {
        hasFinished = true;
    }
}
