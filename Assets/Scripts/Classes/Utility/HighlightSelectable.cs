using UnityEngine;
using System.Collections;

public class HighlightSelectable : Selectable {

	//THIS IS COUPLED WITH THE SELECTABLE CLASS
	public GameObject highlightObject = null;

	void Awake() {
	}
	// Use this for initialization
	void Start () {
		ResetHighlightObjectAndSelectedState();
	}
	
	// Update is called once per frame
	void Update () {
		if(isSelected) {
			highlightObject.SetActive(true);
		}
		else {
			highlightObject.SetActive(false);
		}
	}

	public void ResetHighlightObjectAndSelectedState() {
		isSelected = false;
		if(highlightObject != null) {
			highlightObject.SetActive(false);
		}
	}
}
