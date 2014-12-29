using UnityEngine;
using System.Collections;

public class Fart : BathroomTileBlocker {
    public float fartDurationTimer = 0f;
    public float fartDurationTimerMax = 2f;
    public bool fartDurationTimerMaxIsStochastic = false;
    public float minFartDurationTimerMax = 3f;
    public float maxFartDurationTimerMax = 5f;

    public bool triggerFadeOutAndDestroy = false;

    public override void Start() {
        base.Start();

        ResetTimer();
        bathroomTileBlockerType = BathroomTileBlockerType.Fart;
    }

    public override void Update() {
        base.Update();
        UpdateAnimator();
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

    public void ResetTimer() {
        fartDurationTimer = 0f;
        if(fartDurationTimerMaxIsStochastic) {
            fartDurationTimerMax = Random.Range(minFartDurationTimerMax, maxFartDurationTimerMax);
        }
    }
}
