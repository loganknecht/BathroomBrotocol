using UnityEngine;
using System.Collections;

public class WaveState : MonoBehaviour {

    public bool hasTriggeredLogic = false;
    public bool hasCompletedLogic = false;
    
    public delegate void WaveStateLogic();
    public WaveStateLogic playerWaveStateLogic = null;
    
    public void Awake() {
    }
    
    public void Start() {
    }
    
    public void Update() {
    }
    
    public void ConfigureLogic(WaveStateLogic newPlayerWaveStateLogic) {
        playerWaveStateLogic = new WaveStateLogic(newPlayerWaveStateLogic);
    }
    
    public bool HasStarted() {
        return hasTriggeredLogic;
    }
    
    public bool HasFinished() {
        if(hasTriggeredLogic
            && hasCompletedLogic) {
            return true;
        }
        else {
            return false;
        }
        
    }
    
    public void PerformWaveStateLogic() {
        if(!WaveManager.Instance.IsPaused()) {
            if(playerWaveStateLogic != null) {
                if(!HasStarted()) {
                    // Debug.Log("Starting WaveState.");
                    Started();
                }
                // Debug.Log("Performing Logic.");
                playerWaveStateLogic();
            }
            else {
                // Debug.Log("Wave State does not exist!!!");
            }
        }
    }
    
    public void PerformLoop() {
    
    }
    
    public void Delay(float newDelayTime) {
    }
    
    public void Reset() {
    }
    
    public void Started() {
        hasTriggeredLogic = true;
    }
    
    public void Completed() {
        // Debug.Log("WaveState Completed");
        hasCompletedLogic = true;
    }
}
