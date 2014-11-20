using UnityEngine;
using System.Collections;

public class BaseBroScoreType : MonoBehaviour {
    public float goodChoicePoints = 100f;
    public float badChoicePoints = -100f;

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

    public virtual float GetCurrentScore() {
        float currentScore = 0f;

        // currentScore += entered * goodChoicePoints;
        // currentScore += exited * goodChoicePoints;

        currentScore += relievedVomitInHandDryer * badChoicePoints;
        currentScore += relievedVomitInSink * goodChoicePoints;
        currentScore += relievedVomitInStall * goodChoicePoints;
        currentScore += relievedVomitInUrinal * goodChoicePoints;

        currentScore += relievedPeeInHandDryer * badChoicePoints;
        currentScore += relievedPeeInSink * badChoicePoints;
        currentScore += relievedPeeInStall * goodChoicePoints;
        currentScore += relievedPeeInUrinal * goodChoicePoints;

        currentScore += relievedPoopInHandDryer * badChoicePoints;
        currentScore += relievedPoopInSink * badChoicePoints;
        currentScore += relievedPoopInStall * goodChoicePoints;
        currentScore += relievedPoopInUrinal * goodChoicePoints;

        currentScore += washedHandsInHandDryer * badChoicePoints;
        currentScore += washedHandsInSink * goodChoicePoints;
        currentScore += washedHandsInInStall * badChoicePoints;
        currentScore += washedHandsInUrinal * badChoicePoints;

        currentScore += driedHandsInHandDryer * goodChoicePoints;
        currentScore += driedHandsInSink * badChoicePoints;
        currentScore += driedHandsInStall * badChoicePoints;
        currentScore += driedHandsInUrinal * badChoicePoints;

        currentScore += startedStandoff * 0;
        currentScore += stoppedStandoff * goodChoicePoints;
        currentScore += startedFight * badChoicePoints;
        currentScore += stoppedFight * goodChoicePoints/2;

        currentScore += causedOutOfOrderHandDryer * 0;
        currentScore += causedOutOfOrderSink * 0;
        currentScore += causedOutOfOrderStall * 0;
        currentScore += causedOutOfOrderUrinal * 0;

        currentScore += brokeHandDryerByOutOfOrderUse * badChoicePoints;
        currentScore += brokeSinkByOutOfOrderUse * badChoicePoints;
        currentScore += brokeStallByOutOfOrderUse * badChoicePoints;
        currentScore += brokeUrinalByOutOfOrderUse * badChoicePoints;

        currentScore += brokeHandDryerByPeeing * badChoicePoints;
        currentScore += brokeSinkByPeeing * badChoicePoints;
        currentScore += brokeStallByPeeing * badChoicePoints;
        currentScore += brokeUrinalByPeeing * badChoicePoints;

        currentScore += brokeHandDryerByPooping * badChoicePoints;
        currentScore += brokeSinkByPooping * badChoicePoints;
        currentScore += brokeStallByPooping * badChoicePoints;
        currentScore += brokeUrinalByPoopin * badChoicePoints;

        currentScore += brokeHandDryerByVomitting * badChoicePoints;
        currentScore += brokeSinkByVomitting * badChoicePoints;
        currentScore += brokeStallByVomitting * badChoicePoints;
        currentScore += brokeUrinalByVomitting * badChoicePoints;

        currentScore += brokeHandDryerByWashingHands * badChoicePoints;
        currentScore += brokeSinkByWashingHands * badChoicePoints;
        currentScore += brokeStallByWashingHands * badChoicePoints;
        currentScore += brokeUrinalByWashingHands * badChoicePoints;

        currentScore += brokeHandDryerByDryingHands * badChoicePoints;
        currentScore += brokeSinkByDryingHands * badChoicePoints;
        currentScore += brokeStallByDryingHands * badChoicePoints;
        currentScore += brokeUrinalByDryingHands * badChoicePoints;

        currentScore += brokeHandDryerByFighting * badChoicePoints;
        currentScore += brokeSinkByFighting * badChoicePoints;
        currentScore += brokeStallByFighting * badChoicePoints;
        currentScore += brokeUrinalByFighting * badChoicePoints;

        currentScore += satisfiedBrotocolNoAdjacentBros * goodChoicePoints;
        // currentScore += totalPossibleBrotocolNoAdjacentBros * goodChoicePoints;
        currentScore += satisfiedBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        // currentScore += totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;

        return currentScore;
    }

