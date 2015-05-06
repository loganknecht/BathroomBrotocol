using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadySetBro : MonoBehaviour {

    public GameObject readySpriteObject = null;
    public GameObject setSpriteObject = null;
    public GameObject broSpriteObject = null;
    
    public delegate void OnFinishLogic();
    public List<OnFinishLogic> onFinishLogicsToPerform = null;
    
    public bool finishedLogicTriggered = false;
    public bool hasFinished = false;
    
    //----------------------------------------------------------------------------
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile ReadySetBro _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static ReadySetBro() {
    }
    
    public static ReadySetBro Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject ReadySetBroManagerGameObject = new GameObject("ReadySetBroGameObject");
                        _instance = (ReadySetBroManagerGameObject.AddComponent<ReadySetBro>()).GetComponent<ReadySetBro>();
                    }
                }
            }
            return _instance;
        }
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
        onFinishLogicsToPerform = new List<OnFinishLogic>();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    public bool HasFinished() {
        return hasFinished;
    }
    
    public void AddOnFinishLogic(OnFinishLogic newOnFinishLogic) {
        onFinishLogicsToPerform.Add(newOnFinishLogic);
    }
    
    // This is a convenience method for adding logic
    public ReadySetBro OnFinish(OnFinishLogic newOnFinishLogic) {
        AddOnFinishLogic(newOnFinishLogic);
        return this;
    }
    
    public ReadySetBro StartAnimation() {
        Ready();
        return this;
    }
    
    //----------------------------------------------------------------------------
    // GUI STUFF GOES DOWN HERE
    //----------------------------------------------------------------------------
    public void Ready() {
        // Debug.Log("Ready");
        readySpriteObject.transform.localPosition = new Vector3(0, 200, 0);
        readySpriteObject.GetComponent<UIPanel>().alpha = 1;
        
        float duration = 1f;
        Go.to(readySpriteObject.transform,
              duration,
              new GoTweenConfig()
              .localPosition(new Vector3(0, 0, 0))
              .setEaseType(GoEaseType.BounceOut));
        Go.to(readySpriteObject.GetComponent<UIPanel>(),
              duration,
              new GoTweenConfig()
              .setDelay(duration / 2)
              .floatProp("alpha", 0)
        .onComplete(function => { Set(); }));
    }
    
    public void Set() {
        // Debug.Log("Set");
        setSpriteObject.transform.localPosition = new Vector3(0, 200, 0);
        setSpriteObject.GetComponent<UIPanel>().alpha = 1;
        
        float duration = 1f;
        Go.to(setSpriteObject.transform,
              duration,
              new GoTweenConfig()
              .localPosition(new Vector3(0, 0, 0))
              .setEaseType(GoEaseType.BounceOut));
        Go.to(setSpriteObject.GetComponent<UIPanel>(),
              duration,
              new GoTweenConfig()
              .setDelay(duration / 2)
              .floatProp("alpha", 0)
        .onComplete(function => { Bro(); }));
    }
    
    public void Bro() {
        // Debug.Log("Bro");
        broSpriteObject.transform.localPosition = new Vector3(0, 200, 0);
        broSpriteObject.GetComponent<UIPanel>().alpha = 1;
        
        float duration = 1f;
        Go.to(broSpriteObject.transform,
              duration,
              new GoTweenConfig()
              .localPosition(new Vector3(0, 0, 0))
              .setEaseType(GoEaseType.BounceOut));
        Go.to(broSpriteObject.GetComponent<UIPanel>(),
              duration,
              new GoTweenConfig()
              .setDelay(duration / 2)
              .floatProp("alpha", 0)
        .onComplete(complete => {
            hasFinished = true;
            foreach(OnFinishLogic onFinishLogicToPerform in onFinishLogicsToPerform) {
                onFinishLogicToPerform();
            }
            onFinishLogicsToPerform.Clear();
        }));
    }
}
