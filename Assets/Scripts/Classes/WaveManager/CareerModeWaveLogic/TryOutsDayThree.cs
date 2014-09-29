using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayThree : WaveLogic, WaveLogicContract 
{
    public GameObject startAnimationWaveGameObject;
    public GameObject broEnoughConfirmationWaveGameObject;
    public GameObject broEnoughResponseWaveGameObject;
    public GameObject firstWaveGameObject;
    public GameObject secondWaveGameObject;

    public GameObject broFightingExplanation;
    public GameObject broFightingExplanationConfirmationGameObject;
    public bool broFightingExplanationOccurred = false;

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start () {

        //begin from top level WaveLogic
        base.Start();

        //Start the first game object (the janitor explaining the situation)
        startAnimationWaveGameObject = CreateWaveState("Start Animation Game Object",
                                                                    TriggerStartAnimation,
                                                                    PerformStartAnimation,
                                                                    FinishStartAnimation);
    
        //This is the confirmation that the player is ready to help the bros
        broEnoughConfirmationWaveGameObject = CreateWaveState("BroEnoughConfirmation Game Object",
                                                                            TriggerBroEnoughConfirmation,
                                                                            PerformBroEnoughConfirmation,
                                                                            FinishBroEnoughConfirmation);

        //talking again to the player after they've used the confirmation box
        broEnoughResponseWaveGameObject = CreateWaveState("BroEnoughResponse Game Object",
                                                                        TriggerBroEnoughResponse,
                                                                        PerformBroEnoughResponse,
                                                                        FinishBroEnoughResponse);

        //start the first wave of generic bros
        firstWaveGameObject = CreateWaveState("FirstWave Game Object",
                                                          TriggerFirstWave,
                                                          PerformFirstWave,
                                                          FinishFirstWave);

        //start the second wave of generic bros
        secondWaveGameObject = CreateWaveState("SecondWave Game Object",
                                                          TriggerSecondWave,
                                                          PerformSecondWave,
                                                          FinishSecondWave);

        //show the user the bro fight dialog from janitor if they haven't seen it
        broFightingExplanation = CreateWaveState("BroFightingExplanation Game Object",
                                                  TriggerBroFightingExplanation,
                                                  PerformBroFightingExplanation,
                                                  FinishBroFightingExplanation);

        //show a confirmation that they understand the bro fighting
        broFightingExplanationConfirmationGameObject = CreateWaveState("BroFightingExplanationConfirmation Game Object",
                                                  TriggerBroFightingExplanationConfirmation,
                                                  PerformBroFightingExplanationConfirmation,
                                                  FinishBroFightingExplanationConfirmation);

        //set the wave states to go in order as shown above
        InitializeWaveStates(
                             startAnimationWaveGameObject,
                             broEnoughConfirmationWaveGameObject,
                             broEnoughResponseWaveGameObject,
                             firstWaveGameObject,
                             secondWaveGameObject,
                             broFightingExplanation,
                             broFightingExplanationConfirmationGameObject
                             );
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }
    //----------------------------------------------------------------------------
    //start the level
    public void TriggerStartAnimation() {
        
        // Debug.Log("triggering start animation");

        //Start the sound
        SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);
    
        //fade the screen to white
        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

        //pause the wave manager (no bros come out at this point)
        WaveManager.Instance.isPaused = true;
    
        //move the janitor from bottom up bounce with a little fade in
        TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -445, LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x, -145, 1, 2, UITweener.Method.BounceIn, null);

        //hide the janitor and the pause button by alpha
        LevelManager.Instance.janitorButton.GetComponent<UISprite>().alpha = 0;
        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

        //move text box area in the same way as the janitor (bottom up, with bounce)
        TweenExecutor.TweenObjectPosition(TextboxManager.Instance.gameObject, TextboxManager.Instance.gameObject.transform.localPosition.x, -600, TextboxManager.Instance.gameObject.transform.localPosition.x, -300, 1, 2, UITweener.Method.BounceIn, null);

        //Level 3 start
        //set the text to the text box manager
        Queue textQueue = new Queue();
        textQueue.Enqueue("Well, well well. Look who came crawling back. I've seen turds bigger than you come rolling back.");
        textQueue.Enqueue("Ignore that last part.");
        textQueue.Enqueue("Today I'm going to cover the types of bros there are.");
        textQueue.Enqueue("We're going to cover Generic bros, and X bros.");
        textQueue.Enqueue("Genereic bros are simple. They need to use the urinal sometimes. They need to poop sometimes. Don't we all?");
        textQueue.Enqueue("As long as you point them to urinals when they need to pee, and stalls when they need to poop, they will be ok.");
        textQueue.Enqueue("I've had too much Ice Beer to make any of my future bathroom experiences OK.");
        textQueue.Enqueue("Seriously, it's like type 7 on the Bristol Stool Scale.");
        textQueue.Enqueue("Ignore that last part.");
        textQueue.Enqueue("Don't forget, generic bros can get into fights, and you can't turn back from here!");
        textQueue.Enqueue("Alright, are you ready to help our generic bros relieve their generic selves?");
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
        //Set the confirmation box text
        ConfirmationBoxManager.Instance.SetText("Are you ready to help these generic bros out?");
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
    //reply to the confirmation box
    public void TriggerBroEnoughResponse() {

        Queue startText = new Queue();
        //Yes or no from the confirmation box
        if(ConfirmationBoxManager.Instance.selectedYes) {
            startText.Enqueue("That's the attitude I'm looking for. You just might earn yourself a spot on my butt-wiping crew.");
        }
        else if(ConfirmationBoxManager.Instance.selectedNo) {
            startText.Enqueue("Well, you're too far in to back out now. Man, you should pay more attention.");
        }
        startText.Enqueue("There better be at least a single sink by the time you're done in here.");
        startText.Enqueue("Here they come!");
        TextboxManager.Instance.SetTextboxTextSet(startText);

        ConfirmationBoxManager.Instance.Hide();
    }
    public void PerformBroEnoughResponse() {
        //hide the janitor
        if(TextboxManager.Instance.HasFinishedTextboxTextSet()) {
            LevelManager.Instance.HideJanitorOverlay();
            TextboxManager.Instance.Hide();

            PerformWaveStatePlayingFinishedTrigger();
        }
    }
    public void FinishBroEnoughResponse() {
    }
    //----------------------------------------------------------------------------
    //start the wave!
    public void TriggerFirstWave() {

        //create the dictionary for the bros in the game, and the probability that they will appear
        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };

        //create the dictionary for the entrances that exist, and the probability that the next bro will choose it
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                            { 10, .5f } };

        //BroDistributionObject is the actual Wave object.
        //WaveObject(start_time, end_time, point_modifier, distribution_type, distribution_spacing, entrance_prob)
        //Starts with start time and an end time. Bros are generated during this duration.
        //point_modifier is a value for how many points the user will get during the wave. In this example it's 10 X some factor.
        //distribution_type is how the rush of bros is distributed. Fast, linear, slow, etc.
        //distribution_spacing is how the bros will gain points based on who they are next to. Uniform is X 0 X 0 for max points.
        //entrance_prob is the probabilities for which entrance to go to.

        BroDistributionObject firstWave = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        
        //SetReliefType is setting what can "relieve" a bro. Fighting (aka RandomBros), Pee, or Poop
        firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);

        //Fight probability is set to 100%
        firstWave.SetFightProbability(BroDistribution.AllBros, 1f);

        //The bros will skip the line queue
        firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);

        //The bros will not select a random object to relieve themselves
        firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);

        //The bros will roam around the bathroom until given a task is set to true
        firstWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);

        //The bros will not choose another object to go to after they have finished a relieved state
        firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] 
        {
            firstWave,
        });
    }
    public void PerformFirstWave() {
        //If the bros are all generated, and they have all finished and left the restroom, then finish wave
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            //The wave has finished
            PerformWaveStatePlayingFinishedTrigger();
        }
        
        //If the game isn't finished, then we're still playing.
        //While we're playing, this checks if any bros are fighting currently AND if the dialog that explains fighting has been shown.
        else if(!BroManager.Instance.NoFightingBrosInRestroom()
                && !broFightingExplanationOccurred) {
            PerformWaveStateThenReturn(broFightingExplanation);
            //Then the is_dialog_shown boolean is set to true
            broFightingExplanationOccurred = true;
        }
    }
    public void FinishFirstWave() {
    }
    //----------------------------------------------------------------------------
    //Second wave
    public void TriggerSecondWave() {
        Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.FartingBro, 1f } };
        Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .5f },
                                                                                            { 1, .5f } };

        BroDistributionObject firstBroSet = new BroDistributionObject(0, 5, 5, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
        firstBroSet.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
        firstBroSet.SetFightProbability(BroDistribution.AllBros, 1f);
        firstBroSet.SetLineQueueSkipType(BroDistribution.AllBros, true);
        firstBroSet.SetChooseObjectOnLineSkip(BroDistribution.AllBros, false);
        firstBroSet.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, true);
        firstBroSet.SetChooseObjectOnRelief(BroDistribution.AllBros, false);

        BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] 
        {
            firstBroSet,
        });
    }
    public void PerformSecondWave() {
        //check to see when second part of first wave is finished
        if(BroGenerator.Instance.HasFinishedGenerating()
            && BroManager.Instance.NoBrosInRestroom()) {
            PerformWaveStatePlayingFinishedTrigger();
            waveLogicFinished = true;
        }
        
        //keep checking if the fight dialog has shown
        else if(!BroManager.Instance.NoFightingBrosInRestroom()
                && !broFightingExplanationOccurred) {
            PerformWaveStateThenReturn(broFightingExplanation);
            broFightingExplanationOccurred = true;
        }
    }
    public void FinishSecondWave() {
    }
    //----------------------------------------------------------------------------
    //The logic if a fight happens
    public void TriggerBroFightingExplanation() {
        //Pause the bros
        BroManager.Instance.Pause();
        //Show the janitor overlay
        LevelManager.Instance.ShowJanitorOverlay();
        //Show the text overlay
        TextboxManager.Instance.Show();

        //start another text queue
        Queue textQueue = new Queue();
        textQueue.Enqueue("Ok at this point you may have encountered a bro fight or two.");
        textQueue.Enqueue("If you don't know, bros are emotional. If they run into each other they need to defend their principles, and fight.");
        textQueue.Enqueue("A brodown can occur, and this is bad for your rating to his Bro-ness.");
        textQueue.Enqueue("You can repeatedly tap the fighting bros, and they will stop fighting, however.");
        textQueue.Enqueue("If you aren't able to tap the bros and separate them, then you're going to have a whirlwind of bro-problems.");
        textQueue.Enqueue("The bros will literally become a whirlwind and start destroying all bathroom objects.");
        textQueue.Enqueue("That's right. All urinals, stalls, and sinks can be rendered useless.");
        textQueue.Enqueue("Just make sure to tap the fighting bros a bunch if they start going at it!");
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
        ConfirmationBoxManager.Instance.SetText("Did you understand the bro fighting, scrub?");
        ConfirmationBoxManager.Instance.Reset();
        ConfirmationBoxManager.Instance.Show();
    }
    public void PerformBroFightingExplanationConfirmation() {
        if(ConfirmationBoxManager.Instance.selectedNo) {
            broFightingExplanationOccurred = false;
            EnqueueWaveStateAtFront(broFightingExplanation);
            PerformWaveStatePlayingFinishedTrigger();
        }
        else if(ConfirmationBoxManager.Instance.selectedYes) {
            PerformWaveStatePlayingFinishedTrigger();
        }
    }
    public void FinishBroFightingExplanationConfirmation() {
        BroManager.Instance.Unpause();
        ConfirmationBoxManager.Instance.Hide();
    }
}