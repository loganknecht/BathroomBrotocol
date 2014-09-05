using UnityEngine;
using System.Collections;

public class GenericBro : Bro {
	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.GenericBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

  //This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    // As long as the target object is not null and it's not a bathroom exit
    if(targetObject != null
     && targetObject.GetComponent<BathroomObject>() != null
     && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
      if(!hasRelievedSelf) {
        //This is being checked on arrival before switching to occupying an object
        if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
          // increment correct relief type
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject);
        }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroBrotocolNoAdjacentBro);
        }
      }
    }
  }

	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroExited);
	}
}
