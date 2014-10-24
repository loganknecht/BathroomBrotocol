using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkBro : Bro {
  public float vomitTimer = 0;
  public float vomitTimerMax = Random.Range(10, 15);
  public bool vomitThrowUpPerformed = false;

	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.DrunkBro;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
    PerformVomitTimerLogic();
	}

  public void PerformVomitTimerLogic() {
    if(!hasRelievedSelf) {
      if(state != BroState.InAQueue
         && state != BroState.Standoff
         && state != BroState.Fighting
         && state != BroState.OccupyingObject) {
        vomitTimer += Time.deltaTime;
      }
      if(vomitTimer > vomitTimerMax) {
        if(!vomitThrowUpPerformed) {
          vomitThrowUpPerformed = true;
          hasRelievedSelf = true;
          // GameObject newVomit = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Bathroom/BathroomTileBlockers/Vomit") as GameObject);
          GameObject newVomit = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Vomit);
          newVomit.transform.position = this.gameObject.transform.position;
          BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newVomit);

          BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
          List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);
          int selectedExit = Random.Range(0, exits.Count);
          GameObject randomExit = exits[selectedExit];
          BathroomTile randomExitTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomExit.transform.position.x, randomExit.transform.position.y, true).GetComponent<BathroomTile>();

          List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(), new List<GameObject>(), broTile, randomExitTile); state = BroState.MovingToTargetObject; SetTargetObjectAndTargetPosition(randomExit, movementNodes);
        }
      }
    }
  }

  // public override void PerformInAQueueLogic() {
  //   PerformMovementLogic();
  //   PerformArrivalLogic();
  // }

  public override void PerformOccupyingObjectLogic() {
    if(targetObject != null
       && targetObject.GetComponent<BathroomObject>() != null) {
      BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

      if(occupationTimer > bathObjRef.occupationDuration) {
        //OBJECT LOGIC ACTUALLY STARTS HERE
        if(!hasRelievedSelf) {
          if((bathObjRef.type == BathroomObjectType.Stall)
             || (bathObjRef.type == BathroomObjectType.Urinal)
             || (bathObjRef.type == BathroomObjectType.Sink)) {
            if(bathObjRef.type == BathroomObjectType.Stall) {
              ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallVomitedIn);
              PerformRelievedScore();
            }
            else if(bathObjRef.type == BathroomObjectType.Urinal) {
              ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalVomitedIn);
              PerformRelievedScore();
            }
            else if(bathObjRef.type == BathroomObjectType.Sink) {
              ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkVomitedIn);
              PerformRelievedScore();
            }

            hasRelievedSelf = true;
            reliefRequired = ReliefRequired.WashHands;

            bathObjRef.objectsOccupyingBathroomObject.Remove(this.gameObject);

            collider.enabled = true;
            selectableReference.canBeSelected = true;
            speechBubbleReference.displaySpeechBubble = true;

            SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
          }
          else {
            //bathroom object not stall, urinal, or sink
          }
        }
        else if(hasRelievedSelf
           && !hasWashedHands){
          hasWashedHands = true;

          if(bathObjRef.type == BathroomObjectType.Sink) {
            PerformWashedHandsScore();
            ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkHandsWashedIn);
          }
          else if(bathObjRef.type == BathroomObjectType.Stall) {
            bathObjRef.state = BathroomObjectState.Broken;
            ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);
          }
          else if(bathObjRef.type == BathroomObjectType.Urinal) {
            //destroy the bathroom object
            bathObjRef.state = BathroomObjectState.Broken;
            ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
          }

          bathObjRef.objectsOccupyingBathroomObject.Remove(this.gameObject);

          collider.enabled = false;
          selectableReference.canBeSelected = false;
          speechBubbleReference.displaySpeechBubble = false;

          SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        }
        else if(bathObjRef.type == BathroomObjectType.Exit) {
          PerformExitedScore();
          BroManager.Instance.allBros.Remove(this.gameObject);
          bathObjRef.objectsOccupyingBathroomObject.Remove(this.gameObject);
          EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(this.gameObject);
          Destroy(this.gameObject);
        }
      }
      else {
        //disables the collider because the bro resides in the object, but the timer is still going
        collider.enabled = false;

        occupationTimer += Time.deltaTime;
      }
    }
  }

  //--------------------------------------------------------
  //This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    bool brotocolWasSatisfied = false;

    // As long as the target object is not null and it's not a bathroom exit
    if(targetObject != null
     && targetObject.GetComponent<BathroomObject>() != null
     && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
      if(!hasRelievedSelf) {
        if(CheckIfRelievedSelfBeforeTimeOut()) {
          ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut);
          brotocolWasSatisfied = true;
        }
      }
    }

    if(brotocolWasSatisfied) {
      SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
    }
  }

  public override bool CheckIfRelievedSelfBeforeTimeOut() {
    if(vomitTimer > vomitTimerMax) {
      return false;
    }
    else {
      return true;
    }
  }
	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroRelieved);
  }
	public override void PerformWashedHandsScore() {
	ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroExited);
	}
}
