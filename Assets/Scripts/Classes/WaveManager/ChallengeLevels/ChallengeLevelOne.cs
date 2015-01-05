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

    public void TriggerIntroTextLogic() {
        // Debug.Log("triggering start animation");
        PerformWaveStateStartedTrigger();

        // SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

        WaveManager.Instance.isPaused = true;
        TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -445, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -145, 1, 2, UITweener.Method.BounceIn, null);

        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

        TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.localPosition.x, -600, TextboxManager.Instance.gameObject.transform.localPosition.x, -300, 1, 2, UITweener.Method.BounceIn, null);

        Queue textQueue = new Queue();
        textQueue.Enqueue("Hey there chief! It looks like you're here to try-out out for the prestigious role of 'Bathroom Bro Czar'.");
        textQueue.Enqueue("Alright, alright, let's calm your pecs down. I can see you're excited. Yes, it is true. We are looking for a couple of bros to bring on for his broness' cabinet.");
        textQueue.Enqueue("More specifically, we're looking to bring on a specific a specific bro to replace me, the Head Bathroom Bro Czar.");
        textQueue.Enqueue("I know. You're impressed. Well keep the clapping down to a minimum. We got some weeding out of the weaker candidates to do, and spoilers you may be one of them.");
        textQueue.Enqueue("Here's the thing. The first day of try-outs is to weed out those who don't even know how to manage a restroom. I call these guys the schlubs. Are you a schlub? I don't know... You look like a pretty big schlub to me.");
        textQueue.Enqueue("Prove me wrong. I'm sending in bros into this restroom and you need to send them to the correct location otherwise they're going to destroy the whole restroom...");
        textQueue.Enqueue("If you manage to survive this exercise then you move on to the next try-outs.");
        textQueue.Enqueue("Ready... Set...");
        textQueue.Enqueue("Bro!");
        TextboxManager.Instance.SetTextboxTextSet(textQueue);
    }
    public void PerformIntroTextLogic() {
    }
    public void FinishIntroTextLogic() {
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
            PerformWaveStatePlayingFinishedTrigger();
        }
    }
    public void FinishFirstWave() {
        waveLogicFinished = true;
    }
  //----------------------------------------------------------------------------
} 