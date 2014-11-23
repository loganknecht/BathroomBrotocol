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
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                           { 1, .5f } };

        BroDistributionObject firstWave = new BroDistributionObject(0, 10, 3, DistributionType.LinearIn, DistributionSpacing.Random, broProbabilities, entranceQueueProbabilities);
        firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetFightProbability(BroDistribution.AllBros, 0.5f, 0.5f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, true)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, true);

        BroDistributionObject secondWave = new BroDistributionObject(0, 5, 0, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        secondWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee)
            .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                                 firstWave,
                                                                                 // secondWave
                                                                                });
    }

    public void PerformFirstWave() {
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            PerformWaveStatePlayingFinishedTrigger();
            waveLogicFinished = true;
        }
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            PerformWaveStatePlayingFinishedTrigger();
        }
    }
    public void FinishFirstWave() {
    }
    public void FinishSecondWave() {
    }
  //----------------------------------------------------------------------------
} 