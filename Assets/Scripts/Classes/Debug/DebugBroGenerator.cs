// using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class DebugBroGenerator : BaseBehavior {
public class DebugBroGenerator : MonoBehaviour {
    public int debugEntranceQueue = 0;
    
    public Vector2 debugXMoveSpeed;
    public Vector2 debugYMoveSpeed;
    
    public Dictionary<BathroomObjectType, float> debugOccupationDuration = new Dictionary<BathroomObjectType, float>();
    public BroType debugBroType;
    public ReliefRequired[] debugReliefRequired = new ReliefRequired[] {};
    
    // Bro
    public Vector2 debugFightProbability;
    public bool debugModifyBroFightProbablityUsingScoreRatio = false;
    public bool debugSkipLineQueue = false;
    public bool debugChooseObjectOnLineSkip = false;
    public bool debugStartRoamingOnArrivalAtBathroomObjectInUse = false;
    public bool debugChooseObjectOnRelief = false;
    public bool debugHasRelievedSelf = false;
    public bool debugHasWashedHands = false;
    public bool debugHasDriedHands = false;
    
    // Fart Generator (TileBlocker Properties)
    public Vector2 debugFartGeneratorProbability;
    public Vector2 debugFartGeneratorGenerationFrequency;
    public bool debugFartGeneratorGenerationFrequencyIsStochastic = false;
    public Vector2 debugFartGeneratorMinGenerationFrequency;
    public Vector2 debugFartGeneratorMaxGenerationFrequency;
    // Fart Generator (Fart Properties)
    public Vector2 debugFartDuration;
    public bool debugFartDurationIsStochastic = false;
    public Vector2 debugFartMinDuration;
    public Vector2 debugFartMaxDuration;
    
    // Use this for initialization
    public void Awake() {
        // base.Awake();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        PerformDebugButtonPressLogic();
    }
    
    public void PerformDebugButtonPressLogic() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("Generating bro!");
            
            // GameObject broGameObject = Factory.Instance.GenerateBroGameObject(debugBroType);
            // int lastLineQueueTileIndex = lineQueues[debugEntranceQueue].GetComponent<LineQueue>().queueTileObjects.Count - 1;
            // broGameObject.transform.position = new Vector3(lineQueues[debugEntranceQueue].GetComponent<LineQueue>().queueTileObjects[lastLineQueueTileIndex].transform.position.x,
            //                                                lineQueues[debugEntranceQueue].GetComponent<LineQueue>().queueTileObjects[lastLineQueueTileIndex].transform.position.y,
            //                                                broGameObject.transform.position.z);
            // Bro broReference = broGameObject.GetComponent<Bro>();
            // broReference.reliefRequired = debugReliefRequired;
            // broReference.baseProbabilityOfFightOnCollisionWithBro = debugFightProbability;
            // broReference.modifyBroFightProbablityUsingScoreRatio = debugModifyBroFightProbablityUsingScoreRatio;
            // broReference.occupationDuration = debugOccupationDuration;
            // broReference.skipLineQueue = debugSkipLineQueue;
            // broReference.chooseRandomBathroomObjectOnSkipLineQueue = debugChooseObjectOnLineSkip;
            // broReference.startRoamingOnArrivalAtBathroomObjectInUse = debugStartRoamingOnArrivalAtBathroomObjectInUse;
            // broReference.chooseRandomBathroomObjectAfterRelieved = debugChooseObjectOnRelief;
            // broReference.hasRelievedSelf = debugHasRelievedSelf;
            // broReference.hasWashedHands = debugHasWashedHands;
            // broReference.hasDriedHands = debugHasDriedHands;
            // AddBroToEntranceQueue(broGameObject, debugEntranceQueue);
            //-------------------------------------------------------------------------------------------------------------------
            // Alt generation
            //-------------------------------------------------------------------------------------------------------------------
            // Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { debugBroType, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
            
            BroDistributionObject debugWave = new BroDistributionObject(0, 5, 1, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
            debugWave.broConfigurer.SetReliefType(BroDistribution.AllBros, debugReliefRequired)
            .SetXMoveSpeed(BroDistribution.AllBros, debugXMoveSpeed.x, debugXMoveSpeed.y)
            .SetYMoveSpeed(BroDistribution.AllBros , debugYMoveSpeed.x, debugYMoveSpeed.y)
            .SetFightProbability(BroDistribution.AllBros, debugFightProbability.x, debugFightProbability.y)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, debugModifyBroFightProbablityUsingScoreRatio)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, debugOccupationDuration[BathroomObjectType.Exit], debugOccupationDuration[BathroomObjectType.Exit])
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, debugOccupationDuration[BathroomObjectType.HandDryer], debugOccupationDuration[BathroomObjectType.HandDryer])
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, debugOccupationDuration[BathroomObjectType.Sink], debugOccupationDuration[BathroomObjectType.Sink])
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, debugOccupationDuration[BathroomObjectType.Stall], debugOccupationDuration[BathroomObjectType.Stall])
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, debugOccupationDuration[BathroomObjectType.Urinal], debugOccupationDuration[BathroomObjectType.Urinal])
            .SetLineQueueSkipType(BroDistribution.AllBros, debugSkipLineQueue)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, debugChooseObjectOnLineSkip)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, debugStartRoamingOnArrivalAtBathroomObjectInUse)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, debugChooseObjectOnRelief);
            // Fart Generator if the bro has it (TileBlocker Properties)
            debugWave.fartGeneratorConfigurer.SetProbability(BroDistribution.AllBros, debugFartGeneratorProbability.x, debugFartGeneratorProbability.y)
            .SetGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorGenerationFrequency.x, debugFartGeneratorGenerationFrequency.y)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, debugFartGeneratorGenerationFrequencyIsStochastic)
            .SetMinGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorMinGenerationFrequency.x, debugFartGeneratorMinGenerationFrequency.y)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorMaxGenerationFrequency.x, debugFartGeneratorMaxGenerationFrequency.y);
            // Fart Properties
            debugWave.fartGeneratorConfigurer.SetDuration(BroDistribution.AllBros, debugFartDuration.x, debugFartDuration.y)
            .SetDurationIsStochastic(BroDistribution.AllBros, debugFartDurationIsStochastic)
            .SetMinDuration(BroDistribution.AllBros, debugFartMinDuration.x, debugFartMinDuration.y)
            .SetMaxDuration(BroDistribution.AllBros, debugFartMaxDuration.x, debugFartMaxDuration.y);
            // Vomit Generator if the bro has it (TileBlocker Properties)
            debugWave.vomitGeneratorConfigurer.SetProbability(BroDistribution.AllBros, 1, 1)
            .SetGenerationFrequency(BroDistribution.AllBros, 2, 2)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
            .SetMinGenerationFrequency(BroDistribution.AllBros, 2, 2)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, 2, 2);
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                debugWave,
            });
        }
    }
}
