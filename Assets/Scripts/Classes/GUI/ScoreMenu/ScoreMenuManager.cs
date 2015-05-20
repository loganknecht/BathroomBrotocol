﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreMenuManager : MonoBehaviour {

    public GameObject uiRootGameObject = null;
    public string sceneToChangeTo = "";
    public bool continueButtonPressed = false;
    
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile ScoreMenuManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static ScoreMenuManager() {
    }
    
    public static ScoreMenuManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<ScoreMenuManager>();
                    if(_instance == null) {
                        GameObject scoreMenuManagerGameObject = new GameObject("ScoreMenuManager");
                        _instance = (scoreMenuManagerGameObject.AddComponent<ScoreMenuManager>()).GetComponent<ScoreMenuManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private ScoreMenuManager() {
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
    
    public void HideUI() {
        TweenExecutor.Alpha
        .Object(uiRootGameObject)
        .StartAlpha(1)
        .EndAlpha(0)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
    }
    public void ShowUI() {
        TweenExecutor.Alpha
        .Object(uiRootGameObject)
        .StartAlpha(0)
        .EndAlpha(1)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
    }
    
    public void PerformContinueButtonPress() {
        if(!continueButtonPressed) {
            continueButtonPressed = true;
            
            FadeManager.Instance.SetFadeFinishLogic(TriggerSceneChange);
            FadeManager.Instance.PerformFade(Color.clear, Color.white, 1, false);
            
            HideUI();
        }
    }
    public void TriggerSceneChange() {
        Application.LoadLevel(sceneToChangeTo);
    }
}
