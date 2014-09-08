using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayTwo : WaveLogic, WaveLogicContract {

  public override void Awake() {
    base.Awake();
  }

  // Use this for initialization
  public override void Start () {
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
                         // startAnimationWaveGameObject,
                         // firstWaveGameObject,
                         // encouragementAnimationWaveGameObject,
                         // secondWaveGameObject,
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

    SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

    FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

    WaveManager.Instance.isPaused = true;
    TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -445, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -145, 1, 2, UITweener.Method.BounceIn, null);

    LevelManager.Instance.janitorButton.GetComponent<UISprite>().alpha = 0;
    LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

    TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.localPosition.x, -600, TextboxManager.Instance.gameObject.transform.localPosition.x, -300, 1, 2, UITweener.Method.BounceIn, null);

    Queue textQueue = new Queue();
    textQueue.Enqueue("Oh hey, look this guy is back. Hooray. I can take a big sigh of relief knowing that you're here to show me you're the best candidate for the job.");
    textQueue.Enqueue("Yes. Yes that was sarcasm.");
    textQueue.Enqueue("Now shut up and listen, 'cause yesterday's try out was easy mode. Why? Well for two reasons.");
    textQueue.Enqueue("First, those bros were being polite, they didn't try to relieve themselves in any inappropriate spots. That was on purpose.");
    textQueue.Enqueue("That was intentional. However, from here on out, all bros will be trying to relieve themselves in any bathroom object they choose.");
    textQueue.Enqueue("How does that affect you? Well if a bro relieves himself in the inappropriate location, then that bathroom object will be... for lack of a better word unusable.");
    textQueue.Enqueue("If you lose all of the bathroom objects in the restroom, then well.. you've failed your role, and the try-out.");
    textQueue.Enqueue("No stress though. You got this. You're \"Mr. Big Tough Guy\"");
    textQueue.Enqueue("Alright, are you ready for this?");
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

    PerformWaveStateHasFinishedTrigger();
  }

  public void TriggerFirstWave() {
    PerformWaveStateStartedTrigger();

    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    // BroDistributionObject firstWave = new BroDistributionObject(0, 10, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
    firstWave.SetFightCheckType(BroDistribution.AllBros, false);
    firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
    firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                            });
  }
  public void PerformFirstWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishFirstWave() {
    PerformWaveStateHasFinishedTrigger();
  }

  public void TriggerEncouragementAnimationWave() {
    PerformWaveStateStartedTrigger();

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
    PerformWaveStateHasFinishedTrigger();
  }

  public void TriggerSecondWave() {
    PerformWaveStateStartedTrigger();

    LevelManager.Instance.HideJanitorOverlay();
    TextboxManager.Instance.Hide();

    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
    firstWave.SetFightCheckType(BroDistribution.AllBros, false);
    firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
    firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroDistributionObject secondWave = new BroDistributionObject(10, 15, 5, DistributionType.QuadraticEaseOut, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    secondWave.SetFightCheckType(BroDistribution.AllBros, false);
    secondWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
    secondWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    secondWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                             secondWave,
                                                                            });
  }
  public void PerformSecondWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishSecondWave() {
    PerformWaveStateHasFinishedTrigger();
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
