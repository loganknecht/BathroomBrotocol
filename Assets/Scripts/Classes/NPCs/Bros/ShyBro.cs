﻿using UnityEngine;
using System.Collections;

public class ShyBro : Bro {
  public bool firstArrivalOccurred = false;
  public bool firstArrivalWasWrongObject = false;

	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.ShyBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

  public override void PerformArrivalLogic() {
    if(transform.position.x == targetPosition.x
       && transform.position.y == targetPosition.y
       && movementNodes.Count == 0) {

      if(targetObject != null
         && targetObject.GetComponent<BathroomObject>() != null) {

        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        //Adds bro to occupation list
        if(!bathObjRef.objectsOccupyingBathroomObject.Contains(this.gameObject)) {
          bathObjRef.objectsOccupyingBathroomObject.Add(this.gameObject);
        }

        // if(reliefRequired != ReliefRequired.Pee
        if(reliefRequired == ReliefRequired.Pee && bathObjRef.type == BathroomObjectType.Stall) {
          //Brotocol score check triggered
          PerformOnArrivalBrotocolScoreCheck();
        }
        else {
          targetObject.collider.enabled = false;
          if(!firstArrivalOccurred) {
            firstArrivalWasWrongObject = true;
          }
        }

        selectableReference.ResetHighlightObjectAndSelectedState();
        speechBubbleReference.displaySpeechBubble = false;

        if(SelectionManager.Instance.currentlySelectedBroGameObject != null
           && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
          SelectionManager.Instance.currentlySelectedBroGameObject = null;
        }

        if(!firstArrivalOccurred) {
          firstArrivalOccurred = true;
        }
        state = BroState.OccupyingObject;
      }
      else {
        state = BroState.Roaming;
      }
    }
  }

  public override void PerformOccupyingObjectLogic() {
    if(targetObject != null
       && targetObject.GetComponent<BathroomObject>() != null) {
      BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

      if(occupationTimer > bathObjRef.occupationDuration) {
        //OBJECT LOGIC ACTUALLY STARTS HERE
        if(bathObjRef.type == BathroomObjectType.Stall) {
          PerformStallOccupationFinishedLogic();
        }
        else if(bathObjRef.type == BathroomObjectType.Urinal
                && reliefRequired == ReliefRequired.WashHands) {
          PerformUrinalOccupationFinishedLogic();
        }
        else if(bathObjRef.type == BathroomObjectType.Sink) {
          PerformSinkOccupationFinishedLogic();
        }
        else if(bathObjRef.type == BathroomObjectType.Exit) {
          PerformExitOccupationLogic();
        }
      }
      else {
        if(targetObject != null
           && reliefRequired == ReliefRequired.Pee
           && (targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Urinal
              || targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Sink)) {
          // Debug.Log("in urinal");
          collider.enabled = true;
          probabilityOfFightOnCollisionWithBro = 0f;
          selectableReference.canBeSelected = true;
        }
        else {
          //disables the collider because the bro resides in the object, but the timer is still going
          collider.enabled = false;
          occupationTimer += Time.deltaTime;
        }
      }
    }
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
        // if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
        //   // increment correct relief type
        //   ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject);
        // }
        if(!CheckIfBroInAdjacentBathroomObjects()) {
          // increment bro alone bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolNoAdjacentBro);
          brotocolWasSatisfied = true;
        }
        if(CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry()) {
          // increment no janitor summoned bonus
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry);
          brotocolWasSatisfied = true;
        }
      }
    }

    if(brotocolWasSatisfied) {
      SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
    }
  }

  public override bool CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry() {
    if(!firstArrivalWasWrongObject) {
      return true;
    }
    else {
      return false;
    }
  }

	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroExited);
	}
}
