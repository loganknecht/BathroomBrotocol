// using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class BaseBroScoreType : BaseBehavior {
public class BaseBroScoreType : MonoBehaviour {
    List<BathroomObjectType> bathroomObjectTypes;
    
    public float goodChoicePoints = 100f;
    public float badChoicePoints = -100f;
    
    //-------------------------------------------------------------------------
    // Entering/Exiting Score Logic
    //-------------------------------------------------------------------------
    public float entered = 0f;
    public float exited = 0f;
    
    //-------------------------------------------------------------------------
    // Relief Score Logic
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, float> relievedVomitIn;
    public Dictionary<BathroomObjectType, float> relievedPeeIn;
    public Dictionary<BathroomObjectType, float> relievedPoopIn;
    
    //-------------------------------------------------------------------------
    // Washed Hands Score Logic
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, float> washedHandsIn;
    
    //-------------------------------------------------------------------------
    // Dried Hands Score Logic
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, float> driedHandsIn;
    
    //-------------------------------------------------------------------------
    // Fighting Other Bro Score Logic
    //-------------------------------------------------------------------------
    public float startedStandoff = 0f;
    public float stoppedStandoff = 0f;
    public float startedFight = 0f;
    public float stoppedFight = 0f;
    
    //-------------------------------------------------------------------------
    // Out of Order Object Score Logic
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, float> causedOutOfOrderIn;
    
    //-------------------------------------------------------------------------
    // Breaking Objects Score Logic
    //-------------------------------------------------------------------------
    public Dictionary<BathroomObjectType, float> brokeByOutOfOrderUse;
    public Dictionary<BathroomObjectType, float> brokeByPeeing;
    public Dictionary<BathroomObjectType, float> brokeByPooping;
    public Dictionary<BathroomObjectType, float> brokeByVomiting;
    public Dictionary<BathroomObjectType, float> brokeByWashingHands;
    public Dictionary<BathroomObjectType, float> brokeByDryingHands;
    public Dictionary<BathroomObjectType, float> brokeByFighting;
    
    //-------------------------------------------------------------------------
    // Brotocol Score Logic
    //-------------------------------------------------------------------------
    public float satisfiedBrotocolNoAdjacentBros = 0f;
    public float totalPossibleBrotocolNoAdjacentBros = 0f;
    public float satisfiedBrotocolRelievedInCorrectObjectOnFirstTry = 0f;
    public float totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry = 0f;
    
    // protected override void Awake() {
    public void Awake() {
        bathroomObjectTypes = new List<BathroomObjectType>();
        bathroomObjectTypes.Add(BathroomObjectType.None);
        bathroomObjectTypes.Add(BathroomObjectType.Exit);
        bathroomObjectTypes.Add(BathroomObjectType.HandDryer);
        bathroomObjectTypes.Add(BathroomObjectType.PaperTowelDispenser);
        bathroomObjectTypes.Add(BathroomObjectType.Queue);
        bathroomObjectTypes.Add(BathroomObjectType.Sink);
        bathroomObjectTypes.Add(BathroomObjectType.Stall);
        bathroomObjectTypes.Add(BathroomObjectType.Urinal);
        bathroomObjectTypes.Add(BathroomObjectType.Wall);
        
        InitializeDictionaries();
    }
    public virtual void Start() {
    }
    public virtual void Update() {
    }
    
