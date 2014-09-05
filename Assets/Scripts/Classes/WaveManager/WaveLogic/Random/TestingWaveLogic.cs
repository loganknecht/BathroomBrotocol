using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestingWaveLogic : WaveLogic, WaveLogicContract {

  public bool waveTwentyGenerated = false;

  // public delegate void TextBoxButtonPressLogic();

  // Use this for initialization
  public override void Start () {
    // PerformStartAnimation();
    // Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
    // Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    // BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // BroDistributionObject secondWave = new BroDistributionObject(5, 10, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);

    // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave }, DistributionType.Uniform);
    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             // firstWave,
                                                                             // secondWave
                                                                            });
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  public void PerformStartAnimation() {

    WaveManager.Instance.isPaused = true;
    BroGenerator.Instance.isPaused = true;

    // LevelManager.Instance.StartJanitorOverlaySlideInFromBottom();
    // LevelManager.Instance.HideJanitorButton();
    // LevelManager.Instance.HidePauseButton();

   TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.position.x, -500, TextboxManager.Instance.gameObject.transform.position.x, 0, 1, 2, UITweener.Method.BounceIn, null);
  }

  public override void PerformWaveLogic() {
    PerformGenerationLogic();
  }

  public void PerformGenerationLogic() {
    // if(WaveManager.Instance.currentTimer > 40) {
    //   //need check for fighting bros
    //   //Perform level completed screen
    //   if(BroManager.Instance.allBros.Count == 0) {
    //     LevelManager.Instance.PerformScoreSceneTransition();
    //   }
    //   this.waveLogicFinished = true;
    // }
  }

  // public void performTextboxButtonPressLogic() {
  //   // TextboxManager.Instance.Hide();
  //   TextboxManager.Instance.MoveToNextTextboxText();
  // }

  // public void performTextboxTextFinishedLogic() {
  //   TextboxManager.Instance.Hide();
  //   LevelManager.Instance.HideJanitorOverlay();
  //   LevelManager.Instance.UnhideJanitorButton();
  //   LevelManager.Instance.UnhidePauseButton();

  //   WaveManager.Instance.isPaused = false;
  // }
}
