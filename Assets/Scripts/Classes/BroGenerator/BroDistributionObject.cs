using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BroDistributionObject : DistributionObject {

    //-------------------------------------------------------------------------
    public Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>();
    public Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>();
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionXMoveSpeed = BroDistribution.AllBros;
    public float defaultXMoveSpeed = float.PositiveInfinity;
    public float defaultMinXMoveSpeed = 1;
    public float defaultMaxXMoveSpeed = 1;

    public BroDistribution broDistributionYMoveSpeed = BroDistribution.AllBros;
    public float defaultYMoveSpeed = float.PositiveInfinity;
    public float defaultMinYMoveSpeed = 1;
    public float defaultMaxYMoveSpeed = 1;
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionReliefType = BroDistribution.RandomBros;
    public ReliefRequired defaultReliefRequired = ReliefRequired.None;
    public ReliefRequired[] defaultReliefRequiredToChooseFrom = new ReliefRequired[]{ ReliefRequired.Pee, ReliefRequired.Poop };
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionFightProbability = BroDistribution.AllBros;
    public float defaultBroFightProbability = float.PositiveInfinity;
    public float defaultMinBroFightProbability = 1;
    public float defaultMaxBroFightProbability = 1;
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionModifyBroFightProbabilityUsingScoreRatio = BroDistribution.AllBros;
    public bool defaultModifyBroFightProbablityUsingScoreRatio = false;
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, BroDistribution> broDistributionBathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultBathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultMinBathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultMaxBathroomObjectOccupationDuration = null;
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionLineQueueSkipType = BroDistribution.AllBros;
    public bool defaultLineQueueSkip = true;
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionChooseObjectOnLineSkip = BroDistribution.AllBros;
    public bool defaultChooseObjectOnLineSkip = true;
    public BathroomObjectType defaultLineSkipBathroomObject = BathroomObjectType.None;
    public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnLineQueueSkip = new BathroomObjectType[]{ BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionStartRoamingOnArrivalAtBathroomObjectInUse = BroDistribution.AllBros;
    public bool defaultStartRoamingOnArrivalAtBathroomObjectInUse = true;
    //-------------------------------------------------------------------------
    public BroDistribution broDistributionChooseObjectOnRelief = BroDistribution.AllBros;
    public bool defaultChooseObjectOnRelief = true;
    public BathroomObjectType defaultOnReliefBathroomObject = BathroomObjectType.None;
    public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnRelief = new BathroomObjectType[]{ BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };
    //-------------------------------------------------------------------------

    public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, DistributionSpacing newDistributionSpacing, Dictionary<BroType, float> newBroProbabilities, Dictionary<int, float> newEntranceQueueProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType, newDistributionSpacing) {
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

        InitializeOccupationDuration();
    }

    // all occupation duration defaults to 2 seconds, except exits
    public void InitializeOccupationDuration() {
        broDistributionBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, BroDistribution>();
        defaultBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();
        defaultMinBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();
        defaultMaxBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();

        InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2, 2);
        InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0, 0);
        InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2, 2);
        InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2, 2);
        InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2, 2);
    }

    public void InitializeBathroomObjectTypeOccupationDuration(BroDistribution newBroDistributBathroomObjectionOccupationDuration, BathroomObjectType bathroomObjectType, float newDefaultBathroomObjectTypeMinOccupationDuration, float newDefaultBathroomObjectTypeMaxOccupationDuration) {
        broDistributionBathroomObjectOccupationDuration[bathroomObjectType] = newBroDistributBathroomObjectionOccupationDuration;
        defaultBathroomObjectOccupationDuration[bathroomObjectType]  = float.PositiveInfinity;
        defaultMinBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultBathroomObjectTypeMinOccupationDuration;
        defaultMaxBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultBathroomObjectTypeMaxOccupationDuration;
    }

    public void ResetVariablesToInitialValues() {
        InitializeOccupationDuration();

        // Debug.Log("RESETTING BRO GENERATOR TO DEFAULT VALUES");
    }

    public override List<GameObject> CalculateDistributionPoints() {
        // Reset values to inital defaults
        ResetVariablesToInitialValues();

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
          ConfigureBroXMoveSpeed(broToGenerate);
          ConfigureBroYMoveSpeed(broToGenerate);
          ConfigureBroToGenerateFightCheckType(broToGenerate);
          ConfigureBroToGenerateModifyFightProbabilityUsingScoreRatio(broToGenerate);
          ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Exit);
          ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.HandDryer);
          ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Sink);
          ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Stall);
          ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Urinal);
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
            defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
          break;
          case(BroDistribution.RandomBros):
            defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
            broRef.reliefRequired = defaultReliefRequired;
          break;

          default:
            Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
          break;
        }
        
        broRef.reliefRequired = defaultReliefRequired;

        // if(broRef.type == BroType.ShyBro) {
        //   broRef.reliefRequired = ReliefRequired.Pee;
        // }

        return broToGenerate;
    }

    public GameObject ConfigureBroXMoveSpeed(GameObject broToGenerate) {
        Bro broRef = broToGenerate.GetComponent<Bro>();

        switch(broDistributionReliefType) {
            case(BroDistribution.NoBros):
                if(defaultXMoveSpeed == float.PositiveInfinity) {
                    defaultXMoveSpeed = UnityEngine.Random.Range(defaultMinXMoveSpeed, defaultMaxXMoveSpeed);
                }
                broRef.SetXMoveSpeed(defaultXMoveSpeed);
            break;
            case(BroDistribution.AllBros):
                if(defaultXMoveSpeed == float.PositiveInfinity) {
                    defaultXMoveSpeed = UnityEngine.Random.Range(defaultMinXMoveSpeed, defaultMaxXMoveSpeed);
                }
                broRef.SetXMoveSpeed(defaultXMoveSpeed);
            break;
            case(BroDistribution.RandomBros):
                defaultXMoveSpeed = UnityEngine.Random.Range(defaultMinXMoveSpeed, defaultMaxXMoveSpeed);
                broRef.SetXMoveSpeed(defaultXMoveSpeed);
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return broToGenerate;
    }

    public GameObject ConfigureBroYMoveSpeed(GameObject broToGenerate) {
        Bro broRef = broToGenerate.GetComponent<Bro>();

        switch(broDistributionReliefType) {
            case(BroDistribution.NoBros):
                if(defaultYMoveSpeed == float.PositiveInfinity) {
                    defaultYMoveSpeed = UnityEngine.Random.Range(defaultMinYMoveSpeed, defaultMaxYMoveSpeed);
                }
                broRef.SetYMoveSpeed(defaultYMoveSpeed);
            break;
            case(BroDistribution.AllBros):
                if(defaultYMoveSpeed == float.PositiveInfinity) {
                    defaultYMoveSpeed = UnityEngine.Random.Range(defaultMinYMoveSpeed, defaultMaxYMoveSpeed);
                }
                broRef.SetYMoveSpeed(defaultYMoveSpeed);
            break;
            case(BroDistribution.RandomBros):
                defaultYMoveSpeed = UnityEngine.Random.Range(defaultMinYMoveSpeed, defaultMaxYMoveSpeed);
                broRef.SetYMoveSpeed(defaultYMoveSpeed);
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return broToGenerate;
    }

    public GameObject ConfigureBroToGenerateFightCheckType(GameObject broToGenerate) {
        Bro broRef = broToGenerate.GetComponent<Bro>();
        switch(broDistributionFightProbability) {
            case(BroDistribution.NoBros):
                broRef.baseProbabilityOfFightOnCollisionWithBro = 0f;
            break;
            case(BroDistribution.AllBros):
                if(defaultBroFightProbability == float.PositiveInfinity) {
                    defaultBroFightProbability = UnityEngine.Random.Range(defaultMinBroFightProbability, defaultMaxBroFightProbability);
                }
                broRef.baseProbabilityOfFightOnCollisionWithBro = defaultBroFightProbability;
            break;
            case(BroDistribution.RandomBros):
                defaultBroFightProbability = UnityEngine.Random.Range(defaultMinBroFightProbability, defaultMaxBroFightProbability);
                broRef.baseProbabilityOfFightOnCollisionWithBro = defaultBroFightProbability;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return broToGenerate;
    }

    public GameObject ConfigureBroToGenerateModifyFightProbabilityUsingScoreRatio(GameObject broToGenerate) {
        Bro broRef = broToGenerate.GetComponent<Bro>();

        switch(broDistributionModifyBroFightProbabilityUsingScoreRatio) {
            case(BroDistribution.NoBros):
                broRef.modifyBroFightProbablityUsingScoreRatio = false;
            break;
            case(BroDistribution.AllBros):
                broRef.modifyBroFightProbablityUsingScoreRatio = defaultModifyBroFightProbablityUsingScoreRatio;
            break;
            case(BroDistribution.RandomBros):
                // Simple 50/50 odds of modifying score, don't see a reason to ever use random on this
                // in the future reconfigure for a probability of a random range, like fight probability does
                // except do it in terms of the possibility of the boolean value being set
                if(UnityEngine.Random.Range(0, 1) == 0) {
                    broRef.modifyBroFightProbablityUsingScoreRatio = true;
                }
                else {
                    broRef.modifyBroFightProbablityUsingScoreRatio = false;
                }
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return broToGenerate;
    }

    public GameObject ConfigureBroToGenerateBathroomObjectOccupationDuration(GameObject broToGenerate, BathroomObjectType bathroomObjectType) {
        Bro broRef = broToGenerate.GetComponent<Bro>();
        // Debug.Log(broDistributionBathroomObjectOccupationDuration[bathroomObjectType].ToString());
        // Debug.Log(bathroomObjectType.ToString());
        // Debug.Log(broRef.occupationDuration);
        // Debug.Log(broRef.occupationDuration.ContainsKey(bathroomObjectType));

        switch(broDistributionBathroomObjectOccupationDuration[bathroomObjectType]) {
            case(BroDistribution.NoBros):
                broRef.occupationDuration[bathroomObjectType] = 2;
            break;
            case(BroDistribution.AllBros):
                // Debug.Log(broRef.occupationDuration[bathroomObjectType]);
                if(defaultBathroomObjectOccupationDuration[bathroomObjectType] == float.PositiveInfinity) {
                    defaultBathroomObjectOccupationDuration[bathroomObjectType] = UnityEngine.Random.Range(defaultMinBathroomObjectOccupationDuration[bathroomObjectType], defaultMaxBathroomObjectOccupationDuration[bathroomObjectType]);
                }
                broRef.occupationDuration[bathroomObjectType] = defaultBathroomObjectOccupationDuration[bathroomObjectType];
            break;
            case(BroDistribution.RandomBros):
                defaultBathroomObjectOccupationDuration[bathroomObjectType] = UnityEngine.Random.Range(defaultMinBathroomObjectOccupationDuration[bathroomObjectType], defaultMaxBathroomObjectOccupationDuration[bathroomObjectType]);
                broRef.occupationDuration[bathroomObjectType] = defaultBathroomObjectOccupationDuration[bathroomObjectType];
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
            broRef.skipLineQueue = defaultLineQueueSkip;
          break;
          case(BroDistribution.AllBros):
            broRef.skipLineQueue = defaultLineQueueSkip;
          break;
          case(BroDistribution.RandomBros):
            defaultLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            broRef.skipLineQueue = defaultLineQueueSkip;
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
            broRef.chooseRandomBathroomObjectOnSkipLineQueue = defaultChooseObjectOnLineSkip;
          break;
          case(BroDistribution.RandomBros):
            defaultLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            broRef.chooseRandomBathroomObjectOnSkipLineQueue = defaultChooseObjectOnLineSkip;
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
            broRef.startRoamingOnArrivalAtBathroomObjectInUse = defaultStartRoamingOnArrivalAtBathroomObjectInUse;
          break;
          case(BroDistribution.RandomBros):
            defaultStartRoamingOnArrivalAtBathroomObjectInUse = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            broRef.startRoamingOnArrivalAtBathroomObjectInUse = defaultStartRoamingOnArrivalAtBathroomObjectInUse;
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
            broRef.chooseRandomBathroomObjectAfterRelieved = defaultChooseObjectOnRelief;
          break;
          case(BroDistribution.RandomBros):
            defaultLineQueueSkip = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            broRef.chooseRandomBathroomObjectAfterRelieved = defaultChooseObjectOnRelief;
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
        return this;
    }

    public BroDistributionObject SetXMoveSpeed(BroDistribution typeOfBroDistribution, float newDefaultMinXMoveSpeed, float newDefaultMaxXMoveSpeed) {
        broDistributionXMoveSpeed = typeOfBroDistribution;
        defaultMinXMoveSpeed = newDefaultMinXMoveSpeed;
        defaultMaxXMoveSpeed = newDefaultMaxXMoveSpeed;
        return this;
    }

    public BroDistributionObject SetYMoveSpeed(BroDistribution typeOfBroDistribution, float newDefaultMinYMoveSpeed, float newDefaultMaxYMoveSpeed) {
        broDistributionYMoveSpeed = typeOfBroDistribution;
        defaultMinYMoveSpeed = newDefaultMinYMoveSpeed;
        defaultMaxYMoveSpeed = newDefaultMaxYMoveSpeed;
        return this;
    }

    public BroDistributionObject SetFightProbability(BroDistribution newBroDistributionFightCheckType, float newDefaultMinBroFightProbability, float newDefaultMaxBroFightProbability) {
        broDistributionFightProbability = newBroDistributionFightCheckType;
        defaultMinBroFightProbability = newDefaultMinBroFightProbability;
        defaultMaxBroFightProbability = newDefaultMaxBroFightProbability;
        return this;
    }

    public BroDistributionObject SetModifyFightProbabilityUsingScoreRatio(BroDistribution newBroDistributionModifyBroFightProbabilityUsingScoreRatio, bool newDefaultModifyBroFightProbablityUsingScoreRatio) {
        broDistributionModifyBroFightProbabilityUsingScoreRatio = newBroDistributionModifyBroFightProbabilityUsingScoreRatio;
        defaultModifyBroFightProbablityUsingScoreRatio = newDefaultModifyBroFightProbablityUsingScoreRatio;
        return this;
    }

    public BroDistributionObject SetBathroomObjectOccupationDuration(BroDistribution newBroDistributBathroomObjectionOccupationDuration, BathroomObjectType bathroomObjectType, float newDefaultMinBathroomObjectTypeOccupationDuration, float newMaxDefaultBathroomObjectTypeOccupationDuration) {
        broDistributionBathroomObjectOccupationDuration[bathroomObjectType] = newBroDistributBathroomObjectionOccupationDuration;
        defaultMinBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultMinBathroomObjectTypeOccupationDuration;
        defaultMaxBathroomObjectOccupationDuration[bathroomObjectType] = newMaxDefaultBathroomObjectTypeOccupationDuration;
        return this;
    }

    public BroDistributionObject SetLineQueueSkipType(BroDistribution newBroDistributionLineQueueSkipType, bool newBroLineQueueSkip) {
        broDistributionLineQueueSkipType = newBroDistributionLineQueueSkipType;
        defaultLineQueueSkip = newBroLineQueueSkip;
        return this;
    }

    public BroDistributionObject SetChooseObjectOnLineSkip(BroDistribution newBroDistributionChooseObjectOnLineSkip, bool newBroChooseObjectOnLineSkip) {
        broDistributionChooseObjectOnLineSkip = newBroDistributionChooseObjectOnLineSkip;
        defaultChooseObjectOnLineSkip = newBroChooseObjectOnLineSkip;
        //TO DO SET TYPES OF OBJECTS THAT CAN BE CHOSEN FROM
        return this;
    }

    public BroDistributionObject SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse, bool newBroStartRoamingOnArrivalAtBathroomObjectInUse) {
        broDistributionStartRoamingOnArrivalAtBathroomObjectInUse = newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse;
        defaultStartRoamingOnArrivalAtBathroomObjectInUse = newBroStartRoamingOnArrivalAtBathroomObjectInUse;
        return this;
    }

    public BroDistributionObject SetChooseObjectOnRelief(BroDistribution newBroDistributionChooseObjectOnRelief, bool newBroChooseObjectOnRelief) {
        broDistributionChooseObjectOnRelief = newBroDistributionChooseObjectOnRelief;
        defaultChooseObjectOnRelief = newBroChooseObjectOnRelief;
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
