using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {
    public Dictionary<string, List<Animation>> animations = new Dictionary<string, List<Animation>>();
    public Dictionary<string, List<Animation>> availableAnimations = new Dictionary<string, List<Animation>>();
    
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile AnimationManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static AnimationManager() {
    }
    
    public static AnimationManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<AnimationManager>();
                    if(_instance == null) {
                        GameObject managerGameObject = new GameObject("AnimationManager");
                        _instance = (managerGameObject.AddComponent<AnimationManager>()).GetComponent<AnimationManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private AnimationManager() {
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
}
