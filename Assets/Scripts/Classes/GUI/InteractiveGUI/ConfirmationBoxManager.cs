using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfirmationBoxManager : MonoBehaviour {

    public GameObject background = null;
    public GameObject yesButton = null;
    public GameObject yesButtonText = null;
    public GameObject noButton = null;
    public GameObject noButtonText = null;
    public GameObject bodyText = null;
    public GameObject title = null;
    //----------------------------------------------------------------------------
    public bool hasSelectedAnswer = false;
    public bool selectedYes = false;
    public bool selectedNo = false;
    //----------------------------------------------------------------------------
    public delegate void Selection();
    public List<Selection> onPlayerSelections = null;
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
    public void Start() {
        onPlayerSelections = new List<Selection>();
    }
    
    // Update is called once per frame
    public void Update() {
    }
    
    public bool IsVisible() {
        return (this.gameObject.GetComponent<UIPanel>().alpha > 0) ? true : false;
    }
    
    public bool WasYesSelected() {
        return selectedYes;
    }
    
    public bool WasNoSelected() {
        return selectedNo;
    }
    
    public ConfirmationBoxManager Reset() {
        hasSelectedAnswer = false;
        selectedNo = false;
        selectedYes = false;
        return this;
    }
    
    public ConfirmationBoxManager Hide(float duration = 1f) {
        TweenExecutor.Alpha
        .Object(this.gameObject)
        .StartAlpha(1)
        .EndAlpha(0)
        .Duration(duration)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
        return this;
    }
    public ConfirmationBoxManager Show(float duration = 1f) {
        TweenExecutor.Alpha
        .Object(this.gameObject)
        .StartAlpha(0)
        .EndAlpha(1)
        .Duration(duration)
        .Method(UITweener.Method.Linear)
        .OnFinish(null)
        .Tween();
        return this;
    }
    
    public ConfirmationBoxManager BodyText(string newConfirmationBoxText) {
        bodyText.GetComponent<UILabel>().text = newConfirmationBoxText;
        Reset();
        return this;
    }
    
    public ConfirmationBoxManager YesButtonText(string newConfirmationBoxText) {
        yesButtonText.GetComponent<UILabel>().text = newConfirmationBoxText;
        return this;
    }
    public ConfirmationBoxManager NoButtonText(string newConfirmationBoxText) {
        noButtonText.GetComponent<UILabel>().text = newConfirmationBoxText;
        return this;
    }
    
    public void SelectedYes() {
        if(!hasSelectedAnswer) {
            hasSelectedAnswer = true;
            selectedYes = true;
            PerformOnSelection();
            
            SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
        }
    }
    
    public void SelectedNo() {
        if(!hasSelectedAnswer) {
            hasSelectedAnswer = true;
            selectedNo = true;
            PerformOnSelection();
            
            SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
        }
    }
    
    public ConfirmationBoxManager OnSelection(Selection newOnSelection) {
        onPlayerSelections.Add(newOnSelection);
        return this;
    }
    
    public ConfirmationBoxManager PerformOnSelection() {
        if(onPlayerSelections != null
            && onPlayerSelections.Count > 0) {
            foreach(Selection onPlayerSelection in onPlayerSelections) {
                Debug.Log("Selection made");
                onPlayerSelection();
            }
            onPlayerSelections.Clear();
        }
        return this;
    }
}