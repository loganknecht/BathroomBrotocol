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

    FadeManager.Instance.SetFadeFinishLogic(TriggerSceneChange);
    FadeManager.Instance.PerformFade(Color.clear, Color.white, 1, false);
  }

  void TriggerSceneChange() {
    Application.LoadLevel(levelToLoad);
  }
}
