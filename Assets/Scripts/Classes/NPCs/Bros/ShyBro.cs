using UnityEngine;
using System.Collections;

public class ShyBro : Bro {
    public bool firstArrivalOccurred = false;
    public bool firstArrivalWasWrongObject = false;
    
    // protected override void Awake() {
    //     base.Awake();
    //     type = BroType.ShyBro;
    // }
    public override void Awake() {
        base.Awake();
        type = BroType.ShyBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    public override void PerformArrivalLogic() {
        if(transform.position.x == GetTargetPosition().x
            && transform.position.y == GetTargetPosition().y
            && GetMovementNodes().Count == 0) {
            
            GameObject targetObject = GetTargetObject();
            
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
                    if(bathObjRef.colliderReference != null) {
                        bathObjRef.colliderReference.enabled = false;
                    }
                    if(!firstArrivalOccurred) {
                        firstArrivalWasWrongObject = true;
                    }
                }
                
                selectableReference.Reset();
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
            }
            else {
                if(targetObject != null
                    // && reliefRequired == ReliefRequired.Pee
                    && !hasRelievedSelf
                    && (targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Urinal
                        || targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Sink)) {
                    colliderReference.enabled = true;
                    baseProbabilityOfFightOnCollisionWithBro = 0f;
                    selectableReference.canBeSelected = true;
                }
                else {
                    //disables the collider because the bro resides in the object, but the timer is still going
                    colliderReference.enabled = false;
                    occupationTimer += Time.deltaTime;
                }
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
        //     // if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
        //     //   // increment correct relief type
        //     //   ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject);
        //     // }
        //     if(!CheckIfBroInAdjacentBathroomObjects()) {
        //       // increment bro alone bonus
        //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolNoAdjacentBro);
        //       brotocolWasSatisfied = true;
        //     }
        //     if(CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry()) {
        //       // increment no janitor summoned bonus
        //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry);
        //       brotocolWasSatisfied = true;
        //     }
        //   }
        // }
        
        // if(brotocolWasSatisfied) {
        //   SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
        // }
    }
    
    public override bool CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry() {
        if(!firstArrivalWasWrongObject) {
            return true;
        }
        else {
            return false;
        }
    }
    //=========================================================================
}
