﻿using UnityEngine;
using System.Collections;

public class RichBro : Bro {
    protected override void Awake() {
        base.Awake();
        type = BroType.RichBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    //This is being checked on arrival before switching to occupying an object
    // public override void OnArrivalBrotocolScoreCheck() {
    // // As long as the target object is not null and it's not a bathroom exit
    // if(targetObject != null
    //  && targetObject.GetComponent<BathroomObject>() != null
    //  && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
    //   if(!hasRelievedSelf) {
    //     //This is being checked on arrival before switching to occupying an object
    //     if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
    //       // increment correct relief type
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject);
    //     }
    //     if(!CheckIfBroInAdjacentBathroomObjects()) {
    //       // increment bro alone bonus
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.RichBroBrotocolNoAdjacentBro);
    //     }
    //     // if(!CheckIfJanitorIsCurrentlySummoned()) {
    //       // increment no janitor summoned bonus
    //       // ScoreManager.Instance.IncrementScoreTracker(ScoreType.RichBroBrotocolNoJanitorSummoned);
    //     // }
    //   }
    // }
    // }
    //=========================================================================
}
