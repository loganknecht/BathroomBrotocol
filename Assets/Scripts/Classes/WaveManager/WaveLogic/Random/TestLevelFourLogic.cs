using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLevelFourLogic : WaveLogic, WaveLogicContract {
  public bool startAnimationFinished = false;
  public bool performGenerationLogic = false;
  public bool generatedFirstWave = false;

  // public delegate void TextBoxButtonPressLogic();

  // Use this for initialization
  public override void Start () {
    SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

    PerformStartAnimation();
  }

  // Update is called once per frame
  // public override void Update () {
  //   base.Update();
  // }

  public void PerformStartAnimation() {
    // BroGenerator.Instance.isPaused = true;
    WaveManager.Instance.isPaused = true;

    LevelManager.Instance.HideJanitorButton();
    LevelManager.Instance.HidePauseButton();
    LevelManager.Instance.StartJanitorOverlaySlideInFromBottom();
    TextboxManager.Instance.StartSlideInFromBottom();
    TextboxManager.Instance.textboxTextFinishedLogicToPerform = StartAnimationFinished;
    StartAnimationFinished();
    waveLogicFinished = true;
  }

  public override void PerformWaveLogic() {
    PerformGenerationLogic();
  }

  public void PerformGenerationLogic() {
    //--------------------------------------------------------------------------
    // WAVE GENERATION LOGIC
    //--------------------------------------------------------------------------
    if(startAnimationFinished
       && !generatedFirstWave) {
      Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() {
                                                                                      { BroType.ChattyBro, 0.45f },
                                                                                      { BroType.DrunkBro, 0.10f },
                                                                                      { BroType.GenericBro, 0.45f }
                                                                                    };

      Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

      // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
      BroDistributionObject firstWave = new BroDistributionObject(0, 10, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
      BroDistributionObject secondWave = new BroDistributionObject(30, 45, 5, DistributionType.QuadraticEaseIn, DistributionSpacing.Random, broProbabilities, entranceQueueProbabilities);
      BroDistributionObject thirdWave = new BroDistributionObject(60, 70, 3, DistributionType.LinearIn, DistributionSpacing.Random, broProbabilities, entranceQueueProbabilities);

      // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave });
      BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                               firstWave,
                                                                               secondWave,
                                                                               thirdWave
                                                                              });
      generatedFirstWave = true;
    }
  }

  public void StartAnimationFinished() {
    startAnimationFinished = true;

    LevelManager.Instance.HideJanitorOverlay();
    LevelManager.Instance.ShowJanitorButton();
    LevelManager.Instance.ShowPauseButton();

    TextboxManager.Instance.Hide();
  }
}
