using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {
	public Animator animatorReference;
	public BathroomFacing facing;

	// Use this for initialization
	public virtual void Start () {
		if(!animatorReference) {
			Debug.LogError("Animator Not Attached For: " + this.gameObject.name);
		}
		if(!facing) {
			Debug.LogError("Bathroom Facing Not Attached For: " + this.gameObject.name);
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		UpdateAnimator();
	}

	public virtual void UpdateAnimator() {
		facing.UpdateAnimatorWithFacing(animatorReference);
		// If there were more states, do that enum
		animatorReference.SetBool("Default", true);
	}
}