    public void InitializeDictionaries() {
        relievedVomitIn = new Dictionary<BathroomObjectType, float>();
        relievedPeeIn = new Dictionary<BathroomObjectType, float>();
        relievedPoopIn = new Dictionary<BathroomObjectType, float>();
        washedHandsIn = new Dictionary<BathroomObjectType, float>();
        driedHandsIn = new Dictionary<BathroomObjectType, float>();
        causedOutOfOrderIn = new Dictionary<BathroomObjectType, float>();
        brokeByOutOfOrderUse = new Dictionary<BathroomObjectType, float>();
        brokeByPeeing = new Dictionary<BathroomObjectType, float>();
        brokeByPooping = new Dictionary<BathroomObjectType, float>();
        brokeByVomiting = new Dictionary<BathroomObjectType, float>();
        brokeByWashingHands = new Dictionary<BathroomObjectType, float>();
        brokeByDryingHands = new Dictionary<BathroomObjectType, float>();
        brokeByFighting = new Dictionary<BathroomObjectType, float>();
        foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypes) {
            relievedVomitIn[bathroomObjectType] = 0f;
            relievedPeeIn[bathroomObjectType] = 0f;
            relievedPoopIn[bathroomObjectType] = 0f;
            washedHandsIn[bathroomObjectType] = 0f;
            driedHandsIn[bathroomObjectType] = 0f;
            causedOutOfOrderIn[bathroomObjectType] = 0f;
            brokeByOutOfOrderUse[bathroomObjectType] = 0f;
            brokeByPeeing[bathroomObjectType] = 0f;
            brokeByPooping[bathroomObjectType] = 0f;
            brokeByVomiting[bathroomObjectType] = 0f;
            brokeByWashingHands[bathroomObjectType] = 0f;
            brokeByDryingHands[bathroomObjectType] = 0f;
            brokeByFighting[bathroomObjectType] = 0f;
        }
    }
    
    public virtual float GetCurrentScore() {
        float currentScore = 0f;
        
        // currentScore += entered * goodChoicePoints;
        // currentScore += exited * goodChoicePoints;
        
        
        currentScore += relievedVomitIn[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += relievedPeeIn[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += relievedPoopIn[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += washedHandsIn[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += driedHandsIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += startedStandoff * 0;
        currentScore += stoppedStandoff * goodChoicePoints;
        currentScore += startedFight * badChoicePoints;
        currentScore += stoppedFight * goodChoicePoints / 2;
        
        currentScore += causedOutOfOrderIn[BathroomObjectType.HandDryer] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Sink] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Stall] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByPeeing[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByPeeing[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByPeeing[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByPeeing[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByPooping[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByPooping[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByPooping[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByPooping[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByVomiting[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByVomiting[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByVomiting[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByVomiting[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByWashingHands[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByWashingHands[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByWashingHands[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByWashingHands[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByDryingHands[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByDryingHands[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByDryingHands[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByDryingHands[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += brokeByFighting[BathroomObjectType.HandDryer] * badChoicePoints;
        currentScore += brokeByFighting[BathroomObjectType.Sink] * badChoicePoints;
        currentScore += brokeByFighting[BathroomObjectType.Stall] * badChoicePoints;
        currentScore += brokeByFighting[BathroomObjectType.Urinal] * badChoicePoints;
        
        currentScore += satisfiedBrotocolNoAdjacentBros * goodChoicePoints;
        // currentScore += totalPossibleBrotocolNoAdjacentBros * goodChoicePoints;
        currentScore += satisfiedBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        // currentScore += totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        
        return currentScore;
    }
    
    public virtual float GetPerfectScore() {
        float currentScore = 0f;
        
        // currentScore += entered * goodChoicePoints;
        // currentScore += exited * goodChoicePoints;
        
        currentScore += relievedVomitIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedVomitIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += relievedPeeIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedPeeIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += relievedPoopIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += relievedPoopIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += washedHandsIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += washedHandsIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += driedHandsIn[BathroomObjectType.HandDryer] * goodChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Sink] * goodChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Stall] * goodChoicePoints;
        currentScore += driedHandsIn[BathroomObjectType.Urinal] * goodChoicePoints;
        
        currentScore += startedStandoff * 0;
        currentScore += stoppedStandoff * goodChoicePoints;
        currentScore += startedFight * 0;
        currentScore += stoppedFight * goodChoicePoints;
        
        currentScore += causedOutOfOrderIn[BathroomObjectType.HandDryer] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Sink] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Stall] * 0;
        currentScore += causedOutOfOrderIn[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Sink] * 0;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Stall] * 0;
        currentScore += brokeByOutOfOrderUse[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByPeeing[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByPeeing[BathroomObjectType.Sink] * 0;
        currentScore += brokeByPeeing[BathroomObjectType.Stall] * 0;
        currentScore += brokeByPeeing[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByPooping[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByPooping[BathroomObjectType.Sink] * 0;
        currentScore += brokeByPooping[BathroomObjectType.Stall] * 0;
        currentScore += brokeByPooping[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByVomiting[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByVomiting[BathroomObjectType.Sink] * 0;
        currentScore += brokeByVomiting[BathroomObjectType.Stall] * 0;
        currentScore += brokeByVomiting[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByWashingHands[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByWashingHands[BathroomObjectType.Sink] * 0;
        currentScore += brokeByWashingHands[BathroomObjectType.Stall] * 0;
        currentScore += brokeByWashingHands[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByDryingHands[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByDryingHands[BathroomObjectType.Sink] * 0;
        currentScore += brokeByDryingHands[BathroomObjectType.Stall] * 0;
        currentScore += brokeByDryingHands[BathroomObjectType.Urinal] * 0;
        
        currentScore += brokeByFighting[BathroomObjectType.HandDryer] * 0;
        currentScore += brokeByFighting[BathroomObjectType.Sink] * 0;
        currentScore += brokeByFighting[BathroomObjectType.Stall] * 0;
        currentScore += brokeByFighting[BathroomObjectType.Urinal] * 0;
        
        // currentScore += satisfiedBrotocolNoAdjacentBros * goodChoicePoints;
        currentScore += totalPossibleBrotocolNoAdjacentBros * goodChoicePoints;
        // currentScore += satisfiedBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        currentScore += totalPossibleBrotocolRelievedInCorrectObjectOnFirstTry * goodChoicePoints;
        
        return currentScore;
    }
}
