using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlobBro : Bro {
	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.SlobBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

  // public override void PerformOccupyingObjectLogic() {
  //   if(targetObject != null
  //      && targetObject.GetComponent<BathroomObject>() != null) {
  //     BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

  //     if(occupationTimer > bathObjRef.occupationDuration) {
  //       //OBJECT LOGIC ACTUALLY STARTS HERE
  //         if(reliefRequired == ReliefRequired.Pee) {
  //           bathObjRef.state = BathroomObjectState.BrokenByPee;
  //         }
  //         else if(reliefRequired == ReliefRequired.Poop) {
  //           bathObjRef.state = BathroomObjectState.BrokenByPoop;
  //         }
  //       }
  //       if(bathObjRef.bathroomObjectType == BathroomObjectType.Stall) {
  //         PerformStallOccupationFinishedLogic();
  //       }
  //       else if(bathObjRef.bathroomObjectType == BathroomObjectType.Urinal) {
  //         PerformUrinalOccupationFinishedLogic();
  //       }
  //       else if(bathObjRef.bathroomObjectType == BathroomObjectType.Sink) {
  //         PerformSinkOccupationFinishedLogic();
  //       }
  //       else if(bathObjRef.bathroomObjectType == BathroomObjectType.Exit) {
  //         PerformExitOccupationLogic();
  //       }
  //       SetRandomBathroomObjectTarget(BathroomObjectType.Exit);
  //     }
  //     else {
  //       //disables the collider because the bro resides in the object, but the timer is still going
  //       collider.enabled = false;

  //       occupationTimer += Time.deltaTime;
  //     }
  //   }
  // }
  public override void PerformStallOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(!hasRelievedSelf
       && bathObjRef.state != BathroomObjectState.OutOfOrder) {
      hasRelievedSelf = true;
      PerformRelievedScore();

      if(reliefRequired == ReliefRequired.Pee) {
        bathObjRef.TriggerOutOfOrderState();
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPeedIn);
      }
      else if(reliefRequired == ReliefRequired.Poop) {
        bathObjRef.TriggerOutOfOrderState();
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPoopedIn);
      }

      collider.enabled = true;

      // state = BroState.Roaming;
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
      bathObjRef.RemoveBro(this.gameObject);

      selectableReference.canBeSelected = true;
      speechBubbleReference.displaySpeechBubble = true;
      speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
      reliefRequired = ReliefRequired.WashHands;
    }
    else {
      collider.enabled = false;

      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

      bathObjRef.state = BathroomObjectState.Broken;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);
      probabilityOfFightOnCollisionWithBro = 0f;
    }
  }
  public override void PerformUrinalOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(!hasRelievedSelf
       && bathObjRef.state != BathroomObjectState.OutOfOrder) {
      hasRelievedSelf = true;
      PerformRelievedScore();

      //displaySpeechBubble = true;
      collider.enabled = true;

      // state = BroState.Roaming;
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
      bathObjRef.RemoveBro(this.gameObject);

      if(reliefRequired == ReliefRequired.Pee) {
        bathObjRef.TriggerOutOfOrderState();
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPeedIn);
      }
      else if(reliefRequired == ReliefRequired.Poop) {
        bathObjRef.state = BathroomObjectState.BrokenByPoop;
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPoopedIn);
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
      }

      selectableReference.canBeSelected = true;
      speechBubbleReference.displaySpeechBubble = true;
      speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
      reliefRequired = ReliefRequired.WashHands;
    }
    else {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

      bathObjRef.state = BathroomObjectState.Broken;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
    }
  }
  public override void PerformSinkOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(!hasRelievedSelf) {
      hasRelievedSelf = true;
      PerformRelievedScore();

      collider.enabled = true;
      // state = BroState.Roaming;
      bathObjRef.RemoveBro(this.gameObject);
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

      if(reliefRequired == ReliefRequired.Pee) {
        bathObjRef.state = BathroomObjectState.BrokenByPee;
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkPeedIn);
      }
      else if(reliefRequired == ReliefRequired.Poop) {
        bathObjRef.state = BathroomObjectState.BrokenByPoop;
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);
        ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkPoopedIn);
      }

      selectableReference.canBeSelected = true;
      speechBubbleReference.displaySpeechBubble = true;
      speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
      reliefRequired = ReliefRequired.WashHands;
    }
    else if(hasRelievedSelf
            && !hasWashedHands) {
      if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
        bathObjRef.state = BathroomObjectState.Broken;
      }
      else {
        bathObjRef.TriggerOutOfOrderState();
      }

      hasWashedHands = true;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkHandsWashedIn);
      PerformWashedHandsScore();

      bathObjRef.RemoveBro(this.gameObject);
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    }
    else {
      Debug.Log("SLOB BRO USED A SINK WHEN HE HAD WEIRD VALUES.");
    }
  }
  public override void PerformExitOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    PerformExitedScore();
    BroManager.Instance.allBros.Remove(this.gameObject);
    bathObjRef.objectsOccupyingBathroomObject.Remove(this.gameObject);
    EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(this.gameObject);
    Destroy(this.gameObject);
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
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject);
          brotocolWasSatisfied = true;
        }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroBrotocolNoAdjacentBro);
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
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroExited);
	}
}
