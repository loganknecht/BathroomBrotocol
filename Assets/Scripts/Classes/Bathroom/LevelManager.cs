using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public GameObject uiRootGameObject = null;
    //-------------
    public string sceneToChangeTo = "";
    //-------------
    public GameObject backgroundImage = null;
    //-------------
    public GameObject playerButtonsPanelGameObject = null;
    public bool isPaused = false;
    //-------------
    public GameObject janitorButtonGameObject = null;
    //-------------
    public GameObject rotationButtonPanelGameObject = null;
    //-------------
    public GameObject pausePanelGameObject = null;
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
                    if(_instance == null) {
                    
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
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        UpdateScoreComponents();
    }
    
    // TODO: Move to pause manager?
    public void TogglePause() {
        isPaused = !isPaused;
        
        if(isPaused) {
            pausePanelGameObject.GetComponent<UIPanel>().alpha = 1;
            
            ScoreManager.Instance.Pause();
            BroGenerator.Instance.Pause();
            WaveManager.Instance.Pause();
            SoundManager.Instance.TogglePause(true, AudioType.GreendogIntroAztecTemples);
        }
        else {
            pausePanelGameObject.GetComponent<UIPanel>().alpha = 0;
            
            ScoreManager.Instance.Unpause();
            BroGenerator.Instance.Unpause();
            WaveManager.Instance.Unpause();
            SoundManager.Instance.TogglePause(false);
        }
    }
    
    public LevelManager HideUI(float duration = 1f) {
        // TODO: What this do?!
        return this;
    }
    public LevelManager ShowUI(float duration = 1f) {
        // TODO: What this do?!
        return this;
    }
    
    public LevelManager HidePlayerButtons(float duration = 1f) {
        HidePanel(playerButtonsPanelGameObject, duration);
        return this;
    }
    public LevelManager ShowPlayerButtons(float duration = 1f) {
        ShowPanel(playerButtonsPanelGameObject, duration);
        return this;
    }
    
    public LevelManager HideRotationButtons(float duration = 1f) {
        HidePanel(rotationButtonPanelGameObject, duration);
        return this;
    }
    public LevelManager ShowRotationButtons(float duration = 1f) {
        ShowPanel(rotationButtonPanelGameObject, duration);
        return this;
    }
    
    public LevelManager HideJanitorButton(float duration = 1f) {
        // TODO: REMOVE THIS
        return this;
    }
    public LevelManager ShowJanitorButton(float duration = 1f) {
        // TODO: REMOVE THIS
        return this;
    }
    
    public LevelManager HideJanitorOverlay(float duration = 1f) {
        duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
        UIPanel panelToModify = levelCompletedPanel.GetComponent<UIPanel>();
        panelToModify.alpha = 1f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 0f));
        return this;
    }
    public LevelManager ShowJanitorOverlay(float duration = 1f) {
        duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
        UIPanel panelToModify = levelCompletedPanel.GetComponent<UIPanel>();
        panelToModify.alpha = 0f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 1f));
        return this;
    }
    
    public LevelManager HideLevelCompletedPanel(float duration = 1f) {
        HidePanel(levelCompletedPanel, duration);
        return this;
    }
    public LevelManager ShowLevelCompletedPanel(float duration = 1f) {
        ShowPanel(levelCompletedPanel, duration);
        return this;
    }
    
    public LevelManager HideLevelFailedPanel(float duration = 1f) {
        HidePanel(levelFailedPanel, duration);
        return this;
    }
    public LevelManager ShowLevelFailedPanel(float duration = 1f) {
        ShowPanel(levelFailedPanel, duration);
        return this;
    }
    
    public LevelManager ShowPanel(GameObject panelGameObject, float duration = 1f) {
        duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
        UIPanel panelToModify = panelGameObject.GetComponent<UIPanel>();
        panelToModify.alpha = 0f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 1f));
        return this;
    }
    
    public LevelManager HidePanel(GameObject panelGameObject, float duration = 1f) {
        duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
        UIPanel panelToModify = panelGameObject.GetComponent<UIPanel>();
        // TODO: Move to pause manager?
        panelToModify.alpha = 1f;
        Go.to(panelToModify,
              duration,
              new GoTweenConfig()
              .floatProp("alpha", 0f));
        return this;
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
                scoreRatio = (((float)ScoreManager.Instance.playerOneScoreTracker.currentScore) / ((float)ScoreManager.Instance.playerOneScoreTracker.perfectScore));
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
            
            BroGenerator.Instance.Pause();
            ScoreManager.Instance.Pause();
        }
    }
    
    public void ChangeToScene() {
        Application.LoadLevel(sceneToChangeTo);
    }
}
