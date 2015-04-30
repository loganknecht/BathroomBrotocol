using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bro : BaseBehavior {
    public TargetPathing targetPathingReference = null;
    public Animator animatorReference = null;
    public BathroomFacing bathroomFacing;
    public HighlightSelectable selectableReference = null;
    public SpeechBubble speechBubbleReference = null;
    public IsometricDisplay isometricDisplayReference = null;
    public BoxCollider2D colliderReference = null;
    public DrawNodeList drawNodeList = null;
    
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
        base.Awake();
    }
    
    // Use this for initialization
    public virtual void Start() {
        // base.Start();
        InitializeOccupationDuration();
        InitializeComponents();
        
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
    }
    
    // Update is called once per frame
    public virtual void Update() {
        if(!isPaused) {
            PerformSpeechBubbleLogic();
            PerformFightTimerLogic();
            PerformLogic();
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
        isometricDisplayReference.UpdateDisplayPosition();
    }
    
    public virtual void InitializeOccupationDuration() {
        if(occupationDuration == null) {
            float defaultOccupationDuration = 2f;
            
            occupationDuration = new Dictionary<BathroomObjectType, float>();
            
            occupationDuration[BathroomObjectType.Exit] = 0;
            occupationDuration[BathroomObjectType.HandDryer] = defaultOccupationDuration;
            occupationDuration[BathroomObjectType.Sink] = defaultOccupationDuration;
            occupationDuration[BathroomObjectType.Stall] = defaultOccupationDuration;
            occupationDuration[BathroomObjectType.Urinal] = defaultOccupationDuration;
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
    
    // public override void PopMovementNode() {
    //     if(movementNodes.Count > 0) {
    //         //Debug.Log("Arrived at: " + targetPosition.x + ", " + targetPosition.y);
    //         GameObject currentBroTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, false);
    //         BathroomTile currentBroTile = null;
    //         if(currentBroTileGameObject != null) {
    //             currentBroTile = currentBroTileGameObject.GetComponent<BathroomTile>();
    //         }
    
    //         GameObject nextNode = movementNodes[0];
    //         BathroomTile nextTile = nextNode.GetComponent<BathroomTile>();
    
    //         if((nextTile != null
    //                 && (nextTile.bathroomTileBlockers.Count == 0 || currentBroTile == nextTile))
    //             || (state == BroState.MovingToTargetObject
    //                 && targetObject != null
    //                 && targetObject.GetComponent<BathroomObject>() != null
    //                 && targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit)) {
    //             targetPosition = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, this.transform.position.z);
    //             // Debug.Log("Set new position to: " + targetPosition.x + ", " + targetPosition.y);
    //             movementNodes.RemoveAt(0);
    //             // Destroy(nextNode);
    //             // Debug.Log(this.gameObject.name + " has " + movementNodes.Count + " number of movemeNodes");
    //         }
    //         else {
    //             movementNodes.Clear();
    //             state = BroState.Roaming;
    //         }
    //     }
    //     if(movementNodes == null) {
    //         Debug.Log("movemeNodes is null for " + this.gameObject.name);
    //     }
    // }
    
    // Returns the base probablity of fighting plus the modifier based on the score
    public virtual float GetFightProbability() {
        if(modifyBroFightProbablityUsingScoreRatio) {
            return (baseProbabilityOfFightOnCollisionWithBro); //TODO: GET SCORETRACKER TO RETURN THE MODIFIER BASE ON SCORE TRACKER'S PERFECT SCORE RATIO
        }
        else {
            return baseProbabilityOfFightOnCollisionWithBro;
        }
    }
    public virtual void PerformFightTimerLogic() {
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
            //------------------------------------------------------------------
            // base.UpdateAnimator();
            //------------------------------------------------------------------
            
            // Sets bathroom facing state flags
            if(state != BroState.Standing
                && targetPathingReference.directionBeingLookedAt != Facing.None) {
                bathroomFacing.facing = targetPathingReference.directionBeingLookedAt;
            }
            bathroomFacing.UpdateAnimatorWithFacing(animatorReference);
            
            //------------------------------------------------------------------
            // animatorReference.SetBool(BroState.Fighting.ToString(), false);
            // animatorReference.SetBool(BroState.InAQueue.ToString(), false);
            // animatorReference.SetBool(BroState.MovingToTargetObject.ToString(), false);
            // animatorReference.SetBool(BroState.OccupyingObject.ToString(), false);
            // animatorReference.SetBool(BroState.Roaming.ToString(), false);
            // animatorReference.SetBool(BroState.Standing.ToString(), false);
            // animatorReference.SetBool(BroState.Standoff.ToString(), false);
            //------------------------------------------------------------------
            //Sets bro state flags
            foreach(BroState broState in BroState.GetValues(typeof(BroState))) {
                animatorReference.SetBool(broState.ToString(), false);
            }
            animatorReference.SetBool(state.ToString(), true);
            
            animatorReference.SetBool("None", false);
        }
    }
    
    public virtual void PerformLogic() {
        switch(state) {
        case(BroState.None):
            break;
        case(BroState.Fighting):
            break;
        case(BroState.InAQueue):
            PerformInAQueueLogic();
            break;
        case(BroState.MovingToTargetObject):
            PerformMovingToTargetObjectLogic();
            break;
        case(BroState.OccupyingObject):
            PerformOccupyingObjectLogic();
            break;
        case(BroState.Roaming):
            PerformRoamingLogic();
            break;
        case(BroState.Standing):
            PerformStandingLogic();
            break;
        case(BroState.Standoff):
            PerformStandOffLogic();
            break;
        default:
            break;
        }
    }
    
    public virtual void PerformArrivalLogic() {
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
                    
                    if(bathObjRef.IsBroken()) {
                        state = BroState.Roaming;
                    }
                    else {
                        if(bathObjRef.objectsOccupyingBathroomObject.Count > 0
                            && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit
                            && startRoamingOnArrivalAtBathroomObjectInUse) {
                            state = BroState.Roaming;
                        }
                        else {
                            PerformOnArrivalBrotocolScoreCheck();
                            
                            //Adds bro to occupation list
                            bathObjRef.AddBro(this.gameObject);
                            
                            selectableReference.canBeSelected = false;
                            selectableReference.ResetHighlightObjectAndSelectedState();
                            speechBubbleReference.displaySpeechBubble = false;
                            
                            if(SelectionManager.Instance.currentlySelectedBroGameObject != null
                                && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
                                SelectionManager.Instance.currentlySelectedBroGameObject = null;
                            }
                            
                            state = BroState.OccupyingObject;
                        }
                    }
                }
            }
            else {
                if(targetObject != null
                    && targetObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
                    // Do not roam on move to exit? That's weird though....
                    // Should be removed by default... so moving to exit
                    // shouldn't matter?
                }
                else {
                    Debug.Log("lol starting to roam");
                    state = BroState.Roaming;
                }
            }
        }
    }
    
    public virtual void PerformFightingLogic() {
    }
    
    public virtual void PerformInAQueueLogic() {
        // PerformMovementLogic();
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
    public virtual void PerformMovingToTargetObjectLogic() {
        // PerformMovementLogic();
        PerformArrivalLogic();
    }
    
    public virtual void PerformOccupyingObjectLogic() {
        GameObject targetObject = GetTargetObject();
        if(targetObject != null
            && targetObject.GetComponent<BathroomObject>() != null) {
            BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();
            
            if(occupationTimer > occupationDuration[bathObjRef.type]) {
                // Debug.Log("occupation finished");
                if(bathObjRef.type == BathroomObjectType.Exit) {
                    PerformExitOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.HandDryer) {
                    PerformHandDryerOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Sink) {
                    PerformSinkOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Stall) {
                    PerformStallOccupationFinishedLogic();
                }
                else if(bathObjRef.type == BathroomObjectType.Urinal) {
                    PerformUrinalOccupationFinishedLogic();
                }
            }
            else {
                //disables the collider because the bro resides in the object, but the timer is still going
                colliderReference.enabled = false;
                
                occupationTimer += Time.deltaTime;
            }
        }
    }
    
    public virtual void PerformExitOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        PerformExitedScore();
        BroManager.Instance.RemoveBro(this.gameObject, false);
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(this.gameObject);
        Destroy(this.gameObject);
    }
    
    //===========================================================================
    public virtual void PerformHandDryerOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        // Bathroom object is out of order
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee
                || reliefRequired == ReliefRequired.Poop
                || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Out of Order - has not relieved self");
                PerformOutOfOrderHandDryerRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Out of Order - has not washed hands");
                PerformOutOfOrderHandDryerWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Out of Order - has not dried hands");
                PerformOutOfOrderHandDryerDryHands();
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
                // Debug.Log("Broken - has not relieved self");
                PerformBrokenHandDryerRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Broken - has not washed hands");
                PerformBrokenHandDryerWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Broken - has not dried hands");
                PerformBrokenHandDryerDryHands();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee
                || reliefRequired == ReliefRequired.Poop
                || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Working - has not relieved self");
                PerformWorkingHandDryerRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Working - has not washed hands");
                PerformWorkingHandDryerWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Working - has not dried hands");
                PerformWorkingHandDryerDryHands();
            }
        }
    }
    
    public virtual void PerformOutOfOrderHandDryerRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            // PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            // PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            // PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        
        if(chooseRandomBathroomObjectAfterRelieved) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    
    public virtual void PerformOutOfOrderHandDryerWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    
    public virtual void PerformOutOfOrderHandDryerDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformWorkingHandDryerRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        
        if(chooseRandomBathroomObjectAfterRelieved) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    
    public virtual void PerformWorkingHandDryerWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokenHandDryerWashHands();
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    public virtual void PerformWorkingHandDryerDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        PerformBathroomObjectUsedScore();
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformBrokenHandDryerRelief() {
        Debug.Log("Bro is relieving himself in a broken hand dryer. This shouldn't have happened");
    }
    public virtual void PerformBrokenHandDryerWashHands() {
        Debug.Log("Bro is washing his hands in a broken hand dryer. This shouldn't have happened");
    }
    public virtual void PerformBrokenHandDryerDryHands() {
        Debug.Log("Bro is drying his hands in a broken hand dryer. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void PerformSinkOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        // Bathroom object is out of order
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Out of Order - has not relieved self");
                PerformOutOfOrderSinkRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Out of Order - has not washed hands");
                PerformOutOfOrderSinkWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Out of Order - has not dried hands");
                PerformOutOfOrderSinkDryHands();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Broken - has not relieved self");
                PerformBrokenSinkRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Broken - has not washed hands");
                PerformBrokenSinkWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Broken - has not dried hands");
                PerformBrokenSinkDryHands();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Working - has not relieved self");
                PerformWorkingSinkRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Working - has not washed hands");
                PerformWorkingSinkWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Working - has not dried hands");
                PerformWorkingSinkDryHands();
            }
        }
    }
    
    public virtual void PerformOutOfOrderSinkRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            // PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            // PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            bathObjRef.state = BathroomObjectState.Broken;
            // PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
            PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        }
        
        if(chooseRandomBathroomObjectAfterRelieved) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    
    public virtual void PerformOutOfOrderSinkWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    
    public virtual void PerformOutOfOrderSinkDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    
    public virtual void PerformWorkingSinkRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.Broken;
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            // do nothing
        }
        
        if(chooseRandomBathroomObjectAfterRelieved) {
            SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        }
        else {
            state = BroState.Roaming;
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.WashHands;
        reliefRequired = ReliefRequired.WashHands;
    }
    
    public virtual void PerformWorkingSinkWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    
    public virtual void PerformWorkingSinkDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByDryingHandsBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    
    public virtual void PerformBrokenSinkRelief() {
        Debug.Log("Bro is relieving himself in a broken sink. This shouldn't have happened");
    }
    
    public virtual void PerformBrokenSinkWashHands() {
        Debug.Log("Bro is washing his hands in a broken sink. This shouldn't have happened");
    }
    
    public virtual void PerformBrokenSinkDryHands() {
        Debug.Log("Bro is drying his hands in a broken sink. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void PerformStallOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Out of Order - has not relieved self");
                PerformOutOfOrderStallRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Out of Order - has not washed hands");
                PerformOutOfOrderStallWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Out of Order - has not dried hands");
                PerformOutOfOrderStallDryHands();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Broken - has not relieved self");
                PerformBrokenStallRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Broken - has not washed hands");
                PerformBrokenStallWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Broken - has not dried hands");
                PerformBrokenStallDryHands();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Working - has not relieved self");
                PerformWorkingStallRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Working - has not washed hands");
                PerformWorkingStallWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Working - has not dried hands");
                PerformWorkingStallDryHands();
            }
        }
    }
    
    public virtual void PerformOutOfOrderStallRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        
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
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    
    public virtual void PerformOutOfOrderStallWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    
    public virtual void PerformOutOfOrderStallDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterDriedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformWorkingStallRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
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
        
        SoundManager.Instance.Play(AudioType.Flush1);
    }
    public virtual void PerformWorkingStallWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByWashingHandsBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        
        reliefRequired = ReliefRequired.DryHands;
    }
    public virtual void PerformWorkingStallDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByDryingHandsBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterDriedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = false;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformBrokenStallRelief() {
        Debug.Log("Bro is relieving himself in a broken stall. This shouldn't have happened");
    }
    public virtual void PerformBrokenStallWashHands() {
        Debug.Log("Bro is washing his hands in a broken stall. This shouldn't have happened");
    }
    public virtual void PerformBrokenStallDryHands() {
        Debug.Log("Bro is drying his hands in a broken stall. This shouldn't have happened");
    }
    //===========================================================================
    public virtual void PerformUrinalOccupationFinishedLogic() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        if(bathObjRef.state == BathroomObjectState.OutOfOrder) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Out of Order - has not relieved self");
                PerformOutOfOrderUrinalRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Out of Order - has not washed hands");
                PerformOutOfOrderUrinalWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Out of Order - has not dried hands");
                PerformOutOfOrderUrinalDryHands();
            }
        }
        // Bathroom object is broken
        else if(bathObjRef.state == BathroomObjectState.Broken
                || bathObjRef.state == BathroomObjectState.BrokenByPee
                || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Broken - has not relieved self");
                PerformBrokenUrinalRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Broken - has not washed hands");
                PerformBrokenUrinalWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Broken - has not dried hands");
                PerformBrokenUrinalDryHands();
            }
        }
        // Bathroom object is not broken or out of order
        else {
            // Fall through logic for the bro bathroom lifecycle
            if(reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit) {
                // Debug.Log("Working - has not relieved self");
                PerformWorkingUrinalRelief();
            }
            else if(reliefRequired == ReliefRequired.WashHands) {
                // Debug.Log("Working - has not washed hands");
                PerformWorkingUrinalWashHands();
            }
            else if(reliefRequired == ReliefRequired.DryHands) {
                // Debug.Log("Working - has not dried hands");
                PerformWorkingUrinalDryHands();
            }
        }
    }
    public virtual void PerformOutOfOrderUrinalRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Pee) {
            bathObjRef.state = BathroomObjectState.BrokenByPee;
            PerformBrokeByPeeingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
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
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public virtual void PerformOutOfOrderUrinalWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    public virtual void PerformOutOfOrderUrinalDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByOutOfOrderUseBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterDriedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformWorkingUrinalRelief() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasRelievedSelf = true;
        
        PerformBathroomObjectUsedScore();
        if(reliefRequired == ReliefRequired.Poop) {
            bathObjRef.state = BathroomObjectState.BrokenByPoop;
            PerformBrokeByPoopingBathroomObjectScore(bathObjRef.type);
        }
        else if(reliefRequired == ReliefRequired.Vomit) {
            PerformBrokeByVomitingBathroomObjectScore(bathObjRef.type);
        }
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
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
        
        SoundManager.Instance.Play(AudioType.Flush2);
    }
    public virtual void PerformWorkingUrinalWashHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasWashedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByWashingHandsBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterWashedHands) {
        //   SetRandomBathroomObjectTarget(true, BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = true;
        speechBubbleReference.displaySpeechBubble = true;
        speechBubbleReference.speechBubbleImage = SpeechBubbleImage.DryHands;
        reliefRequired = ReliefRequired.DryHands;
    }
    public virtual void PerformWorkingUrinalDryHands() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        
        hasDriedHands = true;
        
        PerformBathroomObjectUsedScore();
        PerformBrokeByDryingHandsBathroomObjectScore(bathObjRef.type);
        
        bathObjRef.state = BathroomObjectState.Broken;
        bathObjRef.RemoveBroAndIncrementUsedCount(this.gameObject);
        
        // if(chooseRandomBathroomObjectAfterDriedHands) {
        //   SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal);
        // }
        // else {
        //   state = BroState.Roaming;
        // }
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
        colliderReference.enabled = true;
        selectableReference.canBeSelected = false;
        speechBubbleReference.displaySpeechBubble = false;
        reliefRequired = ReliefRequired.None;
    }
    public virtual void PerformBrokenUrinalRelief() {
        Debug.Log("Bro is relieving himself in a broken urinal. This shouldn't have happened");
    }
    public virtual void PerformBrokenUrinalWashHands() {
        Debug.Log("Bro is washing his hands in a broken urinal. This shouldn't have happened");
    }
    public virtual void PerformBrokenUrinalDryHands() {
        Debug.Log("Bro is drying his hands in a broken urinal. This shouldn't have happened");
    }
    
    //===========================================================================
    public virtual void PerformRoamingLogic() {
        // PerformMovementLogic();
        
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
    
    public virtual void PerformStandingLogic() {
    }
    
    public virtual void PerformStandOffLogic() {
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
    
    public virtual void PerformSpeechBubbleLogic() {
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
    // public void SetRandomBathroomObjectTarget(bool chooseOpenBathroomObject, params BathroomObjectType[] bathroomObjectTypesToTarget) {
    //     SetRandomBathroomObjectTarget(chooseOpenBathroomObject, AStarManager.Instance.GetListCopyOfAllClosedNodes(), bathroomObjectTypesToTarget);
    // }
    
    public void SetRandomBathroomObjectTarget(bool chooseOpenBathroomObject, List<GameObject> astarClosedNodesToUse, params BathroomObjectType[] bathroomObjectTypesToTarget) {
        BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                         this.gameObject.transform.position.y,
                                                                                         true).GetComponent<BathroomTile>();
        // Debug.Log(broTile);
        
        // List<GameObject> objects = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(bathroomObjectTypesToTarget);
        // int selectedObject = Random.Range(0, objects.Count);
        // GameObject randomObject = objects[selectedObject];
        GameObject randomObject = null;
        if(chooseOpenBathroomObject) {
            randomObject = BathroomObjectManager.Instance.GetRandomOpenBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
        }
        else {
            randomObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(bathroomObjectTypesToTarget);
        }
        
        if(randomObject != null) {
            // BathroomTile randomObjectTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(randomObject.transform.position.x,
            //                                                                                           randomObject.transform.position.y,
            //                                                                                           true).GetComponent<BathroomTile>();
            // BathroomTile randomObjectTile = randomObject.GetComponent<BathroomObject>().GetBathroomTileIn().GetComponent<BathroomTile>();
            BathroomObject bathObjRef = randomObject.GetComponent<BathroomObject>();
            BathroomTile randomObjectTile = bathObjRef.GetBathroomTileIn().GetComponent<BathroomTile>();
            
            List<GameObject> movementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                      astarClosedNodesToUse,
                                                                                      broTile,
                                                                                      randomObjectTile);
            state = BroState.MovingToTargetObject;
            SetTargetObjectAndTargetPosition(randomObject, movementNodes);
            
            // Debug.Log("number of movementNodes: " + movementNodes.Count);
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
        // return tileMapToSetPositions;
    }
    
    public void UpdateAllTilesIsometricDisplay() {
        // return tilesToUpdate;
    }
    //----------------------------------------------------------------------------
    // Brotocol Score Logic Goes Here
    //----------------------------------------------------------------------------
    //This is being checked on arrival before switching to occupying an object
    public virtual void PerformOnArrivalBrotocolScoreCheck() {
        GameObject targetObject = GetTargetObject();
        if(targetObject != null
            && targetObject.GetComponent<BathroomObject>() != null
            && targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
            if(CheckIfBroHasCorrectReliefTypeForTargetObject()) {
                // increment correct relief type
            }
            if(!CheckIfBroInAdjacentBathroomObjects()) {
                // increment bro alone bonus
            }
        }
    }
    
    // This is so dumb to document this
    /// <summary>This checks to see if the bro's target object is a bathroom object,
    /// and that their relief type matches the correct bathroom object type.
    /// </summary>
    /// <returns>True if target object is a bathroom object, and if reliefRequired
    /// matches the correct bathroom object.</returns>
    public virtual bool CheckIfBroHasCorrectReliefTypeForTargetObject() {
        BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
        if(hasRelievedSelf == false
            // && (reliefRequired == ReliefRequired.Pee || reliefRequired == ReliefRequired.Vomit)
            && (reliefRequired == ReliefRequired.Pee)
            && bathObjRef != null
            && bathObjRef.type == BathroomObjectType.Urinal) {
            return true;
        }
        else if(hasRelievedSelf == false
                && (reliefRequired == ReliefRequired.Poop || reliefRequired == ReliefRequired.Vomit)
                && bathObjRef != null
                && bathObjRef.type == BathroomObjectType.Stall) {
            return true;
        }
        else if(hasWashedHands == false
                && (reliefRequired == ReliefRequired.WashHands)
                && bathObjRef != null
                && bathObjRef.type == BathroomObjectType.Sink) {
            return true;
        }
        else {
            return false;
        }
    }
    
    // Returns true if any of the eight tiles around the bro has a bathroom object,
    // and if that bathroom object has a bro in it
    public virtual bool CheckIfBroInAdjacentBathroomObjects() {
        // bool broIsInAjdacentTile = false;
        
        // BathroomTile currentTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
        //                                                                                                                     this.gameObject.transform.position.y,
        //                                                                                                                                 true).GetComponent<BathroomTile>();
        
        //  bool isBroOnTopLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY + 1);
        //  bool isBroOnTopSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX, currentTile.tileY + 1);
        //  bool isBroOnTopRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY + 1);
        
        //  bool isBroOnLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY);
        //  bool isBroOnRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY);
        
        //  bool isBroOnBottomLeftSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX - 1, currentTile.tileY - 1);
        //  bool isBroOnBottomSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX, currentTile.tileY - 1);
        //  bool isBroOnBottomRightSide = BathroomTileMap.Instance.CheckIfTileContainsBroInBathroomObject(currentTile.tileX + 1, currentTile.tileY - 1);
        //  // Debug.Log("------------------------");
        //  // Debug.Log("On Top Left: " + isBroOnTopLeftSide);
        //  // Debug.Log("On Top: " + isBroOnTopSide);
        //  // Debug.Log("On Top Right: " + isBroOnTopRightSide);
        //  // Debug.Log("On Left: " + isBroOnLeftSide);
        //  // Debug.Log("On Right: " + isBroOnRightSide);
        //  // Debug.Log("On Bottom Left: " + isBroOnBottomLeftSide);
        //  // Debug.Log("On Bottom: " + isBroOnBottomSide);
        //  // Debug.Log("On Bottom Right: " + isBroOnBottomRightSide);
        
        //  if(isBroOnTopLeftSide
        //    || isBroOnTopSide
        //    || isBroOnTopRightSide
        //    || isBroOnLeftSide
        //    || isBroOnRightSide
        //    || isBroOnBottomLeftSide
        //    || isBroOnBottomSide
        //    || isBroOnBottomRightSide) {
        //    // Debug.Log("Bro adjacent");
        //    broIsInAjdacentTile = true;
        //  }
        
        // return broIsInAjdacentTile;
        return false;
    }
    
    public virtual bool CheckIfRelievedSelfBeforeTimeOut() {
        Debug.Log("WARNING YOU HAVE CALLED THE BASE BRO METHOD 'CheckIfRelievedSelfBeforeTimeOut' BUT THE BASE BRO CLASS DOES NOT SUPPORT THIS METHOD AND IS MEANT TO BE EXTENDED");
        return false;
    }
    
    public virtual bool CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry() {
        Debug.Log("WARNING YOU HAVE CALLED THE BASE BRO METHOD 'CheckIfRelievedSelfInCorrectBathroomObjectTypeOnFirstTry' BUT THE BASE BRO CLASS DOES NOT SUPPORT THIS METHOD AND IS MEANT TO BE EXTENDED");
        return false;
    }
    
    //=========================================================================
    // This assumes it's called at the correct point and that the target object and relief required are accessible
    public virtual void PerformBathroomObjectUsedScore() {
        if(GetTargetObject() != null) {
            BathroomObject bathObjRef = GetTargetObject().GetComponent<BathroomObject>();
            if(bathObjRef != null) {
                if(bathObjRef.type == BathroomObjectType.HandDryer) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        // PerformWashedHandsInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
                        PerformDriedHandsInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInBathoomObjectScore(bathObjRef.type);
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Sink) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInBathoomObjectScore(bathObjRef.type);
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Stall) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInBathoomObjectScore(bathObjRef.type);
                    }
                }
                else if(bathObjRef.type == BathroomObjectType.Urinal) {
                    if(reliefRequired == ReliefRequired.DryHands) {
                        PerformDriedHandsInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Pee) {
                        PerformRelievedPeeInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Poop) {
                        PerformRelievedPoopInBathoomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.Vomit) {
                        PerformRelievedVomitInBathroomObjectScore(bathObjRef.type);
                    }
                    else if(reliefRequired == ReliefRequired.WashHands) {
                        PerformWashedHandsInBathoomObjectScore(bathObjRef.type);
                    }
                }
            }
        }
    }
    
    //=========================================================================
    public virtual void PerformEnteredScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroEnteredScore(type);
    }
    public virtual void PerformExitedScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroExitedScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformStartedStandoffScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedStandoffScore(type);
    }
    public virtual void PerformStoppedStandoffScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStoppedStandoffScore(type);
    }
    public virtual void PerformStartedFightScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedFightScore(type);
    }
    public virtual void PerformStoppedFightScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStoppedFightScore(type);
    }
    //--------------------------------------------------------------------
    public virtual void PerformDriedHandsInBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformDriedHandsInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformRelievedPeeInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformRelievedPeeInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformRelievedPoopInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformRelievedPoopInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformRelievedVomitInBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformRelievedVomitInBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformWashedHandsInBathoomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformWashedHandsInBathroomObjectScore(type, bathroomObjectType);
    }
    //---------
    public virtual void PerformBrokeByOutOfOrderUseBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByOutOfOrderUseBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByPeeingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByPeeingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByPoopingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByPoopingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByVomitingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByVomitingBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByWashingHandsBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByWashingHandsBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByDryingHandsBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByDryingHandsBathroomObjectScore(type, bathroomObjectType);
    }
    public virtual void PerformBrokeByFightingBathroomObjectScore(BathroomObjectType bathroomObjectType) {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBrokeByFightingBathroomObjectScore(type, bathroomObjectType);
    }
    //-------------------------------------------------------------------------
    public virtual void PerformSatisfiedBrotocolNoAdjacentBrosScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroSatisfiedBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void PerformTotalPossibleBrotocolNoAdjacentBrosScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroTotalPossibleBrotocolNoAdjacentBrosScore(type);
    }
    public virtual void PerformSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroSatisfiedBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    public virtual void PerformTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore() {
        ScoreManager.Instance.GetPlayerScoreTracker().PerformBroTotalPossibleBrotocolRelievedInCorrectObjectOnFirstTryScore(type);
    }
    //=========================================================================
}
