using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

	public SpeechBubbleImage speechBubbleImage = SpeechBubbleImage.None;

	public bool displaySpeechBubble = false;
	Animator animatorReference = null;

	// Use this for initialization
	public void Start () {
		animatorReference = this.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void Update () {
		UpdateAnimator();
	}

	public void UpdateAnimator() {
		animatorReference.SetBool("DisplaySpeechBubble", displaySpeechBubble);

		animatorReference.SetBool(SpeechBubbleImage.None.ToString(), false);
		animatorReference.SetBool(SpeechBubbleImage.Pee.ToString(), false);
		animatorReference.SetBool(SpeechBubbleImage.Poop.ToString(), false);

		animatorReference.SetBool(speechBubbleImage.ToString(), true);
	}
}
