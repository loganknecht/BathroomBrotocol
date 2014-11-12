using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bro : TargetPathingNPC {
	public HighlightSelectable selectableReference = null;

	public BroType type = BroType.None;
	public BroState state = BroState.None;

	public float occupationTimer = 0f;

  public bool skipLineQueue = false;
  public bool chooseRandomBathroomObjectOnSkipLineQueue = false;
	public bool hasRelievedSelf = false;
  public bool chooseRandomBathroomObjectAfterRelieved = false;
  public bool hasWashedHands = false;
  public bool chooseRandomBathroomObjectAfterWashedHands = false;
  public bool hasDriedHands = false;

  public bool startRoamingOnArrivalAtBathroomObjectInUse = false;

  public GameObject standOffBroGameObject = null;
	public GameObject broFightingWith = null;
  public GameObject lineQueueIn = null;

	public ReliefRequired reliefRequired = ReliefRequired.None;

	public GameObject speechBubbleGameObject = null;
	public SpeechBubble speechBubbleReference = null;

	public float probabilityOfFightOnCollisionWithBro = 0.15f;
	public bool isPaused = false;

  public bool resetFightLogic = false;
  public float fightCooldownTimer = 0f;
  public float fightCooldownTimerMax = 2f;


	public override void Awake() {
		selectableReference = this.gameObject.GetComponent<HighlightSelectable>();
		speechBubbleReference = speechBubbleGameObject.GetComponent<SpeechBubble>();
	}
	// Use this for initialization
	public override void Start () {
		base.Start();

    // this.gameObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

		selectableReference.canBeSelected = true;
	}

	// Update is called once per frame
	public override void Update () {
		if(!isPaused) {
			base.Update();
			PerformSpeechBubbleLogic();
      PerformFightTimerLogic();
		}
	}

  public virtual void Pause() {
    isPaused = true;
    if(this.gameObject.GetComponent<FartGenerator>() != null) {
      this.gameObject.GetComponent<FartGenerator>().isPaused = true;
    }
  }
  public virtual void Unpause() {
    isPaused = false;
    if(this.gameObject.GetComponent<FartGenerator>() != null) {
      this.gameObject.GetComponent<FartGenerator>().isPaused = false;
    }
  }

  public virtual bool IsExiting() {
    if(targetObject != null
      && targetObject.GetComponent<BathroomObject>() != null
      && targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
      return true;
    }
    else {
      return false;
    }
  }

  public override void PopMovementNode() {
    if(movementNodes.Count > 0) {
    //Debug.Log("Arrived at: " + targetPosition.x + ", " + targetPosition.y);
      GameObject nextNode = movementNodes[0];
      BathroomTile nextTile = nextNode.GetComponent<BathroomTile>();

      if(nextTile != null
         && nextTile.bathroomTileBlockers.Count == 0) {
        targetPosition = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, this.transform.position.z);
        // Debug.Log("Set new position to: " + targetPosition.x + ", " + targetPosition.y);
        movementNodes.RemoveAt(0);
        // Destroy(nextNode);
        // Debug.Log(this.gameObject.name + " has " + movementNodes.Count + " number of movemeNodes");
      }
      else {
        movementNodes.Clear();
        state = BroState.Roaming;
      }
    }
    if(movementNodes == null) {
      Debug.Log("movemeNodes is null for " + this.gameObject.name);
    }
  }

  public virtual void PerformFightTimerLogic() {
    if(resetFightLogic) {
      fightCooldownTimer += Time.deltaTime;
      if(fightCooldownTimer > fightCooldownTimerMax) {
        fightCooldownTimer = 0;
        resetFightLogic = false;
      }
    }
  }
  public virtual void ResetFightLogic(float fightResetDuration = 2f) {
    resetFightLogic = true;
    fightCooldownTimer = 0;
    fightCooldownTimerMax = fightResetDuration;
  }

	public virtual void OnMouseDown() {
    // Debug.Log("clicked");
    SelectionManager.Instance.SelectBro(this.gameObject);
    if(state == BroState.Standoff) {
      if(standOffBroGameObject != null) {
        standOffBroGameObject.GetComponent<StandoffBros>().IncrementTapsFromPlayer();
      }
    }
	}

  //  public void OnCollisionEnter(Collision collision) {
  //    Debug.Log("Collision occurred with: " + collision.gameObject.name);
  //  }

	public void OnTriggerEnter(Collider other) {
		// Debug.Log("Trigger occurred with: " + other.gameObject.name);
    Bro otherBroRef = other.gameObject.GetComponent<Bro>();
		if(otherBroRef != null) {
			//------------------------------------------------------------
			if((state == BroState.MovingToTargetObject || state == BroState.Roaming)
         && !IsExiting()
         && !resetFightLogic
			   && probabilityOfFightOnCollisionWithBro > 0
			   && (otherBroRef.state == BroState.MovingToTargetObject || otherBroRef.state == BroState.Roaming)
         && !otherBroRef.IsExiting()
         && !otherBroRef.resetFightLogic
			   && otherBroRef.probabilityOfFightOnCollisionWithBro > 0) {

				float  checkToSeeIfFightOccurs = Random.Range(0.0f, 1f);
				if(checkToSeeIfFightOccurs < probabilityOfFightOnCollisionWithBro) {
					if(state != BroState.Fighting) {
						broFightingWith = other.gameObject;
						state = BroState.Standoff;
						speechBubbleReference.displaySpeechBubble = false;
						movementNodes.Clear();

						otherBroRef.movementNodes.Clear();
						otherBroRef.broFightingWith = this.gameObject;
						otherBroRef.speechBubbleReference.displaySpeechBubble = false;
						otherBroRef.state = BroState.Standoff;
					}
				}
        else {
          ResetFightLogic(0.5f);
        }
			}
		}
	}

	public override void UpdateAnimator() {
		if(animatorReference != null) {
			base.UpdateAnimator();

			animatorReference.SetBool(BroState.Fighting.ToString(), false);
			animatorReference.SetBool(BroState.InAQueue.ToString(), false);
			animatorReference.SetBool(BroState.MovingToTargetObject.ToString(), false);
			animatorReference.SetBool(BroState.OccupyingObject.ToString(), false);
			animatorReference.SetBool(BroState.Roaming.ToString(), false);
			animatorReference.SetBool(BroState.Standing.ToString(), false);
			animatorReference.SetBool(BroState.Standoff.ToString(), false);

			animatorReference.SetBool(state.ToString(), true);

			animatorReference.SetBool("None", false);
		}
	}

	public override void SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
		occupationTimer = 0;
		base.SetTargetObjectAndTargetPosition(newTargetObject, newMovementNodes);
	}

	public override void PerformLogic() {
		switch(state) {
			case(BroState.None):
			break;
			case(BroState.Fighting):
			break;
			case(BroState.InAQueue):
				PerformInAQueueLogic();
			break;
			case(BroState.MovingToTargetObject):
				PerformMovingToTargetObjectLogic();
			break;
			case(BroState.OccupyingObject):
				PerformOccupyingObjectLogic();
			break;
			case(BroState.Roaming):
				PerformRoamingLogic();
			break;
			case(BroState.Standing):
				PerformStandingLogic();
			break;
			case(BroState.Standoff):
				PerformStandOffLogic();
			break;
			default:
			break;
		}
	}

	public virtual void PerformArrivalLogic() {
		if(IsAtTargetPosition()) {
			if(targetObject != null
			   && targetObject.GetComponent<BathroomObject>() != null) {
        BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.transform.position.x, this.transform.position.y, false).GetComponent<BathroomTile>();
        BathroomTile targetObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(targetObject.transform.position.x, targetObject.transform.position.y, false).GetComponent<BathroomTile>();

        if(broTile.tileX == targetObjectTile.tileX && broTile.tileY == targetObjectTile.tileY) {

  				BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

          if(bathObjRef.objectsOccupyingBathroomObject.Count > 0
            && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit
            && startRoamingOnArrivalAtBathroomObjectInUse) {
            state = BroState.Roaming;
          }
          else {
            PerformOnArrivalBrotocolScoreCheck();

    				//Adds bro to occupation list
    				if(!bathObjRef.objectsOccupyingBathroomObject.Contains(this.gameObject)) {
               // wtf, why is this here?
    					bathObjRef.AddBro(this.gameObject);
    				}

    				selectableReference.canBeSelected = false;
    				selectableReference.ResetHighlightObjectAndSelectedState();
    				speechBubbleReference.displaySpeechBubble = false;

    				if(SelectionManager.Instance.currentlySelectedBroGameObject != null
    				   && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
    					SelectionManager.Instance.currentlySelectedBroGameObject = null;
    				}

    				state = BroState.OccupyingObject;
          }
        }
			}
			else {
				state = BroState.Roaming;
			}
		}
	}
	public virtual void PerformFightingLogic() {
	}
	public virtual void PerformInAQueueLogic() {
		PerformMovementLogic();
    if(IsAtTargetPosition()) {
      if(skipLineQueue) {
        if(chooseRandomBathroomObjectOnSkipLineQueue) {
          SetRandomBathroomObjectTarget(true, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
        }
        else {
          state = BroState.Roaming;
        }

        lineQueueIn = null;
      }
      else {
        // skip line queue isn't true and the bro should just wait here
      }
    }
	}
	public virtual void PerformMovingToTargetObjectLogic() {
		PerformMovementLogic();
		PerformArrivalLogic();
	}

	public virtual void PerformOccupyingObjectLogic() {
		if(targetObject != null
		   && targetObject.GetComponent<BathroomObject>() != null) {
			BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

			if(occupationTimer > bathObjRef.occupationDuration) {
        // Debug.Log("occupation finished");
        if(bathObjRef.type == BathroomObjectType.Exit) {
          PerformExitOccupationFinishedLogic();
        }
        else if(bathObjRef.type == BathroomObjectType.HandDryer) {
          PerformHandDryerOccupationFinishedLogic();
        }
        else if(bathObjRef.type == BathroomObjectType.Sink) {
          PerformSinkOccupationFinishedLogic();
        }
				else if(bathObjRef.type == BathroomObjectType.Stall) {
          PerformStallOccupationFinishedLogic();
				}
				else if(bathObjRef.type == BathroomObjectType.Urinal) {
          PerformUrinalOccupationFinishedLogic();
				}
			}
			else {
				//disables the collider because the bro resides in the object, but the timer is still going
				collider.enabled = false;

				occupationTimer += Time.deltaTime;
			}
		}
	}

  public virtual void PerformExitOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    PerformExitedScore();
    BroManager.Instance.RemoveBro(this.gameObject, false);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    // bathObjRef.PerformOutOfOrderCheck();

    EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(this.gameObject);
    Destroy(this.gameObject);
  }

  //===========================================================================
  public virtual void PerformHandDryerOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    // Bathroom object is out of order
    if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Out of Order - has not relieved self");
        PerformOutOfOrderHandDryerRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Out of Order - has not washed hands");
        PerformOutOfOrderHandDryerWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Out of Order - has not dried hands");
        PerformOutOfOrderHandDryerDryHands();
      }
    }
    // Bathroom object is broken
    else if(bathObjRef.state == BathroomObjectState.Broken 
            || bathObjRef.state == BathroomObjectState.BrokenByPee
            || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Broken - has not relieved self");
        PerformBrokenHandDryerRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Broken - has not washed hands");
        PerformBrokenHandDryerWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Broken - has not dried hands");
        PerformBrokenHandDryerDryHands();
      }
    }
    // Bathroom object is not broken or out of order
    else {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Working - has not relieved self");
        PerformWorkingHandDryerRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Working - has not washed hands");
        PerformWorkingHandDryerWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Working - has not dried hands");
        PerformWorkingHandDryerDryHands();
      }
    }
  }
  public virtual void PerformOutOfOrderHandDryerRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;
    PerformRelievedScore();

    collider.enabled = true;
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBrokenByPee);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBrokenByPoop);
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
      bathObjRef.state = BathroomObjectState.Broken;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBroken);
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }
  public virtual void PerformOutOfOrderHandDryerWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;
    PerformWashedHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBroken);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformOutOfOrderHandDryerDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformDriedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerHandsDriedIn);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = false;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformWorkingHandDryerRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    hasRelievedSelf = true;
    PerformRelievedScore();

    collider.enabled = true;
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBrokenByPee);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBrokenByPoop);
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
      bathObjRef.state = BathroomObjectState.Broken;
      // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBroken);
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }
  public virtual void PerformWorkingHandDryerWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;
    PerformWashedHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerBroken);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
 

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingHandDryerDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformDriedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.HandDryerHandsDriedIn);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = false;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformBrokenHandDryerRelief() {
    Debug.Log("Bro is relieving himself in a broken hand dryer. This shouldn't have happened");
  }
  public virtual void PerformBrokenHandDryerWashHands() {
    Debug.Log("Bro is washing his hands in a broken hand dryer. This shouldn't have happened");
  }
  public virtual void PerformBrokenHandDryerDryHands() {
    Debug.Log("Bro is drying his hands in a broken hand dryer. This shouldn't have happened");
  }
  //===========================================================================
  public virtual void PerformSinkOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    // Bathroom object is out of order
    if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Out of Order - has not relieved self");
        PerformOutOfOrderSinkRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Out of Order - has not washed hands");
        PerformOutOfOrderSinkWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Out of Order - has not dried hands");
        PerformOutOfOrderSinkDryHands();
      }
    }
    // Bathroom object is broken
    else if(bathObjRef.state == BathroomObjectState.Broken 
            || bathObjRef.state == BathroomObjectState.BrokenByPee
            || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Broken - has not relieved self");
        PerformBrokenSinkRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Broken - has not washed hands");
        PerformBrokenSinkWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Broken - has not dried hands");
        PerformBrokenSinkDryHands();
      }
    }
    // Bathroom object is not broken or out of order
    else {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Working - has not relieved self");
        PerformWorkingSinkRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Working - has not washed hands");
        PerformWorkingSinkWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Working - has not dried hands");
        PerformWorkingSinkDryHands();
      }
    }
  }

  public virtual void PerformOutOfOrderSinkRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    hasRelievedSelf = true;
    PerformRelievedScore();

    collider.enabled = true;
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

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }

  public virtual void PerformOutOfOrderSinkWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    hasWashedHands = true;
    PerformWashedHandsScore();

    collider.enabled = true;
    bathObjRef.state = BathroomObjectState.Broken;
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }

  public virtual void PerformOutOfOrderSinkDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    hasDriedHands = true;
    PerformDriedHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }

  public virtual void PerformWorkingSinkRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    hasRelievedSelf = true;
    PerformRelievedScore();

    collider.enabled = true;
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

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }

  public virtual void PerformWorkingSinkWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkHandsWashedIn);
    PerformWashedHandsScore();

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }

  public virtual void PerformWorkingSinkDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformRelievedScore();

    bathObjRef.state = BathroomObjectState.Broken;
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;

    reliefRequired = ReliefRequired.None;
  }

  public virtual void PerformBrokenSinkRelief() {
    Debug.Log("Bro is relieving himself in a broken sink. This shouldn't have happened");
  }

  public virtual void PerformBrokenSinkWashHands() {
    Debug.Log("Bro is washing his hands in a broken sink. This shouldn't have happened");
  }

  public virtual void PerformBrokenSinkDryHands() {
    Debug.Log("Bro is drying his hands in a broken sink. This shouldn't have happened");
  }
  //===========================================================================
  public virtual void PerformStallOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) { 
        // Debug.Log("Out of Order - has not relieved self");
        PerformOutOfOrderStallRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Out of Order - has not washed hands");
        PerformOutOfOrderStallWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Out of Order - has not dried hands");
        PerformOutOfOrderStallDryHands();
      }
    }
    // Bathroom object is broken
    else if(bathObjRef.state == BathroomObjectState.Broken 
            || bathObjRef.state == BathroomObjectState.BrokenByPee
            || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Broken - has not relieved self");
        PerformBrokenStallRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Broken - has not washed hands");
        PerformBrokenStallWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Broken - has not dried hands");
        PerformBrokenStallDryHands();
      }
    }
    // Bathroom object is not broken or out of order
    else {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
        // Debug.Log("Working - has not relieved self");
        PerformWorkingStallRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Working - has not washed hands");
        PerformWorkingStallWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Working - has not dried hands");
        PerformWorkingStallDryHands();
      }
    }
  }

  public virtual void PerformOutOfOrderStallRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;
    PerformRelievedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPeedIn);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPoopedIn);
    }

    collider.enabled = true;

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;

    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush1);
  }
  public virtual void PerformOutOfOrderStallWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;
    PerformWashedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallWashedHandsIn);
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;

    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformOutOfOrderStallDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformDriedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallDriedHandsIn);
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = false;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;

    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformWorkingStallRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;
    PerformRelievedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPeedIn);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallPoopedIn);
    }

    collider.enabled = true;

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;

    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush1);
  }
  public virtual void PerformWorkingStallWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;
    PerformWashedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallWashedHandsIn);
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;

    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingStallDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformDriedHandsScore();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallDriedHandsIn);
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    collider.enabled = false;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;

    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformBrokenStallRelief() {
    Debug.Log("Bro is relieving himself in a broken stall. This shouldn't have happened");
  }
  public virtual void PerformBrokenStallWashHands() {
    Debug.Log("Bro is washing his hands in a broken stall. This shouldn't have happened");
  }
  public virtual void PerformBrokenStallDryHands() {
    Debug.Log("Bro is drying his hands in a broken stall. This shouldn't have happened");
  } 
  //===========================================================================
  public virtual void PerformUrinalOccupationFinishedLogic() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) { 
        // Debug.Log("Out of Order - has not relieved self");
        PerformOutOfOrderUrinalRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Out of Order - has not washed hands");
        PerformOutOfOrderUrinalWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Out of Order - has not dried hands");
        PerformOutOfOrderUrinalDryHands();
      }
    }
    // Bathroom object is broken
    else if(bathObjRef.state == BathroomObjectState.Broken 
            || bathObjRef.state == BathroomObjectState.BrokenByPee
            || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) { 
        // Debug.Log("Broken - has not relieved self");
        PerformBrokenUrinalRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Broken - has not washed hands");
        PerformBrokenUrinalWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Broken - has not dried hands");
        PerformBrokenUrinalDryHands();
      }
    }
    // Bathroom object is not broken or out of order
    else {
      // Fall through logic for the bro bathroom lifecycle
      if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) { 
        // Debug.Log("Working - has not relieved self");
        PerformWorkingUrinalRelief();
      }
      else if(reliefRequired == ReliefRequired.WashHands) {
        // Debug.Log("Working - has not washed hands");
        PerformWorkingUrinalWashHands();
      }
      else if(reliefRequired == ReliefRequired.DryHands) {
        // Debug.Log("Working - has not dried hands");
        PerformWorkingUrinalDryHands();
      }
    }
  }
  public virtual void PerformOutOfOrderUrinalRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.Broken;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPeedIn);
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPoopedIn);
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
      SoundManager.Instance.Play(AudioType.Pooping1);
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasRelievedSelf = true;
    PerformRelievedScore();
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush2);
  }
  public virtual void PerformOutOfOrderUrinalWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalWashedHands);
    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasWashedHands = true;
    PerformWashedHandsScore();
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformOutOfOrderUrinalDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    
    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalWashedHands);
    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasDriedHands = true;
    PerformDriedHandsScore();
    collider.enabled = true;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;

    ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
  }
  public virtual void PerformWorkingUrinalRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    if(reliefRequired == ReliefRequired.Pee) {
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPeedIn);
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalPoopedIn);
      ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
      SoundManager.Instance.Play(AudioType.Pooping1);
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasRelievedSelf = true;
    PerformRelievedScore();
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush2);
  }
  public virtual void PerformWorkingUrinalWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalWashedHands);
    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasWashedHands = true;
    PerformWashedHandsScore();
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingUrinalDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    
    // ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalWashedHands);
    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBro(this.gameObject);
    bathObjRef.IncrementTimesUsed();
    bathObjRef.PerformOutOfOrderCheck();

    hasDriedHands = true;
    PerformDriedHandsScore();
    collider.enabled = true;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;

    ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
  }
  public virtual void PerformBrokenUrinalRelief() {
    Debug.Log("Bro is relieving himself in a broken urinal. This shouldn't have happened");
  }
  public virtual void PerformBrokenUrinalWashHands() {
    Debug.Log("Bro is washing his hands in a broken urinal. This shouldn't have happened");
  }
  public virtual void PerformBrokenUrinalDryHands() {
    Debug.Log("Bro is drying his hands in a broken urinal. This shouldn't have happened");
  } 

  //===========================================================================
  public virtual void PerformRoamingLogic() {
    PerformMovementLogic();

    if(IsAtTargetPosition()) {
      bool foundEmptyTile = false;
      GameObject randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
      while(!foundEmptyTile) {
        if(AStarManager.Instance.permanentlyClosedNodes.Contains(randomBathroomTile)) {
          randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
        }
        else {
          foundEmptyTile = true;
        }
      }

      // Debug.Log("Start Position X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y);
      BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
      List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                startTile,
                                                                                randomBathroomTile.GetComponent<BathroomTile>());
      SetTargetObjectAndTargetPosition(null, movementNodes);
    }
  }

	public virtual void PerformStandingLogic() {
	}

	public virtual void PerformStandOffLogic() {
		//GameObject newFightingBros = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/FightingBros") as GameObject);
		if(state == BroState.Standoff) {
      if(standOffBroGameObject == null) {
        this.gameObject.collider.enabled = false;
        broFightingWith.collider.enabled = false;
        broFightingWith.GetComponent<Bro>().selectableReference.ResetHighlightObjectAndSelectedState();

        selectableReference.ResetHighlightObjectAndSelectedState();
        Vector2 standoffAnchor = new Vector2(((this.gameObject.transform.position.x + broFightingWith.transform.position.x)/2), ((this.gameObject.transform.position.y + broFightingWith.transform.position.y)/2));

        standOffBroGameObject  = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/StandoffBros") as GameObject);
        broFightingWith.GetComponent<Bro>().standOffBroGameObject = standOffBroGameObject;
        standOffBroGameObject.GetComponent<StandoffBros>().StandoffBrosInit(this.gameObject, broFightingWith, standoffAnchor);

        BroManager.Instance.AddStandOffBros(standOffBroGameObject);
      }
		}
	}
	public virtual void PerformSpeechBubbleLogic() {
    if(reliefRequired == ReliefRequired.None
       && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.None) {
      speechBubbleReference.speechBubbleImage = SpeechBubbleImage.None;
    }
    else if(reliefRequired == ReliefRequired.DryHands
            && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.DryHands) {
      speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    }
		else if(reliefRequired == ReliefRequired.Pee
		   && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.Pee) {
			speechBubbleReference.speechBubbleImage = SpeechBubbleImage.Pee;
		}
		else if(reliefRequired == ReliefRequired.Poop
		   && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.Poop) {
			speechBubbleReference.speechBubbleImage = SpeechBubbleImage.Poop;
		}
		else if(reliefRequired == ReliefRequired.WashHands
		   && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.WashHands) {
			speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
		}
	}

  public void SetRandomOpenBathroomObjectTarget(params BathroomObjectType[] bathroomObjectTypesToTarget) {
    BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                       this.gameObject.transform.position.y,
                                                                                       true).GetComponent<BathroomTile>();

    GameObject randomObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
    if(randomObject != null) {
      BathroomTile randomObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomObject.transform.position.x,
                                                                                              randomObject.transform.position.y,
                                                                                              true).GetComponent<BathroomTile>();
      //Debug.Log("setting exit tile");
      List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                new List<GameObject>(),
                                                                                broTile,
                                                                                randomObjectTile);
      state = BroState.MovingToTargetObject;
      SetTargetObjectAndTargetPosition(randomObject, movementNodes);
    }
    else {
      state = BroState.Roaming;
    }
  }

  public void SetRandomBathroomObjectTarget(bool chooseOpenBathroomObject, params BathroomObjectType[] bathroomObjectTypesToTarget) {
    BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                     this.gameObject.transform.position.y,
                                                                                     true).GetComponent<BathroomTile>();
    // Debug.Log(broTile);

    // List<GameObject> objects = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(bathroomObjectTypesToTarget);
    // int selectedObject = Random.Range(0, objects.Count);
    // GameObject randomObject = objects[selectedObject];
    GameObject randomObject = null;
    if(chooseOpenBathroomObject) {
      randomObject = BathroomObjectManager.Instance.GetRandomOpenBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
    }
    else {
      randomObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
    }

    if(randomObject) {
      // BathroomTile randomObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomObject.transform.position.x,
      //                                                                                           randomObject.transform.position.y,
      //                                                                                           true).GetComponent<BathroomTile>();
      BathroomTile randomObjectTile = randomObject.GetComponent<BathroomObject>().bathroomTileIn.GetComponent<BathroomTile>();

      //Debug.Log("setting exit tile");
      List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                broTile,
                                                                                randomObjectTile);
      state = BroState.MovingToTargetObject;
      SetTargetObjectAndTargetPosition(randomObject, movementNodes);
    }
    else {
      state = BroState.Roaming;
    }
  }

  //----------------------------------------------------------------------------
  // Brotocol Score Logic Goes Here
  //----------------------------------------------------------------------------
  //This is being checked on arrival before switching to occupying an object
  public virtual void PerformOnArrivalBrotocolScoreCheck() {
    if(targetObject != null
       && targetObject.GetComponent<BathroomObject>() != null
       && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
      if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
        // increment correct relief type
      }
      if(!CheckIfBroInAdjacentBathroomObjects()) {
        // increment bro alone bonus
      }
    }
  }

  // This is so dumb to document this
  /// <summary>This checks to see if the bro's target object is a bathroom object,
  /// and that their relief type matches the correct bathroom object type.
  /// </summary>
  /// <returns>True if target object is a bathroom object, and if reliefRequired
  /// matches the correct bathroom object.</returns>
  public virtual bool CheckIfBroHasCorrectReliefTypeForTargetObject() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    if(hasRelievedSelf == false
     // && (reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Vomit)
     && (reliefRequired == ReliefRequired.Pee)
     && bathObjRef != null
     && bathObjRef.type == BathroomObjectType.Urinal) {
      return true;
    }
    else if(hasRelievedSelf == false
            && (reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit)
            && bathObjRef != null
            && bathObjRef.type == BathroomObjectType.Stall) {
      return true;
    }
    else if(hasWashedHands == false
            && (reliefRequired == ReliefRequired.WashHands)
            && bathObjRef != null
            && bathObjRef.type == BathroomObjectType.Sink) {
      return true;
    }
    else {
      return false;
    }
  }

  // Returns true if any of the eight tiles around the bro has a bathroom object,
  // and if that bathroom object has a bro in it
 	public virtual bool CheckIfBroInAdjacentBathroomObjects() {
 		// bool broIsInAjdacentTile = false;

 		// BathroomTile currentTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
 		// 																	                                                   this.gameObject.transform.position.y,
 		// 																							                                       true).GetComponent<BathroomTile>();

   //  bool isBroOnTopLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY + 1);
   //  bool isBroOnTopSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX, currentTile.tileY + 1);
   //  bool isBroOnTopRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY + 1);

   //  bool isBroOnLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY);
   //  bool isBroOnRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY);

   //  bool isBroOnBottomLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY - 1);
   //  bool isBroOnBottomSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX, currentTile.tileY - 1);
   //  bool isBroOnBottomRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY - 1);
   //  // Debug.Log("------------------------");
   //  // Debug.Log("On Top Left: " + isBroOnTopLeftSide);
   //  // Debug.Log("On Top: " + isBroOnTopSide);
   //  // Debug.Log("On Top Right: " + isBroOnTopRightSide);
   //  // Debug.Log("On Left: " + isBroOnLeftSide);
   //  // Debug.Log("On Right: " + isBroOnRightSide);
   //  // Debug.Log("On Bottom Left: " + isBroOnBottomLeftSide);
   //  // Debug.Log("On Bottom: " + isBroOnBottomSide);
   //  // Debug.Log("On Bottom Right: " + isBroOnBottomRightSide);

   //  if(isBroOnTopLeftSide
   //    || isBroOnTopSide
   //    || isBroOnTopRightSide
   //    || isBroOnLeftSide
   //    || isBroOnRightSide
   //    || isBroOnBottomLeftSide
   //    || isBroOnBottomSide
   //    || isBroOnBottomRightSide) {
   //    // Debug.Log("Bro adjacent");
   //    broIsInAjdacentTile = true;
   //  }

 		// return broIsInAjdacentTile;
    return false;
 	}

  public virtual bool CheckIfRelievedSelfBeforeTimeOut() {
    Debug.Log("WARNING YOU HAVE CALLED THE BASE BRO METHOD 'CheckIfRelievedSelfBeforeTimeOut' BUT THE BASE BRO CLASS DOES NOT SUPPORT THIS METHOD AND IS MEANT TO BE EXTENDED");
    return false;
  }

  public virtual bool CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry() {
    Debug.Log("WARNING YOU HAVE CALLED THE BASE BRO METHOD 'CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry' BUT THE BASE BRO CLASS DOES NOT SUPPORT THIS METHOD AND IS MEANT TO BE EXTENDED");
    return false;
  }

	//--------------------------------------------------------------------
	public virtual void PerformEnteredScore() {
	}
	public virtual void PerformRelievedScore() {
	}
	public virtual void PerformWashedHandsScore() {
	}
  public virtual void PerformDriedHandsScore() {
  }
	public virtual void PerformBroFightScore() {
	}
	public virtual void PerformExitedScore() {
	}
}
