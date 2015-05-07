using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextboxManager : MonoBehaviour {

    public UIPanel textboxPanel = null;
    
    public GameObject textboxBackgroundObject = null;
    
    public GameObject textboxButtonObject = null;
    
    public GameObject textboxTextObject = null;
    public UILabel textboxText = null;
    public Queue textboxTextSet = new Queue();
    
    public delegate void TextboxButtonPressLogic();
    public delegate void TextboxTextFinishedLogic();
    public TextboxButtonPressLogic textboxButtonLogicToPerform = null;
    public TextboxTextFinishedLogic textboxTextFinishedLogicToPerform = null;
    
    public bool textboxFinishLogicTriggered = false;
    public bool finishedTextboxText = false;
    
    //----------------------------------------------------------------------------
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile TextboxManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static TextboxManager() {
    }
    
    public static TextboxManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject textboxManagerManagerGameObject = new GameObject("TextBoxManagerGameObject");
                        _instance = (textboxManagerManagerGameObject.AddComponent<TextboxManager>()).GetComponent<TextboxManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private TextboxManager(TextboxButtonPressLogic newLogic) {
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
        
        textboxPanel = this.gameObject.GetComponent<UIPanel>();
        
        textboxText = textboxTextObject.GetComponent<UILabel>();
        
        textboxButtonLogicToPerform = new TextboxButtonPressLogic(MoveToNextTextboxText);
        SetFinishedLogicToDefault();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    public void PerformTextboxButtonPress() {
        textboxButtonLogicToPerform();
    }
    
    public void PerformTextboxTextFinished() {
        finishedTextboxText = true;
        if(!textboxFinishLogicTriggered
            && textboxTextFinishedLogicToPerform != null) {
            textboxFinishLogicTriggered = true;
            textboxTextFinishedLogicToPerform();
            textboxTextFinishedLogicToPerform = null;
        }
    }
    
    public void DefaultTextFinishedLogicToPerform() {
    }
    
    public void SetNewLogic(TextboxButtonPressLogic newLogic) {
        textboxButtonLogicToPerform = newLogic;
    }
    public TextboxManager OnFinish(TextboxTextFinishedLogic newTextboxTextFinishedLogic) {
        textboxTextFinishedLogicToPerform = new TextboxTextFinishedLogic(newTextboxTextFinishedLogic);
        return this;
    }
    public void SetFinishedLogicToDefault() {
        textboxTextFinishedLogicToPerform = new TextboxTextFinishedLogic(DefaultTextFinishedLogicToPerform);
    }
    
    public bool HasFinished() {
        return finishedTextboxText;
    }
    
    //----------------------------------------------------------------------------
    // GUI STUFF GOES DOWN HERE
    //----------------------------------------------------------------------------
    public TextboxManager Hide(float duration = 1f) {
        UIPanel panelToModify = this.gameObject.GetComponent<UIPanel>();
        panelToModify.alpha = 1f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 0f));
        return this;
    }
    
    public TextboxManager Show(float duration = 1f) {
        UIPanel panelToModify = this.gameObject.GetComponent<UIPanel>();
        panelToModify.alpha = 0f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 1f));
        return this;
    }
    
    public void MoveToNextTextboxText() {
        // Debug.Log("button press for moving to the next textbox.");
        SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
        PopNextTextboxText();
    }
    public void PopNextTextboxText() {
        TypewriterEffect textboxTextTypewriterEffect = textboxTextObject.GetComponent<TypewriterEffect>();
        if(textboxTextTypewriterEffect == null
            || (textboxTextTypewriterEffect != null && !textboxTextTypewriterEffect.isActive)) {
            if(textboxTextSet != null
                && textboxTextSet.Count > 0) {
                textboxText.text = "" + textboxTextSet.Dequeue();
                TypewriterEffect typewriterEffectRef = textboxTextObject.GetComponent<TypewriterEffect>();
                if(typewriterEffectRef != null) {
                    typewriterEffectRef.ResetToBeginning();
                }
            }
            else {
                PerformTextboxTextFinished();
                //finished text crap, should do something here??
            }
        }
    }
    
    public TextboxManager SetText(params string[] textboxTexts) {
        Queue textboxTextsQueue = new Queue();
        foreach(string textboxText in textboxTexts) {
            textboxTextsQueue.Enqueue(textboxText);
        }
        SetText(textboxTextsQueue);
        
        return this;
    }
    
    public TextboxManager SetText(Queue newTextboxTextSet) {
        TypewriterEffect textboxTextTypewriterEffect = textboxTextObject.GetComponent<TypewriterEffect>();
        
        textboxFinishLogicTriggered = false;
        finishedTextboxText = false;
        textboxTextSet = newTextboxTextSet;
        PopNextTextboxText();
        
        if(textboxTextTypewriterEffect != null) {
            textboxTextTypewriterEffect.ResetToBeginning();
        }
        
        return this;
    }
}
