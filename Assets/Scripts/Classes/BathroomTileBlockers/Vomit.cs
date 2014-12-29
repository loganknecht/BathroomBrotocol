using UnityEngine;
using System.Collections;

public class Vomit : BathroomTileBlocker {
	public override void Start() {
		base.Start();
		bathroomTileBlockerType = BathroomTileBlockerType.Vomit;
        tappableReference.ResetTaps();
	}
    public override void Update() {
    }

	public override void OnMouseDown() {
		// Debug.Log("Clicked");
	}

    public override void UpdateAnimator() {
        // animatorReference.SetBool("TriggerFadeOutAndDestroy", triggerFadeOutAndDestroy);
    }
}
