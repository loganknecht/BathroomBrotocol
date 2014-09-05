using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreMenuManager : MonoBehaviour {

  public GameObject uiRootGameObject = null;

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
          if (_instance == null) {
            GameObject careerMenuManagerGameObject = new GameObject("CareerMenuManagerGameObject");
            _instance = (careerMenuManagerGameObject.AddComponent<ScoreMenuManager>()).GetComponent<ScoreMenuManager>();
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
  void Update () {
  }

  public void HideUI() {
    TweenExecutor.TweenObjectAlpha(uiRootGameObject, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowUI() {
    TweenExecutor.TweenObjectAlpha(uiRootGameObject, 0, 1, 0, 1, UITweener.Method.Linear, null);
    // public static void TweenObjectAlpha(GameObject objectToTween, float startOpacity, float endOpacity, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
  }

  public void PerformContinueButtonPress() {
    FadeManager.Instance.fadeFinishLogic = new FadeManager.FadeFinishEvent(TriggerSceneChange);
    StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.clear, Color.white, 1, false));

    HideUI();
  }
  public void TriggerSceneChange() {
    Application.LoadLevel("CareerMenu");
  }
}
