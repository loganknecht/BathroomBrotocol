using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    // public int maximumScorePossible = 0;

    //SINGLETON CONFIGURATION INFORMATION GOES HERE
    private static volatile ScoreManager _instance;
    private static object _lock = new object();
    public ScoreTracker playerOneScoreTracker;
    public bool isPaused = false;

    //Stops the lock being created ahead of time if it's not necessary
    // static ScoreManager() {
    // }

    public static ScoreManager Instance {
        get {
            if (_instance == null) {
                lock(_lock) {
                    if (_instance == null) {
                        GameObject gameObjectInstance = new GameObject("Score Manager");
                        _instance = (gameObjectInstance.AddComponent<ScoreManager>()).GetComponent<ScoreManager>();
                    }
                }
            }
            return _instance;
        }
    }

    // private ScoreManager() {
    // }

    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;

        if(GetPlayerScoreTracker() == null) {
            playerOneScoreTracker = (new GameObject("Player One Score")).AddComponent<ScoreTracker>();
            playerOneScoreTracker.gameObject.transform.parent = this.gameObject.transform;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    //END SINGLETON CONFIGURATION INFORMATION

    public void Update() {
    }

    public void Pause() {
       isPaused = true; 
    }

    public void Unpause() {
       isPaused = false; 
    }

    // TODO: Create an enum that represents the player, then return the score tracker based on the respective enum
    public ScoreTracker GetPlayerScoreTracker() {
        return playerOneScoreTracker;
    }
}
