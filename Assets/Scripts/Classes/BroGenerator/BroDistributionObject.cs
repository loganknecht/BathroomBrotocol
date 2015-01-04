using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BroDistributionObject : DistributionObject {
    public Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>();
    public Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>();

    public BroDistributionBro broGenerator = null;
    // public BroDistributionBathroomTileBlockerGenerator fartGenerator = null;
    public BroDistributionFartGenerator fartGenerator = null;

    public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, DistributionSpacing newDistributionSpacing, Dictionary<BroType, float> newBroProbabilities, Dictionary<int, float> newEntranceQueueProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType, newDistributionSpacing) {
        broGenerator = new BroDistributionBro();
        fartGenerator = new BroDistributionFartGenerator();

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
            // Shouldn't be used, remove by segmenting generation to be respective of each bro
            // Bro broRefToGenerate = broToGenerate.GetComponent<Bro>();

            broToGenerate.transform.parent = BroManager.Instance.transform;
            broToGenerate.SetActive(false);
            ConfigureBro(broToGenerate);
            ConfigureFartGenerator(broToGenerate);

            // Replace with drunk bro generator
            // if(broRefToGenerate.type == BroType.DrunkBro) {
            //     broRefToGenerate.speechBubbleReference.displaySpeechBubble = false;
            // }

            // Pretty sure this isn't needed anymore, bros should be rotated to match the camera on entrance in the restroom
            // CameraManager.Instance.rotateCameraReference.RotateBroGameObject(broToGenerate);

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
        broGenerator.ConfigureBroToGenerateReliefType(broToGenerate);
        broGenerator.ConfigureBroXMoveSpeed(broToGenerate);
        broGenerator.ConfigureBroYMoveSpeed(broToGenerate);
        broGenerator.ConfigureBroToGenerateFightCheckType(broToGenerate);
        broGenerator.ConfigureBroToGenerateModifyFightProbabilityUsingScoreRatio(broToGenerate);
        broGenerator.ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Exit);
        broGenerator.ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.HandDryer);
        broGenerator.ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Sink);
        broGenerator.ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Stall);
        broGenerator.ConfigureBroToGenerateBathroomObjectOccupationDuration(broToGenerate, BathroomObjectType.Urinal);
        broGenerator.ConfigureBroToGenerateLineQueueSkipType(broToGenerate);
        broGenerator.ConfigureBroToGenerateChooseObjectOnLineSkip(broToGenerate);
        broGenerator.ConfigureBroToGenerateStartRoamingOnArrivalAtBathroomObjectInUse(broToGenerate);
        broGenerator.ConfigureBroToGenerateChooseObjectOnRelief(broToGenerate);

        return broToGenerate;
    }

    public GameObject ConfigureFartGenerator(GameObject broToGenerate) {
        FartGenerator fartGeneratorReference = broToGenerate.GetComponent<FartGenerator>();

        if(fartGeneratorReference != null) {
            // Bathroom Tile Blocker Properties
            fartGenerator.ConfigureProbability(fartGeneratorReference);
            fartGenerator.ConfigureGenerationFrequency(fartGeneratorReference);
            fartGenerator.ConfigureGenerationFrequencyIsStochastic(fartGeneratorReference);
            fartGenerator.ConfigureMinGenerationFrequency(fartGeneratorReference);
            fartGenerator.ConfigureMaxGenerationFrequency(fartGeneratorReference);
            // Fart Generator Properties
            fartGenerator.ConfigureDuration(fartGeneratorReference);
            fartGenerator.ConfigureDurationIsStochastic(fartGeneratorReference);
            fartGenerator.ConfigureMinDuration(fartGeneratorReference);
            fartGenerator.ConfigureMaxDuration(fartGeneratorReference);
        }
        return broToGenerate;
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
