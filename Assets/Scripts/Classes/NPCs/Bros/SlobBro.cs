using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlobBro : Bro {

    // protected override void Awake() {
    //     base.Awake();
    //     type = BroType.SlobBro;
    // }
    public override void Awake() {
        base.Awake();
        type = BroType.SlobBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    public override void PerformOccupyingObjectLogic() {
        // base.PerformOccupyingObjectLogic();
        GameObject targetObject = GetTargetObject();
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
                
                if(bathObjRef.type != BathroomObjectType.Exit
                    && !bathObjRef.IsBroken()
                    && bathObjRef.state != BathroomObjectState.OutOfOrder) {
                    bathObjRef.state = BathroomObjectState.OutOfOrder;
                }
            }
            else {
                //disables the collider because the bro resides in the object, but the timer is still going
                colliderReference.enabled = false;
                
                occupationTimer += Time.deltaTime;
            }
        }
    }
    
    //This is being checked on arrival before switching to occupying an object
    public override void PerformOnArrivalBrotocolScoreCheck() {
        // bool brotocolWasSatisfied = false;
        
        // // As long as the target object is not null and it's not a bathroom exit
        // if(targetObject != null
        //  && targetObject.GetComponent<BathroomObject>() != null
        //  && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
        //   if(!hasRelievedSelf) {
        //     //This is being checked on arrival before switching to occupying an object
        //     if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
        //       // increment correct relief type
        //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject);
        //       brotocolWasSatisfied = true;
        //     }
        //     if(!CheckIfBroInAdjacentBathroomObjects()) {
        //       // increment bro alone bonus
        //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.SlobBroBrotocolNoAdjacentBro);
        //       brotocolWasSatisfied = true;
        //     }
        //   }
        // }
        
        // if(brotocolWasSatisfied) {
        //   SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
        // }
    }
    //=========================================================================
}
