using UnityEngine;
using System.Collections;

public class Vomit : BathroomTileBlocker {
	public override void Start() {
		base.Start();
		bathroomTileBlockerType = BathroomTileBlockerType.Vomit;
        tappableReference.ResetTaps();
	}
    public override void Update() {
        if(tappableReference.tapLimitReached) {
            SelfDestruct();
        }
        PerformSpriteScaling(); 
    }

	public override void OnMouseDown() {
		// Debug.Log("Clicked");
	}

    public override void UpdateAnimator() {
        // animatorReference.SetBool("TriggerFadeOutAndDestroy", triggerFadeOutAndDestroy);
    }

    public void PerformSpriteScaling() {
        if(bathroomTileBlockerSpriteGameObject != null) {
            float currentTapRatio = tappableReference.GetTapRatio();
            float currentSpriteScale = 1 - currentTapRatio;
            if(bathroomTileBlockerSpriteGameObject.transform.localScale.x != currentSpriteScale
               && bathroomTileBlockerSpriteGameObject.transform.localScale.y != currentSpriteScale) {
                bathroomTileBlockerSpriteGameObject.transform.localScale = new Vector3(currentSpriteScale, currentSpriteScale, bathroomTileBlockerSpriteGameObject.transform.localScale.z);
            }
        }
    }
}
