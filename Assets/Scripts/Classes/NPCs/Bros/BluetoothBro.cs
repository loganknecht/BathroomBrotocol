using UnityEngine;
using System.Collections;

public class BluetoothBro : Bro {
	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.BluetoothBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

   // This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    // As long as the target object is not null and it's not a bathroom exit
    if(targetObject != null
     && targetObject.GetComponent<BathroomObject>() != null
     && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
      if(!hasRelievedSelf) {
        //This is being checked on arrival before switching to occupying an object
        if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
          // increment correct relief type
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject);
        }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroBrotocolNoAdjacentBro);
        }
      }
    }
  }

	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroExited);
	}
}
