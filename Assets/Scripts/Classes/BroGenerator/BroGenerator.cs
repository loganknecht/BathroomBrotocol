using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// TODO FEATURES
// Think about bolting on features where you can give the distributionobject a boolean that when true says to generate only the first type of bro created.
// Think about adding feature where bros are distributed uniformally?, but in such a way that it's applied to the distribution type uniformally?

// Example Usage:
// Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
// Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

// public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
// BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
// firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
// firstWave.SetFightCheckType(BroDistribution.AllBros, false);
// firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
// firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
// firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

// BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave }, DistributionType.Uniform);
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
    // Debug.Log("Setting distribution logic");
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

  public bool HasGeneratorFinished() {
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
