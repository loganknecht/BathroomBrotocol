using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum BroDistribution {
  NoBros,
  AllBros, //  picks a random bro type,sets them all to same bro type
  RandomBros // picks a random bro type everytime
}

public class BroDistributionObject : DistributionObject {
  public Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>();
  public Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>();

  public BroDistribution broDistributionReliefType = BroDistribution.RandomBros;
  public ReliefRequired defaultReliefRequired = ReliefRequired.None;
  public ReliefRequired[] defaultReliefRequiredToChooseFrom = new ReliefRequired[]{ ReliefRequired.Pee, ReliefRequired.Poop };

  public BroDistribution broDistributionFightProbability = BroDistribution.AllBros;
  public float defaultBroFightProbability = 1;

  public BroDistribution broDistributionLineQueueSkipType = BroDistribution.AllBros;
  public bool defaultBroLineQueueSkip = true;

  public BroDistribution broDistributionChooseObjectOnLineSkip = BroDistribution.AllBros;
  public bool defaultBroChooseObjectOnLineSkip = true;
  public BathroomObjectType defaultLineSkipBathroomObject = BathroomObjectType.None;
  public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnLineQueueSkip = new BathroomObjectType[]{ BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };

  public BroDistribution broDistributionStartRoamingOnArrivalAtBathroomObjectInUse = BroDistribution.AllBros;
  public bool defaultBroStartRoamingOnArrivalAtBathroomObjectInUse = true;

  public BroDistribution broDistributionChooseObjectOnRelief = BroDistribution.AllBros;
  public bool defaultBroChooseObjectOnRelief = true;
  public BathroomObjectType defaultOnReliefBathroomObject = BathroomObjectType.None;
  public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnRelief = new BathroomObjectType[]{ BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };

  public List<BathroomObjectType> bathroomObjectTypesToChooseFromOnLineQueueSkip = new List<BathroomObjectType>();

