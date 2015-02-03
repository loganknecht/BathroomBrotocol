using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLevel : WaveLogic, WaveLogicContract {
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

        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 1;

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
    }
    //----------------------------------------------------------------------------
    public void TriggerFirstWave() {
        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { 
                                                                                            { 0, .5f },
                                                                                            { 1, .5f } 
                                                                                        };

        BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        firstWave.broConfigurer.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop)
                               .SetFightProbability(BroDistribution.AllBros, 1f, 1f)
                               .SetLineQueueSkipType(BroDistribution.AllBros, true)
                               .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
                               .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
                               .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                                 firstWave,
                                                                                });
    }
    public void PerformFirstWave() {
    }

    public void FinishFirstWave() {
    }
} 