using UnityEngine;
using System.Collections;

public class FartingBro : Bro {

  public float fartProbability = 1f;
  public float fartTimer = 0f;
  public float fartTimerMax = 3f;
  public bool fartTimerMaxIsStochastic = false;
  public float minFartTimerMax = 2f;
  public float maxFartTimerMax = 4f;

	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.FartingBro;
    if(fartTimerMaxIsStochastic) {
      fartTimerMax = Random.Range(minFartTimerMax, maxFartTimerMax);
    }
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
    PerformFartTimerLogic();
	}

  public void PerformFartTimerLogic() {
    if(state == BroState.MovingToTargetObject
       || state == BroState.Roaming) {
      fartTimer += Time.deltaTime;

      if(fartTimer > fartTimerMax) {
        fartTimer = 0f;
        float fartProbabilityRolled = Random.value;
        if(fartProbability > fartProbabilityRolled) {
          //generate fart
          GameObject newFart = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Fart);
          newFart.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newFart.transform.position.z);
          BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newFart);
        }
      }

      //------------------------------
      if(fartTimerMaxIsStochastic) {
        fartTimerMax = Random.Range(minFartTimerMax, maxFartTimerMax);
      }
    }
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
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject);
        }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.FartingBroBrotocolNoAdjacentBro);
        }
      }
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
