using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeLevelTwo : WaveLogic, WaveLogicContract {

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {
        base.Start();

        GameObject firstWaveGameObject = CreateWaveState("FirstWave Game Object",
                                              TriggerFirstWave,
                                              PerformFirstWave,
                                              FinishFirstWave);

        GameObject finalWaveGameObject = CreateWaveState("FinalWave Game Object",
                                              TriggerFinalWave,
                                              PerformFinalWave,
                                              FinishFinalWave);

        InitializeWaveStates(
                             firstWaveGameObject,
                             finalWaveGameObject
                             );
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public override void PerformLevelFailCheck() {
        base.PerformLevelFailCheck();
        
        if(BroManager.Instance.allFightingBros.Count > 0) {
            LevelManager.Instance.TriggerFailedLevel();
        }
    }

    //----------------------------------------------------------------------------
    public void TriggerFirstWave() {
        BroGenerator.Instance.Pause();

        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

        TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.localPosition.x, -600, TextboxManager.Instance.gameObject.transform.localPosition.x, -300, 1, 2, UITweener.Method.BounceIn, null);

        Queue textQueue = new Queue();
        textQueue.Enqueue("Don't let any object be broken!!!");
        TextboxManager.Instance.SetTextboxTextSet(textQueue);
        //----------------------------------------------------------------------------

        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { 
                                                                                        { BroType.GenericBro, 0.5f },
                                                                                        { BroType.SlobBro, 0.5f },
                                                                                       };
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { 
                                                                                            { 0, 0.5f },
                                                                                            { 1, 0.5f },
                                                                                         };

        BroDistributionObject firstWave = new BroDistributionObject(0, 5, 3, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        firstWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
            .SetXMoveSpeed(BroDistribution.AllBros, 1.3f, 1.3f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.3f, 1.3f)
            .SetFightProbability(BroDistribution.AllBros, .1f, .2f)
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
            .SetFightProbability(BroDistribution.AllBros, .1f, .2f)
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
            .SetFightProbability(BroDistribution.AllBros, .1f, .2f)
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
            .SetFightProbability(BroDistribution.AllBros, .1f, .2f)
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
                                                                                 secondWave,
                                                                                 thirdWave,
                                                                                 fourthWave
                                                                                });
    }

    public void PerformFirstWave() {
        if(TextboxManager.Instance.HasFinished()) {
            TriggerWaveFinish();
        }
    }
    public void FinishFirstWave() {
        BroGenerator.Instance.Unpause();
        LevelManager.Instance.HideJanitorOverlay();
        LevelManager.Instance.ShowPauseButton();
        TextboxManager.Instance.Hide();
    }
  //----------------------------------------------------------------------------
    public void TriggerFinalWave() {
    }
    public void PerformFinalWave() {
        TriggerWaveFinish();
    }
    public void FinishFinalWave() {
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            waveLogicFinished = true;
        }
    }
} 