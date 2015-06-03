// Example Usage
// Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
// Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

// BroDistributionObject firstWave = new BroDistributionObject(0, 5, 1, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
// firstWave.broConfigurer
// .SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
// .SetXMoveSpeed(BroDistribution.AllBros, 1.5f, 1.5f)
// .SetYMoveSpeed(BroDistribution.AllBros , 1.5f, 1.5f)
// .SetFightProbability(BroDistribution.AllBros, 0.15f, 0.15f)
// .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, false)
// .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
// .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
// .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
// .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
// .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
// .SetLineQueueSkipType(BroDistribution.AllBros, true)
// .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
// .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
// .SetChooseObjectOnRelief(BroDistribution.AllBros, true);
// // Fart Generator if the bro has it (TileBlocker Properties)
// firstWave.fartGeneratorConfigurer
// .SetProbability(BroDistribution.AllBros, 2f, 2f)
// .SetDuration(BroDistribution.AllBros, 2f, 2f)
// .SetDurationIsStochastic(BroDistribution.AllBros, false)
// .SetMinDuration(BroDistribution.AllBros, 2f, 2f)
// .SetMaxDuration(BroDistribution.AllBros, 2f, 2f)
// .SetGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
// .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
// .SetMinGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
// .SetMaxGenerationFrequency(BroDistribution.AllBros, 2f, 2f);
// // Vomit Generator if the bro has it (TileBlocker Properties)
// firstWave.vomitGeneratorConfigurer
// .SetProbability(BroDistribution.AllBros, 1f, 1f)
// .SetGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
// .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
// .SetMinGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
// .SetMaxGenerationFrequency(BroDistribution.AllBros, 2f, 2f);

// BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
//     firstWave
// });
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BroDistributionObject : DistributionObject {
    public Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>();
    public Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>();
    
    public BroConfigurer broConfigurer = null;
    // public BroDistributionBathroomTileBlockerGenerator fartGenerator = null;
    public FartGeneratorConfigurer fartGeneratorConfigurer = null;
    public VomitGeneratorConfigurer vomitGeneratorConfigurer = null;
    
    public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, DistributionSpacing newDistributionSpacing, Dictionary<BroType, float> newBroProbabilities, Dictionary<int, float> newEntranceQueueProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType, newDistributionSpacing) {
        broConfigurer = new BroConfigurer();
        fartGeneratorConfigurer = new FartGeneratorConfigurer();
        vomitGeneratorConfigurer = new VomitGeneratorConfigurer();
        
        if(newBroProbabilities == null) {
            ConfigureUniformBroTypeDistribution();
        }
        else {
            broProbabilities = newBroProbabilities;
        }
        
        if(newEntranceQueueProbabilities == null) {
            // Guarantees that the first entrance will always be used if no entrance queue probabilities are used.
            entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
        }
        else {
            entranceQueueProbabilities = newEntranceQueueProbabilities;
        }
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
                selectedPointOverInterval = ((1f / numberOfPointsToGenerate) * i);
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
            
            broToGenerate.transform.parent = BroManager.Instance.transform;
            broToGenerate.SetActive(false);
            ConfigureBro(broToGenerate);
            ConfigureFartGenerator(broToGenerate);
            ConfigureVomitGenerator(broToGenerate);
            
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
    
    public GameObject ConfigureBro(GameObject broToGenerate) {
        Bro broReference = broToGenerate.GetComponent<Bro>();
        if(broReference != null) {
            broConfigurer.ConfigureReliefType(broReference)
            .ConfigureXMoveSpeed(broReference)
            .ConfigureYMoveSpeed(broReference)
            .ConfigureFightCheckType(broReference)
            .ConfigureModifyFightProbabilityUsingScoreRatio(broReference)
            .ConfigureBathroomObjectOccupationDuration(broReference, BathroomObjectType.Exit)
            .ConfigureBathroomObjectOccupationDuration(broReference, BathroomObjectType.HandDryer)
            .ConfigureBathroomObjectOccupationDuration(broReference, BathroomObjectType.Sink)
            .ConfigureBathroomObjectOccupationDuration(broReference, BathroomObjectType.Stall)
            .ConfigureBathroomObjectOccupationDuration(broReference, BathroomObjectType.Urinal)
            .ConfigureLineQueueSkipType(broReference)
            .ConfigureChooseObjectOnLineSkip(broReference)
            .ConfigureStartRoamingOnArrivalAtBathroomObjectInUse(broReference)
            .ConfigureChooseObjectOnRelief(broReference);
        }
        
        return broToGenerate;
    }
    
    public GameObject ConfigureFartGenerator(GameObject broToGenerate) {
        FartGenerator fartGeneratorReference = broToGenerate.GetComponent<FartGenerator>();
        
        if(fartGeneratorReference != null) {
            // Bathroom Tile Blocker Properties
            fartGeneratorConfigurer.ConfigureProbability(fartGeneratorReference)
            .ConfigureGenerationFrequency(fartGeneratorReference)
            .ConfigureGenerationFrequencyIsStochastic(fartGeneratorReference)
            .ConfigureMinGenerationFrequency(fartGeneratorReference)
            .ConfigureMaxGenerationFrequency(fartGeneratorReference)
            .ConfigureAmountToGenerate(fartGeneratorReference);
            // Fart Generator Properties
            fartGeneratorConfigurer.ConfigureDuration(fartGeneratorReference)
            .ConfigureDurationIsStochastic(fartGeneratorReference)
            .ConfigureMinDuration(fartGeneratorReference)
            .ConfigureMaxDuration(fartGeneratorReference);
        }
        return broToGenerate;
    }
    public GameObject ConfigureVomitGenerator(GameObject broToGenerate) {
        VomitGenerator vomitGeneratorReference = broToGenerate.GetComponent<VomitGenerator>();
        
        if(vomitGeneratorReference != null) {
            // Bathroom Tile Blocker Properties
            vomitGeneratorConfigurer.ConfigureProbability(vomitGeneratorReference)
            .ConfigureGenerationFrequency(vomitGeneratorReference)
            .ConfigureGenerationFrequencyIsStochastic(vomitGeneratorReference)
            .ConfigureMinGenerationFrequency(vomitGeneratorReference)
            .ConfigureMaxGenerationFrequency(vomitGeneratorReference)
            .ConfigureAmountToGenerate(vomitGeneratorReference);
            // Vomit Generator Properties
        }
        return broToGenerate;
    }
    
    public void ConfigureUniformBroTypeDistribution() {
        // float uniformDistributionStepSizeForAllBros = 0f;
        BroType[] broTypes = (BroType[])BroType.GetValues(typeof(BroType));
        float uniformBroTypeDistribution = 0;
        if(broTypes.Length > 0) {
            uniformBroTypeDistribution = 1 / broTypes.Length;
        }
        
        // broProbabilities[BroType.None] = uniformBroTypeDistribution;
        foreach(BroType broType in broTypes) {
            broProbabilities[broType] = uniformBroTypeDistribution;
        }
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
}
