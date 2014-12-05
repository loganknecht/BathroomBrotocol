using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// TODO FEATURES
// Think about bolting on features where you can give the distributionobject a boolean that when true says to generate only the first type of bro created.

// Example Usage:
// public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {

// Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
// Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

// BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
// firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
//     .SetXMoveSpeed(BroDistribution.AllBros, 1, 1)
//     .SetYMoveSpeed(BroDistribution.AllBros , 1, 1)
//     .SetFightProbability(BroDistribution.AllBros, 0f, 1f)
//     .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, true)
//     .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 2f, 2f)
//     .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
//     .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
//     .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
//     .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
//     .SetLineQueueSkipType(BroDistribution.AllBros, true)
//     .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
//     .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
//     .SetChooseObjectOnRelief(BroDistribution.AllBros, false);
    // TODO
    // SetReliefType(BroDistribution typeOfBroDistribution, params ReliefRequired[] newReliefRequiredToChooseFrom) {

// BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                         // firstWave,
                                                                        // });
public class BroGenerator : MonoBehaviour {

  public List<GameObject> distributionPoints = new List<GameObject>();

  public float broGenerationTimer = 0f;
  public bool loopGenerationLogic = false;
  // public float broGenerationTimerMax = 5f;

  public bool isPaused = false;

  //BEGINNING OF SINGLETON CODE CONFIGURATION
  private static volatile BroGenerator _instance;
  private static object _lock = new object();

  //Stops the lock being created ahead of time if it's not necessary
  static BroGenerator() {
  }

  public static BroGenerator Instance {
    get {
      if(_instance == null) {
        lock(_lock) {
          if (_instance == null) {
            GameObject broManagerGameObject = new GameObject("BroGenerator");
            _instance = (broManagerGameObject.AddComponent<BroGenerator>()).GetComponent<BroGenerator>();
          }
        }
      }
      return _instance;
    }
  }

  private BroGenerator() {
  }

  public void Awake() {
    //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
    //the script, which is assumedly attached to some GameObject. This in turn allows the instance
    //to be assigned when a game object is given this script in the scene view.
    //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
    //another call to it, so that you don't need to worry if it exists or not.
    _instance = this;
  }
  //END OF SINGLETON CODE CONFIGURATION

  void Start() {
  }

	// Update is called once per frame
	void Update () {
    PerformGenerationLogic();
    PerformGenerationTimerLogic();
	}

  public void SetDistributionLogic(DistributionObject[] distributionObjects) {
    broGenerationTimer = 0;

    // Debug.Log("Setting distribution logic");
    foreach(GameObject gameObj in distributionPoints) {
      Destroy(gameObj);
    }
    distributionPoints = new List<GameObject>();
    foreach(BroDistributionObject broDistributionObject in distributionObjects) {

      List<GameObject> broDistributionPointsToAdd = null;
      broDistributionPointsToAdd = broDistributionObject.CalculateDistributionPoints();

      // Change to add range?
      foreach(GameObject broDistributionPoint in broDistributionPointsToAdd) {
        distributionPoints.Add(broDistributionPoint);
      }
    }
  }

  // Convert to override this if base class created
  public void PerformGenerationLogic() {
    foreach(GameObject gameObj in distributionPoints) {
      if(!gameObj.GetComponent<BroDistributionPoint>().hasBeenDistributed) {
        if(gameObj.GetComponent<BroDistributionPoint>().distributionTime < broGenerationTimer) {
          gameObj.GetComponent<BroDistributionPoint>().hasBeenDistributed = true;
          // PerformBroDistribution(gameObj.GetComponent<BroDistributionPoint>().broTypeToDistribute, gameObj.GetComponent<BroDistributionPoint>().selectedEntrance);
          PerformBroDistribution(gameObj.GetComponent<BroDistributionPoint>().broToDistribute, gameObj.GetComponent<BroDistributionPoint>().selectedEntrance);
        }
      }
    }
  }

  // public void PerformBroDistribution(GameObject broTypeToDistribute, int selectedEntrance) {
  public void PerformBroDistribution(GameObject broToDistribute, int selectedEntrance) {
    // EntranceQueueManager.Instance.GenerateBroInEntranceQueueByType(broTypeToDistribute, selectedEntrance);
    broToDistribute.SetActive(true);
    // Bro broRef = broToDistribute.GetComponent<Bro>();
    Vector3 lineQueueBeingAddedToLastTilePosition =  EntranceQueueManager.Instance.lineQueues[selectedEntrance].GetComponent<LineQueue>().queueTileObjects[EntranceQueueManager.Instance.lineQueues[selectedEntrance].GetComponent<LineQueue>().queueTileObjects.Count-1].transform.position;
    // Vector3 lineQueueBeingAddedToLastTilePosition =  EntranceQueueManager.Instance.lineQueues[selectedEntrance].GetComponent<LineQueue>().queueTileObjects.Last().transform.position;
    broToDistribute.transform.position = new Vector3(lineQueueBeingAddedToLastTilePosition.x, lineQueueBeingAddedToLastTilePosition.y, broToDistribute.transform.position.z);
    EntranceQueueManager.Instance.AddBroToEntranceQueue(broToDistribute, selectedEntrance);
  }

  public void PerformGenerationTimerLogic() {
    if(!isPaused) {
      broGenerationTimer += Time.deltaTime;
      // if(broGenerationTimer > broGenerationTimerMax) {
      //   PerformGenerationTimerResetLogic();
      // }
    }
  }

  public void PerformGenerationTimerResetLogic() {
    broGenerationTimer = 0f;
    // if(generationTimerMaxIsStochastic) {
    //   broGenerationTimerMax = Random.Range(minBroGenerationTimerMax, maxBroGenerationTimerMax);
    // }
  }

  public bool HasFinishedGenerating() {
    bool foundDistributionPointThatWasNotDistributed = false;
    foreach(GameObject distributionPointGameObject in distributionPoints) {
      if(!distributionPointGameObject.GetComponent<DistributionPoint>().hasBeenDistributed) {
        foundDistributionPointThatWasNotDistributed = true;
      }
    }

    if(foundDistributionPointThatWasNotDistributed) {
      return false;
    }
    else {
      return true;
    }
  }
}
