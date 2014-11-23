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

    public override void PerformOccupyingObjectLogic() {
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
                    if(!bathObjRef.IsBroken()) {
                        bathObjRef.state = BathroomObjectState.OutOfOrder;
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Sink) {
                    PerformSinkOccupationFinishedLogic();
                    if(!bathObjRef.IsBroken()) {
                        bathObjRef.state = BathroomObjectState.OutOfOrder;
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Stall) {
                    PerformStallOccupationFinishedLogic();
                    if(!bathObjRef.IsBroken()) {
                        bathObjRef.state = BathroomObjectState.OutOfOrder;
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Urinal) {
                    PerformUrinalOccupationFinishedLogic();
                    if(!bathObjRef.IsBroken()) {
                        bathObjRef.state = BathroomObjectState.OutOfOrder;
                    }
                }
            }
            else {
                //disables the collider because the bro resides in the object, but the timer is still going
                collider.enabled = false;

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
