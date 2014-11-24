using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bro : TargetPathingNPC {
    public HighlightSelectable selectableReference = null;

    public BroType type = BroType.None;
    public BroState state = BroState.None;

    public float occupationTimer = 0f;
    public Dictionary<BathroomObjectType, float> occupationDuration;

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

    public float baseProbabilityOfFightOnCollisionWithBro = 0.15f;
    public bool modifyBroFightProbablityUsingScoreRatio = false;

    public bool resetFightLogic = false;
    public float fightCooldownTimer = 0f;
    public float fightCooldownTimerMax = 2f;

    public bool isPaused = false;


    public override void Awake() {
        selectableReference = this.gameObject.GetComponent<HighlightSelectable>();
        speechBubbleReference = speechBubbleGameObject.GetComponent<SpeechBubble>();

        InitializeOccupationDuration();
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

    public virtual void InitializeOccupationDuration() {
        float defaultOccupationDuration = 2f;

        occupationDuration = new Dictionary<BathroomObjectType, float>();

        occupationDuration[BathroomObjectType.Exit] = 0;
        occupationDuration[BathroomObjectType.HandDryer] = defaultOccupationDuration;
        occupationDuration[BathroomObjectType.Sink] = defaultOccupationDuration;
        occupationDuration[BathroomObjectType.Stall] = defaultOccupationDuration;
        occupationDuration[BathroomObjectType.Urinal] = defaultOccupationDuration;
    }
    public virtual float GetOccupationDuration(BathroomObjectType bathroomObjectType) {
        return occupationDuration[bathroomObjectType];
    }
    public virtual void SetOccupationDuration(BathroomObjectType bathroomObjectType, float occupationDurationTime) {
        occupationDuration[bathroomObjectType] = occupationDurationTime;
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
            GameObject currentBroTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, false);
            BathroomTile currentBroTile = null;
            if(currentBroTileGameObject != null) {
                currentBroTile = currentBroTileGameObject.GetComponent<BathroomTile>();
            }

            GameObject nextNode = movementNodes[0];
            BathroomTile nextTile = nextNode.GetComponent<BathroomTile>();

            if(nextTile != null
               && (nextTile.bathroomTileBlockers.Count == 0 || currentBroTile == nextTile)) {
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

    // Returns the base probablity of fighting plus the modifier based on the score
    public virtual float GetFightProbability() {
        if(modifyBroFightProbablityUsingScoreRatio) {
            return (baseProbabilityOfFightOnCollisionWithBro); //TODO: GET SCORETRACKER TO RETURN THE MODIFIER BASE ON SCORE TRACKER'S PERFECT SCORE RATIO
        }
        else {
            return baseProbabilityOfFightOnCollisionWithBro;
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
                && baseProbabilityOfFightOnCollisionWithBro > 0
                && (otherBroRef.state == BroState.MovingToTargetObject || otherBroRef.state == BroState.Roaming)
                && !otherBroRef.IsExiting()
                && !otherBroRef.resetFightLogic
                && otherBroRef.baseProbabilityOfFightOnCollisionWithBro > 0) {

                float  checkToSeeIfFightOccurs = Random.Range(0.0f, 1f);
                if(checkToSeeIfFightOccurs < baseProbabilityOfFightOnCollisionWithBro) {
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

                if(broTile.tileX == targetObjectTile.tileX 
                    && broTile.tileY == targetObjectTile.tileY) {

                    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

                    if(bathObjRef.IsBroken()) {
                        state = BroState.Roaming;
                    }
                    else {
                        if(bathObjRef.objectsOccupyingBathroomObject.Count > 0
                            && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit
                            && startRoamingOnArrivalAtBathroomObjectInUse) {
                            state = BroState.Roaming;
                        }
                        else {
                            PerformOnArrivalBrotocolScoreCheck();

                            //Adds bro to occupation list
                            bathObjRef.AddBro(this.gameObject);

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

            if(occupationTimer > occupationDuration[bathObjRef.type]) {
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

    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

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

        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
          bathObjRef.state = BathroomObjectState.BrokenByPee;
          // PerformBrokeHandDryerByPeeingScore();
          PerformBrokeHandDryerByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Poop) {
          bathObjRef.state = BathroomObjectState.BrokenByPoop;
          // PerformBrokeHandDryerByPoopingScore();
          PerformBrokeHandDryerByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
          bathObjRef.state = BathroomObjectState.Broken;
          // PerformBrokeHandDryerByVomittingScore();
          PerformBrokeHandDryerByOutOfOrderUseScore();
        }

        if(chooseRandomBathroomObjectAfterRelieved) {
          SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
          state = BroState.Roaming;
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    public virtual void PerformOutOfOrderHandDryerWashHands() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasWashedHands = true;

        PerformBathroomObjectUsedScore();
        PerformBrokeHandDryerByOutOfOrderUseScore();

        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    public virtual void PerformOutOfOrderHandDryerDryHands() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasDriedHands = true;

        PerformBathroomObjectUsedScore();
        PerformBrokeHandDryerByOutOfOrderUseScore();

        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
  public virtual void PerformWorkingHandDryerRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      PerformBrokeHandDryerByPeeingScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      PerformBrokeHandDryerByPoopingScore();
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
      bathObjRef.state = BathroomObjectState.Broken;
      PerformBrokeHandDryerByVomittingScore();
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }
  public virtual void PerformWorkingHandDryerWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokenHandDryerWashHands();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingHandDryerDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;
    PerformBathroomObjectUsedScore();
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
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

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      // PerformBrokeSinkByPeeingScore();
      PerformBrokeSinkByOutOfOrderUseScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      // PerformBrokeSinkByPoopingScore();
      PerformBrokeSinkByOutOfOrderUseScore();
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
      bathObjRef.state = BathroomObjectState.Broken;
      // PerformBrokeSinkByVomittingScore();
      PerformBrokeSinkByOutOfOrderUseScore();
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }

  public virtual void PerformOutOfOrderSinkWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeSinkByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);


    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }

  public virtual void PerformOutOfOrderSinkDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeSinkByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }

  public virtual void PerformWorkingSinkRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      PerformBrokeSinkByPeeingScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      PerformBrokeSinkByPoopingScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.Broken;
      PerformBrokeSinkByVomittingScore();
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
        // do nothing
    }

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;
  }

  public virtual void PerformWorkingSinkWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();

    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }

  public virtual void PerformWorkingSinkDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeSinkByDryingHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
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

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Pee) {
        PerformBrokeStallByPeeingScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
        PerformBrokeStallByPoopingScore();
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
        PerformBrokeStallByVomittingScore();
    }

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);


    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush1);
  }
  public virtual void PerformOutOfOrderStallWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeStallByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformOutOfOrderStallDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeStallByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    collider.enabled = false;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformWorkingStallRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;

    PerformBathroomObjectUsedScore();

    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush1);
  }
  public virtual void PerformWorkingStallWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeStallByWashingHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;

    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingStallDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeStallByDryingHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
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

    hasRelievedSelf = true;

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Pee) {
      bathObjRef.state = BathroomObjectState.BrokenByPee;
      PerformBrokeUrinalByPeeingScore();
    }
    else if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      PerformBrokeUrinalByPoopingScore();
    }
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }

    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush2);
  }
  public virtual void PerformOutOfOrderUrinalWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeUrinalByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformOutOfOrderUrinalDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    
    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeUrinalByOutOfOrderUseScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
  }
  public virtual void PerformWorkingUrinalRelief() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasRelievedSelf = true;

    PerformBathroomObjectUsedScore();
    if(reliefRequired == ReliefRequired.Poop) {
      bathObjRef.state = BathroomObjectState.BrokenByPoop;
      PerformBrokeUrinalByPoopingScore();
    }
    else if(reliefRequired == ReliefRequired.Vomit) {
      PerformBrokeUrinalByVomittingScore();
    }
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    if(chooseRandomBathroomObjectAfterRelieved) {
      SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    }
    else {
      state = BroState.Roaming;
    }
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
    reliefRequired = ReliefRequired.WashHands;

    SoundManager.Instance.Play(AudioType.Flush2);
  }
  public virtual void PerformWorkingUrinalWashHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

    hasWashedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeUrinalByWashingHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterWashedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = true;
    speechBubbleReference.displaySpeechBubble = true;
    speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
    reliefRequired = ReliefRequired.DryHands;
  }
  public virtual void PerformWorkingUrinalDryHands() {
    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
    
    hasDriedHands = true;

    PerformBathroomObjectUsedScore();
    PerformBrokeUrinalByDryingHandsScore();

    bathObjRef.state = BathroomObjectState.Broken;
    bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

    // if(chooseRandomBathroomObjectAfterDriedHands) {
    //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    // }
    // else {
    //   state = BroState.Roaming;
    // }
    SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
    collider.enabled = true;
    selectableReference.canBeSelected = false;
    speechBubbleReference.displaySpeechBubble = false;
    reliefRequired = ReliefRequired.None;
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
        if(AStarManager.Instance.permanentClosedNodes.Contains(randomBathroomTile)) {
          randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
        }
        else {
          foundEmptyTile = true;
        }
      }

      // Debug.Log("Start Position X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y);
      BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
      List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                startTile,
                                                                                randomBathroomTile.GetComponent<BathroomTile>());
      SetTargetObjectAndTargetPosition(null, movementNodes);
    }
  }

	public virtual void PerformStandingLogic() {
	}

    public virtual void PerformStandOffLogic() {
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
        else if(reliefRequired == ReliefRequired.Vomit
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.None) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.None;
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
        SetRandomBathroomObjectTarget(chooseOpenBathroomObject, AStarManager.Instance.GetListCopyOfAllClosedNodes(), bathroomObjectTypesToTarget);
    }

  public void SetRandomBathroomObjectTarget(bool chooseOpenBathroomObject, List<GameObject> astarClosedNodesToUse, params BathroomObjectType[] bathroomObjectTypesToTarget) {
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

    if(randomObject != null) {
      // BathroomTile randomObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomObject.transform.position.x,
      //                                                                                           randomObject.transform.position.y,
      //                                                                                           true).GetComponent<BathroomTile>();
      BathroomTile randomObjectTile = randomObject.GetComponent<BathroomObject>().bathroomTileIn.GetComponent<BathroomTile>();

      List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                astarClosedNodesToUse,
                                                                                broTile,
                                                                                randomObjectTile);
      state = BroState.MovingToTargetObject;
      SetTargetObjectAndTargetPosition(randomObject, movementNodes);

      // Debug.Log("number of movementNodes: " + movementNodes.Count);
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

    //=========================================================================
    // This assumes it's called at the correct point and that the target object and relief required are accessible
    public virtual void PerformBathroomObjectUsedScore() {
        if(targetObject != null) {
            BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
            if(bathObjRef != null) {
                if(bathObjRef.type == BathroomObjectType.HandDryer) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInHandDryerScore();
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInHandDryerScore();
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInHandDryerScore();
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInHandDryerScore();
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInHandDryerScore();
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Sink) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInSinkScore();
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInSinkScore();
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInSinkScore();
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInSinkScore();
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInSinkScore();
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Stall) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInStallScore();
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInStallScore();
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInStallScore();
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInStallScore();
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInStallScore();
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Urinal) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInUrinalScore();
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInUrinalScore();
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInUrinalScore();
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInUrinalScore();
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInUrinalScore();
                    }
                }
            }
        }
    }

    //=========================================================================
    public virtual void PerformEnteredScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroEnteredScore(type);
    }
    public virtual void PerformExitedScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroExitedScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformRelievedPeeInHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPeeInHandDryerScore(type);
    }
    public virtual void PerformRelievedPeeInSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPeeInSinkScore(type);
    }
    public virtual void PerformRelievedPeeInStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPeeInStallScore(type);
    }
    public virtual void PerformRelievedPeeInUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPeeInUrinalScore(type);
    }

    public virtual void PerformRelievedPoopInHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPoopInHandDryerScore(type);
    }
    public virtual void PerformRelievedPoopInSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPoopInSinkScore(type);
    }
    public virtual void PerformRelievedPoopInStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPoopInStallScore(type);
    }
    public virtual void PerformRelievedPoopInUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedPoopInUrinalScore(type);
    }

    public virtual void PerformRelievedVomitInHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedVomitInHandDryerScore(type);
    }
    public virtual void PerformRelievedVomitInSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedVomitInSinkScore(type);
    }
    public virtual void PerformRelievedVomitInStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedVomitInStallScore(type);
    }
    public virtual void PerformRelievedVomitInUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroRelievedVomitInUrinalScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformWashedHandsInHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroWashedHandsInHandDryerScore(type);
    }
    public virtual void PerformWashedHandsInSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroWashedHandsInSinkScore(type);
    }
    public virtual void PerformWashedHandsInStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroWashedHandsInStallScore(type);
    }
    public virtual void PerformWashedHandsInUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroWashedHandsInUrinalScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformDriedHandsInHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroDriedHandsInHandDryerScore(type);
    }
    public virtual void PerformDriedHandsInSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroDriedHandsInSinkScore(type);
    }
    public virtual void PerformDriedHandsInStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroDriedHandsInStallScore(type);
    }
    public virtual void PerformDriedHandsInUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroDriedHandsInUrinalScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformStartedStandoffScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedStandoffScore(type);
    }
    public virtual void PerformStoppedStandoffScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStoppedStandoffScore(type);
    }
    public virtual void PerformStartedFightScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedFightScore(type);
    }
    public virtual void PerformStoppedFightScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStoppedFightScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformCausedOutOfOrderHandDryerScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroCausedOutOfOrderHandDryerScore(type);
    }
    public virtual void PerformCausedOutOfOrderSinkScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroCausedOutOfOrderSinkScore(type);
    }
    public virtual void PerformCausedOutOfOrderStallScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroCausedOutOfOrderStallScore(type);
    }
    public virtual void PerformCausedOutOfOrderUrinalScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroCausedOutOfOrderUrinalScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformBrokeHandDryerByOutOfOrderUseScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByOutOfOrderUseScore(type);
    }
    public virtual void PerformBrokeSinkByOutOfOrderUseScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByOutOfOrderUseScore(type);
    }
    public virtual void PerformBrokeStallByOutOfOrderUseScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByOutOfOrderUseScore(type);
    }
    public virtual void PerformBrokeUrinalByOutOfOrderUseScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByOutOfOrderUseScore(type);
    }

    public virtual void PerformBrokeHandDryerByPeeingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByPeeingScore(type);
    }
    public virtual void PerformBrokeSinkByPeeingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByPeeingScore(type);
    }
    public virtual void PerformBrokeStallByPeeingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByPeeingScore(type);
    }
    public virtual void PerformBrokeUrinalByPeeingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByPeeingScore(type);
    }

    public virtual void PerformBrokeHandDryerByPoopingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByPoopingScore(type);
    }
    public virtual void PerformBrokeSinkByPoopingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByPoopingScore(type);
    }
    public virtual void PerformBrokeStallByPoopingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByPoopingScore(type);
    }
    public virtual void PerformBrokeUrinalByPoopingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByPoopingScore(type);
    }

    public virtual void PerformBrokeHandDryerByVomittingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByVomittingScore(type);
    }
    public virtual void PerformBrokeSinkByVomittingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByVomittingScore(type);
    }
    public virtual void PerformBrokeStallByVomittingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByVomittingScore(type);
    }
    public virtual void PerformBrokeUrinalByVomittingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByVomittingScore(type);
    }

    public virtual void PerformBrokeHandDryerByWashingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByWashingHandsScore(type);
    }
    public virtual void PerformBrokeSinkByWashingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByWashingHandsScore(type);
    }
    public virtual void PerformBrokeStallByWashingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByWashingHandsScore(type);
    }
    public virtual void PerformBrokeUrinalByWashingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByWashingHandsScore(type);
    }

    public virtual void PerformBrokeHandDryerByDryingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByDryingHandsScore(type);
    }
    public virtual void PerformBrokeSinkByDryingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByDryingHandsScore(type);
    }
    public virtual void PerformBrokeStallByDryingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByDryingHandsScore(type);
    }
    public virtual void PerformBrokeUrinalByDryingHandsScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByDryingHandsScore(type);
    }

    public virtual void PerformBrokeHandDryerByFightingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeHandDryerByFightingScore(type);
    }
    public virtual void PerformBrokeSinkByFightingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeSinkByFightingScore(type);
    }
    public virtual void PerformBrokeStallByFightingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeStallByFightingScore(type);
    }
    public virtual void PerformBrokeUrinalByFightingScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBrokeUrinalByFightingScore(type);
    }
    //-------------------------------------------------------------------------
    public virtual void PerformSatisfiedBrotocolNoAdjacentBrosScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroSatisfiedBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void PerformTotalPossibleBrotocolNoAdjacentBrosScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroTotalPossibleBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void PerformSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    public virtual void PerformTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    //=========================================================================
}
