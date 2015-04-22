﻿using UnityEngine;
using System.Collections;

public class ConfirmationBoxManager : MonoBehaviour {

    public GameObject background = null;
    public GameObject yesButton = null;
    public GameObject noButton = null;
    public GameObject text = null;
    public GameObject title = null;
    //----------------------------------------------------------------------------
    public bool hasSelectedAnswer = false;
    public bool selectedYes = false;
    public bool selectedNo = false;
    //----------------------------------------------------------------------------
    
    //Probably doesn't need to be a singleton
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile ConfirmationBoxManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static ConfirmationBoxManager() {
    }
    
    public static ConfirmationBoxManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject ConfirmationBoxManagerManagerGameObject = new GameObject("ConfirmationBoxManagerGameObject");
                        _instance = (ConfirmationBoxManagerManagerGameObject.AddComponent<ConfirmationBoxManager>()).GetComponent<ConfirmationBoxManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private ConfirmationBoxManager() {
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
    
    public void Hide() {
        TweenExecutor.Alpha
        .Object(this.gameObject)
        .StartAlpha(1)
        .EndAlpha(0)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
    }
    public void Show() {
        TweenExecutor.Alpha
        .Object(this.gameObject)
        .StartAlpha(0)
        .EndAlpha(1)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
    }
    
    public void SetText(string newConfirmationBoxText) {
        text.GetComponent<UILabel>().text = newConfirmationBoxText;
    }
    
    public void Reset() {
        hasSelectedAnswer = false;
        selectedNo = false;
        selectedYes = false;
    }
    
    public void SelectedYes() {
        if(!hasSelectedAnswer) {
            hasSelectedAnswer = true;
            selectedYes = true;
            
            SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
        }
    }
    
    public void SelectedNo() {
        if(!hasSelectedAnswer) {
            hasSelectedAnswer = true;
            selectedNo = true;
            
            SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
        }
    }
}
