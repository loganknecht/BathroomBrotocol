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

    GameObject broEnoughConfirmationWaveGameObject = CreateWaveState("BroEnoughConfirmation Game Object",
                                                                      TriggerBroEnoughConfirmation,
                                                                      PerformBroEnoughConfirmation,
                                                                      FinishBroEnoughConfirmation);

    GameObject broEnoughResponseWaveGameObject = CreateWaveState("BroEnoughResponse Game Object",
                                                                  TriggerBroEnoughResponse,
                                                                  PerformBroEnoughResponse,
                                                                  FinishBroEnoughResponse);

    GameObject secondWaveGameObject = CreateWaveState("Second Wave Game Object",
                                                      TriggerSecondWave,
                                                      PerformSecondWave,
                                                      FinishSecondWave);
    InitializeWaveStates(
                         startAnimationWaveGameObject,
                         broEnoughConfirmationWaveGameObject,
                         broEnoughResponseWaveGameObject,
                         secondWaveGameObject
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
    // TextboxManager.Instance.Hide();
    // LevelManager.Instance.HideJanitorOverlay();

  }
  //----------------------------------------------------------------------------
  public void TriggerBroEnoughConfirmation() {

    ConfirmationBoxManager.Instance.Show();
    ConfirmationBoxManager.Instance.Reset();
  }
  public void PerformBroEnoughConfirmation() {
    if(ConfirmationBoxManager.Instance.hasSelectedAnswer) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishBroEnoughConfirmation() {
  }
  //----------------------------------------------------------------------------
  public void TriggerBroEnoughResponse() {

    Queue startText = new Queue();
    if(ConfirmationBoxManager.Instance.selectedYes) {
      startText.Enqueue("Oh look at you. You really are Mr. Tough Guy. Alright then. Let's do this.");
    }
    else if(ConfirmationBoxManager.Instance.selectedNo) {
      startText.Enqueue("C'mon brah! Get it together! You're already here! Don't you want to be someone?!");
    }
    startText.Enqueue("I wanna see you manage this bathroom and make sure that you have AT LEAST one bathroom object remaining. If you can manage that, then you get to move on to the next round of try-outs.");
    TextboxManager.Instance.SetTextboxTextSet(startText);

    ConfirmationBoxManager.Instance.Hide();
  }
  public void PerformBroEnoughResponse() {

    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      LevelManager.Instance.HideJanitorOverlay();
      TextboxManager.Instance.Hide();

      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishBroEnoughResponse() {
  }
  //----------------------------------------------------------------------------
  public void TriggerSecondWave() {

  Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
  Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                     { 1, .5f } };

  BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
  firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
  firstWave.SetFightCheckType(BroDistribution.AllBros, true);
  firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
  firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
  firstWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
  firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

  BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                           firstWave,
                                                                          });
  }
  public void PerformSecondWave() {
    if(BroManager.Instance.NoBrosInRestroom()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishSecondWave() {
  }
}
