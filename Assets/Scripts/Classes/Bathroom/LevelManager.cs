using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject uiRootGameObject = null;
  //-------------
	public string sceneToChangeTo = "";
  //-------------
  public GameObject backgroundImage = null;
  //-------------
  public GameObject pauseButton = null;
  public bool isPaused = false;
  //-------------
  public GameObject janitorButton = null;
  //-------------
  public GameObject pausePanel = null;
  //-------------
  public GameObject levelCompletedPanel = null;
  public GameObject levelFailedPanel = null;
  //-------------
  public GameObject janitorOverlayGameObject = null;
  //-------------
  public GameObject currentScoreLabel = null;
  public GameObject perfectScoreLabel = null;
  public GameObject colorGUIScoreRating = null;
  //-------------
  bool failedLevel = false;
  bool passedLevel = false;

  bool levelChangeTriggered = false;

	//BEGINNING OF SINGLETON CODE CONFIGURATION5
	private static volatile LevelManager _instance;
	private static object _lock = new object();

	//Stops the lock being created ahead of time if it's not necessary
	static LevelManager() {
	}

	public static LevelManager Instance {
		get {
			if(_instance == null) {
				lock(_lock) {
					if (_instance == null) {

						GameObject levelManagerGameObject = new GameObject("LevelGUIManagerGameObject");
						_instance = (levelManagerGameObject.AddComponent<LevelManager>()).GetComponent<LevelManager>();
					}
				}
			}
			return _instance;
		}
	}

	private LevelManager() {
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
    // TweenExecutor.TweenObjectColor(colorGUIScoreRating, Color.red, Color.green, 0, 10, UITweener.Method.Linear, null);
	}

	// Update is called once per frame
	void Update () {
    UpdateScoreComponents();
	}

  public void TogglePause() {
    isPaused = !isPaused;

    if(isPaused) {
      pausePanel.GetComponent<UIPanel>().alpha = 1;

      ScoreManager.Instance.isPaused = true;
      BroGenerator.Instance.isPaused = true;
      WaveManager.Instance.isPaused = true;
      SoundManager.Instance.TogglePause(true, AudioType.GreendogIntroAztecTemples);
    }
    else {
      pausePanel.GetComponent<UIPanel>().alpha = 0;

      ScoreManager.Instance.isPaused = false;
      BroGenerator.Instance.isPaused = false;
      WaveManager.Instance.isPaused = false;
      SoundManager.Instance.TogglePause(false);
    }
  }

  public void HideUI() {
    TweenExecutor.TweenObjectAlpha(uiRootGameObject, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowUI() {
    TweenExecutor.TweenObjectAlpha(uiRootGameObject, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
  public void HidePauseButton() {
    TweenExecutor.TweenObjectAlpha(pauseButton, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowPauseButton() {
    TweenExecutor.TweenObjectAlpha(pauseButton, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
  public void HideJanitorButton() {
    TweenExecutor.TweenObjectAlpha(janitorButton, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowJanitorButton() {
    TweenExecutor.TweenObjectAlpha(janitorButton, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
  public void HideJanitorOverlay() {
    TweenExecutor.TweenObjectAlpha(janitorOverlayGameObject, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowJanitorOverlay() {
    TweenExecutor.TweenObjectAlpha(janitorOverlayGameObject, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
  public void HideLevelCompletedPanel() {
    TweenExecutor.TweenObjectAlpha(levelCompletedPanel, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowLevelCompletedPanel() {
    TweenExecutor.TweenObjectAlpha(levelCompletedPanel, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }
  public void HideLevelFailedPanel() {
    TweenExecutor.TweenObjectAlpha(levelFailedPanel, 1, 0, 0, 1, UITweener.Method.Linear, null);
  }
  public void ShowLevelFailedPanel() {
    TweenExecutor.TweenObjectAlpha(levelFailedPanel, 0, 1, 0, 1, UITweener.Method.Linear, null);
  }

  public void UpdateScoreComponents() {
    if(currentScoreLabel != null
       && currentScoreLabel.GetComponent<UILabel>()  != null) {
       // currentScoreLabel.GetComponent<UILabel>().text = "Score: " + ScoreManager.Instance.playerOneScoreTracker.CalculateCurrentScore();
       currentScoreLabel.GetComponent<UILabel>().text = "Current Score: " + ScoreManager.Instance.playerOneScoreTracker.currentScore;
    }
    if(perfectScoreLabel != null
       && perfectScoreLabel.GetComponent<UILabel>()  != null) {
       // perfectScoreLabel.GetComponent<UILabel>().text = "Score: " + ScoreManager.Instance.playerOneScoreTracker.CalculateCurrentScore();
       perfectScoreLabel.GetComponent<UILabel>().text = "Perfect Score: " + ScoreManager.Instance.playerOneScoreTracker.perfectScore;
    }
    if(colorGUIScoreRating != null
       && colorGUIScoreRating.GetComponent<UISprite>()) {
      float scoreRatio =  0;
      // Debug.Log(ScoreManager.Instance.playerOneScoreTracker.perfectScore);
      if(ScoreManager.Instance.playerOneScoreTracker.perfectScore > 0) {
        scoreRatio = (((float)ScoreManager.Instance.playerOneScoreTracker.currentScore)/((float)ScoreManager.Instance.playerOneScoreTracker.perfectScore));
        // Debug.Log("Score Ratio: " + scoreRatio);
      }
      colorGUIScoreRating.GetComponent<UISprite>().color = Color.Lerp(Color.red, Color.green, scoreRatio);
    }
  }

  public void TriggerFailedLevel() {
    if(!passedLevel
       && !failedLevel) {
      failedLevel = true;
      // Change to be level failed panel
      ShowLevelFailedPanel();
    }
  }
  public void TriggerFinishedLevel() {
    if(!passedLevel
       && !failedLevel) {
      passedLevel = true;
      ShowLevelCompletedPanel();
    }
  }

  public void TriggerLevelChangeToScoreMenu() {
    TriggerLevelChange("ScoreMenu");
  }

  void TriggerLevelChange(string newSceneToChangeTo) {
    if(!levelChangeTriggered) {
      sceneToChangeTo = newSceneToChangeTo;
      FadeManager.Instance.SetFadeFinishLogic(ChangeToScene);
      FadeManager.Instance.PerformFade(Color.clear, Color.white, 1, false);

      HideUI();

      BroGenerator.Instance.isPaused = true;
      ScoreManager.Instance.isPaused = true;
    }
  }

  public void ChangeToScene() {
    Application.LoadLevel(sceneToChangeTo);
  }
}
