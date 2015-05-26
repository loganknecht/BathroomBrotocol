using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bro : BaseBehavior {
// public class Bro : MonoBehaviour {
    public TargetPathing targetPathingReference = null;
    public Animator animatorReference = null;
    public BathroomFacing bathroomFacing;
    public HighlightSelectable selectableReference = null;
    public SpeechBubble speechBubbleReference = null;
    public IsometricDisplay isometricDisplayReference = null;
    public BoxCollider2D colliderReference = null;
    public DrawNodeList drawNodeList = null;
    public BroScoreLogic broScoreLogic = null;
    
    public BroType type;
    public BroState state = BroState.None;
    
    public float occupationTimer = 0f;
    public Dictionary<BathroomObjectType, float> occupationDuration;
    
    public bool skipLineQueue = false;
    public bool chooseRandomBathroomObjectOnSkipLineQueue = false;
    public bool hasRelievedSelf = false;
    public bool chooseRandomBathroomObjectAfterRelieved = false;
    public bool hasWashedHands = false;
    public bool chooseRandomBathroomObjectAfterWashedHands = false;
    public bool hasDriedHands = false;
    
    public bool startRoamingOnArrivalAtBathroomObjectInUse = false;
    
    public GameObject standoffBroGameObject = null;
    public GameObject broFightingWith = null;
    public GameObject lineQueueIn = null;
    
    public ReliefRequired reliefRequired = ReliefRequired.None;
    
    public float baseProbabilityOfFightOnCollisionWithBro = 0.15f;
    public bool modifyBroFightProbablityUsingScoreRatio = false;
    
    public bool resetFightLogic = false;
    public float fightCooldownTimer = 0f;
    public float fightCooldownTimerMax = 3f;
    
    public bool isPaused = false;
    
    public Color selectionColor = Color.white;
    
    protected override void Awake() {
        InitializeOccupationDuration();
        InitializeComponents();
    }
    
    // Use this for initialization
    public virtual void Start() {
        // base.Start();
        float newSelectionColor = Random.Range(0, 10000) + (0.618033988749895f * Random.Range(0f, 1f)) % 1;
        selectionColor = CustomColor.HSVToRGB(new Vector3(newSelectionColor, 0.75f, 0.95f));
        // Debug.Log("Color: " + selectionColor.ToString());
        //----------------------------------------------------------------------
        // Sets the on pop movement logic to redraw the draw nodes every time
        // targetPathingReference.SetOnArrivalAtMovementNodeLogic(() => drawNodeList.SetDrawNodes(targetPathingReference.GetMovementNodes()));
        // targetPathingReference.SetOnPopMovementNodeLogic(() => drawNodeList.SetDrawNodes(new Vector2(this.gameObject.transform.position.x,
        //----------------------------------------------------------------------
        if(drawNodeList != null) {
            drawNodeList.SetColor(selectionColor);
        }
        if(selectableReference != null) {
            selectableReference.SetColor(selectionColor);
        }
        
        isometricDisplayReference.UpdateDisplayPosition();
    }
    
    // Update is called once per frame
    public virtual void Update() {
        if(!isPaused) {
            SpeechBubbleLogic();
            FightTimerLogic();
            Logic();
            if(targetPathingReference.directionBeingLookedAt != Facing.None) {
                bathroomFacing.facing = targetPathingReference.directionBeingLookedAt;
            }
            UpdateAnimator();
        }
    }
    
    public virtual void InitializeComponents() {
        if(targetPathingReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'targetPathingReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(animatorReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'animatorReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(selectableReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'selectableReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(speechBubbleReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'speechBubbleReference', it is NULL. Please fix this by assigned it before use.");
        }
        if(isometricDisplayReference == null) {
            Debug.LogError("There was an issue with '" + this.gameObject.name + "'. It is missing its 'isometricDisplayReference', it is NULL. Please fix this by assigned it before use.");
        }
    }
    
    public virtual void InitializeOccupationDuration() {
        if(occupationDuration == null) {
            float defaultOccupationDuration = 2f;
            
            occupationDuration = new Dictionary<BathroomObjectType, float>();
            
            BathroomObjectType[] bathroomObjectTypes = (BathroomObjectType[])BathroomObjectType.GetValues(typeof(BathroomObjectType));
            foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypes) {
                switch(bathroomObjectType) {
                case(BathroomObjectType.Exit):
                    defaultOccupationDuration = 0f;
                    break;
                case(BathroomObjectType.HandDryer):
                    break;
                case(BathroomObjectType.Sink):
                    break;
                case(BathroomObjectType.Stall):
                    break;
                case(BathroomObjectType.Urinal):
                    break;
                default:
                    break;
                }
                if(bathroomObjectType != BathroomObjectType.None) {
                    occupationDuration[bathroomObjectType] = defaultOccupationDuration;
                }
            }
        }
    }
    
    public Bro SetState(BroState newState) {
        state = newState;
        return this;
    }
    public Bro SetFacing(Facing newFacing) {
        bathroomFacing.facing = newFacing;
        return this;
    }
    
    public Bro SetColliderActive(bool isActive) {
        colliderReference.enabled = isActive;
        return this;
    }
    
    public Bro SetXMoveSpeed(float newXMoveSpeed) {
        targetPathingReference.SetXMoveSpeed(newXMoveSpeed);
        return this;
    }
    public Bro SetYMoveSpeed(float newYMoveSpeed) {
        targetPathingReference.SetYMoveSpeed(newYMoveSpeed);
        return this;
    }
    
    public Bro SetTargetObject(GameObject newTargetObject) {
        targetPathingReference.SetTargetObject(newTargetObject);
        return this;
    }
    
    public GameObject GetTargetObject() {
        return targetPathingReference.GetTargetObject();
    }
    
    public Bro SetLocation(Vector3 newLocation) {
        SetLocation(new Vector2(newLocation.x, newLocation.y));
        return this;
    }
    public Bro SetLocation(Vector2 newLocation) {
        this.gameObject.transform.position = new Vector3(newLocation.x,
                                                         newLocation.y,
                                                         this.gameObject.transform.position.z);
        return this;
    }
    
    public Bro ToggleTargetPathing(bool useTargetPathing) {
        // Set to the opposite because the state is opposite in target pathing :|
        targetPathingReference.disableMovementLogic = !useTargetPathing;
        return this;
    }
    
    public virtual Bro SetMovementNodes(List<GameObject> newMovementNodes) {
        SetTargetObjectAndTargetPosition(targetPathingReference.GetTargetObject(),
                                         newMovementNodes);
        return this;
    }
    
    public virtual Bro SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector3 newTargetPosition) {
        targetPathingReference.SetTargetObjectAndTargetPosition(newTargetObject, newTargetPosition);
        return this;
    }
    public virtual Bro SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector2 newTargetPosition) {
        targetPathingReference.SetTargetObjectAndTargetPosition(newTargetObject, newTargetPosition);
        return this;
    }
    
    public virtual Bro SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
        occupationTimer = 0;
        targetPathingReference.SetTargetObjectAndTargetPosition(newTargetObject, newMovementNodes);
        if(drawNodeList != null) {
            drawNodeList.SetDrawNodes(new Vector2(this.gameObject.transform.position.x,
                                                  this.gameObject.transform.position.y),
                                      targetPathingReference.GetMovementNodes());
        }
        return this;
    }
    
    public virtual Bro AddMovementNodes(List<GameObject> newMovementNodes) {
        // public TargetPathing AddMovementNodes(List<GameObject> newMovementNodes, bool resetArrivalFlag = false) {
        targetPathingReference.AddMovementNodes(newMovementNodes);
        return this;
    }
    
    public bool HasMovementNodes() {
        return targetPathingReference.HasMovementNodes();
    }
    
    public List<GameObject> GetMovementNodes() {
        return targetPathingReference.GetMovementNodes();
    }
    
    public GameObject GetFirstMovementNode() {
        return targetPathingReference.GetFirstMovementNode();
    }
    
    public GameObject GetLastMovementNode() {
        return targetPathingReference.GetLastMovementNode();
    }
    
    public Vector3 GetTargetPosition() {
        return targetPathingReference.GetTargetPosition();
    }
    
    public virtual bool IsAtTargetPosition() {
        return targetPathingReference.IsAtTargetPosition();
    }
    
    public virtual float GetOccupationDuration(BathroomObjectType bathroomObjectType) {
        return occupationDuration[bathroomObjectType];
    }
    public virtual void SetOccupationDuration(BathroomObjectType bathroomObjectType, float occupationDurationTime) {
        occupationDuration[bathroomObjectType] = occupationDurationTime;
    }
    
    public virtual void Pause() {
        isPaused = true;
        if(this.gameObject.GetComponent<FartGenerator>() != null) {
            this.gameObject.GetComponent<FartGenerator>().isPaused = true;
        }
    }
    
    public virtual void Unpause() {
        isPaused = false;
        if(this.gameObject.GetComponent<FartGenerator>() != null) {
            this.gameObject.GetComponent<FartGenerator>().isPaused = false;
        }
    }
    
    public virtual bool IsExiting() {
        if(GetTargetObject() != null
            && GetTargetObject().GetComponent<BathroomObject>() != null
            && GetTargetObject().GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
            return true;
        }
        else {
            return false;
        }
    }
    
    // Returns the base probablity of fighting plus the modifier based on the score
    public virtual float GetFightProbability() {
        if(modifyBroFightProbablityUsingScoreRatio) {
            return (baseProbabilityOfFightOnCollisionWithBro); //TODO: GET SCORETRACKER TO RETURN THE MODIFIER BASE ON SCORE TRACKER'S PERFECT SCORE RATIO
        }
        else {
            return baseProbabilityOfFightOnCollisionWithBro;
        }
    }
    public virtual void FightTimerLogic() {
        if(resetFightLogic) {
            fightCooldownTimer += Time.deltaTime;
            if(fightCooldownTimer > fightCooldownTimerMax) {
                fightCooldownTimer = 0;
                resetFightLogic = false;
            }
        }
    }
    public virtual void ResetFightLogic(float fightResetDuration = 2f) {
        resetFightLogic = true;
        fightCooldownTimer = 0;
        fightCooldownTimerMax = fightResetDuration;
    }
    
    public virtual void OnMouseDown() {
        // Debug.Log("clicked");
        SelectionManager.Instance.SelectBro(this.gameObject);
        if(state == BroState.Standoff) {
            if(standoffBroGameObject != null) {
                standoffBroGameObject.GetComponent<StandoffBros>().IncrementTapsFromPlayer();
            }
        }
    }
    
    //  public void OnCollisionEnter(Collision collision) {
    //    Debug.Log("Collision occurred with: " + collision.gameObject.name);
    //  }
    
    public void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Trigger occurred with: " + other.gameObject.name);
        GameObject otherBroGameObject = other.transform.parent.transform.parent.gameObject;
        // Debug.Log("otherBroGameObject: " + otherBroGameObject.name);
        Bro otherBroRef = otherBroGameObject.GetComponent<Bro>();
        // Bro otherBroRef = other.gameObject.GetComponent<Bro>();
        if(otherBroRef != null) {
            // Debug.Log("not null dude!");
            //------------------------------------------------------------
            if((state == BroState.MovingToTargetObject || state == BroState.Roaming)
                && !IsExiting()
                && !resetFightLogic
                && baseProbabilityOfFightOnCollisionWithBro > 0
                && (otherBroRef.state == BroState.MovingToTargetObject || otherBroRef.state == BroState.Roaming)
                && !otherBroRef.IsExiting()
                && !otherBroRef.resetFightLogic
                && otherBroRef.baseProbabilityOfFightOnCollisionWithBro > 0) {
                // Debug.Log("CHECKING FOR FIGHT");
                
                float  checkToSeeIfFightOccurs = Random.Range(0.0f, 1f);
                if(checkToSeeIfFightOccurs < baseProbabilityOfFightOnCollisionWithBro) {
                    if(state != BroState.Fighting) {
                        // broFightingWith = other.gameObject;
                        broFightingWith = otherBroGameObject;
                        state = BroState.Standoff;
                        targetPathingReference.ClearMovementNodes();
                        speechBubbleReference.displaySpeechBubble = false;
                        
                        otherBroRef.broFightingWith = this.gameObject;
                        otherBroRef.state = BroState.Standoff;
                        otherBroRef.targetPathingReference.ClearMovementNodes();
                        otherBroRef.speechBubbleReference.displaySpeechBubble = false;
                    }
                }
                else {
                    ResetFightLogic(0.5f);
                }
            }
        }
    }
    
    public virtual void UpdateAnimator() {
        if(animatorReference != null) {
            foreach(BroState broState in BroState.GetValues(typeof(BroState))) {
                if(broState != BroState.None) {
                    animatorReference.SetBool(broState.ToString(), false);
                }
            }
            if(state != BroState.None) {
                animatorReference.SetBool(state.ToString(), true);
            }
            
            bathroomFacing.UpdateAnimatorWithFacing(animatorReference);
        }
    }
    
    public virtual void Logic() {
        switch(state) {
        case(BroState.None):
            break;
        case(BroState.Fighting):
            break;
        case(BroState.InAQueue):
            InAQueueLogic();
            break;
        case(BroState.MovingToTargetObject):
            MovingToTargetObjectLogic();
            break;
        case(BroState.OccupyingObject):
            OccupyingObjectLogic();
            break;
        case(BroState.Roaming):
            RoamingLogic();
            break;
        case(BroState.Standing):
            StandingLogic();
            break;
        case(BroState.Standoff):
            StandOffLogic();
            break;
        default:
            break;
        }
    }
    
    // TODO: Convert this so that it's relying on a function configured and attached to the
    //       target moving script
    public virtual void ArrivalLogic() {
        if(IsAtTargetPosition()) {
            GameObject targetObject = GetTargetObject();
            if(targetObject != null
                && targetObject.GetComponent<BathroomObject>() != null) {
                // Debug.Log("target object is not null");
                BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.transform.position.x, this.transform.position.y, false).GetComponent<BathroomTile>();
                BathroomTile targetObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(targetObject.transform.position.x, targetObject.transform.position.y, true).GetComponent<BathroomTile>();
                
                if(broTile.tileX == targetObjectTile.tileX
                    && broTile.tileY == targetObjectTile.tileY) {
                    
                    BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
                    
                    // if broken
                    // OR if startRoamingOnArrivalAtBathroomObjectInUse is true and there is another occupant
                    if(bathObjRef.IsBroken()
                        || (bathObjRef.objectsOccupyingBathroomObject.Count > 0
                            && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit
                            && startRoamingOnArrivalAtBathroomObjectInUse)) {
                        state = BroState.Roaming;
                    }
                    else {
                        broScoreLogic.OnArrivalBrotocolScoreCheck(GetTargetObject());
                        
                        //Adds bro to occupation list
                        bathObjRef.AddBro(this.gameObject);
                        
                        selectableReference.canBeSelected = false;
                        selectableReference.Reset();
                        speechBubbleReference.displaySpeechBubble = false;
                        
                        if(SelectionManager.Instance.currentlySelectedBroGameObject != null
                            && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
                            SelectionManager.Instance.currentlySelectedBroGameObject = null;
                        }
                        
                        state = BroState.OccupyingObject;
                    }
                }
            }
            else {
                state = BroState.Roaming;
            }
        }
    }
    
    public virtual void FightingLogic() {
    }
    
    public virtual void InAQueueLogic() {
        if(IsAtTargetPosition()) {
            if(skipLineQueue) {
                if(chooseRandomBathroomObjectOnSkipLineQueue) {
                    // Debug.Log("Setting target object!");
                    SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), new BathroomObjectType[] { BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal });
                }
                else {
                    // Debug.Log("Now Roaming!");
                    state = BroState.Roaming;
                }
                
                lineQueueIn = null;
            }
            else {
                // skip line queue isn't true and the bro should just wait here
            }
        }
    }
    
    public virtual void MovingToTargetObjectLogic() {
        ArrivalLogic();
    }
    
    public virtual void OccupyingObjectLogic() {
        GameObject targetObject = GetTargetObject();
        if(targetObject != null
            && targetObject.GetComponent<BathroomObject>() != null) {
            BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
            
            if(occupationTimer > occupationDuration[bathObjRef.type]) {
                // Debug.Log("occupation finished");
                if(bathObjRef.type == BathroomObjectType.Exit) {
                    ExitOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.HandDryer) {
                    HandDryerOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Sink) {
                    SinkOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Stall) {
                    StallOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Urinal) {
                    UrinalOccupationFinishedLogic();
                }
            }
            else {
                //disables the collider because the bro resides in the object, but the timer is still going
                colliderReference.enabled = false;
                
                occupationTimer += Time.deltaTime;
            }
        }
    }
    
    //===========================================================================
    public virtual void ReliefLogic(GameObject objectRelievedIn) {
        BathroomObject bathObjRef = objectRelievedIn.GetComponent<BathroomObject>();
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        hasRelievedSelf = true;
        if(chooseRandomBathroomObjectAfterRelieved) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    // public virtual void ReliefScore(GameObject objectOccupiedForRelief) {
    //     BathroomObject bathObjRef = objectOccupiedForRelief.GetComponent<BathroomObject>();
    
    //     broScoreLogic.BathroomObjectUsedScore();
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
    public virtual void WashHandsLogic(GameObject objectHandsWashedIn) {
        BathroomObject bathObjRef = objectHandsWashedIn.GetComponent<BathroomObject>();
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        hasWashedHands = true;
        if(chooseRandomBathroomObjectAfterWashedHands) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    // public virtual void WashHandsScore(GameObject objectHandsWashedIn) {
    //     BathroomObject bathObjRef = objectHandsWashedIn.GetComponent<BathroomObject>();
    //     broScoreLogic.BathroomObjectUsedScore();
    //     broScoreLogic.BrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
    // }
    //===========================================================================
    public virtual void DryHandsLogic(GameObject objectDriedHandsIn) {
        BathroomObject bathObjRef = objectDriedHandsIn.GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    // public virtual void DryHandsScore(GameObject objectDriedHandsIn) {
    //     BathroomObject bathObjRef = objectDriedHandsIn.GetComponent<BathroomObject>();
    
    //     broScoreLogic.BathroomObjectUsedScore();
    //     broScoreLogic.BrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
    // }
    //===========================================================================
    public virtual void ExitOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        broScoreLogic.ExitedScore();
        BroManager.Instance.RemoveBro(this.gameObject, false);
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(this.gameObject);
        Destroy(this.gameObject);
    }
    //===========================================================================
    public virtual void HandDryerOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        // Bathroom object is out of order
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee
                || reliefRequired == ReliefRequired.Poop
                || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Out of Order - has not relieved self");
                RelievedInOutOfOrderHandDryer();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Out of Order - has not washed hands");
                WashedHandsInOutOfOrderHandDryer();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Out of Order - has not dried hands");
                DriedHAndsInOutOfOrderHandDryer();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee
                || reliefRequired == ReliefRequired.Poop
                || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Broken - has not relieved self");
                RelievedInBrokenHandDryer();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Broken - has not washed hands");
                WashedHandsInBrokenHandDryer();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Broken - has not dried hands");
                DriedHandsInBrokenHandDryer();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee
                || reliefRequired == ReliefRequired.Poop
                || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Working - has not relieved self");
                RelievedInWorkingHandDryer();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Working - has not washed hands");
                WashedHandsInWorkingHandDryer();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Working - has not dried hands");
                DriedHandsInWorkingHandDryer();
            }
        }
    }
    
    //--------------------------
    // Hand Dryer  - Out of Order
    //--------------------------
    public virtual void RelievedInOutOfOrderHandDryer() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void WashedHandsInOutOfOrderHandDryer() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void DriedHAndsInOutOfOrderHandDryer() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    //--------------------------
    // Hand Dryer  - Working
    //--------------------------
    public virtual void RelievedInWorkingHandDryer() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void WashedHandsInWorkingHandDryer() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void DriedHandsInWorkingHandDryer() {
        GameObject targetObject = GetTargetObject();
        // BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
    }
    
    public virtual void RelievedInBrokenHandDryer() {
        Debug.Log("Bro is relieving himself in a broken hand dryer. This shouldn't have happened");
    }
    public virtual void WashedHandsInBrokenHandDryer() {
        Debug.Log("Bro is relieving himself in a broken hand dryer. This shouldn't have happened");
    }
    public virtual void DriedHandsInBrokenHandDryer() {
        Debug.Log("Bro is drying his hands in a broken hand dryer. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void SinkOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        // Bathroom object is out of order
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Out of Order - has not relieved self");
                RelievedInOutOfOrderSink();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Out of Order - has not washed hands");
                WashedHandsInOutOfOrderSink();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Out of Order - has not dried hands");
                DriedHandsInOutOfOrderSink();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Broken - has not relieved self");
                RelievedInBrokenSink();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Broken - has not washed hands");
                WashedHandsInBrokenSink();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Broken - has not dried hands");
                DriedHandsInBrokenSink();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Working - has not relieved self");
                RelievedInWorkingSink();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Working - has not washed hands");
                WashedHandsInWorkingSink();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Working - has not dried hands");
                DriedHandsInWorkingSink();
            }
        }
    }
    
    //--------------------------
    // Sink  - Out of Order
    //--------------------------
    public virtual void RelievedInOutOfOrderSink() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void WashedHandsInOutOfOrderSink() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void DriedHandsInOutOfOrderSink() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    //--------------------------
    // Sink  - Working
    //--------------------------
    public virtual void RelievedInWorkingSink() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void WashedHandsInWorkingSink() {
        GameObject targetObject = GetTargetObject();
        // BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
    }
    
    public virtual void DriedHandsInWorkingSink() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void RelievedInBrokenSink() {
        Debug.Log("Bro is relieving himself in a broken sink. This shouldn't have happened");
    }
    
    public virtual void WashedHandsInBrokenSink() {
        Debug.Log("Bro is washing his hands in a broken sink. This shouldn't have happened");
    }
    
    public virtual void DriedHandsInBrokenSink() {
        Debug.Log("Bro is drying his hands in a broken sink. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void StallOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Out of Order - has not relieved self");
                RelievedInOutOfOrderStall();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Out of Order - has not washed hands");
                WashedHandsInOutOfOrderStall();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Out of Order - has not dried hands");
                DriedHandsInOutOfOrderStall();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Broken - has not relieved self");
                RelievedInBrokenStall();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Broken - has not washed hands");
                WashedHandsInBrokenStall();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Broken - has not dried hands");
                DriedHandsInBrokenStall();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Working - has not relieved self");
                RelievedInWorkingStall();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Working - has not washed hands");
                WashedHandsInWorkingStall();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Working - has not dried hands");
                DriedHandsInWorkingStall();
            }
        }
    }
    //--------------------------
    // Stall  - OutOfOrder
    //--------------------------
    public virtual void RelievedInOutOfOrderStall() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    
    public virtual void WashedHandsInOutOfOrderStall() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    
    public virtual void DriedHandsInOutOfOrderStall() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    //--------------------------
    // Stall  - Working
    //--------------------------
    public virtual void RelievedInWorkingStall() {
        GameObject targetObject = GetTargetObject();
        // BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    public virtual void WashedHandsInWorkingStall() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void DriedHandsInWorkingStall() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void RelievedInBrokenStall() {
        Debug.Log("Bro is relieving himself in a broken stall. This shouldn't have happened");
    }
    public virtual void WashedHandsInBrokenStall() {
        Debug.Log("Bro is washing his hands in a broken stall. This shouldn't have happened");
    }
    public virtual void DriedHandsInBrokenStall() {
        Debug.Log("Bro is drying his hands in a broken stall. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void UrinalOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Out of Order - has not relieved self");
                RelievedInOutOfOrderUrinal();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Out of Order - has not washed hands");
                WashedHandsInOutOfOrderUrinal();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Out of Order - has not dried hands");
                DriedHandsInOutOfOrderUrinal();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Broken - has not relieved self");
                RelievedInBrokenUrinal();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Broken - has not washed hands");
                WashedHandsInBrokenUrinal();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Broken - has not dried hands");
                DriedHandsInBrokenUrinal();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                Debug.Log("Working - has not relieved self");
                RelievedInWorkingUrinal();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                Debug.Log("Working - has not washed hands");
                WashedHandsInWorkingUrinal();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                Debug.Log("Working - has not dried hands");
                DriedHandsInWorkingUrinal();
            }
        }
    }
    //--------------------------
    // Urinal - Out Of Order
    //--------------------------
    public virtual void RelievedInOutOfOrderUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public virtual void WashedHandsInOutOfOrderUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void DriedHandsInOutOfOrderUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    //--------------------------
    // Urinal - Working
    //--------------------------
    public virtual void RelievedInWorkingUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        ReliefLogic(targetObject);
        if(reliefRequired != ReliefRequired.Pee) {
            bathroomObject.state = BathroomObjectState.Broken;
        }
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public virtual void WashedHandsInWorkingUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        WashHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void DriedHandsInWorkingUrinal() {
        GameObject targetObject = GetTargetObject();
        BathroomObject bathroomObject = targetObject.GetComponent<BathroomObject>();
        DryHandsLogic(targetObject);
        bathroomObject.state = BathroomObjectState.Broken;
    }
    public virtual void RelievedInBrokenUrinal() {
        Debug.Log("Bro is relieving himself in a broken urinal. This shouldn't have happened");
    }
    public virtual void WashedHandsInBrokenUrinal() {
        Debug.Log("Bro is washing his hands in a broken urinal. This shouldn't have happened");
    }
    public virtual void DriedHandsInBrokenUrinal() {
        Debug.Log("Bro is drying his hands in a broken urinal. This shouldn't have happened");
    }
    
    //===========================================================================
    public virtual void RoamingLogic() {
        if(IsAtTargetPosition()) {
            // Debug.Log("setting roaming targetting!");
            GameObject randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
            
            // Debug.Log("Start Position X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y);
            BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
            // Debug.Log(AStarManager.Instance.GetListCopyOfAllClosedNodes());
            // Debug.Log(startTile);
            // Debug.Log(randomBathroomTile);
            List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                      AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                      startTile,
                                                                                      randomBathroomTile.GetComponent<BathroomTile>());
            SetTargetObjectAndTargetPosition(null, movementNodes);
        }
    }
    
    public virtual void StandingLogic() {
    }
    
    public virtual void StandOffLogic() {
        if(state == BroState.Standoff) {
            if(broFightingWith != null) {
                if(standoffBroGameObject == null
                    && broFightingWith.GetComponent<Bro>().standoffBroGameObject == null) {
                    Vector2 standoffAnchor = new Vector2(((this.gameObject.transform.position.x + broFightingWith.transform.position.x) / 2), ((this.gameObject.transform.position.y + broFightingWith.transform.position.y) / 2));
                    
                    standoffBroGameObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/StandoffBros") as GameObject);
                    standoffBroGameObject.GetComponent<StandoffBros>().StandoffBrosInit(this.gameObject, broFightingWith, standoffAnchor);
                    
                    BroManager.Instance.AddStandOffBros(standoffBroGameObject);
                }
            }
        }
    }
    
    public virtual void SpeechBubbleLogic() {
        if(reliefRequired == ReliefRequired.None
            && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.None) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.None;
        }
        else if(reliefRequired == ReliefRequired.DryHands
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.DryHands) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        }
        else if(reliefRequired == ReliefRequired.Pee
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.Pee) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.Pee;
        }
        else if(reliefRequired == ReliefRequired.Poop
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.Poop) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.Poop;
        }
        else if(reliefRequired == ReliefRequired.Vomit
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.None) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.None;
        }
        else if(reliefRequired == ReliefRequired.WashHands
                && speechBubbleReference.speechBubbleImage != SpeechBubbleImage.WashHands) {
            speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        }
    }
    
    public void SetRandomOpenBathroomObjectTarget(params BathroomObjectType[] bathroomObjectTypesToTarget) {
        BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                         this.gameObject.transform.position.y,
                                                                                         true).GetComponent<BathroomTile>();
                                                                                         
        GameObject randomObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
        if(randomObject != null) {
            BathroomTile randomObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomObject.transform.position.x,
                                                                                                      randomObject.transform.position.y,
                                                                                                      true).GetComponent<BathroomTile>();
            //Debug.Log("setting exit tile");
            List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                      AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                      broTile,
                                                                                      randomObjectTile);
            state = BroState.MovingToTargetObject;
            SetTargetObjectAndTargetPosition(randomObject, movementNodes);
        }
        else {
            state = BroState.Roaming;
        }
    }
    
    public void SetRandomBathroomObjectTarget(bool chooseOpenBathroomObject, List<GameObject> astarClosedNodesToUse, params BathroomObjectType[] bathroomObjectTypesToTarget) {
        BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                         this.gameObject.transform.position.y,
                                                                                         true).GetComponent<BathroomTile>();
        GameObject randomObject = null;
        if(chooseOpenBathroomObject) {
            randomObject = BathroomObjectManager.Instance.GetRandomOpenBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
        }
        else {
            randomObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
        }
        
        if(randomObject != null) {
            BathroomObject bathObjRef = randomObject.GetComponent<BathroomObject>();
            BathroomTile randomObjectTile = bathObjRef.GetBathroomTileIn().GetComponent<BathroomTile>();
            
            List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                      astarClosedNodesToUse,
                                                                                      broTile,
                                                                                      randomObjectTile);
            state = BroState.MovingToTargetObject;
            SetTargetObjectAndTargetPosition(randomObject, movementNodes);
        }
        else {
            state = BroState.Roaming;
        }
    }
    
    public void ExitBathroom() {
        SetRandomBathroomObjectTarget(false, AStarManager.Instance.GetListCopyOfPermanentClosedNodes(), BathroomObjectType.Exit);
    }
    
    public Vector2 GetOffsetFromCurrentTile() {
        if(targetPathingReference.targetTile != null) {
            return new Vector2(targetPathingReference.targetTile.transform.position.x - this.gameObject.transform.position.x,
                               targetPathingReference.targetTile.transform.position.y - this.gameObject.transform.position.y);
        }
        else {
            return Vector2.zero;
        }
    }
    
    public void SetPositionInWorldBasedOnCurrentTile(Vector2 tileOffset) {
        this.gameObject.transform.position = new Vector3(targetPathingReference.targetTile.transform.position.x + tileOffset.x,
                                                         targetPathingReference.targetTile.transform.position.y + tileOffset.y,
                                                         this.gameObject.transform.position.z);
    }
}
