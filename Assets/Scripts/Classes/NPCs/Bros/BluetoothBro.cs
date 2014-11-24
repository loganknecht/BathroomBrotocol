using UnityEngine;
using System.Collections;

public class BluetoothBro : Bro {

    public override void Awake() {
        type = BroType.BluetoothBro;
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {
        base.Start();
        type = BroType.BluetoothBro;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

   // This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    // As long as the target object is not null and it's not a bathroom exit
    // if(targetObject != null
    //  && targetObject.GetComponent<BathroomObject>() != null
    //  && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
    //   if(!hasRelievedSelf) {
    //     //This is being checked on arrival before switching to occupying an object
    //     if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
    //       // increment correct relief type
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject);
    //     }
    //     if(!CheckIfBroInAdjacentBathroomObjects()) {
    //       // increment bro alone bonus
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.BluetoothBroBrotocolNoAdjacentBro);
    //     }
    //   }
    // }
  }
    //=========================================================================
}
