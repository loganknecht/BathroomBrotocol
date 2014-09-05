using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleWaveLogic : WaveLogic, WaveLogicContract {

  public bool introPerformed = false;

  public bool waveOneGenerated = false;
  public bool waveTwoGenerated = false;
  public bool waveThreeGenerated = false;
  public bool waveFourGenerated = false;
  public bool waveFiveGenerated = false;
  public bool waveSixGenerated = false;
  public bool waveSevenGenerated = false;
  public bool waveEightGenerated = false;
  public bool waveNineGenerated = false;
  public bool waveTenGenerated = false;
  public bool waveElevenGenerated = false;
  public bool waveTwelveGenerated = false;
  public bool waveThirteenGenerated = false;
  public bool waveFourteenGenerated = false;
  public bool waveFifteenGenerated = false;
  public bool waveSixteenGenerated = false;
  public bool waveSeventeenGenerated = false;
  public bool waveEighteenGenerated = false;
  public bool waveNineteenGenerated = false;
  public bool waveTwentyGenerated = false;

  // public delegate void TextBoxButtonPressLogic();

  // Use this for initialization
  public override void Start () {
    TextboxManager.Instance.textboxButtonLogicToPerform = new TextboxManager.TextboxButtonPressLogic(performTextboxButtonPressLogic);
    TextboxManager.Instance.textboxTextFinishedLogicToPerform = new TextboxManager.TextboxTextFinishedLogic(performTextboxTextFinishedLogic);

    PerformStartAnimation();
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  public void PerformStartAnimation() {
    WaveManager.Instance.isPaused = true;
    // LevelManager.Instance.StartJanitorOverlaySlideInFromBottom();
    LevelManager.Instance.HideJanitorButton();
    LevelManager.Instance.HidePauseButton();

    TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.position.x, -500, TextboxManager.Instance.gameObject.transform.position.x, 0, 1, 2, UITweener.Method.BounceIn, null);

    Queue queue = new Queue();
    queue.Enqueue("Alright buddy. Today's the first day of his broness' try-outs to become a bathroom bro czar. This role requires you to bring order and sanity to bathrooms!");
    queue.Enqueue("My role is to bring sanitary to the bathrooms on your behalf. Why? Well your esteemed predecessor happened to retire, and he also happened to have signed up for this job a year ahead of me.");
    queue.Enqueue("This means I have to deal with you for a year until I can retire.");
    queue.Enqueue("You see he has to rub elbows and party down through out the brobal world so that we remain united, strong, and proud as bros.");
    queue.Enqueue("So why need a bathroom bro czar? Cmon man. Think about it! His broness brings gravitas and parties everywhere he goes! When there's a party it gets out of hand! And when a party gets out of hand, bros everywhere will break whatever they see.");
    queue.Enqueue("You just can't trust your average bro to not break something. They love it. LOVE IT.");
    queue.Enqueue("Oh. You mean why is this job needed? Well... Basically his broness', in his infinite wisdom, and graciousness has to go to a lot of locations through out the world.");
    queue.Enqueue("Hence this position. We hire someone to manage the washroom, and its attendees, while his broness is visiting in order to help maintain a positive relationship with whomever. A well managed and clean bathroom just seems to make things go easier.");
    queue.Enqueue("Alright then. With all that out of the way let's see how you can handle some bros in this here bathroom. So I can move on to the rest of these other schlubs in order to find a replacement for someone they could never live up to anyway.");
    queue.Enqueue("Just go ahead and tell the bros who come in where they need to go. Don't worry too much about brotocol right now.");
    TextboxManager.Instance.SetTextboxTextSet(queue);
  }

  public override void PerformWaveLogic() {
    PerformGenerationLogic();
  }

  public void PerformGenerationLogic() {
    if(WaveManager.Instance.currentTimer > 5) {
      if(!waveOneGenerated) {
        // Debug.Log("Performing wave 1.");
        // waveLogicFinished = true;
        // EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveOneGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 5.5) {
      if(!waveTwoGenerated) {
        // Debug.Log("Performing wave 2.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveTwoGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 6) {
      if(!waveThreeGenerated) {
        // Debug.Log("Performing wave 3.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveThreeGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 10) {
      if(!waveFourGenerated) {
        // Debug.Log("Performing wave 4.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveFourGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 15) {
      if(!waveFiveGenerated) {
        // Debug.Log("Performing wave 5.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveFiveGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 20) {
      if(!waveSixGenerated) {
        // Debug.Log("Performing wave 6.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveSixGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 25) {
      if(!waveSevenGenerated) {
        // Debug.Log("Performing wave 7.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveSevenGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 30) {
      if(!waveEightGenerated) {
        // Debug.Log("Performing wave 8.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveEightGenerated = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 35) {
      if(!waveNineGenerated) {
        // Debug.Log("Performing wave 9.");
        EntranceQueueManager.Instance.GenerateSpecificBroInRandomEntranceQueue(BroType.GenericBro);
        waveNineGenerated = true;

        waveLogicFinished = true;
      }
    }

    if(WaveManager.Instance.currentTimer > 40) {
      //need check for fighting bros
      //Perform level completed screen
      if(BroManager.Instance.allBros.Count == 0) {
        LevelManager.Instance.PerformScoreSceneTransition();
      }
      this.waveLogicFinished = true;
    }
  }

  public void performTextboxButtonPressLogic() {
    // TextboxManager.Instance.Hide();
    TextboxManager.Instance.MoveToNextTextboxText();
  }
  public void performTextboxTextFinishedLogic() {
    TextboxManager.Instance.Hide();
    LevelManager.Instance.HideJanitorOverlay();
    LevelManager.Instance.ShowJanitorButton();
    LevelManager.Instance.ShowPauseButton();

    WaveManager.Instance.isPaused = false;
  }
}
