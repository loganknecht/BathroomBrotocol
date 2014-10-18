using UnityEngine;
using System.Collections;

public class FartingBro : Bro {


	// Use this for initialization
	public override void Start () {
    base.Start();
    type = BroType.FartingBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

  //This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    bool brotocolWasSatisfied = false;

    // As long as the target object is not null and it's not a bathroom exit
    if(targetObject != null
        && targetObject.GetComponent<BathroomObject>() != null
        && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
      if(!hasRelievedSelf) {
        //This is being checked on arrival before switching to occupying an object
        if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
          // increment correct relief type
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject);
          brotocolWasSatisfied = true;
        }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroBrotocolNoAdjacentBro);
          brotocolWasSatisfied = true;
        }
      }
    }

    if(brotocolWasSatisfied) {
        SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
    }
  }

	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroExited);
	}
}
