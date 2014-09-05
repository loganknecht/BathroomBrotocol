using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class ScoreTracker : MonoBehaviour {
public class ScoreTracker : BaseBehavior {

  public int currentScore = 0;
  public int perfectScore = 0;

  public int regularPointModifier = 100;
  public int brotocolPointModifier = 200;
  public int badManagementPointModifier = -100;
  // public int arbitraryPointModifier = 100;

  public Dictionary<ScoreType, int> scorePoints;

	// Use this for initialization
	void Start () {
    InitializeScoreTracker();
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
	}

  void InitializeScoreTracker() {
    scorePoints = new Dictionary<ScoreType, int>();

    // scorePoints.Add(ScoreType.None, 0);

    //----------------------------------------------------------------------------
    scorePoints.Add(ScoreType.BluetoothBroEntered, 0);
    scorePoints.Add(ScoreType.BluetoothBroRelieved, 0);
    scorePoints.Add(ScoreType.BluetoothBroHandsWashed, 0);
    scorePoints.Add(ScoreType.BluetoothBroExited, 0);
    //----------------------------------------------------------------------------
    scorePoints.Add(ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.BluetoothBroBrotocolNoAdjacentBro, 0);

    scorePoints.Add(ScoreType.ChattyBroEntered, 0);
    scorePoints.Add(ScoreType.ChattyBroRelieved, 0);
    scorePoints.Add(ScoreType.ChattyBroHandsWashed, 0);
    scorePoints.Add(ScoreType.ChattyBroExited, 0);
    //----------------------------------------------------------------------------
    scorePoints.Add(ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.ChattyBroBrotocolAdjacentBro, 0);

    scorePoints.Add(ScoreType.CuttingBroEntered, 0);
    scorePoints.Add(ScoreType.CuttingBroRelieved, 0);
    scorePoints.Add(ScoreType.CuttingBroHandsWashed, 0);
    scorePoints.Add(ScoreType.CuttingBroExited, 0);
    //----------------------------------------------------------------------------
    //No brotocol, not fair to player

    scorePoints.Add(ScoreType.DrunkBroEntered, 0);
    scorePoints.Add(ScoreType.DrunkBroRelieved, 0);
    scorePoints.Add(ScoreType.DrunkBroHandsWashed, 0);
    scorePoints.Add(ScoreType.DrunkBroExited, 0);
    //---------------------------------------------------------------------------
    scorePoints.Add(ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut, 0);

    scorePoints.Add(ScoreType.FartingBroEntered, 0);
    scorePoints.Add(ScoreType.FartingBroRelieved, 0);
    scorePoints.Add(ScoreType.FartingBroHandsWashed, 0);
    scorePoints.Add(ScoreType.FartingBroExited, 0);
    //---------------------------------------------------------------------------
    scorePoints.Add(ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.FartingBroBrotocolNoAdjacentBro, 0);

    scorePoints.Add(ScoreType.GenericBroEntered, 0);
    scorePoints.Add(ScoreType.GenericBroRelieved, 0);
    scorePoints.Add(ScoreType.GenericBroHandsWashed, 0);
    scorePoints.Add(ScoreType.GenericBroExited, 0);
    //---------------------------------------------------------------------------
    scorePoints.Add(ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.GenericBroBrotocolNoAdjacentBro, 0);

    scorePoints.Add(ScoreType.RichBroEntered, 0);
    scorePoints.Add(ScoreType.RichBroRelieved, 0);
    scorePoints.Add(ScoreType.RichBroHandsWashed, 0);
    scorePoints.Add(ScoreType.RichBroExited, 0);
    //----------------------------------------------------------------------------
    scorePoints.Add(ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.RichBroBrotocolNoAdjacentBro, 0);
    scorePoints.Add(ScoreType.RichBroBrotocolNoJanitorSummoned, 0);

    scorePoints.Add(ScoreType.ShyBroEntered, 0);
    scorePoints.Add(ScoreType.ShyBroRelieved, 0);
    scorePoints.Add(ScoreType.ShyBroHandsWashed, 0);
    scorePoints.Add(ScoreType.ShyBroExited, 0);
    //----------------------------------------------------------------------------
    // scorePoints.Add(ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.ShyBroBrotocolNoAdjacentBro, 0);
    scorePoints.Add(ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry, 0);

    scorePoints.Add(ScoreType.SlobBroEntered, 0);
    scorePoints.Add(ScoreType.SlobBroRelieved, 0);
    scorePoints.Add(ScoreType.SlobBroHandsWashed, 0);
    scorePoints.Add(ScoreType.SlobBroExited, 0);
    //----------------------------------------------------------------------------
    scorePoints.Add(ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject, 0);
    scorePoints.Add(ScoreType.SlobBroBrotocolNoAdjacentBro, 0);

    scorePoints.Add(ScoreType.TimeWasterBroEntered, 0);
    scorePoints.Add(ScoreType.TimeWasterBroRelieved, 0);
    scorePoints.Add(ScoreType.TimeWasterBroBrotocolFollowed, 0);
    scorePoints.Add(ScoreType.TimeWasterBroHandsWashed, 0);
    scorePoints.Add(ScoreType.TimeWasterBroExited, 0);
    //----------------------------------------------------------------------------

    scorePoints.Add(ScoreType.BroStandoffOccurred, 0);
    scorePoints.Add(ScoreType.BroFightPrevented, 0);
    scorePoints.Add(ScoreType.BroFightOccurred, 0);

    scorePoints.Add(ScoreType.UrinalBroken, 0);
    scorePoints.Add(ScoreType.UrinalPeedIn, 0);
    scorePoints.Add(ScoreType.UrinalPoopedIn, 0);
    scorePoints.Add(ScoreType.UrinalRepaired, 0);
    scorePoints.Add(ScoreType.UrinalVomitedIn, 0);

    scorePoints.Add(ScoreType.StallBroken, 0);
    scorePoints.Add(ScoreType.StallPeedIn, 0);
    scorePoints.Add(ScoreType.StallPoopedIn, 0);
    scorePoints.Add(ScoreType.StallRepaired, 0);
    scorePoints.Add(ScoreType.StallVomitedIn, 0);

    scorePoints.Add(ScoreType.SinkBroken, 0);
    scorePoints.Add(ScoreType.SinkPeedIn, 0);
    scorePoints.Add(ScoreType.SinkPoopedIn, 0);
    scorePoints.Add(ScoreType.SinkRepaired, 0);
    scorePoints.Add(ScoreType.SinkHandsWashedIn, 0);
    scorePoints.Add(ScoreType.SinkVomitedIn, 0);
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
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroRelieved]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroHandsWashed]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroExited]*regularPointModifier;

    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolNoAdjacentBro]*brotocolPointModifier;

    return bluetoothBrosScore;
  }

  public int CalculateChattyBroScore() {
    int chattyBrosScore = 0;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroRelieved]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroHandsWashed]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroExited]*regularPointModifier;

    chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolAdjacentBro]*brotocolPointModifier;

    return chattyBrosScore;
  }

  public int CalculateCuttingBroScore() {
    int cuttingBrosScore = 0;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroRelieved]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroHandsWashed]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroExited]*regularPointModifier;

    return cuttingBrosScore;
  }

  public int CalculateDrunkBroScore() {
    int drunkBrosScore = 0;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroRelieved]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroHandsWashed]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroExited]*regularPointModifier;

    drunkBrosScore += scorePoints[ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut]*brotocolPointModifier;

    return drunkBrosScore;
  }

  public int CalculateFartingBroScore() {
    int fartingBrosScore = 0;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroRelieved]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroHandsWashed]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroExited]*regularPointModifier;

    fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolNoAdjacentBro]*brotocolPointModifier;

    return fartingBrosScore;
  }

  public int CalculateGenericBroScore() {
    int genericBrosScore = 0;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroRelieved]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroHandsWashed]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroExited]*regularPointModifier;

    genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolNoAdjacentBro]*brotocolPointModifier;

    return genericBrosScore;
  }

  public int CalculateRichBroScore() {
    int richBrosScore = 0;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroRelieved]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroHandsWashed]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroExited]*regularPointModifier;

    richBrosScore += scorePoints[ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoJanitorSummoned]*brotocolPointModifier;

    return richBrosScore;
  }

  public int CalculateShyBroScore() {
    int shyBrosScore = 0;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroRelieved]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroHandsWashed]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroExited]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject];
    shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry]*brotocolPointModifier;

    return shyBrosScore;
  }

  public int CalculateSlobBroScore() {
    int slobBrosScore = 0;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroRelieved]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroHandsWashed]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroExited]*regularPointModifier;

    slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolNoAdjacentBro]*brotocolPointModifier;

    return slobBrosScore;
  }

  public int CalculateTimeWasterBroScore() {
    int timeWasterBrosScore = 0;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroRelieved]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroBrotocolFollowed]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroHandsWashed]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroExited]*regularPointModifier;
    return timeWasterBrosScore;
  }

  public int CalculateBroFightScore() {
    int broFightScore = 0;
    // broFightScore += scorePoints[ScoreType.BroStandoffOccurred];
    broFightScore += scorePoints[ScoreType.BroFightPrevented]*regularPointModifier;
    broFightScore += scorePoints[ScoreType.BroFightOccurred]*badManagementPointModifier;
    return broFightScore;
  }

  public int CalculateUrinalsScore() {
    int urinalsScore = 0;
    urinalsScore += scorePoints[ScoreType.UrinalBroken]*badManagementPointModifier;
    urinalsScore += scorePoints[ScoreType.UrinalPeedIn]*regularPointModifier;
    urinalsScore += scorePoints[ScoreType.UrinalPoopedIn]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalRepaired]*regularPointModifier;
    urinalsScore += scorePoints[ScoreType.UrinalVomitedIn]*regularPointModifier;
    return urinalsScore;
  }

  public int CalculateStallsScore() {
    int stallsScore = 0;
    stallsScore += scorePoints[ScoreType.StallBroken]*badManagementPointModifier;
    stallsScore += scorePoints[ScoreType.StallPeedIn]*regularPointModifier;
    stallsScore += scorePoints[ScoreType.StallPoopedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallRepaired]*regularPointModifier;
    stallsScore += scorePoints[ScoreType.StallVomitedIn]*regularPointModifier;
    return stallsScore;
  }

  public int CalculateSinksScore() {
    int sinksScore = 0;
    sinksScore += scorePoints[ScoreType.SinkBroken]*badManagementPointModifier;
    sinksScore += scorePoints[ScoreType.SinkPeedIn]*badManagementPointModifier;
    sinksScore += scorePoints[ScoreType.SinkPoopedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkRepaired]*regularPointModifier;
    sinksScore += scorePoints[ScoreType.SinkHandsWashedIn]*regularPointModifier;
    sinksScore += scorePoints[ScoreType.SinkVomitedIn]*regularPointModifier;
    return sinksScore;
  }

  //----------------------------------------------------------------------------
  // PERFECT SCORE LOGIC GOES HERE
  //----------------------------------------------------------------------------
  public int CalculatePerfectBluetoothBroScore() {
    int bluetoothBrosScore = 0;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroRelieved]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroHandsWashed]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroExited]*regularPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*regularPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*brotocolPointModifier;

    // bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    bluetoothBrosScore += scorePoints[ScoreType.BluetoothBroEntered]*brotocolPointModifier;

    return bluetoothBrosScore;
  }

  public int CalculatePerfectChattyBroScore() {
    int chattyBrosScore = 0;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroRelieved]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroHandsWashed]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroExited]*regularPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*regularPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*brotocolPointModifier;

    // chattyBrosScore += scorePoints[ScoreType.ChattyBroBrotocolAdjacentBro]*brotocolPointModifier;
    chattyBrosScore += scorePoints[ScoreType.ChattyBroEntered]*brotocolPointModifier;

    return chattyBrosScore;
  }

  public int CalculatePerfectCuttingBroScore() {
    int cuttingBrosScore = 0;
    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroRelieved]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroHandsWashed]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

    // cuttingBrosScore += scorePoints[ScoreType.CuttingBroExited]*regularPointModifier;
    cuttingBrosScore += scorePoints[ScoreType.CuttingBroEntered]*regularPointModifier;

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
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroRelieved]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroHandsWashed]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroExited]*regularPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*regularPointModifier;

    // drunkBrosScore += scorePoints[ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut]*brotocolPointModifier;
    drunkBrosScore += scorePoints[ScoreType.DrunkBroEntered]*brotocolPointModifier;

    return drunkBrosScore;
  }

  public int CalculatePerfectFartingBroScore() {
    int fartingBrosScore = 0;
    // fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroRelieved]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroHandsWashed]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroExited]*regularPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*regularPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*brotocolPointModifier;

    // fartingBrosScore += scorePoints[ScoreType.FartingBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    fartingBrosScore += scorePoints[ScoreType.FartingBroEntered]*brotocolPointModifier;

    return fartingBrosScore;
  }

  public int CalculatePerfectGenericBroScore() {
    int genericBrosScore = 0;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroRelieved]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroHandsWashed]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroExited]*regularPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*regularPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*brotocolPointModifier;

    // genericBrosScore += scorePoints[ScoreType.GenericBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    genericBrosScore += scorePoints[ScoreType.GenericBroEntered]*brotocolPointModifier;

    return genericBrosScore;
  }

  public int CalculatePerfectRichBroScore() {
    int richBrosScore = 0;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroRelieved]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroHandsWashed]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroExited]*regularPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*regularPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    // richBrosScore += scorePoints[ScoreType.RichBroBrotocolNoJanitorSummoned]*brotocolPointModifier;
    richBrosScore += scorePoints[ScoreType.RichBroEntered]*brotocolPointModifier;

    return richBrosScore;
  }

  public int CalculatePerfectShyBroScore() {
    int shyBrosScore = 0;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroRelieved]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroHandsWashed]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroExited]*regularPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*regularPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    // shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    // shyBrosScore += scorePoints[ScoreType.ShyBroBrotocolRelievedSelfInCorrectBathroomObjectTypeOnFirstTry]*brotocolPointModifier;
    shyBrosScore += scorePoints[ScoreType.ShyBroEntered]*brotocolPointModifier;

    return shyBrosScore;
  }

  public int CalculatePerfectSlobBroScore() {
    int slobBrosScore = 0;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroRelieved]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroHandsWashed]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroExited]*regularPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*regularPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolCorrectReliefTypeForTargetObject]*brotocolPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*brotocolPointModifier;

    // slobBrosScore += scorePoints[ScoreType.SlobBroBrotocolNoAdjacentBro]*brotocolPointModifier;
    slobBrosScore += scorePoints[ScoreType.SlobBroEntered]*brotocolPointModifier;

    return slobBrosScore;
  }

  public int CalculatePerfectTimeWasterBroScore() {
    int timeWasterBrosScore = 0;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroRelieved]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroBrotocolFollowed]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroHandsWashed]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    // timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroExited]*regularPointModifier;
    timeWasterBrosScore += scorePoints[ScoreType.TimeWasterBroEntered]*regularPointModifier;

    return timeWasterBrosScore;
  }

  public int CalculatePerfectBroFightScore() {
    int broFightScore = 0;
    // broFightScore += scorePoints[ScoreType.BroStandoffOccurred]*badManagementPointModifier;
    broFightScore += scorePoints[ScoreType.BroFightPrevented]*regularPointModifier;
    // broFightScore += scorePoints[ScoreType.BroFightOccurred]*badManagementPointModifier;
    return broFightScore;
  }

  public int CalculatePerfectUrinalsScore() {
    int urinalsScore = 0;
    // urinalsScore += scorePoints[ScoreType.UrinalBroken]*badManagementPointModifier;
    urinalsScore += scorePoints[ScoreType.UrinalPeedIn]*regularPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalPoopedIn]*badManagementPointModifier;
    // urinalsScore += scorePoints[ScoreType.UrinalRepaired]*regularPointModifier;
    urinalsScore += scorePoints[ScoreType.UrinalVomitedIn]*regularPointModifier;
    return urinalsScore;
  }

  public int CalculatePerfectStallsScore() {
    int stallsScore = 0;
    // stallsScore += scorePoints[ScoreType.StallBroken]*badManagementPointModifier;
    stallsScore += scorePoints[ScoreType.StallPeedIn]*regularPointModifier;
    stallsScore += scorePoints[ScoreType.StallPoopedIn]*regularPointModifier;
    // stallsScore += scorePoints[ScoreType.StallRepaired]*regularPointModifier;
    stallsScore += scorePoints[ScoreType.StallVomitedIn]*regularPointModifier;
    return stallsScore;
  }

  public int CalculatePerfectSinksScore() {
    int sinksScore = 0;
    // sinksScore += scorePoints[ScoreType.SinkBroken]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPeedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkPoopedIn]*badManagementPointModifier;
    // sinksScore += scorePoints[ScoreType.SinkRepaired]*regularPointModifier;
    sinksScore += scorePoints[ScoreType.SinkHandsWashedIn]*regularPointModifier;
    sinksScore += scorePoints[ScoreType.SinkVomitedIn]*regularPointModifier;
    return sinksScore;
  }

}
