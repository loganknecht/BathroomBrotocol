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

    // Use this for initialization
    void Start () {
        InitializeScoreTrackers();
    }

    // Update is called once per frame
    void Update () {
        currentScore = CalculateCurrentScore();
        if(currentScore < 0) {
            currentScore = 0;
        }

        perfectScore = CalculatePerfectScore();
        if(perfectScore < 0) {
            perfectScore = 0;
        }

        CalculateCurrentToPerfectScoreRatio();
    }

    void InitializeScoreTrackers() {
        broScores = new Dictionary<BroType, BaseBroScoreType>();

        broScores[BroType.DrunkBro]= this.gameObject.GetComponent<DrunkBroScoreType>();
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
        int totalPointScore = 0;

        totalPointScore += CalculateBluetoothBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateChattyBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateCuttingBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateDrunkBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateFartingBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateGenericBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateRichBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateShyBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateSlobBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateTimeWasterBroScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateBroFightScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateUrinalsScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateStallsScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        totalPointScore += CalculateSinksScore();
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        // totalPointScore = totalPointScore * arbitraryPointModifier;
        // Debug.Log("Current Total Point Score: " + totalPointScore);
        // Debug.Log("---------------------------------------");

        return totalPointScore;
    }

    public int CalculatePerfectScore() {
        int totalPointScore = 0;

        totalPointScore += CalculatePerfectBluetoothBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectChattyBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectCuttingBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectDrunkBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectFartingBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectGenericBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectRichBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectShyBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectSlobBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectTimeWasterBroScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectBroFightScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectUrinalsScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectStallsScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        totalPointScore += CalculatePerfectSinksScore();
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        // totalPointScore = totalPointScore * arbitraryPointModifier;
        // Debug.Log("Perfect Total Point Score: " + totalPointScore);
        // Debug.Log("=======================================");

        return totalPointScore;
    }

    public int CalculateBluetoothBroScore() {
        int bluetoothBrosScore = 0;
        
        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;
        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroRelieved]*regularPointModifier;
        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroHandsWashed]*regularPointModifier;
        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroExited]*regularPointModifier;

        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
        // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolNoAdjacentBro]*brotocolPointModifier;

        return bluetoothBrosScore;
    }

    public int CalculateChattyBroScore() {
        int chattyBrosScore = 0;

        // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;
        // chattyBrosScore += scorePoints[ScoreType.ChattyBroRelieved]*regularPointModifier;
        // chattyBrosScore += scorePoints[ScoreType.ChattyBroHandsWashed]*regularPointModifier;
        // chattyBrosScore += scorePoints[ScoreType.ChattyBroExited]*regularPointModifier;

        // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
        // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolAdjacentBro]*brotocolPointModifier;

        return chattyBrosScore;
    }

    public int CalculateCuttingBroScore() {
        int cuttingBrosScore = 0;

        // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;
        // cuttingBrosScore += scorePoints[ScoreType.CuttingBroRelieved]*regularPointModifier;
        // cuttingBrosScore += scorePoints[ScoreType.CuttingBroHandsWashed]*regularPointModifier;
        // cuttingBrosScore += scorePoints[ScoreType.CuttingBroExited]*regularPointModifier;

        return cuttingBrosScore;
    }

    public int CalculateDrunkBroScore() {
        int drunkBrosScore = 0;

        // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;
        // drunkBrosScore += scorePoints[ScoreType.DrunkBroRelieved]*regularPointModifier;
        // drunkBrosScore += scorePoints[ScoreType.DrunkBroHandsWashed]*regularPointModifier;
        // drunkBrosScore += scorePoints[ScoreType.DrunkBroExited]*regularPointModifier;

        // drunkBrosScore += scorePoints[ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut]*brotocolPointModifier;

        return drunkBrosScore;
    }

    public int CalculateFartingBroScore() {
        int fartingBrosScore = 0;

        // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;
        // fartingBrosScore += scorePoints[ScoreType.FartingBroRelieved]*regularPointModifier;
        // fartingBrosScore += scorePoints[ScoreType.FartingBroHandsWashed]*regularPointModifier;
        // fartingBrosScore += scorePoints[ScoreType.FartingBroExited]*regularPointModifier;

        // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
        // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolNoAdjacentBro]*brotocolPointModifier;

        return fartingBrosScore;
    }

    public int CalculateGenericBroScore() {
        int genericBrosScore = 0;

        // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;
        // genericBrosScore += scorePoints[ScoreType.GenericBroRelieved]*regularPointModifier;
        // genericBrosScore += scorePoints[ScoreType.GenericBroHandsWashed]*regularPointModifier;
        // genericBrosScore += scorePoints[ScoreType.GenericBroExited]*regularPointModifier;

        // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
        // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolNoAdjacentBro]*brotocolPointModifier;

        return genericBrosScore;
    }

  public int CalculateRichBroScore() {
    int richBrosScore = 0;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroRelieved]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroHandsWashed]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroExited]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoJanitorSummoned]*brotocolPointModifier;

    return richBrosScore;
  }

  public int CalculateShyBroScore() {
    int shyBrosScore = 0;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroRelieved]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroHandsWashed]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroExited]*regularPointModifier;

    // // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject];
    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry]*brotocolPointModifier;

    return shyBrosScore;
  }

  public int CalculateSlobBroScore() {
    int slobBrosScore = 0;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroRelieved]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroHandsWashed]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroExited]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolNoAdjacentBro]*brotocolPointModifier;

    return slobBrosScore;
  }

  public int CalculateTimeWasterBroScore() {
    int timeWasterBrosScore = 0;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroRelieved]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroBrotocolFollowed]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroHandsWashed]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroExited]*regularPointModifier;
    return timeWasterBrosScore;
  }

  public int CalculateBroFightScore() {
    int broFightScore = 0;
    // broFightScore += scorePoints[ScoreType.BroStandoffOccurred];
    // broFightScore += scorePoints[ScoreType.BroFightPrevented]*regularPointModifier;
    // broFightScore += scorePoints[ScoreType.BroFightOccurred]*badManagementPointModifier;
    return broFightScore;
  }

  public int CalculateUrinalsScore() {
    int urinalsScore = 0;
    // urinalsScore += scorePoints[ScoreType.UrinalBroken]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalPeedIn]*regularPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalPoopedIn]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalRepaired]*regularPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalVomitedIn]*regularPointModifier;
    return urinalsScore;
  }

  public int CalculateStallsScore() {
    int stallsScore = 0;
    // stallsScore += scorePoints[ScoreType.StallBroken]*badManagementPointModifier;
    // stallsScore += scorePoints[ScoreType.StallPeedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallPoopedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallRepaired]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallVomitedIn]*regularPointModifier;
    return stallsScore;
  }

  public int CalculateSinksScore() {
    int sinksScore = 0;
    // sinksScore += scorePoints[ScoreType.SinkBroken]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPeedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPoopedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkRepaired]*regularPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkHandsWashedIn]*regularPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkVomitedIn]*regularPointModifier;
    return sinksScore;
  }

  //----------------------------------------------------------------------------
  // PERFECT SCORE LOGIC GOES HERE
  //----------------------------------------------------------------------------
  public int CalculatePerfectBluetoothBroScore() {
    int bluetoothBrosScore = 0;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroRelieved]*regularPointModifier;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroHandsWashed]*regularPointModifier;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroExited]*regularPointModifier;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*brotocolPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*brotocolPointModifier;

    return bluetoothBrosScore;
  }

  public int CalculatePerfectChattyBroScore() {
    int chattyBrosScore = 0;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroRelieved]*regularPointModifier;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroHandsWashed]*regularPointModifier;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroExited]*regularPointModifier;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*brotocolPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolAdjacentBro]*brotocolPointModifier;
    // chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*brotocolPointModifier;

    return chattyBrosScore;
  }

  public int CalculatePerfectCuttingBroScore() {
    int cuttingBrosScore = 0;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroRelieved]*regularPointModifier;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroHandsWashed]*regularPointModifier;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroExited]*regularPointModifier;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    return cuttingBrosScore;
  }

  public int CalculatePerfectDrunkBroScore() {
    int drunkBrosScore = 0;

    //-----------------
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroRelieved]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroHandsWashed]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroExited]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut]*brotocolPointModifier;
    //-----------------
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroRelieved]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroHandsWashed]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroExited]*regularPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut]*brotocolPointModifier;
    // drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*brotocolPointModifier;

    return drunkBrosScore;
  }

  public int CalculatePerfectFartingBroScore() {
    int fartingBrosScore = 0;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroRelieved]*regularPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroHandsWashed]*regularPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroExited]*regularPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*brotocolPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*brotocolPointModifier;

    return fartingBrosScore;
  }

  public int CalculatePerfectGenericBroScore() {
    int genericBrosScore = 0;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroRelieved]*regularPointModifier;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroHandsWashed]*regularPointModifier;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroExited]*regularPointModifier;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*brotocolPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*brotocolPointModifier;

    return genericBrosScore;
  }

  public int CalculatePerfectRichBroScore() {
    int richBrosScore = 0;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroRelieved]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroHandsWashed]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroExited]*regularPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoJanitorSummoned]*brotocolPointModifier;
    // richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    return richBrosScore;
  }

  public int CalculatePerfectShyBroScore() {
    int shyBrosScore = 0;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroRelieved]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroHandsWashed]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroExited]*regularPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry]*brotocolPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    return shyBrosScore;
  }

  public int CalculatePerfectSlobBroScore() {
    int slobBrosScore = 0;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroRelieved]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroHandsWashed]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroExited]*regularPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*brotocolPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    // slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*brotocolPointModifier;

    return slobBrosScore;
  }

  public int CalculatePerfectTimeWasterBroScore() {
    int timeWasterBrosScore = 0;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroRelieved]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroBrotocolFollowed]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroHandsWashed]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroExited]*regularPointModifier;
    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    return timeWasterBrosScore;
  }

  public int CalculatePerfectBroFightScore() {
    int broFightScore = 0;
    // broFightScore += scorePoints[ScoreType.BroStandoffOccurred]*badManagementPointModifier;
    // broFightScore += scorePoints[ScoreType.BroFightPrevented]*regularPointModifier;
    // broFightScore += scorePoints[ScoreType.BroFightOccurred]*badManagementPointModifier;
    return broFightScore;
  }

  public int CalculatePerfectUrinalsScore() {
    int urinalsScore = 0;
    // urinalsScore += scorePoints[ScoreType.UrinalBroken]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalPeedIn]*regularPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalPoopedIn]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalRepaired]*regularPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalVomitedIn]*regularPointModifier;
    return urinalsScore;
  }

  public int CalculatePerfectStallsScore() {
    int stallsScore = 0;
    // stallsScore += scorePoints[ScoreType.StallBroken]*badManagementPointModifier;
    // stallsScore += scorePoints[ScoreType.StallPeedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallPoopedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallRepaired]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallVomitedIn]*regularPointModifier;
    return stallsScore;
  }

  public int CalculatePerfectSinksScore() {
    int sinksScore = 0;
    // sinksScore += scorePoints[ScoreType.SinkBroken]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPeedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPoopedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkRepaired]*regularPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkHandsWashedIn]*regularPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkVomitedIn]*regularPointModifier;
    return sinksScore;
  }

}
