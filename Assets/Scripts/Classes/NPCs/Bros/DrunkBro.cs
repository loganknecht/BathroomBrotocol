using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkBro : Bro {
    public float vomitTimer = 0;
    public float vomitTimerMax = 0;
    public bool vomitThrowUpPerformed = false;
    
    // protected override void Awake() {
    // base.Awake();
    
    // vomitTimerMax = Random.Range(10, 15);
    // type = BroType.DrunkBro;
    // }
    public override void Awake() {
        base.Awake();
        vomitTimerMax = Random.Range(10, 15);
        type = BroType.DrunkBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        if(!isPaused) {
            PerformFightTimerLogic();
            PerformLogic();
            PerformVomitTimerLogic();
            UpdateAnimator();
        }
    }
    
    public override void InitializeComponents() {
        if(targetPathingReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'targetPathingReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(animatorReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'animatorReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(selectableReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'selectableReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(isometricDisplayReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'isometricDisplayReference', it is NULL. Please fix this by assigned it before use.");
        }
        isometricDisplayReference.UpdateDisplayPosition();
    }
    
    public void PerformVomitTimerLogic() {
        if(!hasRelievedSelf) {
            if(vomitTimer > vomitTimerMax) {
                hasRelievedSelf = true;
                // GameObject newVomit = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Vomit);
                // newVomit.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newVomit.transform.position.z);
                // BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newVomit);
                
                // BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
                // List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);
                // int selectedExit = Random.Range(0, exits.Count);
                // GameObject randomExit = exits[selectedExit];
                // BathroomTile randomExitTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomExit.transform.position.x, randomExit.transform.position.y, true).GetComponent<BathroomTile>();
                ExitBathroom();
            }
            else {
                if(state != BroState.InAQueue
                    && state != BroState.Standoff
                    && state != BroState.Fighting
                    && state != BroState.OccupyingObject) {
                    vomitTimer += Time.deltaTime;
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
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            // PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            // PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            // PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //     SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //     state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    public override void PerformWorkingHandDryerRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    //===========================================================================
    public override void PerformOutOfOrderSinkRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            // PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            // PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            // PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    public override void PerformWorkingSinkRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.Broken;
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    //===========================================================================
    public override void PerformOutOfOrderStallRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            // PerformBrokeStallByPeeingScore();
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            // PerformBrokeStallByPoopingScore();
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            // PerformBrokeStallByVomittingScore();
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    public override void PerformWorkingStallRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    //===========================================================================
    public override void PerformOutOfOrderUrinalRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            // PerformBrokeUrinalByPeeingScore();
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            // PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public override void PerformWorkingUrinalRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            // do nothing
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterRelieved) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        GetComponent<Collider>().enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        reliefRequired = ReliefRequired.WashHands;
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    //===========================================================================
    
    //--------------------------------------------------------
    //This is being checked on arrival before switching to occupying an object
    public override void PerformOnArrivalBrotocolScoreCheck() {
        // bool brotocolWasSatisfied = false;
        
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
