using UnityEngine;
using System.Collections;

public class CareerMenuNode : MonoBehaviour {

  public string levelTitle = "";
  public string levelToLoad = "";
  [Multiline]
  public string levelDescription = "";

  public bool isLocked = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  public void PerformLevelChange() {
    CareerMenuManager.Instance.HideUI();
    FadeManager.Instance.fadeFinishLogic = new FadeManager.FadeFinishEvent(TriggerSceneChange);
    StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.clear, Color.white, 1, false));
  }

  void TriggerSceneChange() {
    Application.LoadLevel(levelToLoad);
  }
}
