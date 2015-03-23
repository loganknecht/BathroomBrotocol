using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryManager : MonoBehaviour {
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile SceneryManager _instance;
    private static object _lock = new object();

    public List<GameObject> sceneryGameObjects = null;

    //Stops the lock being created ahead of time if it's not necessary
    static SceneryManager() {
    }

    public static SceneryManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if (_instance == null) {
                        GameObject SceneryManagerGameObject = new GameObject("SceneryManagerGameObject");
                        _instance = (SceneryManagerGameObject.AddComponent<SceneryManager>()).GetComponent<SceneryManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private SceneryManager() {
    }

    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;

        //----------------------------------------------------------------------
        //TODO: Replace the implementation to not rely on tags and instead rely on the component attached.
        // Scenery[] sceneryObjects = Resources.FindObjectsOfTypeAll(typeof(Scenery)) as Scenery[];
        // Debug.Log("Scenery Components Found: " + sceneryObjects.Length);
        // Debug.Log("Scenery Tag Found: " + GameObject.FindGameObjectsWithTag("Scenery").Length);
        // sceneryGameObjects = GameObject.FindGameObjectsWithTag("Scenery");
        //----------------------------------------------------------------------

        foreach(GameObject gameObj in GameObject.FindGameObjectsWithTag("Scenery")) {
            sceneryGameObjects.Add(gameObj);
        }
    }
    //END OF SINGLETON CODE CONFIGURATION

    public List<GameObject> GetScenery() {
        return sceneryGameObjects;
    }

    public void Start() {
    }

    // Update is called once per frame
    void Update () {
    }
}
