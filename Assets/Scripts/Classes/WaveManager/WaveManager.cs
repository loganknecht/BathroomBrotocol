using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

    public GameObject waveLogicGameObject = null;
    WaveLogic waveLogicReference = null;
    public bool isPaused = false;
    public float currentTimer = 0;
    
    //Probably doesn't need to be a singleton
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile WaveManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static WaveManager() {
    }
    
    public static WaveManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject waveManagerGameObject = new GameObject("WaveManagerGameObject");
                        _instance = (waveManagerGameObject.AddComponent<WaveManager>()).GetComponent<WaveManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private WaveManager() {
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
        waveLogicReference = waveLogicGameObject.GetComponent<WaveLogic>();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    public void Start() {
    }
    
    public void Update() {
        // Would remove and place in wave logic...
        if(!isPaused) {
            currentTimer += Time.deltaTime;
            
            PerformWaveLogic();
        }
        
        if(Input.GetKeyDown(KeyCode.Q)) {
            // EntranceQueueManager.Instance.GenerateBroInEntranceQueueByType(BroType.GenericBro, 0);
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            // EntranceQueueManager.Instance.GenerateBroInEntranceQueueByType(BroType.GenericBro, 1);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            // EntranceQueueManager.Instance.GenerateBroInEntranceQueueByType(BroType.GenericBro, 2);
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            // EntranceQueueManager.Instance.GenerateBroInEntranceQueueByType(BroType.GenericBro, 3);
        }
        if(Input.GetKeyDown(KeyCode.T)) {
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.BluetoothBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.ChattyBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.CuttingBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.DrunkBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.FartingBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.RichBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.ShyBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.SlobBro);
            // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.TimeWasterBro);
            
            // EntranceQueueManager.Instance.GenerateRandomBroInRandomEntranceQueue();
        }
    }
    
    public void Pause() {
        isPaused = true;
    }
    public void Unpause() {
        isPaused = false;
    }
    
    public void PerformWaveLogic() {
        waveLogicReference.PerformWaveLogic();
    }
}
