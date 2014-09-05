using UnityEngine;
using System.Collections;

public class HideMissionBriefingOnTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void OnPress() {
    CareerMenuManager.Instance.missionBriefing.GetComponent<UIPanel>().alpha = 0;
  }
}
