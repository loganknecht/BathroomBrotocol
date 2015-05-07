using UnityEngine;
using System.Collections;

public class ChattyBro : Bro {

    // protected override void Awake() {
    // base.Awake();
    // type = BroType.ChattyBro;
    // }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    public override void PerformOnArrivalBrotocolScoreCheck() {
        // As long as the target object is not null and it's not a bathroom exit
        //   if(targetObject != null
        //    && targetObject.GetComponent<BathroomObject>() != null
        //    && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
        //     if(!hasRelievedSelf) {
        //       //This is being checked on arrival before switching to occupying an object
        //       if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
        //         // increment correct relief type
        //         ScoreManager.Instance.IncrementScoreTracker(ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject);
        //       }
        //       if(CheckIfBroInAdjacentBathroomObjects()) {
        //         // increment bro alone bonus
        //         ScoreManager.Instance.IncrementScoreTracker(ScoreType.ChattyBroBrotocolAdjacentBro);
        //       }
        //     }
        //   }
    }
    
    //=========================================================================
}
