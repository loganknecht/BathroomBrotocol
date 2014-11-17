using UnityEngine;
using System.Collections;

public class BaseBroScoreType : MonoBehaviour {
    //-------------------------------------------------------------------------
    // Entering/Exiting Score Logic
    //-------------------------------------------------------------------------
    public float entered = 0f;
    public float exited = 0f;

    //-------------------------------------------------------------------------
    // Relief Score Logic
    //-------------------------------------------------------------------------
    public float relievedVomitInHandDryer = 0f;
    public float relievedVomitInSink = 0f;
    public float relievedVomitInStall = 0f;
    public float relievedVomitInUrinal = 0f;

    public float relievedPeeInHandDryer = 0f;
    public float relievedPeeInSink = 0f;
    public float relievedPeeInStall = 0f;
    public float relievedPeeInUrinal = 0f;

    public float relievedPoopInHandDryer = 0f;
    public float relievedPoopInSink = 0f;
    public float relievedPoopInStall = 0f;
    public float relievedPoopInUrinal = 0f;

    //-------------------------------------------------------------------------
    // Washed Hands Score Logic
    //-------------------------------------------------------------------------
    public float washedHandsInHandDryer = 0f;
    public float washedHandsInSink = 0f;
    public float washedHandsInInStall = 0f;
    public float washedHandsInUrinal = 0f;

    //-------------------------------------------------------------------------
    // Dried Hands Score Logic
    //-------------------------------------------------------------------------
    public float driedHandsInHandDryer = 0f;
    public float driedHandsInSink = 0f;
    public float driedHandsInStall = 0f;
    public float driedHandsInUrinal = 0f;

    //-------------------------------------------------------------------------
    // Fighting Other Bro Score Logic
    //-------------------------------------------------------------------------
    public float startedStandoff = 0f;
    public float stoppedStandoff = 0f;
    public float startedFight = 0f;
    public float stoppedFight = 0f;

    //-------------------------------------------------------------------------
    // Out of Order Object Score Logic
    //-------------------------------------------------------------------------
    public float causedOutOfOrderHandDryer = 0f;
    public float causedOutOfOrderSink = 0f;
    public float causedOutOfOrderStall = 0f;
    public float causedOutOfOrderUrinal = 0f;

    //-------------------------------------------------------------------------
    // Breaking Objects Score Logic
    //-------------------------------------------------------------------------
    public float brokeHandDryerByOutOfOrderUse = 0f;
    public float brokeSinkByOutOfOrderUse = 0f;
    public float brokeStallByOutOfOrderUse = 0f;
    public float brokeUrinalByOutOfOrderUse = 0f;

    public float brokeHandDryerByPeeing = 0f;
    public float brokeSinkByPeeing = 0f;
    public float brokeStallByPeeing = 0f;
    public float brokeUrinalByPeeing = 0f;

    public float brokeHandDryerByPooping = 0f;
    public float brokeSinkByPooping = 0f;
    public float brokeStallByPooping = 0f;
    public float brokeUrinalByPoopin = 0f;

    public float brokeHandDryerByVomitting = 0f;
    public float brokeSinkByVomitting = 0f;
    public float brokeStallByVomitting = 0f;
    public float brokeUrinalByVomitting = 0f;

    public float brokeHandDryerByWashingHands = 0f;
    public float brokeSinkByWashingHands = 0f;
    public float brokeStallByWashingHands = 0f;
    public float brokeUrinalByWashingHands = 0f;

    public float brokeHandDryerByDryingHands = 0f;
    public float brokeSinkByDryingHands = 0f;
    public float brokeStallByDryingHands = 0f;
    public float brokeUrinalByDryingHands = 0f;

    public float brokeHandDryerByFighting = 0f;
    public float brokeSinkByFighting = 0f;
    public float brokeStallByFighting = 0f;
    public float brokeUrinalByFighting = 0f;
    //-------------------------------------------------------------------------
    // Brotocol Score Logic
    //-------------------------------------------------------------------------
    public float satisfiedBrotocolNoAdjacentBros = 0f;
    public float totalPossibleBrotocolNoAdjacentBros = 0f;
    public float satisfiedBrotocolRelievedInCorrectObjectOnFirstTry = 0f;
    public float totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry = 0f;
}
