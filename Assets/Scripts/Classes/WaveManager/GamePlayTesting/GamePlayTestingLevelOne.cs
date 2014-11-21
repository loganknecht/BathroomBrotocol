using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayTestingLevelOne : WaveLogic, WaveLogicContract {
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

        BroDistributionObject firstWave = new BroDistributionObject(0, 60, 10, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
        firstWave.SetFightProbability(BroDistribution.AllBros, 0.5f);
        firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
        firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, true);
        firstWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
        firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, true);

        // BroDistributionObject secondWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        // secondWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
        // secondWave.SetFightProbability(BroDistribution.AllBros, 1f);
        // secondWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
        // secondWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
        // secondWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
        // secondWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

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