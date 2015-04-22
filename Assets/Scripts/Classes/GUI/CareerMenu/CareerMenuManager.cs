using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CareerMenuManager : MonoBehaviour {

    public GameObject uiRootGameObject = null;
    public UILabel missionTitle = null;
    public GameObject missionBriefing = null;
    public UILabel missionDescription = null;
    public UIButton missionButton = null;
    
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile CareerMenuManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static CareerMenuManager() {
    }
    
    public static CareerMenuManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject careerMenuManagerGameObject = new GameObject("CareerMenuManagerGameObject");
                        _instance = (careerMenuManagerGameObject.AddComponent<CareerMenuManager>()).GetComponent<CareerMenuManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private CareerMenuManager() {
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
        SoundManager.Instance.PlayMusic(AudioType.GreendogIntroAztecTemples);
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    public void ScaleToCenterMissionBriefing(GameObject careerMenuNode) {
        // Debug.Log(careerMenuNode);
        // Debug.Log(careerMenuNode.GetComponent<CareerMenuNode>());
        // Vector3 startPosition = this.gameObject.transform.localPosition;
        Vector3 startPosition = careerMenuNode.transform.localPosition;
        Vector3 endPosition = Vector3.zero;
        
        CareerMenuNode careerMenuNodeReference = careerMenuNode.GetComponent<CareerMenuNode>();
        if(!careerMenuNodeReference.isLocked) {
            // Debug.Log(careerMenuNodeReference.levelTitle);
            missionTitle.text = careerMenuNodeReference.levelTitle;
            
            missionDescription.text = careerMenuNodeReference.levelDescription;
            missionBriefing.GetComponent<UIPanel>().alpha = 1;
            
            missionBriefing.transform.localPosition = startPosition;
            TweenPosition.Begin(CareerMenuManager.Instance.missionBriefing, 1f, endPosition);
            
            missionBriefing.transform.localScale = new Vector3(0, 0, 0);
            TweenScale.Begin(CareerMenuManager.Instance.missionBriefing, 1f, Vector3.one);
            
            missionButton.onClick = new List<EventDelegate>();
            missionButton.onClick.Add(new EventDelegate(careerMenuNodeReference.PerformLevelChange));
        }
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
}