  public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, DistributionSpacing newDistributionSpacing, Dictionary<BroType, float> newBroProbabilities, Dictionary<int, float> newEntranceQueueProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType, newDistributionSpacing) {

    if(bathroomObjectTypesToChooseFromOnLineQueueSkip.Count == 0) {
      bathroomObjectTypesToChooseFromOnLineQueueSkip.Add(BathroomObjectType.Sink);
      bathroomObjectTypesToChooseFromOnLineQueueSkip.Add(BathroomObjectType.Stall);
      bathroomObjectTypesToChooseFromOnLineQueueSkip.Add(BathroomObjectType.Urinal);
    }

    if(newBroProbabilities == null) {
      ConfigureUniformBroTypeDistribution();
    }
    else {
      broProbabilities = newBroProbabilities;
    }

    if(newEntranceQueueProbabilities == null) {
    // entranceQueueProbabilities[0]
    }
    else {
      entranceQueueProbabilities = newEntranceQueueProbabilities;
    }

    defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
    defaultLineSkipBathroomObject = Factory.Instance.SelectRandomBathroomObjectType(defaultBathroomObjectsToChooseFromOnLineQueueSkip);
    defaultOnReliefBathroomObject = Factory.Instance.SelectRandomBathroomObjectType(defaultBathroomObjectsToChooseFromOnRelief);
  }

  public override List<GameObject> CalculateDistributionPoints() {
    foreach(GameObject gameObj in distributionPoints) {
      UnityEngine.GameObject.Destroy(gameObj);
    }

    List<GameObject> newDistributionPoints = new List<GameObject>();

    for(int i = 0; i < numberOfPointsToGenerate; i++) {
      float selectedPointOverInterval = 0;
      switch(distributionSpacing) {
        case(DistributionSpacing.Random):
          selectedPointOverInterval = UnityEngine.Random.Range(0f, 1f);
        break;
        case(DistributionSpacing.Uniform):
          selectedPointOverInterval = ((1f/numberOfPointsToGenerate) * i);
        break;
        default:
          Debug.Log("AN UNEXPECTED DISTRIBUTION SPACING WAS USED FOR A DISTRIBUTION OBJECT!!!");
        break;
      }
      selectedPointOverInterval = selectedPointOverInterval * (endTime - startTime);
      if(selectedPointOverInterval != 0) {
        selectedPointOverInterval = ApplyDistributionTypeToValue(selectedPointOverInterval, distributionType);
      }

      int selectedEntrance = CalculateProbabilityValue<int>(entranceQueueProbabilities);

      GameObject broToGenerate = Factory.Instance.GenerateBroGameObject(CalculateProbabilityValue<BroType>(broProbabilities));
      Bro broRefToGenerate = broToGenerate.GetComponent<Bro>();

      broToGenerate.transform.parent = BroManager.Instance.transform;
      broToGenerate.SetActive(false);

      ConfigureBroToGenerateReliefType(broToGenerate);
      ConfigureBroToGenerateFightCheckType(broToGenerate);
      ConfigureBroToGenerateLineQueueSkipType(broToGenerate);
      ConfigureBroToGenerateChooseObjectOnLineSkip(broToGenerate);
      ConfigureBroToGenerateStartRoamingOnArrivalAtBathroomObjectInUse(broToGenerate);
      ConfigureBroToGenerateChooseObjectOnRelief(broToGenerate);
      if(broRefToGenerate.type == BroType.DrunkBro) {
        broRefToGenerate.speechBubbleReference.displaySpeechBubble = false;
      }

      CameraManager.Instance.rotateCameraReference.RotateBroGameObject(broToGenerate);

      GameObject newDistributionPoint = new GameObject("BroDistributionPoint");
      newDistributionPoint.transform.parent = BroGenerator.Instance.transform;
      newDistributionPoint.AddComponent<BroDistributionPoint>().ConfigureDistributionPoint(selectedPointOverInterval + startTime,
                                                                                           selectedEntrance,
                                                                                           // CalculateProbabilityValue<BroType>(broProbabilities));
                                                                                           broToGenerate);
      // Debug.Log("Dist point entrance: " + newDistributionPoint.GetComponent<BroDistributionPoint>().selectedEntrance);
      newDistributionPoints.Add(newDistributionPoint);
    }

    distributionPoints = newDistributionPoints;

    return newDistributionPoints;
  }

  public GameObject ConfigureBroToGenerateReliefType(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    // Factory.Instance.SelectRandomReliefType(ReliefRequired.Pee, ReliefRequired.Poop)
    switch(broDistributionReliefType) {
      case(BroDistribution.NoBros):
      break;
      case(BroDistribution.AllBros):
        broRef.reliefRequired = defaultReliefRequired;
      break;
      case(BroDistribution.RandomBros):
        defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
        broRef.reliefRequired = defaultReliefRequired;
      break;

      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    if(broRef.type == BroType.ShyBro) {
      broRef.reliefRequired = ReliefRequired.Pee;
    }

    return broToGenerate;
  }

  public GameObject ConfigureBroToGenerateFightCheckType(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    switch(broDistributionFightProbability) {
      case(BroDistribution.NoBros):
        broRef.probabilityOfFightOnCollisionWithBro = 0f;
      break;
      case(BroDistribution.AllBros):
        broRef.probabilityOfFightOnCollisionWithBro = defaultBroFightProbability;
      break;
      case(BroDistribution.RandomBros):
        defaultBroFightProbability = UnityEngine.Random.Range(0f, 1f);
        broRef.probabilityOfFightOnCollisionWithBro = defaultBroFightProbability;
      break;
      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    return broToGenerate;
  }

  public GameObject ConfigureBroToGenerateLineQueueSkipType(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    switch(broDistributionLineQueueSkipType) {
      case(BroDistribution.NoBros):
        broRef.skipLineQueue = defaultBroLineQueueSkip;
      break;
      case(BroDistribution.AllBros):
        broRef.skipLineQueue = defaultBroLineQueueSkip;
      break;
      case(BroDistribution.RandomBros):
        defaultBroLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
        broRef.skipLineQueue = defaultBroLineQueueSkip;
      break;
      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    return broToGenerate;
  }

  public GameObject ConfigureBroToGenerateChooseObjectOnLineSkip(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    switch(broDistributionChooseObjectOnLineSkip) {
      case(BroDistribution.NoBros):
        // do nothing
      break;
      case(BroDistribution.AllBros):
        broRef.chooseRandomBathroomObjectOnSkipLineQueue = defaultBroChooseObjectOnLineSkip;
      break;
      case(BroDistribution.RandomBros):
        defaultBroLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
        broRef.chooseRandomBathroomObjectOnSkipLineQueue = defaultBroChooseObjectOnLineSkip;
      break;
      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    return broToGenerate;
  }

//---------
  public GameObject ConfigureBroToGenerateStartRoamingOnArrivalAtBathroomObjectInUse(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    switch(broDistributionStartRoamingOnArrivalAtBathroomObjectInUse) {
      case(BroDistribution.NoBros):
        // do nothing
      break;
      case(BroDistribution.AllBros):
        broRef.startRoamingOnArrivalAtBathroomObjectInUse = defaultBroStartRoamingOnArrivalAtBathroomObjectInUse;
      break;
      case(BroDistribution.RandomBros):
        defaultBroStartRoamingOnArrivalAtBathroomObjectInUse = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
        broRef.startRoamingOnArrivalAtBathroomObjectInUse = defaultBroStartRoamingOnArrivalAtBathroomObjectInUse;
      break;
      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    return broToGenerate;
  }
//----------

  public GameObject ConfigureBroToGenerateChooseObjectOnRelief(GameObject broToGenerate) {
    Bro broRef = broToGenerate.GetComponent<Bro>();
    switch(broDistributionChooseObjectOnRelief) {
      case(BroDistribution.NoBros):
        // do nothing
      break;
      case(BroDistribution.AllBros):
        broRef.chooseRandomBathroomObjectAfterRelieved = defaultBroChooseObjectOnRelief;
      break;
      case(BroDistribution.RandomBros):
        defaultBroLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
        broRef.chooseRandomBathroomObjectAfterRelieved = defaultBroChooseObjectOnRelief;
      break;
      default:
        Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
      break;
    }

    return broToGenerate;
  }
  //-------------
 public BroDistributionObject SetReliefType(BroDistribution typeOfBroDistribution, params ReliefRequired[] newReliefRequiredToChooseFrom) {
    broDistributionReliefType = typeOfBroDistribution;
    defaultReliefRequiredToChooseFrom = newReliefRequiredToChooseFrom;
    defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
    return this;
  }

  public BroDistributionObject SetFightProbability(BroDistribution newBroDistributionFightCheckType, float newDefaultBroFightProbability) {
    broDistributionFightProbability = newBroDistributionFightCheckType;
    defaultBroFightProbability = newDefaultBroFightProbability;
    return this;
  }

  public BroDistributionObject SetLineQueueSkipType(BroDistribution newBroDistributionLineQueueSkipType, bool newBroLineQueueSkip) {
    broDistributionLineQueueSkipType = newBroDistributionLineQueueSkipType;
    defaultBroLineQueueSkip = newBroLineQueueSkip;
    return this;
  }

  public BroDistributionObject SetChooseObjectOnLineSkip(BroDistribution newBroDistributionChooseObjectOnLineSkip, bool newBroChooseObjectOnLineSkip) {
    broDistributionChooseObjectOnLineSkip = newBroDistributionChooseObjectOnLineSkip;
    defaultBroChooseObjectOnLineSkip = newBroChooseObjectOnLineSkip;
    //TO DO SET TYPES OF OBJECTS THAT CAN BE CHOSEN FROM
    return this;
  }

  public BroDistributionObject SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse, bool newBroStartRoamingOnArrivalAtBathroomObjectInUse) {
    broDistributionStartRoamingOnArrivalAtBathroomObjectInUse = newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse;
    defaultBroStartRoamingOnArrivalAtBathroomObjectInUse = newBroStartRoamingOnArrivalAtBathroomObjectInUse;
    return this;
  }

  public BroDistributionObject SetChooseObjectOnRelief(BroDistribution newBroDistributionChooseObjectOnRelief, bool newBroChooseObjectOnRelief) {
    broDistributionChooseObjectOnRelief = newBroDistributionChooseObjectOnRelief;
    defaultBroChooseObjectOnRelief = newBroChooseObjectOnRelief;
    //TO DO SET TYPES OF OBJECTS THAT CAN BE CHOSEN FROM
    return this;
  }

  // Default of enum is zero
  // From http://msdn.microsoft.com/en-us/library/xwth0h0d.aspx
  // Returns null for reference types and zero for numeric value types.
  // For structs, it will return each member of the struct initialized to zero
  // or null depending on whether they are value or reference types.
  public T CalculateProbabilityValue<T>(Dictionary<T, float> probabilities) {
    float randomlySelectedFloat = UnityEngine.Random.value;
    float cumulativeWeight = 0;

    foreach(KeyValuePair<T, float> probability in probabilities) {
      cumulativeWeight += probability.Value;
      if(randomlySelectedFloat < cumulativeWeight) {
        return probability.Key;
      }
    }

    return default(T);
  }

  public void ConfigureUniformBroTypeDistribution() {
    // float uniformDistributionStepSizeForAllBros = 0f;
    float uniformBroTypeDistribution = 1/11;

    // broProbabilities[BroType.None] = uniformBroTypeDistribution;
    broProbabilities[BroType.BluetoothBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.ChattyBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.CuttingBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.DrunkBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.GassyBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.GenericBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.Hobro] = uniformBroTypeDistribution;
    broProbabilities[BroType.RichBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.ShyBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.SlobBro] = uniformBroTypeDistribution;
    broProbabilities[BroType.TimeWasterBro] = uniformBroTypeDistribution;
  }
}
