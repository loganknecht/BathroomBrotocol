using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTutorialLogic : WaveLogic, WaveLogicContract {

  public bool performingIntroAnimation = false;
  public bool performedIntroAnimation = false;

  public bool performingSecondAnimation = false;
  public bool performedSecondAnimation = false;

  public bool performingThirdAnimation = false;
  public bool performedThirdAnimation = false;

  public bool generatingWaveOne = false;
  public bool generatedWaveOne = false;
  public int numberOfWaveOneBrosToGenerate = 2;

  public bool generatingWaveTwo = false;
  public bool generatedWaveTwo = false;

  // Use this for initialization
  public override void Start () {
    // TextboxManager.Instance.textboxButtonLogicToPerform = new TextboxManager.TextboxButtonPressLogic(performTextboxButtonPressLogic);
    TextboxManager.Instance.textboxTextFinishedLogicToPerform = new TextboxManager.TextboxTextFinishedLogic(performTextboxTextFinishedLogic);

    PerformStartAnimation();
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  public void PerformStartAnimation() {
    StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false));

    WaveManager.Instance.isPaused = true;
    TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, -285, -600, -285, -145, 1, 2, UITweener.Method.BounceIn, null);

    LevelManager.Instance.janitorButton.GetComponent<UISprite>().alpha = 0;
    LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

    TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.position.x, -600, TextboxManager.Instance.gameObject.transform.position.x, -300, 1, 2, UITweener.Method.BounceIn, null);

    Queue queue = new Queue();
    queue.Enqueue("Just go ahead and tell the bros who come in where they need to go. Don't worry too much about brotocol right now.");
    queue.Enqueue("Alright then. With all that out of the way let's see how you can handle some bros in this here bathroom. So I can move on to the rest of these other schlubs in order to find a replacement for someone they could never live up to anyway.");
    queue.Enqueue("Hence this position. We hire someone to manage the washroom, and its attendees, while his broness is visiting in order to help maintain a positive relationship with whomever. A well managed and clean bathroom just seems to make things go easier.");
    queue.Enqueue("You just can't trust your average bro to not break something. They love it. LOVE IT.");
    queue.Enqueue("So why need a bathroom bro czar? Cmon man. Think about it! His broness brings gravitas and parties everywhere he goes! When there's a party it gets out of hand! And when a party gets out of hand, bros everywhere will break whatever they see.");
    queue.Enqueue("You see he has to rub elbows and party down through out the brobal world so that we remain united, strong, and proud as bros.");
    queue.Enqueue("Oh. You mean why is this job needed? Well... Basically his broness', in his infinite wisdom, and graciousness has to go to a lot of locations through out the world.");
    queue.Enqueue("This means I have to deal with you for a year until I can retire.");
    queue.Enqueue("My role is to bring sanitation to the bathrooms on your behalf. Why? Well your esteemed predecessor happened to retire, and he also happened to have signed up for this job a year ahead of me.");
    queue.Enqueue("Alright buddy. Today's the first day of his broness' try-outs to become a bathroom bro czar. This role requires you to bring order and sanity to bathrooms!");
    TextboxManager.Instance.SetTextboxTextSet(queue);

    performingIntroAnimation = true;
  }

  public override void PerformWaveLogic() {
    //this assumes that the paused state is already managed by the wave manager
    if(performingIntroAnimation) {
      // && !performedIntroAnimation) {
      //do nothing
    }
    else if(performingSecondAnimation) {
      //do nothing
    }
    else {
      PerformGenerationLogic();
    }
  }

  public void PerformGenerationLogic() {
    //generates wave one
    if(generatingWaveOne) {
      if(BroManager.Instance.allBros.Count == 0
         && numberOfWaveOneBrosToGenerate > 0) {
        GameObject newBroAdded = EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        newBroAdded.GetComponent<Bro>().canBeCheckedToFightAgainst = false;
        numberOfWaveOneBrosToGenerate--;
        if(numberOfWaveOneBrosToGenerate == 0) {
          generatedWaveOne = true;
        }
      }
      if(numberOfWaveOneBrosToGenerate == 0
        && BroManager.Instance.allBros.Count == 0) {
        WaveManager.Instance.isPaused = true;
        generatingWaveOne = false;
        performingSecondAnimation = true;

        LevelManager.Instance.ShowJanitorOverlay();
        TextboxManager.Instance.Show();

        LevelManager.Instance.HideJanitorButton();
        LevelManager.Instance.HidePauseButton();
        // LevelManager.Instance.StartJanitorOverlaySlideInFromBottom();
        // LevelManager.Instance.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.position.x, -500, TextboxManager.Instance.gameObject.transform.position.x, 0, 1, 2, UITweener.Method.BounceIn, null);

        Queue queue = new Queue();
        queue.Enqueue("You ready? Let's get to it");
        queue.Enqueue("The next task is going to be testing your ability to handled stress, with a lot of bros.");
        queue.Enqueue("Alright, alright, alright. I can see you know how to tell people what to do. Pretty good job.");
        TextboxManager.Instance.SetTextboxTextSet(queue);
      }
    }
    if(generatingWaveTwo) {
      bool generateBro = false;
      //first bro generates, makes sure hasn't already generated by looking at bro count
      if(!generatedWaveTwo) {
        if(WaveManager.Instance.waveTimer > 1
           && BroManager.Instance.allBros.Count == 0) {
          generateBro = true;
        }
        else if(WaveManager.Instance.waveTimer > 2
           && BroManager.Instance.allBros.Count == 1) {
          generateBro = true;
        }
        else if(WaveManager.Instance.waveTimer > 3
           && BroManager.Instance.allBros.Count == 2) {
          generateBro = true;
        }
        else if(WaveManager.Instance.waveTimer > 4
           && BroManager.Instance.allBros.Count == 3) {
          generateBro = true;
        }
        else if(WaveManager.Instance.waveTimer > 5
           && BroManager.Instance.allBros.Count == 4) {
          generateBro = true;
        }
        else if(WaveManager.Instance.waveTimer > 6
           && BroManager.Instance.allBros.Count == 5) {
          generateBro = true;
          generatedWaveTwo = true;
        }
      }

      if(generateBro) {
        GameObject newBroAdded = EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        newBroAdded.GetComponent<Bro>().canBeCheckedToFightAgainst = false;
      }

      if(generatedWaveTwo
         && BroManager.Instance.allBros.Count == 0) {
        WaveManager.Instance.isPaused = true;
        generatingWaveTwo = false;
        performingThirdAnimation = true;

        LevelManager.Instance.ShowJanitorOverlay();
        TextboxManager.Instance.Show();

        LevelManager.Instance.HideJanitorButton();
        LevelManager.Instance.HidePauseButton();

        Queue queue = new Queue();
        queue.Enqueue("Now get out of here! I have to look into your better, more capable, competition.");
        queue.Enqueue("I see a lot of potential in you. Don't make me look like an idiot.");
        queue.Enqueue("Alright... Against my better judgement I'm going to have you come back for the next try-outs.");
        queue.Enqueue("*grumble*  *grumble*  *grumble*");
        queue.Enqueue("Eeehhhhh.....");
        TextboxManager.Instance.SetTextboxTextSet(queue);
      }
    }
  }

  public void performTextboxButtonPressLogic() {
    // TextboxManager.Instance.Hide();
    TextboxManager.Instance.MoveToNextTextboxText();
  }
  public void performTextboxTextFinishedLogic() {
    //Intro button press end
    if(performingIntroAnimation) {
      performingIntroAnimation = false;
      performedIntroAnimation = true;
      generatingWaveOne = true;

      TextboxManager.Instance.Hide();
      LevelManager.Instance.HideJanitorOverlay();
      LevelManager.Instance.ShowJanitorButton();
      LevelManager.Instance.ShowPauseButton();

      WaveManager.Instance.isPaused = false;
      WaveManager.Instance.waveTimer = 0;
    }
    else if(performingSecondAnimation) {
      performingSecondAnimation = false;
      performedSecondAnimation = true;

      generatingWaveTwo = true;

      TextboxManager.Instance.Hide();
      LevelManager.Instance.HideJanitorOverlay();
      LevelManager.Instance.ShowJanitorButton();
      LevelManager.Instance.ShowPauseButton();

      WaveManager.Instance.isPaused = false;
      WaveManager.Instance.waveTimer = 0;
    }
    else if(performingThirdAnimation) {
      performingThirdAnimation = false;
      performedThirdAnimation = true;

      LevelManager.Instance.HideJanitorOverlay();
      LevelManager.Instance.HidePauseButton();
      LevelManager.Instance.HideJanitorButton();
      TextboxManager.Instance.Hide();

      FadeManager.Instance.fadeFinishLogic = new FadeManager.FadeFinishEvent(TriggerSceneChange);
      StartCoroutine(FadeManager.Instance.PerformFullScreenFade(Color.clear, Color.white, 1, false));
      // PerformFullScreenFade(Color startFadeColor, Color endFadeColor, float fadeDuration, bool destroyFullScreenTextureAtFinish) {
    }
  }

  void TriggerSceneChange() {
    // Debug.Log("Performing Scene Change");
    Application.LoadLevel("ScoreMenu");
  }
}
