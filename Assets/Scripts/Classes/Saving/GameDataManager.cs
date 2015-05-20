using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour {
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile GameDataManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static GameDataManager() {
    }
    
    public static GameDataManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<GameDataManager>();
                    if(_instance == null) {
                        GameObject GameDataManager = new GameObject("GameDataManagerGameObject");
                        _instance = (GameDataManager.AddComponent<GameDataManager>()).GetComponent<GameDataManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private GameDataManager() {
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
}
