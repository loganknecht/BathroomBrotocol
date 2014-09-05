using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLevelWaveLogic : WaveLogic, WaveLogicContract {

  public bool waveTwentyGenerated = false;

  // public delegate void TextBoxButtonPressLogic();

  // Use this for initialization
  public override void Start () {
    PerformStartAnimation();

    // Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    // Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    // BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // BroDistributionObject secondWave = new BroDistributionObject(5, 10, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);

    // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave }, DistributionType.Uniform);
    // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
    //                                                                          firstWave,
    //                                                                          // secondWave
    //                                                                         });
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();

  }

  public void PerformStartAnimation() {
    // BroGenerator.Instance.isPaused = true;
    WaveManager.Instance.isPaused = true;

    LevelManager.Instance.HideJanitorButton();
    LevelManager.Instance.HidePauseButton();
    LevelManager.Instance.StartJanitorOverlaySlideInFromBottom();
    TextboxManager.Instance.StartSlideInFromBottom();
    TextboxManager.Instance.textboxTextFinishedLogicToPerform = StartAnimationFinished;
    StartAnimationFinished();


    //--------------------------------------------------------------------------
    // WAVE GENERATION LOGIC
    //--------------------------------------------------------------------------
    // Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    // Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    // BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // BroDistributionObject secondWave = new BroDistributionObject(5, 10, 5, DistributionType.LinearIn, DistributionSpacing.Random, broProbabilities, entranceQueueProbabilities);

    // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave });
    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             // firstWave,
                                                                             // secondWave
                                                                            });
  }

  public override void PerformWaveLogic() {
    PerformGenerationLogic();
  }

  public void PerformGenerationLogic() {
  }

  public void StartAnimationFinished() {
    LevelManager.Instance.HideJanitorOverlay();
    LevelManager.Instance.ShowJanitorButton();
    LevelManager.Instance.ShowPauseButton();

    TextboxManager.Instance.Hide();
  }
}
