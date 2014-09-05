using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayOne : WaveLogic, WaveLogicContract {

  public override void Awake() {
    base.Awake();
  }

  // Use this for initialization
  public override void Start () {
    GameObject startAnimationGameObject = ((new GameObject("Start Animation Game Object")).AddComponent<WaveState>()).gameObject;
    startAnimationGameObject.GetComponent<WaveState>().ConfigureLogic(TriggerStartAnimation, PerformStartAnimationLogic, FinishStartAnimation);
    startAnimationGameObject.transform.parent = this.gameObject.transform.parent.transform;

    GameObject firstWaveGameObject = ((new GameObject("First Wave Game Object")).AddComponent<WaveState>()).gameObject;
    firstWaveGameObject.GetComponent<WaveState>().ConfigureLogic(TriggerFirstWaveStart, PerformFirstWaveLogic, FinishFirstWave);
    firstWaveGameObject.transform.parent = this.gameObject.transform.parent.transform;

    waveStatesQueue = new Queue();
    waveStatesQueue.Enqueue(startAnimationGameObject);
    waveStatesQueue.Enqueue(firstWaveGameObject);

    if(waveStatesQueue.Count != 0) {
      currentWaveStateGameObject = (GameObject)waveStatesQueue.Dequeue();
    }


  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  public void TriggerStartAnimation() {
    // Debug.Log("triggering start animation");
    PerformWaveStateStartedTrigger();

    SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

    StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false));

    WaveManager.Instance.isPaused = true;
    TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -445, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -145, 1, 2, UITweener.Method.BounceIn, null);

    LevelManager.Instance.janitorButton.GetComponent<UISprite>().alpha = 0;
    LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

    Debug.Log(TextboxManager.Instance.gameObject);
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
  public void PerformStartAnimationLogic() {
    // Debug.Log("performing start animation");
    if(TextboxManager.Instance.hasFinishedTextBoxText) {
      PerformWaveStatePlayingFinishedTrigger();
    }
  }
  public void FinishStartAnimation() {
    // Debug.Log("finishing start animation");
    TextboxManager.Instance.Hide();
    LevelManager.Instance.HideJanitorOverlay();

    PerformWaveStateHasFinishedTrigger();
  }

  public void TriggerFirstWaveStart() {
    PerformWaveStateStartedTrigger();

    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    BroDistributionObject firstWave = new BroDistributionObject(0, 20, 10, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // firstWave.SetReliefType(BroDistribution.AllBros, new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
    firstWave.SetFightCheckType(BroDistribution.AllBros, false);
    firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
    // firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
    // firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                            });
  }
  public void PerformFirstWaveLogic() {
    PerformWaveStatePlayingFinishedTrigger();
  }
  public void FinishFirstWave() {
    PerformWaveStateHasFinishedTrigger();
  }

  public void PerformWaveStateStartedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().hasBeenTriggered = true;
    currentWaveStateGameObject.GetComponent<WaveState>().isPlaying = true;
  }
  public void PerformWaveStatePlayingFinishedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().triggerFinishLogic = true;
  }
  public void PerformWaveStateHasFinishedTrigger() {
    currentWaveStateGameObject.GetComponent<WaveState>().hasFinished = true;
  }


}
