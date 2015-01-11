using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeLevelOne : WaveLogic, WaveLogicContract {

    GameObject firstWaveGameObject;
    GameObject secondWaveGameObject;

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {
        base.Start();

        firstWaveGameObject = CreateWaveState("FirstWave Game Object",
                                              TriggerFirstWave,
                                              PerformFirstWave,
                                              FinishFirstWave);

        InitializeWaveStates(
                             firstWaveGameObject
                             );
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    //----------------------------------------------------------------------------
    public void TriggerFirstWave() {
        TextboxManager.Instance.Hide();

        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

        BroDistributionObject firstWave = new BroDistributionObject(0, 5, 3, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        firstWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetXMoveSpeed(BroDistribution.AllBros, 1.3f, 1.3f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.3f, 1.3f)
            .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, true)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroDistributionObject secondWave = new BroDistributionObject(15, 30, 2, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        secondWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetXMoveSpeed(BroDistribution.AllBros, 1.3f, 1.3f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.3f, 1.3f)
            .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, true)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroDistributionObject thirdWave = new BroDistributionObject(30, 35, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        thirdWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetXMoveSpeed(BroDistribution.AllBros, 1.3f, 1.3f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.3f, 1.3f)
            .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, true)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroDistributionObject fourthWave = new BroDistributionObject(50, 65, 10, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        fourthWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetXMoveSpeed(BroDistribution.AllBros, 1.3f, 1.3f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.3f, 1.3f)
            .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, true)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                                 firstWave,
                                                                                 // secondWave,
                                                                                 // thirdWave,
                                                                                 // fourthWave
                                                                                });
    }

    public void PerformFirstWave() {
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            TriggerWaveFinish();
        }
    }
    public void FinishFirstWave() {
        waveLogicFinished = true;
    }
  //----------------------------------------------------------------------------
} 