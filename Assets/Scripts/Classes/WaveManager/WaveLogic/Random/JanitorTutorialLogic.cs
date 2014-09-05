using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JanitorTutorialLogic : WaveLogic, WaveLogicContract {

  public bool performingVomitCleanUpExample = false;
  public bool performedVomitCleanUpExample = false;

  public bool performingDrunkBroExplanation = false;
  public bool performedDrunkBroExplanation = false;

  public bool performingDrunkBroGenerating = false;
  public bool performedDrunkBroGenerating = false;

  // Use this for initialization
  public override void Start () {
    PerformStartAnimation();
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();

    //this assumes that the paused state is already managed by the wave manager
    if(performingVomitCleanUpExample
       && !performedVomitCleanUpExample) {
      if(!JanitorManager.Instance.IsJanitorSummoned()
         && BathroomTileBlockerManager.Instance.bathroomTileBlockers.Count == 0) {

        performingVomitCleanUpExample = false;
        performedVomitCleanUpExample = true;

        LevelManager.Instance.ShowJanitorOverlay();
        TextboxManager.Instance.Show();

        Queue levelGoalExplanationText = new Queue();
        levelGoalExplanationText.Enqueue("Not bad. Not great, but not bad...");
        levelGoalExplanationText.Enqueue("I'm sure you're wondering how the vomit got there in the first place, so I'll humor you.");
        levelGoalExplanationText.Enqueue("In our pursual of an ever-clean restroom we will encounter many types of bros. Today we're going to put you through your paces by serving drunk bros.");
        levelGoalExplanationText.Enqueue("Yes. That means this bathroom will be full of drunk bros. These bros will come in and possibly throw up - if not assisted fast enough. Following that they would then leave the restroom.");
        levelGoalExplanationText.Enqueue("If you are able to help the drunks bros throw up in any of the bathroom objects available, instead of letting them throw up on the floor, you will satisfy brotocol.");
        levelGoalExplanationText.Enqueue("What is brotocol? Well... that's something for another time, but in short there needs to be order in the restroom, and adhering to the rules of brotocol helps maintain order.");
        levelGoalExplanationText.Enqueue("Today your goal is provide reliable brotocol while drunk bros enter this restroom.");
        levelGoalExplanationText.Enqueue("If you manage to help relieve at least 70 percent of the bros arriving then you can move on to the final round of try-outs to be a part of his broness' entourage.");
        levelGoalExplanationText.Enqueue("If you fail, well... we'll talk about it if it gets to that point.");
        levelGoalExplanationText.Enqueue("Alright. You ready? Do your best!");
        TextboxManager.Instance.SetTextboxTextSet(levelGoalExplanationText);
        TextboxManager.Instance.textboxTextFinishedLogicToPerform = new TextboxManager.TextboxTextFinishedLogic(OnLevelGoalExplanationFinished);

        performingDrunkBroExplanation = true;

        WaveManager.Instance.isPaused = true;
      }
    }
    if(performingDrunkBroExplanation
       && !performedDrunkBroExplanation) {
    }
  }

  public void PerformStartAnimation() {
    StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, true));

    WaveManager.Instance.isPaused = true;
    TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, -240, -600, -240, -178, 1, 2, UITweener.Method.BounceIn, null);

    LevelManager.Instance.janitorButton.GetComponent<UISprite>().alpha = 0;
    LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

    TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.position.x, -600, TextboxManager.Instance.gameObject.transform.position.x, -300, 1, 2, UITweener.Method.BounceIn, null);

    Queue queue = new Queue();
    queue.Enqueue("*sigh* Wha? Who is it? Oh it's you try-out 567-7? 7 - something. Whatever.");
    queue.Enqueue("Okay. Okay. Let's see. You're back to show me your amazing gusto and wow me. I'm sure.");
    queue.Enqueue("In order to do that you need to summon me into the restroom. Then, I need you to work with me and tell me what to clean up. Finally after that unsummon me.");
    queue.Enqueue("Again, Step One: Summon me. Step two: tell me what needs a fixin'. Step three: unsummon me from the restroom.");
    queue.Enqueue("We'll... talk about the rest of the exam after that... But, as you can see one of the previous try-outs failed to keep the bathroom sanitary and we need to clean that up before we get into the full swing of your test.");
    TextboxManager.Instance.SetTextboxTextSet(queue);
    TextboxManager.Instance.textboxTextFinishedLogicToPerform = new TextboxManager.TextboxTextFinishedLogic(OnIntroExplanationFinished);
  }

  public override void PerformWaveLogic() {

  }

  public void PerformGenerationLogic() {
  }

  void TriggerSceneChange() {
    // Debug.Log("Performing Scene Change");
    Application.LoadLevel("ScoreMenu");
  }

  public void OnIntroExplanationFinished() {
    if(!performingVomitCleanUpExample) {
      performingVomitCleanUpExample = true;

      LevelManager.Instance.HideJanitorOverlay();
      LevelManager.Instance.ShowJanitorButton();
      LevelManager.Instance.ShowPauseButton();

      TextboxManager.Instance.Hide();

      WaveManager.Instance.isPaused = false;
    }
  }

  public void OnLevelGoalExplanationFinished() {
    if(performingDrunkBroExplanation) {
      performingDrunkBroExplanation = false;
      performedDrunkBroExplanation = true;

      LevelManager.Instance.HideJanitorOverlay();
      LevelManager.Instance.ShowJanitorButton();
      LevelManager.Instance.ShowPauseButton();

      TextboxManager.Instance.Hide();

      WaveManager.Instance.isPaused = false;
      WaveManager.Instance.currentTimer = 0;
    }
  }
}
