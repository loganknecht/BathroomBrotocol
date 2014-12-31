using UnityEngine;
using System.Collections;

public class Fart : BathroomTileBlocker {
    public float fartDurationTimer = 0f;
    public float fartDurationTimerMax = 2f;

    public bool triggerFadeOutAndDestroy = false;

    public override void Start() {
        base.Start();

        bathroomTileBlockerType = BathroomTileBlockerType.Fart;
    }

    public override void Update() {
        base.Update();
        PerformFartTimerLogic();
    }

    public override void OnMouseDown() {
        // Debug.Log("Clicked");
    }

    public override void UpdateAnimator() {
        animatorReference.SetBool("TriggerFadeOutAndDestroy", triggerFadeOutAndDestroy);
    }

    public void PerformFartTimerLogic() {
        fartDurationTimer += Time.deltaTime;
        if(fartDurationTimer > fartDurationTimerMax) {
            triggerFadeOutAndDestroy = true;
        }
    }
}