    public virtual float GetPerfectScore() {
        float currentScore = 0f;

        // currentScore += entered * goodChoicePoints;
        // currentScore += exited * goodChoicePoints;

        currentScore += relievedVomitInHandDryer * goodChoicePoints;
        currentScore += relievedVomitInSink * goodChoicePoints;
        currentScore += relievedVomitInStall * goodChoicePoints;
        currentScore += relievedVomitInUrinal * goodChoicePoints;

        currentScore += relievedPeeInHandDryer * goodChoicePoints;
        currentScore += relievedPeeInSink * goodChoicePoints;
        currentScore += relievedPeeInStall * goodChoicePoints;
        currentScore += relievedPeeInUrinal * goodChoicePoints;

        currentScore += relievedPoopInHandDryer * goodChoicePoints;
        currentScore += relievedPoopInSink * goodChoicePoints;
        currentScore += relievedPoopInStall * goodChoicePoints;
        currentScore += relievedPoopInUrinal * goodChoicePoints;

        currentScore += washedHandsInHandDryer * goodChoicePoints;
        currentScore += washedHandsInSink * goodChoicePoints;
        currentScore += washedHandsInInStall * goodChoicePoints;
        currentScore += washedHandsInUrinal * goodChoicePoints;

        currentScore += driedHandsInHandDryer * goodChoicePoints;
        currentScore += driedHandsInSink * goodChoicePoints;
        currentScore += driedHandsInStall * goodChoicePoints;
        currentScore += driedHandsInUrinal * goodChoicePoints;

        currentScore += startedStandoff * 0;
        currentScore += stoppedStandoff * goodChoicePoints;
        currentScore += startedFight * 0;
        currentScore += stoppedFight * goodChoicePoints;

        currentScore += causedOutOfOrderHandDryer * 0;
        currentScore += causedOutOfOrderSink * 0;
        currentScore += causedOutOfOrderStall * 0;
        currentScore += causedOutOfOrderUrinal * 0;

        currentScore += brokeHandDryerByOutOfOrderUse * 0;
        currentScore += brokeSinkByOutOfOrderUse * 0;
        currentScore += brokeStallByOutOfOrderUse * 0;
        currentScore += brokeUrinalByOutOfOrderUse * 0;

        currentScore += brokeHandDryerByPeeing * 0;
        currentScore += brokeSinkByPeeing * 0;
        currentScore += brokeStallByPeeing * 0;
        currentScore += brokeUrinalByPeeing * 0;

        currentScore += brokeHandDryerByPooping * 0;
        currentScore += brokeSinkByPooping * 0;
        currentScore += brokeStallByPooping * 0;
        currentScore += brokeUrinalByPoopin * 0;

        currentScore += brokeHandDryerByVomitting * 0;
        currentScore += brokeSinkByVomitting * 0;
        currentScore += brokeStallByVomitting * 0;
        currentScore += brokeUrinalByVomitting * 0;

        currentScore += brokeHandDryerByWashingHands * 0;
        currentScore += brokeSinkByWashingHands * 0;
        currentScore += brokeStallByWashingHands * 0;
        currentScore += brokeUrinalByWashingHands * 0;

        currentScore += brokeHandDryerByDryingHands * 0;
        currentScore += brokeSinkByDryingHands * 0;
        currentScore += brokeStallByDryingHands * 0;
        currentScore += brokeUrinalByDryingHands * 0;

        currentScore += brokeHandDryerByFighting * 0;
        currentScore += brokeSinkByFighting * 0;
        currentScore += brokeStallByFighting * 0;
        currentScore += brokeUrinalByFighting * 0;

        // currentScore += satisfiedBrotocolNoAdjacentBros * goodChoicePoints;
        currentScore += totalPossibleBrotocolNoAdjacentBros * goodChoicePoints;
        // currentScore += satisfiedBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        currentScore += totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;

        return currentScore;
    }
}
