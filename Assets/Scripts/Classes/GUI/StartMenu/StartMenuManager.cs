using UnityEngine;
using System;
using System.Collections;

public class StartMenuManager : MonoBehaviour {
    public GameObject menuBackground = null;
    public GameObject menuTitle = null;
    public GameObject newGameButton = null;
    public GameObject continueGameButton = null;
    public GameObject freeplayButton = null;
    public GameObject optionsButton = null;
    public GameObject fadeBackground = null;
    
    public string nextState = "";
    //-------------
    //BEGINNING OF SINGLETON CODE CONFIGURATION5
    private static volatile StartMenuManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static StartMenuManager() {
    }
    
    public static StartMenuManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<StartMenuManager>();
                    if(_instance == null) {
                        GameObject startMenuManagerGameObject = new GameObject("StartMenuManager");
                        _instance = (startMenuManagerGameObject.AddComponent<StartMenuManager>()).GetComponent<StartMenuManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private StartMenuManager() {
    }
    
    // Use this for initialization
    void Awake() {
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
    
    public void PerformNewGameSlideOut() {
        PerformGUISlideOut(NewGameSlideOutComplete);
    }
    public void PerformContinueGameSlideOut() {
        PerformGUISlideOut(ContinueGameSlideOutComplete);
    }
    public void PerformFreeplaySlideOut() {
        PerformGUISlideOut(FreeplaySlideOutComplete);
    }
    public void PerformOptionsSlideOut() {
        PerformGUISlideOut(OptionsSlideOutComplete);
    }
    
    public void PerformGUISlideOut(EventDelegate.Callback callbackToPerform) {
        Go.to(menuBackground.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(0,
                                         -1200,
                                         0
                                        ), true)
        .onComplete(guiComplete => {
            callbackToPerform();
        }));
        
        Go.to(menuTitle.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(800,
                                         0,
                                         0
                                        ), true));
                                        
        Go.to(newGameButton.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(-1000,
                                         0,
                                         0
                                        ), true));
                                        
        Go.to(continueGameButton.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(1000,
                                         0,
                                         0
                                        ), true));
                                        
        Go.to(freeplayButton.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(-1000,
                                         0,
                                         0
                                        ), true));
                                        
        Go.to(optionsButton.transform,
              1f,
              new GoTweenConfig()
              .localPosition(new Vector3(1000,
                                         0,
                                         0
                                        ), true));
                                        
    }
    
    //--------------------------------------------------------------------
    void NewGameSlideOutComplete() {
        // Debug.Log("FINISHED");
        nextState = "CareerMenu";
        TriggerFadeIn(1, 0, 0, 1, true);
    }
    void ContinueGameSlideOutComplete() {
        //Debug.Log("FINISHED");
        nextState = "CareerMenu";
        TriggerFadeIn(1, 0, 0, 1, true);
    }
    void FreeplaySlideOutComplete() {
        //Debug.Log("FINISHED");
        nextState = "CareerMenu";
        TriggerFadeIn(1, 0, 0, 1, true);
    }
    void OptionsSlideOutComplete() {
        //Debug.Log("FINISHED");
        nextState = "CareerMenu";
        TriggerFadeIn(1, 0, 0, 1, true);
    }
    //--------------------------------------------------------------------
    void TriggerFadeIn(float duration, float delay, float from, float to, bool changeState) {
        fadeBackground.GetComponent<TweenAlpha>().delay = delay;
        
        fadeBackground.GetComponent<TweenAlpha>().from = from;
        TweenAlpha tweenAlpha = TweenAlpha.Begin(fadeBackground, duration, to);
        if(changeState) {
            EventDelegate.Add(tweenAlpha.onFinished, ChangeState);
        }
    }
    
    void ChangeState() {
        // Debug.Log("CHANGING STATE");
        Application.LoadLevel(nextState);
    }
    //--------------------------------------------------------------------
}
