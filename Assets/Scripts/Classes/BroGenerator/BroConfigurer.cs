using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Jesus christ this is way too big, convert each property to be some object
//       that is managed and then modify those instead
public class BroConfigurer {
    //-------------------------------------------------------------------------
    public BroDistribution xMoveSpeed = BroDistribution.AllBros;
    public float defaultXMoveSpeed = float.PositiveInfinity;
    public float defaultMinXMoveSpeed = 2.5f;
    public float defaultMaxXMoveSpeed = 2.5f;
    
    public BroDistribution yMoveSpeed = BroDistribution.AllBros;
    public float defaultYMoveSpeed = float.PositiveInfinity;
    public float defaultMinYMoveSpeed = 2.5f;
    public float defaultMaxYMoveSpeed = 2.5f;
    //-------------------------------------------------------------------------
    public BroDistribution reliefType = BroDistribution.RandomBros;
    public ReliefRequired defaultReliefRequired = ReliefRequired.None;
    public ReliefRequired[] defaultReliefRequiredToChooseFrom = new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop };
    //-------------------------------------------------------------------------
    public BroDistribution fightProbability = BroDistribution.AllBros;
    public float defaultBroFightProbability = float.PositiveInfinity;
    public float defaultMinBroFightProbability = 1;
    public float defaultMaxBroFightProbability = 1;
    //-------------------------------------------------------------------------
    public BroDistribution modifyBroFightProbabilityUsingScoreRatio = BroDistribution.AllBros;
    public bool defaultModifyBroFightProbablityUsingScoreRatio = false;
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, BroDistribution> bathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultBathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultMinBathroomObjectOccupationDuration = null;
    public Dictionary<BathroomObjectType, float> defaultMaxBathroomObjectOccupationDuration = null;
    //-------------------------------------------------------------------------
    public BroDistribution lineQueueSkipType = BroDistribution.AllBros;
    public bool defaultLineQueueSkip = true;
    //-------------------------------------------------------------------------
    public BroDistribution chooseObjectOnLineSkip = BroDistribution.AllBros;
    public bool defaultChooseObjectOnLineSkip = true;
    public BathroomObjectType defaultLineSkipBathroomObject = BathroomObjectType.None;
    public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnLineQueueSkip = new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };
    //-------------------------------------------------------------------------
    public BroDistribution startRoamingOnArrivalAtBathroomObjectInUse = BroDistribution.AllBros;
    public bool defaultStartRoamingOnArrivalAtBathroomObjectInUse = true;
    //-------------------------------------------------------------------------
    public BroDistribution chooseObjectOnRelief = BroDistribution.AllBros;
    public bool defaultChooseObjectOnRelief = true;
    public BathroomObjectType defaultOnReliefBathroomObject = BathroomObjectType.None;
    public BathroomObjectType[] defaultBathroomObjectsToChooseFromOnRelief = new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal };
    //-------------------------------------------------------------------------
    
    public BroConfigurer() {
        // Configure default values where needed
        defaultReliefRequired = Factory.Instance.SelectRandomReliefType(defaultReliefRequiredToChooseFrom);
        defaultLineSkipBathroomObject = Factory.Instance.SelectRandomBathroomObjectType(defaultBathroomObjectsToChooseFromOnLineQueueSkip);
        defaultOnReliefBathroomObject = Factory.Instance.SelectRandomBathroomObjectType(defaultBathroomObjectsToChooseFromOnRelief);
        InitializeOccupationDuration();
    }
    
    // all occupation duration defaults to 2 seconds, except exits
    public void InitializeOccupationDuration() {
        bathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, BroDistribution>();
        defaultBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();
        defaultMinBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();
        defaultMaxBathroomObjectOccupationDuration = new Dictionary<BathroomObjectType, float>();
        
        
        BathroomObjectType[] bathroomObjectTypes = (BathroomObjectType[])BathroomObjectType.GetValues(typeof(BathroomObjectType));
        foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypes) {
            float minDuration = 2f;
            float maxDuration = 2f;
            switch(bathroomObjectType) {
            case(BathroomObjectType.Exit):
                minDuration = 0f;
                maxDuration = 0f;
                break;
            case(BathroomObjectType.HandDryer):
                break;
            case(BathroomObjectType.Sink):
                break;
            case(BathroomObjectType.Stall):
                break;
            case(BathroomObjectType.Urinal):
                break;
            default:
                break;
            }
            if(bathroomObjectType != BathroomObjectType.None) {
                InitializeBathroomObjectTypeOccupationDuration(BroDistribution.AllBros, bathroomObjectType, minDuration, maxDuration);
            }
        }
    }
    
    public void InitializeBathroomObjectTypeOccupationDuration(BroDistribution newBathroomObjectionOccupationDuration, BathroomObjectType bathroomObjectType, float newDefaultBathroomObjectTypeMinOccupationDuration, float newDefaultBathroomObjectTypeMaxOccupationDuration) {
        bathroomObjectOccupationDuration[bathroomObjectType] = newBathroomObjectionOccupationDuration;
        defaultBathroomObjectOccupationDuration[bathroomObjectType]  = float.PositiveInfinity;
        defaultMinBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultBathroomObjectTypeMinOccupationDuration;
        defaultMaxBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultBathroomObjectTypeMaxOccupationDuration;
    }
    
    public void ResetVariablesToInitialValues() {
        InitializeOccupationDuration();
        
        // Debug.Log("RESETTING BRO GENERATOR TO DEFAULT VALUES");
    }
    
    public BroConfigurer SetReliefType(BroDistribution typeOfBroDistribution, params ReliefRequired[] newReliefRequiredToChooseFrom) {
        reliefType = typeOfBroDistribution;
        defaultReliefRequiredToChooseFrom = newReliefRequiredToChooseFrom;
        return this;
    }
    public BroConfigurer ConfigureReliefType(Bro broRef) {
        // Factory.Instance.SelectRandomReliefType(ReliefRequired.Pee, ReliefRequired.Poop)
        switch(reliefType) {
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
        
        return this;
    }
    
    public BroConfigurer SetXMoveSpeed(BroDistribution typeOfBroDistribution, float newDefaultMinXMoveSpeed, float newDefaultMaxXMoveSpeed) {
        xMoveSpeed = typeOfBroDistribution;
        defaultMinXMoveSpeed = newDefaultMinXMoveSpeed;
        defaultMaxXMoveSpeed = newDefaultMaxXMoveSpeed;
        return this;
    }
    public BroConfigurer ConfigureXMoveSpeed(Bro broRef) {
        switch(reliefType) {
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
        
        return this;
    }
    
    public BroConfigurer SetYMoveSpeed(BroDistribution typeOfBroDistribution, float newDefaultMinYMoveSpeed, float newDefaultMaxYMoveSpeed) {
        yMoveSpeed = typeOfBroDistribution;
        defaultMinYMoveSpeed = newDefaultMinYMoveSpeed;
        defaultMaxYMoveSpeed = newDefaultMaxYMoveSpeed;
        return this;
    }
    public BroConfigurer ConfigureYMoveSpeed(Bro broRef) {
        switch(reliefType) {
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
        
        return this;
    }
//---------------------
    public BroConfigurer SetFightProbability(BroDistribution newBroDistributionFightCheckType, float newDefaultMinBroFightProbability, float newDefaultMaxBroFightProbability) {
        fightProbability = newBroDistributionFightCheckType;
        defaultMinBroFightProbability = newDefaultMinBroFightProbability;
        defaultMaxBroFightProbability = newDefaultMaxBroFightProbability;
        return this;
    }
    public BroConfigurer ConfigureFightCheckType(Bro broRef) {
        switch(fightProbability) {
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
        
        return this;
    }
    //---------------------
    public BroConfigurer SetModifyFightProbabilityUsingScoreRatio(BroDistribution newBroDistributionModifyBroFightProbabilityUsingScoreRatio, bool newDefaultModifyBroFightProbablityUsingScoreRatio) {
        modifyBroFightProbabilityUsingScoreRatio = newBroDistributionModifyBroFightProbabilityUsingScoreRatio;
        defaultModifyBroFightProbablityUsingScoreRatio = newDefaultModifyBroFightProbablityUsingScoreRatio;
        return this;
    }
    public BroConfigurer ConfigureModifyFightProbabilityUsingScoreRatio(Bro broRef) {
        switch(modifyBroFightProbabilityUsingScoreRatio) {
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
        
        return this;
    }
    //---------------------
    public BroConfigurer SetBathroomObjectOccupationDuration(BroDistribution newBathroomObjectionOccupationDuration, BathroomObjectType bathroomObjectType, float newDefaultMinBathroomObjectTypeOccupationDuration, float newMaxDefaultBathroomObjectTypeOccupationDuration) {
        bathroomObjectOccupationDuration[bathroomObjectType] = newBathroomObjectionOccupationDuration;
        defaultMinBathroomObjectOccupationDuration[bathroomObjectType] = newDefaultMinBathroomObjectTypeOccupationDuration;
        defaultMaxBathroomObjectOccupationDuration[bathroomObjectType] = newMaxDefaultBathroomObjectTypeOccupationDuration;
        return this;
    }
    public BroConfigurer ConfigureBathroomObjectOccupationDuration(Bro broRef, BathroomObjectType bathroomObjectType) {
        // Debug.Log("-----------------------");
        // Debug.Log(bathroomObjectOccupationDuration[bathroomObjectType].ToString());
        // Debug.Log(bathroomObjectType.ToString());
        // Debug.Log(broRef.occupationDuration);
        // Debug.Log(broRef.occupationDuration.ContainsKey(bathroomObjectType));
        
        switch(bathroomObjectOccupationDuration[bathroomObjectType]) {
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
        
        return this;
    }
    //---------------------
    public BroConfigurer SetLineQueueSkipType(BroDistribution newBroDistributionLineQueueSkipType, bool newBroLineQueueSkip) {
        lineQueueSkipType = newBroDistributionLineQueueSkipType;
        defaultLineQueueSkip = newBroLineQueueSkip;
        return this;
    }
    public BroConfigurer ConfigureLineQueueSkipType(Bro broRef) {
        switch(lineQueueSkipType) {
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
        
        return this;
    }
    //---------------------
    public BroConfigurer SetChooseObjectOnLineSkip(BroDistribution newBroDistributionChooseObjectOnLineSkip, bool newBroChooseObjectOnLineSkip) {
        chooseObjectOnLineSkip = newBroDistributionChooseObjectOnLineSkip;
        defaultChooseObjectOnLineSkip = newBroChooseObjectOnLineSkip;
        //TO DO SET TYPES OF OBJECTS THAT CAN BE CHOSEN FROM
        return this;
    }
    public BroConfigurer ConfigureChooseObjectOnLineSkip(Bro broRef) {
        switch(chooseObjectOnLineSkip) {
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
        
        return this;
    }
    //---------------------
    public BroConfigurer SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse, bool newBroStartRoamingOnArrivalAtBathroomObjectInUse) {
        startRoamingOnArrivalAtBathroomObjectInUse = newBroDistributionStartRoamingOnArrivalAtBathroomObjectInUse;
        defaultStartRoamingOnArrivalAtBathroomObjectInUse = newBroStartRoamingOnArrivalAtBathroomObjectInUse;
        return this;
    }
    public BroConfigurer ConfigureStartRoamingOnArrivalAtBathroomObjectInUse(Bro broRef) {
        switch(startRoamingOnArrivalAtBathroomObjectInUse) {
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
        
        return this;
    }
//---------------------
    public BroConfigurer SetChooseObjectOnRelief(BroDistribution newBroDistributionChooseObjectOnRelief, bool newBroChooseObjectOnRelief) {
        chooseObjectOnRelief = newBroDistributionChooseObjectOnRelief;
        defaultChooseObjectOnRelief = newBroChooseObjectOnRelief;
        //TO DO SET TYPES OF OBJECTS THAT CAN BE CHOSEN FROM
        return this;
    }
    public BroConfigurer ConfigureChooseObjectOnRelief(Bro broRef) {
        switch(chooseObjectOnRelief) {
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
        
        return this;
    }
}
