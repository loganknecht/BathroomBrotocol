using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLevel : WaveLogic, WaveLogicContract {
  GameObject startAnimationWaveGameObject;
  GameObject broEnoughConfirmationWaveGameObject;
  GameObject broEnoughResponseWaveGameObject;
  GameObject firstWaveGameObject;
  GameObject secondWaveGameObject;

  GameObject broFightingExplanation;
  GameObject broFightingExplanationConfirmationGameObject;
  bool broFightingExplanationOccurred = false;

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

    broEnoughConfirmationWaveGameObject = CreateWaveState("BroEnoughConfirmation Game Object",
                                                                        TriggerBroEnoughConfirmation,
                                                                        PerformBroEnoughConfirmation,
                                                                        FinishBroEnoughConfirmation);

    broEnoughResponseWaveGameObject = CreateWaveState("BroEnoughResponse Game Object",
                                                                    TriggerBroEnoughResponse,
                                                                    PerformBroEnoughResponse,
                                                                    FinishBroEnoughResponse);

    firstWaveGameObject = CreateWaveState("FirstWave Game Object",
                                                      TriggerFirstWave,
                                                      PerformFirstWave,
                                                      FinishFirstWave);

    secondWaveGameObject = CreateWaveState("SecondWave Game Object",
                                                      TriggerSecondWave,
                                                      PerformSecondWave,
                                                      FinishSecondWave);

    broFightingExplanation = CreateWaveState("BroFightingExplanation Game Object",
                                              TriggerBroFightingExplanation,
                                              PerformBroFightingExplanation,
                                              FinishBroFightingExplanation);

    broFightingExplanationConfirmationGameObject = CreateWaveState("BroFightingExplanationConfirmation Game Object",
                                              TriggerBroFightingExplanationConfirmation,
                                              PerformBroFightingExplanationConfirmation,
                                              FinishBroFightingExplanationConfirmation);
    InitializeWaveStates(
                         // startAnimationWaveGameObject,
                         // broEnoughConfirmationWaveGameObject,
                         // broEnoughResponseWaveGameObject,
                         // firstWaveGameObject,
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
    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishStartAnimation() {
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
  public void TriggerFirstWave() {
    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                       { 1, .5f } };

    BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
    firstWave.SetFightProbability(BroDistribution.AllBros, 1f);
    firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
    firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    firstWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
    firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                            });
  }
  public void PerformFirstWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom()) {
      PerformWaveStatePlayingFinishedTrigger();
    }
    else if(!BroManager.Instance.NoFightingBrosInRestroom()
            && !broFightingExplanationOccurred) {
      PerformWaveStateThenReturn(broFightingExplanation);
      broFightingExplanationOccurred = true;
    }
  }
  public void FinishFirstWave() {
  }
  //----------------------------------------------------------------------------
  public void TriggerSecondWave() {
    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.SlobBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                       { 1, .5f } };

    BroDistributionObject firstBroSet = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    firstBroSet.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
    firstBroSet.SetFightProbability(BroDistribution.AllBros, 0f);
    firstBroSet.SetLineQueueSkipType(BroDistribution.AllBros, true);
    firstBroSet.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    firstBroSet.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
    firstBroSet.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstBroSet,
                                                                            });
  }
  public void PerformSecondWave() {
    if(BroGenerator.Instance.HasFinishedGenerating()
       && BroManager.Instance.NoBrosInRestroom()) {
      PerformWaveStatePlayingFinishedTrigger();
      waveLogicFinished = true;
    }
    // else if(!BroManager.Instance.NoFightingBrosInRestroom()
    //         && !broFightingExplanationOccurred) {
    //   PerformWaveStateThenReturn(broFightingExplanation);
    //   broFightingExplanationOccurred = true;
    // }
  }
  public void FinishSecondWave() {
  }
  //----------------------------------------------------------------------------
  public void TriggerBroFightingExplanation() {
    BroManager.Instance.Pause();
    LevelManager.Instance.ShowJanitorOverlay();
    TextboxManager.Instance.Show();

    Queue textQueue = new Queue();
    textQueue.Enqueue("Whoa, whoa, whoa, whoa, whoa! Looks like you got a bro fight in full swing here!");
    textQueue.Enqueue("Now I'm not sure if you understand how this happens, so I'm going to go ahead and explain it just in case you need a little refresher.");
    textQueue.Enqueue("The bathroom is a pretty dangerous place, period. Sometimes bros will run into eachother, and when that happens tempers flare and a brodown occurs.");
    textQueue.Enqueue("This is why you're here. When a brodown occurs you need to tap the bros that are fighting in order to tell them to knock it off!");
    textQueue.Enqueue("If you fail to stop a brodown turning into a full blown fight, well... then you've got bigger issues to look out for.");
    textQueue.Enqueue("First and foremost, when a bro fight breaks out, anything in the bathroom is fair game. This means if a bro fight touches a bathroom object, then it goes kaput.");
    textQueue.Enqueue("Obviously you can see why this is an issue. Because of this you're going to want to bounce all bro fights in the restroom on account that it's not good for you.");
    textQueue.Enqueue("To bounce the bros fighting, just tap them enough times to split them up, and they'll leave the restroom on their own.");
    textQueue.Enqueue("Now hurry up and handle those fighting bros!");
    TextboxManager.Instance.SetTextboxTextSet(textQueue);
  }
  public void PerformBroFightingExplanation() {
    if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
      EnqueueWaveStateAtFront(broFightingExplanationConfirmationGameObject);
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishBroFightingExplanation() {
    LevelManager.Instance.HideJanitorOverlay();
    TextboxManager.Instance.Hide();
  }
  //----------------------------------------------------------------------------
  public void TriggerBroFightingExplanationConfirmation() {
    ConfirmationBoxManager.Instance.SetText("Did you get all that, or do you need me to explain it again?");
    ConfirmationBoxManager.Instance.Reset();
    ConfirmationBoxManager.Instance.Show();
  }
  public void PerformBroFightingExplanationConfirmation() {
    if(ConfirmationBoxManager.Instance.selectedYes) {
      EnqueueWaveStateAtFront(broFightingExplanation);
      PerformWaveStatePlayingFinishedTrigger();
    }
    else if(ConfirmationBoxManager.Instance.selectedNo) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishBroFightingExplanationConfirmation() {
    BroManager.Instance.Unpause();
    ConfirmationBoxManager.Instance.Hide();
  }
} 