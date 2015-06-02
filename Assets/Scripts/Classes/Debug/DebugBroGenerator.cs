using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugBroGenerator : BaseBehavior {
    // public class DebugBroGenerator : MonoBehaviour {
    public int debugEntranceQueue = 0;
    
    public Vector2 debugXMoveSpeed = new Vector2(3f, 3f);
    public Vector2 debugYMoveSpeed = new Vector2(3f, 3f);
    
    public Dictionary<BathroomObjectType, float> debugOccupationDuration;
    public BroType debugBroType;
    public List<ReliefRequired> debugReliefRequired;
    
    // Bro
    public Vector2 debugFightProbability = new Vector2(0.5f, 0.5f);
    public bool debugModifyBroFightProbablityUsingScoreRatio = false;
    public bool debugSkipLineQueue = false;
    public bool debugChooseObjectOnLineSkip = false;
    public bool debugStartRoamingOnArrivalAtBathroomObjectInUse = false;
    public bool debugChooseObjectOnRelief = false;
    public bool debugHasRelievedSelf = false;
    public bool debugHasWashedHands = false;
    public bool debugHasDriedHands = false;
    
    // Vomit Generator
    public Vector2 debugVomitGeneratorProbability = new Vector2(1f, 1f);
    public Vector2 debugVomitGeneratorGenerationFrequency = new Vector2(3, 3);
    public bool debugVomitGeneratorGenerationFrequencyIsStochastic = false;
    public Vector2 debugVomitGeneratorMinGenerationFrequency = new Vector2(3, 3);
    public Vector2 debugVomitGeneratorMaxGenerationFrequency = new Vector2(3, 3);
    public Vector2 debugVomitGeneratorAmountToGenerate = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    // Fart Generator (Fart Properties)
    public Vector2 debugFartGeneratorProbability = new Vector2(1f, 1f);
    public Vector2 debugFartGeneratorGenerationFrequency = new Vector2(3f, 3f);
    public bool debugFartGeneratorGenerationFrequencyIsStochastic = false;
    public Vector2 debugFartGeneratorMinGenerationFrequency = new Vector2(3f, 3f);
    public Vector2 debugFartGeneratorMaxGenerationFrequency = new Vector2(3f, 3f);
    public Vector2 debugFartGeneratorAmountToGenerate = new Vector2(1f, 1f);
    //--------
    public Vector2 debugFartDuration = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    public bool debugFartDurationIsStochastic = false;
    public Vector2 debugFartMinDuration = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    public Vector2 debugFartMaxDuration = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
    
    // Use this for initialization
    protected override void Awake() {
        base.Awake();
        InitializeReliefRequiredDefaults();
        InitializeBathroomObjectDurationDefaults();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
        PerformDebugButtonPressLogic();
    }
    
    public void InitializeReliefRequiredDefaults() {
        if(debugReliefRequired == null) {
            debugReliefRequired = new List<ReliefRequired>();
            ReliefRequired[] allReliefRequired = (ReliefRequired[])ReliefRequired.GetValues(typeof(ReliefRequired));
            foreach(ReliefRequired reliefRequired in allReliefRequired) {
                if(reliefRequired != ReliefRequired.None) {
                    debugReliefRequired.Add(reliefRequired);
                }
            }
        }
    }
    
    public void InitializeBathroomObjectDurationDefaults() {
        if(debugOccupationDuration.Count == 0) {
            debugOccupationDuration = new Dictionary<BathroomObjectType, float>();
            
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
                    // Do nothing
                    break;
                case(BathroomObjectType.Sink):
                    // Do nothing
                    break;
                case(BathroomObjectType.Stall):
                    // Do nothing
                    break;
                case(BathroomObjectType.Urinal):
                    // Do nothing
                    break;
                default:
                    break;
                }
                
                if(bathroomObjectType != BathroomObjectType.None) {
                    debugOccupationDuration[bathroomObjectType] = Random.Range(minDuration, maxDuration);
                }
            }
        }
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
            debugWave.broConfigurer
            .SetReliefType(BroDistribution.AllBros, debugReliefRequired.ToArray())
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
            debugWave.fartGeneratorConfigurer
            .SetProbability(BroDistribution.AllBros, debugFartGeneratorProbability.x, debugFartGeneratorProbability.y)
            .SetGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorGenerationFrequency.x, debugFartGeneratorGenerationFrequency.y)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, debugFartGeneratorGenerationFrequencyIsStochastic)
            .SetMinGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorMinGenerationFrequency.x, debugFartGeneratorMinGenerationFrequency.y)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, debugFartGeneratorMaxGenerationFrequency.x, debugFartGeneratorMaxGenerationFrequency.y)
            .SetDuration(BroDistribution.AllBros, debugFartDuration.x, debugFartDuration.y)
            .SetDurationIsStochastic(BroDistribution.AllBros, debugFartDurationIsStochastic)
            .SetMinDuration(BroDistribution.AllBros, debugFartMinDuration.x, debugFartMinDuration.y)
            .SetMaxDuration(BroDistribution.AllBros, debugFartMaxDuration.x, debugFartMaxDuration.y);
            // Vomit Generator if the bro has it (TileBlocker Properties)
            debugWave.vomitGeneratorConfigurer
            .SetProbability(BroDistribution.AllBros, debugVomitGeneratorProbability.x, debugVomitGeneratorProbability.y)
            .SetGenerationFrequency(BroDistribution.AllBros, debugVomitGeneratorGenerationFrequency.x, debugVomitGeneratorGenerationFrequency.y)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, debugVomitGeneratorGenerationFrequencyIsStochastic)
            .SetMinGenerationFrequency(BroDistribution.AllBros, debugVomitGeneratorMinGenerationFrequency.x, debugVomitGeneratorMinGenerationFrequency.y)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, debugVomitGeneratorMaxGenerationFrequency.x, debugVomitGeneratorMaxGenerationFrequency.y)
            .SetAmountToGenerate(BroDistribution.AllBros, debugVomitGeneratorAmountToGenerate.x, debugVomitGeneratorAmountToGenerate.y);
            // debugVomitGeneratorAmountToGenerate
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                debugWave,
            });
        }
    }
}
