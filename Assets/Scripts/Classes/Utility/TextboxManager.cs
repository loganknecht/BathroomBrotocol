using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextboxManager : MonoBehaviour {

  public UIPanel textboxPanel = null;
  public Vector2 textboxPanelPosition = new Vector2(50, -300);

  public GameObject textboxBackgroundObject = null;

  public GameObject textboxButtonObject = null;

  public GameObject textboxTextObject = null;
  public UILabel textboxText = null;
  public Queue textboxTextSet = new Queue();

  public delegate void TextboxButtonPressLogic();
  public delegate void TextboxTextFinishedLogic();
  public TextboxButtonPressLogic textboxButtonLogicToPerform = null;
  public TextboxTextFinishedLogic textboxTextFinishedLogicToPerform = null;

  public bool hasFinishedTextBoxText = false;
  //----------------------------------------------------------------------------

  //Probably doesn't need to be a singleton
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
          if (_instance == null) {
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
  }
  //END OF SINGLETON CODE CONFIGURATION

	// Use this for initialization
	void Start () {
    textboxPanel = this.gameObject.GetComponent<UIPanel>();

    textboxText = textboxTextObject.GetComponent<UILabel>();

    textboxButtonLogicToPerform = new TextboxButtonPressLogic(MoveToNextTextboxText);
    textboxTextFinishedLogicToPerform = new TextboxTextFinishedLogic(DefaultTextFinishedLogicToPerform);
	}

	// Update is called once per frame
	void Update () {
	}

  public void PerformTextboxButtonPress() {
    textboxButtonLogicToPerform();
  }

  public void PerformTextboxTextFinished() {
    textboxTextFinishedLogicToPerform();
  }

  public void DefaultTextFinishedLogicToPerform() {
    hasFinishedTextBoxText = true;
  }

  public void SetNewTextBoxLogic(TextboxButtonPressLogic newLogic) {
    textboxButtonLogicToPerform = newLogic;
  }

  public void Hide() {
    TweenExecutor.TweenObjectAlpha(this.gameObject, 1, 0, 0, 1, UITweener.Method.Linear, null);
    // textboxPanel.alpha = 0;
  }
  public void Show() {
    // textboxPanel.alpha = 1;
    TweenExecutor.TweenObjectAlpha(this.gameObject, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }

  public void MoveToNextTextboxText() {
    // Debug.Log("button press for moving to the next textbox.");
    SoundManager.Instance.Play(AudioType.TextboxNextButtonPressBeep);
    PopNextTextboxText();
  }
  public void PopNextTextboxText() {
    if(textboxTextSet != null
       && textboxTextSet.Count > 0) {
      textboxText.text = "" + textboxTextSet.Dequeue();
    }
    else {
      PerformTextboxTextFinished();
      //finished text crap, should do something here??
    }
  }

  public void SetTextboxTextSet(Queue newTextboxTextSet) {
    textboxTextSet = newTextboxTextSet;
    PopNextTextboxText();
  }

  public void StartSlideInFromBottom() {
    // public static void TweenObjectPosition(GameObject objectToTween, float startX, float startY, float endX, float endY, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    // public static void TweenObjectAlpha(GameObject objectToTween, float startOpacity, float endOpacity, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    // Debug.Log("end y: " + textboxPanelPosition);

    // TweenExecutor.TweenObjectPosition(textboxPanel.gameObject, textboxPanelPosition.x, -500, textboxPanelPosition.x, textboxPanelPosition.y, 0, 1, UITweener.Method.Linear, null);
    // TweenExecutor.TweenObjectAlpha(textboxPanel.gameObject, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
}
