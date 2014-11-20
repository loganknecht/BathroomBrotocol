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

                    GameObject newVomit = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Vomit);
                    newVomit.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newVomit.transform.position.z);
                    BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newVomit);

                    BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
                    List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);
                    int selectedExit = Random.Range(0, exits.Count);
                    GameObject randomExit = exits[selectedExit];
                    BathroomTile randomExitTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomExit.transform.position.x, randomExit.transform.position.y, true).GetComponent<BathroomTile>();

                    SetRandomBathroomObjectTarget(false, AStarManager.Instance.GetListCopyOfPermanentClosedNodes(), BathroomObjectType.Exit);
                }
            }
        }
    }
    //===========================================================================
    public override void PerformExitOccupationFinishedLogic() {
        base.PerformExitOccupationFinishedLogic();
    }
    //===========================================================================
    public override void PerformOutOfOrderHandDryerRelief() {
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

        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //     SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //     state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);

        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    public override void PerformWorkingHandDryerRelief() {
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

        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    //===========================================================================
    public override void PerformOutOfOrderSinkRelief() {
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

        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    public override void PerformWorkingSinkRelief() {
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

        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    //===========================================================================
    public override void PerformOutOfOrderStallRelief() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasRelievedSelf = true;

        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            // PerformBrokeStallByPeeingScore();
            PerformBrokeStallByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            // PerformBrokeStallByPoopingScore();
            PerformBrokeStallByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            // PerformBrokeStallByVomittingScore();
            PerformBrokeStallByOutOfOrderUseScore();
        }

        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;

        SoundManager.Instance.Play(AudioType.Flush1);
    }
    public override void PerformWorkingStallRelief() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasRelievedSelf = true;

        PerformBathroomObjectUsedScore();

        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;

        SoundManager.Instance.Play(AudioType.Flush1);
    }
    //===========================================================================
    public override void PerformOutOfOrderUrinalRelief() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasRelievedSelf = true;

        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
          bathObjRef.state = BathroomObjectState.BrokenByPee;
          // PerformBrokeUrinalByPeeingScore();
          PerformBrokeUrinalByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Poop) {
          bathObjRef.state = BathroomObjectState.BrokenByPoop;
          // PerformBrokeUrinalByPoopingScore();
          PerformBrokeUrinalByOutOfOrderUseScore();
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
          bathObjRef.state = BathroomObjectState.Broken;
          PerformBrokeUrinalByOutOfOrderUseScore();
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;

        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public override void PerformWorkingUrinalRelief() {
        BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

        hasRelievedSelf = true;

        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Poop) {
          bathObjRef.state = BathroomObjectState.BrokenByPoop;
          PerformBrokeUrinalByPoopingScore();
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            // do nothing
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);

        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        reliefRequired = ReliefRequired.WashHands;

        SoundManager.Instance.Play(AudioType.Flush2);
    }
    //===========================================================================

    //--------------------------------------------------------
    //This is being checked on arrival before switching to occupying an object
    public override void PerformOnArrivalBrotocolScoreCheck() {
        bool brotocolWasSatisfied = false;

    // // As long as the target object is not null and it's not a bathroom exit
    // if(targetObject != null
    //  && targetObject.GetComponent<BathroomObject>() != null
    //  && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
    //   if(!hasRelievedSelf) {
    //     if(CheckIfRelievedSelfBeforeTimeOut()) {
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut);
    //       brotocolWasSatisfied = true;
    //     }
    //   }
    // }

    // if(brotocolWasSatisfied) {
    //   SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
    // }
    }

  public override bool CheckIfRelievedSelfBeforeTimeOut() {
    if(vomitTimer > vomitTimerMax) {
      return false;
    }
    else {
      return true;
    }
  }

    //=========================================================================
}
