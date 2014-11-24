using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class ScoreTracker : MonoBehaviour {
public class ScoreTracker : BaseBehavior {

    public int currentScore = 0;
    public int perfectScore = 0;
    public float currentToPerfectScoreRatio = 0f;

    public int regularPointModifier = 100;
    public int brotocolPointModifier = 200;
    public int badManagementPointModifier = -100;
    // public int arbitraryPointModifier = 100;

    public Dictionary<BroType, BaseBroScoreType> broScores = null;
    // public DrunkBroScoreType drunkBroScore = null;
    // public GassyBroScoreType gassyBroScore = null;
    // public GenericBroScoreType genericBroScore = null;
    // public SlobBroScoreType slobBroScore = null;
    // public ShyBroScoreType shyBroScore = null;

    void Awake() {
        InitializeScoreTrackers();
    }

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        currentScore = CalculateCurrentScore();
        perfectScore = CalculatePerfectScore();
        CalculateCurrentToPerfectScoreRatio();
    }

    void InitializeScoreTrackers() {
        broScores = new Dictionary<BroType, BaseBroScoreType>();

        broScores[BroType.DrunkBro] = this.gameObject.GetComponent<DrunkBroScoreType>();
        if(broScores[BroType.DrunkBro]== null) {
            broScores[BroType.DrunkBro]= this.gameObject.AddComponent<DrunkBroScoreType>().GetComponent<DrunkBroScoreType>();
        }

        broScores[BroType.GassyBro] = this.gameObject.GetComponent<GassyBroScoreType>();
        if(broScores[BroType.GassyBro] == null) {
            broScores[BroType.GassyBro] = this.gameObject.AddComponent<GassyBroScoreType>().GetComponent<GassyBroScoreType>();
        }

        broScores[BroType.GenericBro] = this.gameObject.GetComponent<GenericBroScoreType>();
        if(broScores[BroType.GenericBro] == null) {
            broScores[BroType.GenericBro] = this.gameObject.AddComponent<GenericBroScoreType>().GetComponent<GenericBroScoreType>();
        }

        broScores[BroType.ShyBro] = this.gameObject.GetComponent<ShyBroScoreType>();
        if(broScores[BroType.ShyBro] == null) {
            broScores[BroType.ShyBro] = this.gameObject.AddComponent<ShyBroScoreType>().GetComponent<ShyBroScoreType>();
        }

        broScores[BroType.SlobBro] = this.gameObject.GetComponent<SlobBroScoreType>();
        if(broScores[BroType.SlobBro] == null) {
            broScores[BroType.SlobBro] = this.gameObject.AddComponent<SlobBroScoreType>().GetComponent<SlobBroScoreType>();
        }

        if(broScores[BroType.DrunkBro] == null
           || broScores[BroType.GassyBro] == null
           || broScores[BroType.GenericBro] == null
           || broScores[BroType.ShyBro] == null
           || broScores[BroType.SlobBro] == null) {
            Debug.LogError("ScoreTracker is missing one of the score scripts.");
        }
    }

    public BaseBroScoreType GetBroScore(BroType broScoreTypeToReturn) {
        return broScores[broScoreTypeToReturn];
    }

    public void PerformBroEnteredScore(BroType broType) {
        broScores[broType].entered++;
    }
    public void PerformBroExitedScore(BroType broType) {
        broScores[broType].exited++;
    }
    //--------------------------------------------------------------------
    public void PerformBroRelievedPeeInHandDryerScore(BroType broType) {
        broScores[broType].relievedPeeInHandDryer++;
    }
    public void PerformBroRelievedPeeInSinkScore(BroType broType) {
        broScores[broType].relievedPeeInSink++;
    }
    public void PerformBroRelievedPeeInStallScore(BroType broType) {
        broScores[broType].relievedPeeInStall++;
    }
    public void PerformBroRelievedPeeInUrinalScore(BroType broType) {
        broScores[broType].relievedPeeInUrinal++;
    }

    public void PerformBroRelievedPoopInHandDryerScore(BroType broType) {
        broScores[broType].relievedPoopInHandDryer++;
    }
    public void PerformBroRelievedPoopInSinkScore(BroType broType) {
        broScores[broType].relievedPoopInSink++;
    }
    public void PerformBroRelievedPoopInStallScore(BroType broType) {
        broScores[broType].relievedPoopInStall++;
    }
    public void PerformBroRelievedPoopInUrinalScore(BroType broType) {
        broScores[broType].relievedPoopInUrinal++;
    }

    public void PerformBroRelievedVomitInHandDryerScore(BroType broType) {
        broScores[broType].relievedVomitInHandDryer++;
    }
    public void PerformBroRelievedVomitInSinkScore(BroType broType) {
        broScores[broType].relievedVomitInSink++;
    }
    public void PerformBroRelievedVomitInStallScore(BroType broType) {
        broScores[broType].relievedVomitInStall++;
    }
    public void PerformBroRelievedVomitInUrinalScore(BroType broType) {
        broScores[broType].relievedVomitInUrinal++;
    }
    //--------------------------------------------------------------------
    public void PerformBroWashedHandsInHandDryerScore(BroType broType) {
        broScores[broType].washedHandsInHandDryer++;
    }
    public void PerformBroWashedHandsInSinkScore(BroType broType) {
        broScores[broType].washedHandsInSink++;
    }
    public void PerformBroWashedHandsInStallScore(BroType broType) {
        broScores[broType].washedHandsInInStall++;
    }
    public void PerformBroWashedHandsInUrinalScore(BroType broType) {
        broScores[broType].washedHandsInUrinal++;
    }
    //--------------------------------------------------------------------
    public void PerformBroDriedHandsInHandDryerScore(BroType broType) {
        broScores[broType].driedHandsInHandDryer++;
    }
    public void PerformBroDriedHandsInSinkScore(BroType broType) {
        broScores[broType].driedHandsInSink++;
    }
    public void PerformBroDriedHandsInStallScore(BroType broType) {
        broScores[broType].driedHandsInStall++;
    }
    public void PerformBroDriedHandsInUrinalScore(BroType broType) {
        broScores[broType].driedHandsInUrinal++;
    }
    //--------------------------------------------------------------------
    public void PerformBroStartedStandoffScore(BroType broType) {
        broScores[broType].startedStandoff++;
    }
    public void PerformBroStoppedStandoffScore(BroType broType) {
        broScores[broType].stoppedStandoff++;
    }
    public void PerformBroStartedFightScore(BroType broType) {
        broScores[broType].startedFight++;
    }
    public void PerformBroStoppedFightScore(BroType broType) {
        broScores[broType].stoppedFight++;
    }
    //--------------------------------------------------------------------
    public void PerformBroCausedOutOfOrderHandDryerScore(BroType broType) {
        broScores[broType].causedOutOfOrderHandDryer++;
    }
    public void PerformBroCausedOutOfOrderSinkScore(BroType broType) {
        broScores[broType].causedOutOfOrderSink++;
    }
    public void PerformBroCausedOutOfOrderStallScore(BroType broType) {
        broScores[broType].causedOutOfOrderStall++;
    }
    public void PerformBroCausedOutOfOrderUrinalScore(BroType broType) {
        broScores[broType].causedOutOfOrderUrinal++;
    }
    //--------------------------------------------------------------------
    public void PerformBroBrokeHandDryerByOutOfOrderUseScore(BroType broType) {
        broScores[broType].brokeHandDryerByOutOfOrderUse++;
    }
    public void PerformBroBrokeSinkByOutOfOrderUseScore(BroType broType) {
        broScores[broType].brokeSinkByOutOfOrderUse++;
    }
    public void PerformBroBrokeStallByOutOfOrderUseScore(BroType broType) {
        broScores[broType].brokeStallByOutOfOrderUse++;
    }
    public void PerformBroBrokeUrinalByOutOfOrderUseScore(BroType broType) {
        broScores[broType].brokeUrinalByOutOfOrderUse++;
    }

    public void PerformBroBrokeHandDryerByPeeingScore(BroType broType) {
        broScores[broType].brokeHandDryerByPeeing++;
    }
    public void PerformBroBrokeSinkByPeeingScore(BroType broType) {
        broScores[broType].brokeSinkByPeeing++;
    }
    public void PerformBroBrokeStallByPeeingScore(BroType broType) {
        broScores[broType].brokeStallByPeeing++;
    }
    public void PerformBroBrokeUrinalByPeeingScore(BroType broType) {
        broScores[broType].brokeUrinalByPeeing++;
    }

    public void PerformBroBrokeHandDryerByPoopingScore(BroType broType) {
        broScores[broType].brokeHandDryerByPooping++;
    }
    public void PerformBroBrokeSinkByPoopingScore(BroType broType) {
        broScores[broType].brokeSinkByPooping++;
    }
    public void PerformBroBrokeStallByPoopingScore(BroType broType) {
        broScores[broType].brokeStallByPooping++;
    }
    public void PerformBroBrokeUrinalByPoopingScore(BroType broType) {
        broScores[broType].brokeUrinalByPoopin++;
    }

    public void PerformBroBrokeHandDryerByVomittingScore(BroType broType) {
        broScores[broType].brokeHandDryerByVomitting++;
    }
    public void PerformBroBrokeSinkByVomittingScore(BroType broType) {
        broScores[broType].brokeSinkByVomitting++;
    }
    public void PerformBroBrokeStallByVomittingScore(BroType broType) {
        broScores[broType].brokeStallByVomitting++;
    }
    public void PerformBroBrokeUrinalByVomittingScore(BroType broType) {
        broScores[broType].brokeUrinalByVomitting++;
    }

    public void PerformBroBrokeHandDryerByFightingScore(BroType broType) {
        broScores[broType].brokeHandDryerByFighting++;
    }
    public void PerformBroBrokeSinkByFightingScore(BroType broType) {
        broScores[broType].brokeSinkByFighting++;
    }
    public void PerformBroBrokeStallByFightingScore(BroType broType) {
        broScores[broType].brokeStallByFighting++;
    }
    public void PerformBroBrokeUrinalByFightingScore(BroType broType) {
        broScores[broType].brokeUrinalByFighting++;
    }

    public void PerformBroBrokeHandDryerByWashingHandsScore(BroType broType) {
        broScores[broType].brokeHandDryerByWashingHands++;
    }
    public void PerformBroBrokeSinkByWashingHandsScore(BroType broType) {
        broScores[broType].brokeSinkByWashingHands++;
    }
    public void PerformBroBrokeStallByWashingHandsScore(BroType broType) {
        broScores[broType].brokeStallByWashingHands++;
    }
    public void PerformBroBrokeUrinalByWashingHandsScore(BroType broType) {
        broScores[broType].brokeUrinalByWashingHands++;
    }

    public void PerformBroBrokeHandDryerByDryingHandsScore(BroType broType) {
        broScores[broType].brokeHandDryerByDryingHands++;
    }
    public void PerformBroBrokeSinkByDryingHandsScore(BroType broType) {
        broScores[broType].brokeSinkByDryingHands++;
    }
    public void PerformBroBrokeStallByDryingHandsScore(BroType broType) {
        broScores[broType].brokeStallByDryingHands++;
    }
    public void PerformBroBrokeUrinalByDryingHandsScore(BroType broType) {
        broScores[broType].brokeUrinalByDryingHands++;
    }
    //-------------------------------------------------------------------------
    public void PerformBroSatisfiedBrotocolNoAdjacentBrosScore(BroType broType) {
        broScores[broType].satisfiedBrotocolNoAdjacentBros++;
    }
    public void PerformBroTotalPossibleBrotocolNoAdjacentBrosScore(BroType broType) {
        broScores[broType].totalPossibleBrotocolNoAdjacentBros++;
    }
    public void PerformBroSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore(BroType broType) {
        broScores[broType].satisfiedBrotocolRelievedInCorrectObjectOnFirstTry++;
    }
    public void PerformBroTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore(BroType broType) {
        broScores[broType].totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry++;
    }

    public void PerformBroBroFightStartScore(BroType broTypeFightScoreToIncrement) {
        broScores[broTypeFightScoreToIncrement].startedFight++;
        if(!broScores.ContainsKey(broTypeFightScoreToIncrement)) {
            Debug.Log("Incremented score for unanticipated bro type!");
        }
    }

    public void PerformBroBathroomObjectBrokenByFightingScore(BroType broTypeFighting, BathroomObjectType bathroomObjectTypeToBreak) {
        switch(bathroomObjectTypeToBreak) {
            case(BathroomObjectType.HandDryer):
                broScores[broTypeFighting].brokeHandDryerByFighting++;
            break;
            case(BathroomObjectType.Sink):
                broScores[broTypeFighting].brokeSinkByFighting++;
            break;
            case(BathroomObjectType.Stall):
                broScores[broTypeFighting].brokeStallByFighting++;
            break;
            case(BathroomObjectType.Urinal):
                broScores[broTypeFighting].brokeUrinalByFighting++;
            break;
            default:
                Debug.Log("A BROFIGHT HAS OCCURRED IN OBJECT THAT IT SHOULD NOT HAVE BEEN ABLE TO. THIS OBJECT IS THE OBJECT: " + this.gameObject.name);
            break;
        }
    }

    public void CalculateCurrentToPerfectScoreRatio() {
        if(perfectScore == 0) {
            currentToPerfectScoreRatio = 0;
        }
        else {
            currentToPerfectScoreRatio = currentScore/perfectScore;
        }
    }

    public int CalculateCurrentScore() {
        float totalPointScore = 0;

        foreach(BaseBroScoreType dictEntry in broScores.Values) {
            totalPointScore += dictEntry.GetCurrentScore();
            // Debug.Log(dictEntry.GetCurrentScore());
            // Debug.Log("Current Total Point Score: " + totalPointScore);
        }
        // Debug.Log("---------------------------------------");

        return (int)totalPointScore;
    }

    public int CalculatePerfectScore() {
        float totalPointScore = 0;

        foreach(BaseBroScoreType dictEntry in broScores.Values) {
            totalPointScore += dictEntry.GetPerfectScore();
            // Debug.Log(dictEntry.GetCurrentScore());
            // Debug.Log("Current Total Point Score: " + totalPointScore);
        }
        // Debug.Log("---------------------------------------");

        return (int)totalPointScore;
    }
}
