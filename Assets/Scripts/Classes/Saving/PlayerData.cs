// using FullInspector;

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// class PlayerData : BaseBehavior {
class PlayerData : MonoBehaviour {
    Dictionary<ChallengeLevel, ChallengeLevelData> challengeLevels;
    // Dictionary<StoryLevel, StoryLevelData> storyLevels;
    
    // protected override void Awake() {
    // base.Awake();
    // }
    public void Start() {
    }
    public void Updated() {
    }
    public void InitializeDictionaries() {
        foreach(ChallengeLevel challengeLevel in GetAllChallengeLevelEnums()) {
            // TODO: lol configure this
            challengeLevels[challengeLevel] = null;
        }
    }
    public List<StoryLevel> GetAllStoryLevelEnums() {
        List<StoryLevel> allStoryLevelEnums = new List<StoryLevel>();
        allStoryLevelEnums.Add(StoryLevel.One);
        allStoryLevelEnums.Add(StoryLevel.Two);
        allStoryLevelEnums.Add(StoryLevel.Three);
        allStoryLevelEnums.Add(StoryLevel.Four);
        allStoryLevelEnums.Add(StoryLevel.Five);
        allStoryLevelEnums.Add(StoryLevel.Six);
        allStoryLevelEnums.Add(StoryLevel.Seven);
        allStoryLevelEnums.Add(StoryLevel.Eight);
        allStoryLevelEnums.Add(StoryLevel.Nine);
        allStoryLevelEnums.Add(StoryLevel.Ten);
        allStoryLevelEnums.Add(StoryLevel.Eleven);
        allStoryLevelEnums.Add(StoryLevel.Twelve);
        allStoryLevelEnums.Add(StoryLevel.Thirteen);
        allStoryLevelEnums.Add(StoryLevel.Fourteen);
        allStoryLevelEnums.Add(StoryLevel.Fifteen);
        allStoryLevelEnums.Add(StoryLevel.Sixteen);
        allStoryLevelEnums.Add(StoryLevel.Seventeen);
        allStoryLevelEnums.Add(StoryLevel.Eighteen);
        allStoryLevelEnums.Add(StoryLevel.Nineteen);
        allStoryLevelEnums.Add(StoryLevel.Twenty);
        allStoryLevelEnums.Add(StoryLevel.TwentyOne);
        allStoryLevelEnums.Add(StoryLevel.TwentyTwo);
        allStoryLevelEnums.Add(StoryLevel.TwentyThree);
        allStoryLevelEnums.Add(StoryLevel.TwentyFour);
        allStoryLevelEnums.Add(StoryLevel.TwentyFive);
        allStoryLevelEnums.Add(StoryLevel.TwentySix);
        allStoryLevelEnums.Add(StoryLevel.TwentySeven);
        allStoryLevelEnums.Add(StoryLevel.TwentyEight);
        allStoryLevelEnums.Add(StoryLevel.TwentyNine);
        allStoryLevelEnums.Add(StoryLevel.Thirty);
        return allStoryLevelEnums;
    }
    public List<ChallengeLevel> GetAllChallengeLevelEnums() {
        List<ChallengeLevel> allChallengeLevelEnums = new List<ChallengeLevel>();
        allChallengeLevelEnums.Add(ChallengeLevel.One);
        allChallengeLevelEnums.Add(ChallengeLevel.Two);
        allChallengeLevelEnums.Add(ChallengeLevel.Three);
        allChallengeLevelEnums.Add(ChallengeLevel.Four);
        allChallengeLevelEnums.Add(ChallengeLevel.Five);
        allChallengeLevelEnums.Add(ChallengeLevel.Six);
        allChallengeLevelEnums.Add(ChallengeLevel.Seven);
        allChallengeLevelEnums.Add(ChallengeLevel.Eight);
        allChallengeLevelEnums.Add(ChallengeLevel.Nine);
        allChallengeLevelEnums.Add(ChallengeLevel.Ten);
        allChallengeLevelEnums.Add(ChallengeLevel.Eleven);
        allChallengeLevelEnums.Add(ChallengeLevel.Twelve);
        allChallengeLevelEnums.Add(ChallengeLevel.Thirteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Fourteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Fifteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Sixteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Seventeen);
        allChallengeLevelEnums.Add(ChallengeLevel.Eighteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Nineteen);
        allChallengeLevelEnums.Add(ChallengeLevel.Twenty);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyOne);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyTwo);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyThree);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyFour);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyFive);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentySix);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentySeven);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyEight);
        allChallengeLevelEnums.Add(ChallengeLevel.TwentyNine);
        allChallengeLevelEnums.Add(ChallengeLevel.Thirty);
        return allChallengeLevelEnums;
    }
    
    public void ResetData() {
        challengeLevels = new Dictionary<ChallengeLevel, ChallengeLevelData>();
        // storyLevels = new Dictionary<StoryLevel, StoryLevelData>();
    }
}