using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	//--------------------------------------------------------------------
	public void CareerSlideLeft() {
//		this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
//		                                                      85,
//		                                                      this.gameObject.transform.localPosition.z);
		this.gameObject.GetComponent<TweenPosition>().delay = 0f;
		TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.25f, new Vector3(-500, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z));
		EventDelegate.Add(tweenPosition.onFinished, CareerSlideLeftComplete);
	}
	void CareerSlideLeftComplete() {
		//Debug.Log("FINISHED");
		StartMenuManager.Instance.nextState = "CareerMenu";
		TriggerFadeIn(1, 0, 0, 1, true);
	}
	//--------------------------------------------------------------------
	public void QuickPlaySlideRight() {
//		this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
//		                                                      0,
//		                                                      this.gameObject.transform.localPosition.z);
		this.gameObject.GetComponent<TweenPosition>().delay = 0f;
		TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.25f, new Vector3(500, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z));
		EventDelegate.Add(tweenPosition.onFinished, QuickPlaySlideRightComplete);
	}
	void QuickPlaySlideRightComplete() {
		//Debug.Log("FINISHED");
		//StartMenuManager.Instance.nextState = "";
		TriggerFadeIn(1, 0, 0, 1, true);
	}
	//--------------------------------------------------------------------
	public void OptionsSlideLeft() {
//		this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
//		                                                      -85,
//		                                                      this.gameObject.transform.localPosition.z);
		this.gameObject.GetComponent<TweenPosition>().delay = 0f;
		TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.25f, new Vector3(-500, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z));
		EventDelegate.Add(tweenPosition.onFinished, OptionsSlideLeftComplete);
	}
	void OptionsSlideLeftComplete() {
		//Debug.Log("FINISHED");
		TriggerFadeIn(1, 0, 0, 1, true);
	}
	//--------------------------------------------------------------------
	public void WindowSlideDown() {
		//		this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
		//		                                                      -85,
		//		                                                      this.gameObject.transform.localPosition.z);
		this.gameObject.GetComponent<TweenPosition>().delay = 0f;
		TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.25f, new Vector3(this.gameObject.transform.localPosition.x, -500, this.gameObject.transform.localPosition.z));
		EventDelegate.Add(tweenPosition.onFinished, WindowSlideDownComplete);
	}
	void WindowSlideDownComplete() {
		//Debug.Log("FINISHED");
		//TriggerFadeIn(5, 0, 0, 1);
	}
	//--------------------------------------------------------------------
	public void TitleSlideUp() {
		//		this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x,
		//		                                                      -85,
		//		                                                      this.gameObject.transform.localPosition.z);
		this.gameObject.GetComponent<TweenPosition>().delay = 0f;
		TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.25f, new Vector3(this.gameObject.transform.localPosition.x, 500, this.gameObject.transform.localPosition.z));
		EventDelegate.Add(tweenPosition.onFinished, TitleSlideUpComplete);
	}
	void TitleSlideUpComplete() {
		//Debug.Log("FINISHED");
		//TriggerFadeIn(5, 0, 0, 1);
	}
	//--------------------------------------------------------------------
	void TriggerFadeIn(float duration, float delay, float from, float to, bool changeState) {
		StartMenuManager.Instance.fadeBackground.GetComponent<TweenAlpha>().delay = delay;

		StartMenuManager.Instance.fadeBackground.GetComponent<TweenAlpha>().from = from;
		TweenAlpha tweenAlpha = TweenAlpha.Begin(StartMenuManager.Instance.fadeBackground, duration, to);
		if(changeState) {
			EventDelegate.Add(tweenAlpha.onFinished, ChangeState);
		}
	}

	void ChangeState() {
		Application.LoadLevel(StartMenuManager.Instance.nextState);
	}
	//--------------------------------------------------------------------
}
