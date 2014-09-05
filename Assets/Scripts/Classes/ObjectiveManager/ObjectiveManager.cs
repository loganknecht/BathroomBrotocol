using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour {

  public bool isPaused = false;
  public List<GameObject> objectives  = new List<GameObject>();

  //Probably doesn't need to be a singleton
  //BEGINNING OF SINGLETON CODE CONFIGURATION
  private static volatile ObjectiveManager _instance;
  private static object _lock = new object();

  //Stops the lock being created ahead of time if it's not necessary
  static ObjectiveManager() {
  }

  public static ObjectiveManager Instance {
    get {
      if(_instance == null) {
        lock(_lock) {
          if (_instance == null) {
            GameObject objectiveManagerGameObject = new GameObject("ObjectiveManagerGameObject");
            _instance = (objectiveManagerGameObject.AddComponent<ObjectiveManager>()).GetComponent<ObjectiveManager>();
          }
        }
      }
      return _instance;
    }
  }

  private ObjectiveManager() {
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

  public void Start() {
  }

  public void Update() {
    if(!isPaused) {
      // Debug.Log("currentTime: " + currentTime);
    }
  }
}
