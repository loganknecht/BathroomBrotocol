using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayOne : WaveLogic, WaveLogicContract {

  public override void Awake() {
    base.Awake();
  }

  // Use this for initialization
  public override void Start () {
    SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);
    
    base.Start();

    foreach(GameObject gameObj in BathroomObjectManager.Instance.allBathroomObjects) {
      BathroomObject bathObjRef = gameObj.GetComponent<BathroomObject>();
      bathObjRef.destroyObjectIfMoreThanTwoOccupants = false;
    }

    GameObject startAnimationWaveGameObject = CreateWaveState("Start Animation Game Object",
                                                          TriggerStartAnimation,
                                                          PerformStartAnimation,
                                                          FinishStartAnimation);

    GameObject firstWaveGameObject = CreateWaveState("First Wave Game Object",
                                                     TriggerFirstWave,
                                                     PerformFirstWave,
                                                     FinishFirstWave);

    GameObject encouragementAnimationWaveGameObject = CreateWaveState("EncouragementAnimation Wave Game Object",
                                                     TriggerEncouragementAnimationWave,
                                                     PerformEncouragementAnimationWave,
                                                     FinishEncouragementAnimationWave);

    GameObject secondWaveGameObject = CreateWaveState("Second Wave Game Object",
                                                 TriggerSecondWave,
                                                 PerformSecondWave,
                                                 FinishSecondWave);

    GameObject endOfLevelAnimationWaveGameObject = CreateWaveState("EndOfLevelAnimation Wave Game Object",
                                                                   TriggerEndOfLevelAnimationWave,
                                                                   PerformEndOfLevelAnimationWave,
                                                                   FinishEndOfLevelAnimationWave);

    InitializeWaveStates(
                         startAnimationWaveGameObject,
                         firstWaveGameObject,
                         encouragementAnimationWaveGameObject,
                         secondWaveGameObject,
                         endOfLevelAnimationWaveGameObject
                         );
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  public void TriggerStartAnimation() {
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
  public void PerformStartAnimation() {
    // Debug.Log("performing start animation");
    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishStartAnimation() {
    // Debug.Log("finishing start animation");
    TextboxManager.Instance.Hide();
    LevelManager.Instance.HideJanitorOverlay();
  }

  public void TriggerFirstWave() {
    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    // BroDistributionObject firstWave = new BroDistributionObject(0, 10, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    BroDistributionObject firstWave = new BroDistributionObject(0, 3, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
    firstWave.broConfigurer.SetFightProbability(BroDistribution.AllBros, 0f, 0f)
             .SetLineQueueSkipType(BroDistribution.AllBros, true)
             .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
             .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
             .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                            });
  }
  public void PerformFirstWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishFirstWave() {
  }

  public void TriggerEncouragementAnimationWave() {
    LevelManager.Instance.ShowJanitorOverlay();
    TextboxManager.Instance.Show();

    float percentageBroken = BathroomObjectManager.Instance.GetPercentageOfBathroomObjectTypeBroken(BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
    float percentageOfBathroomLeft = System.Convert.ToInt32((1 - percentageBroken) * 100);
    Queue textQueue = new Queue();
    textQueue.Enqueue("Not bad, not bad...");
    textQueue.Enqueue("Looks like you have... about... " + percentageOfBathroomLeft + "% of the bathroom remaining...");
    textQueue.Enqueue("Again, not bad... but you know... you could do better. One of the things you need to understand is that in this role, as Bathroom Bro Czar, you can't expect to be able to repair your restroom while you work.");
    textQueue.Enqueue("What I'm trying to say is that once an object in the bathroom is broken you can't repair it. Them's the breaks.");
    textQueue.Enqueue("We'll send a clean up and repair crew in after we do our job, but that's just how it goes.");
    textQueue.Enqueue("Alright enough talking, get back to impressing me! If you can manage this restroom without losing all the objects in this restroom, then we'll talk about moving you forward.");
    TextboxManager.Instance.SetTextboxTextSet(textQueue);
  }
  public void PerformEncouragementAnimationWave() {
    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishEncouragementAnimationWave() {
  }

  public void TriggerSecondWave() {
    LevelManager.Instance.HideJanitorOverlay();
    TextboxManager.Instance.Hide();

    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
    firstWave.broConfigurer.SetFightProbability(BroDistribution.AllBros, 0f, 0f)
             .SetLineQueueSkipType(BroDistribution.AllBros, true)
             .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
             .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
             .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroDistributionObject secondWave = new BroDistributionObject(10, 15, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    secondWave.broConfigurer.SetFightProbability(BroDistribution.AllBros, 0f, 0f)
              .SetLineQueueSkipType(BroDistribution.AllBros, true)
              .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
              .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true)
              .SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                             secondWave,
                                                                            });
  }
  public void PerformSecondWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishSecondWave() {
  }

  public void TriggerEndOfLevelAnimationWave() {
    PerformWaveStateStartedTrigger();

    LevelManager.Instance.ShowJanitorOverlay();
    TextboxManager.Instance.Show();

    Queue textQueue = new Queue();
    textQueue.Enqueue("Alright, alright, alright. It looks like you have a knack for this.");
    textQueue.Enqueue("Guess you're going to the next round. Come back for the next round of try-outs. We'll see if you still have what it takes.");
    textQueue.Enqueue("Now get out of here!");
    TextboxManager.Instance.SetTextboxTextSet(textQueue);
    TextboxManager.Instance.SetTextboxFinishedLogic(PerformFinalEndOfLevelAnimationActions);
  }
  public void PerformEndOfLevelAnimationWave() {
    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishEndOfLevelAnimationWave() {
    waveLogicFinished = true;
    PerformWaveStateHasFinishedTrigger();
  }
  public void PerformFinalEndOfLevelAnimationActions() {
    LevelManager.Instance.HideJanitorOverlay();
    TextboxManager.Instance.Hide();
  }
}
