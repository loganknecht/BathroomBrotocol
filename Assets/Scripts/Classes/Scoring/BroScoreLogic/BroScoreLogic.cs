using UnityEngine;
using System.Collections;

public class BroScoreLogic : MonoBehaviour {
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    //=========================================================================
    // Score Logic
    //=========================================================================
    //This is being checked on arrival before switching to occupying an object
    public virtual void OnArrivalBrotocolScoreCheck(GameObject targetObject) {
        // if(targetObject != null
        //     && targetObject.GetComponent<BathroomObject>() != null
        //     && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
        //     if(CheckIfBroHasCorrectReliefTypeForTargetObject(targetObject)) {
        //         // increment correct relief type
        //     }
        //     if(!CheckIfBroInAdjacentBathroomObjects()) {
        //         // increment bro alone bonus
        //     }
        // }
    }
    
    // This is so dumb to document this
    /// <summary>This checks to see if the bro's target object is a bathroom object,
    /// and that their relief type matches the correct bathroom object type.
    /// </summary>
    /// <returns>True if target object is a bathroom object, and if reliefRequired
    /// matches the correct bathroom object.</returns>
    public virtual bool CheckIfBroHasCorrectReliefTypeForTargetObject(GameObject targetObject) {
        // BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
        // if(hasRelievedSelf == false
        //     // && (reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Vomit)
        //     && (reliefRequired == ReliefRequired.Pee)
        //     && bathObjRef != null
        //     && bathObjRef.type == BathroomObjectType.Urinal) {
        //     return true;
        // }
        // else if(hasRelievedSelf == false
        //         && (reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit)
        //         && bathObjRef != null
        //         && bathObjRef.type == BathroomObjectType.Stall) {
        //     return true;
        // }
        // else if(hasWashedHands == false
        //         && (reliefRequired == ReliefRequired.WashHands)
        //         && bathObjRef != null
        //         && bathObjRef.type == BathroomObjectType.Sink) {
        //     return true;
        // }
        // else {
        //     return false;
        // }
        return false;
    }
    
    // Returns true if any of the eight tiles around the bro has a bathroom object,
    // and if that bathroom object has a bro in it
    public virtual bool CheckIfBroInAdjacentBathroomObjects() {
        // bool broIsInAjdacentTile = false;
        
        // BathroomTile currentTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
        
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
    public virtual void BathroomObjectUsedScore() {
        // if(GetTargetObject() != null) {
        //     BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        //     if(bathObjRef != null) {
        //         if(bathObjRef.type == BathroomObjectType.HandDryer) {
        //             if(reliefRequired == ReliefRequired.DryHands) {
        //                 // WashedHandsInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        //                 DriedHandsInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Pee) {
        //                 RelievedPeeInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Poop) {
        //                 RelievedPoopInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Vomit) {
        //                 RelievedVomitInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.WashHands) {
        //                 WashedHandsInBathoomObjectScore(bathObjRef.type);
        //             }
        //         }
        //         else if(bathObjRef.type == BathroomObjectType.Sink) {
        //             if(reliefRequired == ReliefRequired.DryHands) {
        //                 DriedHandsInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Pee) {
        //                 RelievedPeeInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Poop) {
        //                 RelievedPoopInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Vomit) {
        //                 RelievedVomitInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.WashHands) {
        //                 WashedHandsInBathoomObjectScore(bathObjRef.type);
        //             }
        //         }
        //         else if(bathObjRef.type == BathroomObjectType.Stall) {
        //             if(reliefRequired == ReliefRequired.DryHands) {
        //                 DriedHandsInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Pee) {
        //                 RelievedPeeInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Poop) {
        //                 RelievedPoopInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Vomit) {
        //                 RelievedVomitInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.WashHands) {
        //                 WashedHandsInBathoomObjectScore(bathObjRef.type);
        //             }
        //         }
        //         else if(bathObjRef.type == BathroomObjectType.Urinal) {
        //             if(reliefRequired == ReliefRequired.DryHands) {
        //                 DriedHandsInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Pee) {
        //                 RelievedPeeInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Poop) {
        //                 RelievedPoopInBathoomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.Vomit) {
        //                 RelievedVomitInBathroomObjectScore(bathObjRef.type);
        //             }
        //             else if(reliefRequired == ReliefRequired.WashHands) {
        //                 WashedHandsInBathoomObjectScore(bathObjRef.type);
        //             }
        //         }
        //     }
        // }
    }
    
    // This should really be removed to a separate component or something...
    public virtual void EnteredScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroEnteredScore(type);
    }
    public virtual void ExitedScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroExitedScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void StartedStandoffScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroStartedStandoffScore(type);
    }
    public virtual void StoppedStandoffScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroStoppedStandoffScore(type);
    }
    public virtual void StartedFightScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroStartedFightScore(type);
    }
    public virtual void StoppedFightScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroStoppedFightScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void DriedHandsInBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().DriedHandsInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void RelievedPeeInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().RelievedPeeInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void RelievedPoopInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().RelievedPoopInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void RelievedVomitInBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().RelievedVomitInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void WashedHandsInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().WashedHandsInBathroomObjectScore(type, bathroomObjectType);
    }
    //---------
    public virtual void BrokeByOutOfOrderUseBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByOutOfOrderUseBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByPeeingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByPeeingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByPoopingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByPoopingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByVomitingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByVomitingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByWashingHandsBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByWashingHandsBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByDryingHandsBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByDryingHandsBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void BrokeByFightingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        // ScoreManager.Instance.GetPlayerScoreTracker().BrokeByFightingBathroomObjectScore(type, bathroomObjectType);
    }
    //-------------------------------------------------------------------------
    public virtual void SatisfiedBrotocolNoAdjacentBrosScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroSatisfiedBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void TotalPossibleBrotocolNoAdjacentBrosScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroTotalPossibleBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void SatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    public virtual void TotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        // ScoreManager.Instance.GetPlayerScoreTracker().BroTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    //=========================================================================
}
