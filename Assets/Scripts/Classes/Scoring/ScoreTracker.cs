// using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class ScoreTracker : BaseBehavior {
public class ScoreTracker : MonoBehaviour {

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
    
    // protected override void Awake() {
    public void Awake() {
        InitializeScoreTrackers();
    }
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        currentScore = CalculateCurrentScore();
        perfectScore = CalculatePerfectScore();
        CalculateCurrentToPerfectScoreRatio();
    }
    
    void InitializeScoreTrackers() {
        broScores = new Dictionary<BroType, BaseBroScoreType>();
        
        foreach(BroType broType in BroType.GetValues(typeof(BroType))) {
            switch(broType) {
            case(BroType.DrunkBro):
                broScores[BroType.DrunkBro] = this.gameObject.AddComponent<DrunkBroScoreType>().GetComponent<DrunkBroScoreType>();
                break;
            case(BroType.GassyBro):
                broScores[BroType.GassyBro] = this.gameObject.AddComponent<GassyBroScoreType>().GetComponent<GassyBroScoreType>();
                break;
            case(BroType.GenericBro):
                broScores[BroType.GenericBro] = this.gameObject.AddComponent<GenericBroScoreType>().GetComponent<GenericBroScoreType>();
                break;
            case(BroType.ShyBro):
                broScores[BroType.ShyBro] = this.gameObject.AddComponent<ShyBroScoreType>().GetComponent<ShyBroScoreType>();
                break;
            case(BroType.SlobBro):
                broScores[BroType.SlobBro] = this.gameObject.AddComponent<SlobBroScoreType>().GetComponent<SlobBroScoreType>();
                break;
            }
        }
    }
    
    public BaseBroScoreType GetBroScore(BroType broScoreTypeToReturn) {
        return broScores[broScoreTypeToReturn];
    }
    
    public int GetTotalBathroomObjectsBroken() {
        return 0;
    }
    public int GetNumberOfBathroomObjectsBroken(BathroomObjectState bathroomObjectState, BathroomObjectType bathroomObjectType) {
        return 0;
    }
    
    //--------------------------------------------------------------------
    public void PerformBroEnteredScore(BroType broType) {
        broScores[broType].entered++;
    }
    public void PerformBroExitedScore(BroType broType) {
        broScores[broType].exited++;
    }
    //--------------------------------------------------------------------
    public void PerformRelievedPeeInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].relievedPeeIn[bathroomObjectType]++;
    }
    public void PerformRelievedPoopInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].relievedPoopIn[bathroomObjectType]++;
    }
    public void PerformRelievedVomitInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].relievedVomitIn[bathroomObjectType]++;
    }
    public void PerformWashedHandsInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].washedHandsIn[bathroomObjectType]++;
    }
    public void PerformDriedHandsInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].driedHandsIn[bathroomObjectType]++;
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
    public void PerformBroCausedOutOfOrderInBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].causedOutOfOrderIn[bathroomObjectType]++;
    }
    //--------------------------------------------------------------------
    public void PerformBrokeByOutOfOrderUseBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByOutOfOrderUse[bathroomObjectType]++;
    }
    public void PerformBrokeByPeeingBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByPeeing[bathroomObjectType]++;
    }
    public void PerformBrokeByPoopingBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByPooping[bathroomObjectType]++;
    }
    public void PerformBrokeByVomitingBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByVomiting[bathroomObjectType]++;
    }
    public void PerformBrokeByFightingBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByFighting[bathroomObjectType]++;
    }
    public void PerformBrokeByWashingHandsBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByWashingHands[bathroomObjectType]++;
    }
    public void PerformBrokeByDryingHandsBathroomObjectScore(BroType broType, BathroomObjectType bathroomObjectType) {
        broScores[broType].brokeByDryingHands[bathroomObjectType]++;
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
        broScores[broTypeFighting].brokeByFighting[bathroomObjectTypeToBreak]++;
    }
    
    public void CalculateCurrentToPerfectScoreRatio() {
        if(perfectScore == 0) {
            currentToPerfectScoreRatio = 0;
        }
        else {
            currentToPerfectScoreRatio = currentScore / perfectScore;
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
