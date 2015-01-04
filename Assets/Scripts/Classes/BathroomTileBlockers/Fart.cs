using UnityEngine;
using System.Collections;

public class Fart : BathroomTileBlocker {
    public float durationTimer = 0f;
    public float duration = 2f;

    public bool triggerFadeOutAndDestroy = false;

    public override void Start() {
        base.Start();

        bathroomTileBlockerType = BathroomTileBlockerType.Fart;
    }

    public override void Update() {
        base.Update();
        PerformTimerLogic();
    }

    public override void OnMouseDown() {
        // Debug.Log("Clicked");
    }

    public override void UpdateAnimator() {
        animatorReference.SetBool("TriggerFadeOutAndDestroy", triggerFadeOutAndDestroy);
    }

    public void PerformTimerLogic() {
        durationTimer += Time.deltaTime;
        if(durationTimer > duration) {
            triggerFadeOutAndDestroy = true;
        }
    }
}
