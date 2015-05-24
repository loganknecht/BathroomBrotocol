using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkBro : Bro {
    public bool vomitingFinished = false;
    
    public GameObject bathroomTileBlockerGeneratorGameObject = null;
    BathroomTileBlockerGenerator bathroomTileBlockerGenerator = null;
    
    public override void Awake() {
        base.Awake();
        
        bathroomTileBlockerGenerator = bathroomTileBlockerGeneratorGameObject.GetComponent<BathroomTileBlockerGenerator>();
        
        type = BroType.DrunkBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        if(!isPaused) {
            FightTimerLogic();
            Logic();
            VomitFinishedCheck();
            UpdateAnimator();
        }
    }
    
    public override void InitializeComponents() {
        if(targetPathingReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'targetPathingReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(animatorReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'animatorReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(selectableReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'selectableReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(isometricDisplayReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'isometricDisplayReference', it is NULL. Please fix this by assigned it before use.");
        }
        isometricDisplayReference.UpdateDisplayPosition();
    }
    
    public void VomitFinishedCheck() {
        if(bathroomTileBlockerGenerator != null
            && bathroomTileBlockerGenerator.HasFinished()
            && !vomitingFinished) {
            vomitingFinished = true;
            hasRelievedSelf = true;
            selectableReference.Reset();
            selectableReference.canBeSelected = false;
            bathroomTileBlockerGenerator.enabled = false;
            ExitBathroom();
        }
        else {
            // if(state != BroState.InAQueue
            //     && state != BroState.Standoff
            //     && state != BroState.Fighting
            //     && state != BroState.OccupyingObject) {
            //     bathroomTileBlockerGenerator.enabled = true;
            // }
            // else {
            //     bathroomTileBlockerGenerator.enabled = false;
            // }
        }
    }
    //==========================================================================
    public override void ReliefLogic(GameObject objectRelievedIn) {
        BathroomObject bathObjRef = objectRelievedIn.GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    // public override void ReliefScore(GameObject objectRelievedIn) {
    //     BathroomObject bathObjRef = objectRelievedIn.GetComponent<BathroomObject>();
    
    //     if(reliefRequired == ReliefRequired.Pee) {
    //         bathObjRef.state = BathroomObjectState.BrokenByPee;
    //         broScoreLogic.BrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
    //     }
    //     else if(reliefRequired == ReliefRequired.Poop) {
    //         bathObjRef.state = BathroomObjectState.BrokenByPoop;
    //         broScoreLogic.BrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
    //     }
    //     else if(reliefRequired == ReliefRequired.Vomit) {
    //         bathObjRef.state = BathroomObjectState.Broken;
    //         broScoreLogic.BrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
    //     }
    // }
    //===========================================================================
    public override void ExitOccupationFinishedLogic() {
        base.ExitOccupationFinishedLogic();
    }
    //===========================================================================
    public override void RelievedInOutOfOrderHandDryer() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
    }
    public override void RelievedInWorkingHandDryer() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
    }
    //===========================================================================
    public override void RelievedInOutOfOrderSink() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
    }
    public override void RelievedInWorkingSink() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
    }
//===========================================================================
    public override void RelievedInOutOfOrderStall() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    public override void RelievedInWorkingStall() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    //===========================================================================
    public override void RelievedInOutOfOrderUrinal() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public override void RelievedInWorkingUrinal() {
        ReliefLogic(GetTargetObject());
        broScoreLogic.BathroomObjectUsedScore();
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    //--------------------------------------------------------------------------
    //This is being checked on arrival before switching to occupying an object
    // public override void OnArrivalBrotocolScoreCheck() {
    // bool brotocolWasSatisfied = false;
    
    // // As long as the target object is not null and it's not a bathroom exit
    // if(targetObject != null
    //  && targetObject.GetComponent<BathroomObject>() != null
    //  && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
    //   if(!hasRelievedSelf) {
    //     if(CheckIfRelievedSelfBeforeTimeOut()) {
    //       ScoreManager.Instance.IncrementScoreTracker(ScoreType.DrunkBroBrotocolRelievedSelfBeforeTimeOut);
    //       brotocolWasSatisfied = true;
    //     }
    //   }
    // }
    
    // if(brotocolWasSatisfied) {
    //   SpriteEffectManager.Instance.GenerateSpriteEffectType(SpriteEffectType.BrotocolAchieved, targetObject.transform.position);
    // }
    // }
}
