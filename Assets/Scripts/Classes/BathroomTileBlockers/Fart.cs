using UnityEngine;
using System.Collections;

public class Fart : BathroomTileBlocker {
  public float fartDurationTimer = 0f;
  public float fartDurationTimerMax = 2f;
  public bool fartDurationTimerMaxIsStochastic = false;
  public float minFartDurationTimerMax = 3f;
  public float maxFartDurationTimerMax = 5f;

  public bool triggerFadeOutAndDestroy = false;

  public Animator fartAnimationController = null;

  public override void Start() {
    base.Start();

    fartAnimationController = this.gameObject.GetComponent<Animator>();
    ResetTimer();
    bathroomTileBlockerType = BathroomTileBlockerType.Fart;
    repairDuration = 1f;
  }

  public override void Update() {
    base.Update();
    UpdateAnimator();
    PerformFartTimerLogic();
  }

  public void UpdateAnimator() {
    fartAnimationController.SetBool("TriggerFadeOutAndDestroy", triggerFadeOutAndDestroy);
  }

  //  public void OnMouseDown() {
  //    Debug.Log("Clicked");
  //  }

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

  public void SelfDestruct() {
    BathroomTileBlockerManager.Instance.RemoveBathroomTileBlockerGameObject(this.gameObject);
    Destroy(this.gameObject);
  }
}
