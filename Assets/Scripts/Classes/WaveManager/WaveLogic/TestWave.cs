using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestWave : WaveLogic, WaveLogicContract {
    GameObject startAnimationWaveGameObject;
    GameObject firstWaveGameObject;

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {
    base.Start();
        startAnimationWaveGameObject = CreateWaveState("Start Animation Game Object",
                                                                    TriggerStartAnimation,
                                                                    PerformStartAnimation,
                                                                    FinishStartAnimation);

        firstWaveGameObject = CreateWaveState("FirstWave Game Object",
                                                          TriggerFirstWave,
                                                          PerformFirstWave,
                                                          FinishFirstWave);

        InitializeWaveStates(
                             startAnimationWaveGameObject,
                             firstWaveGameObject
                             );
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }
    //----------------------------------------------------------------------------

    public void TriggerStartAnimation() {
        // Debug.Log("triggering start animation");

        // SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

        // FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

        // TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -445, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -145, 1, 2, UITweener.Method.BounceIn, null);

        LevelManager.Instance.HidePauseButton();

        // TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.localPosition.x, -600, TextboxManager.Instance.gameObject.transform.localPosition.x, -300, 1, 2, UITweener.Method.BounceIn, null);

        TextboxManager.Instance.Show();

        Queue textQueue = new Queue();
        textQueue.Enqueue("TODO: FILL IN");
        TextboxManager.Instance.SetTextboxTextSet(textQueue);
    }
    public void PerformStartAnimation() {
        if(TextboxManager.Instance.HasFinished()) {
            TriggerWaveFinish();
        }
    }
    public void FinishStartAnimation() {
        TextboxManager.Instance.Hide();
        LevelManager.Instance.HideJanitorOverlay();
        LevelManager.Instance.ShowPauseButton();
    }
    //----------------------------------------------------------------------------
    public void TriggerFirstWave() {
        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { 
                                                                                            { BroType.GenericBro, 1f } 
                                                                                        };
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { 
                                                                                            { 0, .5f },
                                                                                            { 1, .5f } 
                                                                                        };
        BroDistributionObject firstWave = new BroDistributionObject(0,                              // Start Time
                                                                    5,                              // End Time
                                                                    1,                              // Number to Generate
                                                                    DistributionType.LinearIn,      // DistributionType
                                                                    DistributionSpacing.Uniform,    // DistributionSpacing
                                                                    broProbabilities,               // broProbabilities
                                                                    entranceQueueProbabilities);    // entranceQueueProbabilities
        firstWave.broConfigurer.SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
            .SetXMoveSpeed(BroDistribution.AllBros, 1.5f, 1.5f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.5f, 1.5f)
            .SetFightProbability(BroDistribution.AllBros, 0.15f, 0.15f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, false)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0, 0)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2, 2)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2, 2)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2, 2)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2, 2)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, true);
        // Fart Generator if the bro has it (TileBlocker Properties)
        firstWave.fartGeneratorConfigurer.SetProbability(BroDistribution.AllBros, 2, 2)
            .SetGenerationFrequency(BroDistribution.AllBros, 2, 2)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
            .SetMinGenerationFrequency(BroDistribution.AllBros, 2, 2)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, 2, 2);
        // Fart Properties
        firstWave.fartGeneratorConfigurer.SetDuration(BroDistribution.AllBros, 2, 2)
            .SetDurationIsStochastic(BroDistribution.AllBros, false)
            .SetMinDuration(BroDistribution.AllBros, 2, 2)
            .SetMaxDuration(BroDistribution.AllBros, 2, 2);
        // Vomit Generator if the bro has it (TileBlocker Properties)
        firstWave.vomitGeneratorConfigurer.SetProbability(BroDistribution.AllBros, 1f, 1f)
            .SetGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
            .SetMinGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, 2f, 2f);
        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                                 firstWave,
                                                                                });
    }
    public void PerformFirstWave() {
    }

    public void FinishFirstWave() {
    }
} 