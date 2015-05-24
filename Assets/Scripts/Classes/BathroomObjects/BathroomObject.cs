using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomObject : MonoBehaviour {
    public BathroomObjectType type = BathroomObjectType.None;
    public BathroomObjectState state = BathroomObjectState.None;
    public bool destroyObjectIfMoreThanTwoOccupants = true;
    
    public Animator animatorReference = null;
    public BathroomFacing bathroomFacingReference = null;
    public BoxCollider2D colliderReference = null;
    
    public Selectable selectableReference = null;
    
    //public bool isBroken = false;
    public float repairDuration = 2.0f;
    
    public int scoreValue = 0;
    
    public bool markOutOfOrderWhenOverUsed = false;
    public int timesUsed = 0;
    public int timesUsedNeededForOutOfOrder = 10;
    public int numberOfTaps = 0;
    public int numberOfTapsNeededToRestoreToOrder = 5;
    
    // This gets set in the bathroom tile manager singleton
    public GameObject bathroomTileIn = null;
    public List<GameObject> objectsOccupyingBathroomObject = new List<GameObject>();
    
    public virtual void Start() {
        animatorReference = this.gameObject.GetComponent<Animator>();
        bathroomFacingReference = this.gameObject.GetComponent<BathroomFacing>();
        
        selectableReference = this.gameObject.GetComponent<Selectable>();
        selectableReference.canBeSelected = true;
    }
    
    public virtual void Update() {
        MoreThanTwoOccupantsCheck();
        UpdateAnimator();
    }
    
    public virtual void UpdateAnimator() {
        bathroomFacingReference.UpdateAnimatorWithFacing(animatorReference);
        foreach(BathroomObjectState bathroomObjectState in BathroomObjectState.GetValues(typeof(BathroomObjectState))) {
            if(bathroomObjectState != BathroomObjectState.None) {
                animatorReference.SetBool(bathroomObjectState.ToString(), false);
            }
        }
        if(bathroomFacingReference.facing != Facing.None) {
            animatorReference.SetBool(bathroomFacingReference.facing.ToString(), true);
        }
        if(state != BathroomObjectState.None) {
            animatorReference.SetBool(state.ToString(), true);
        }
    }
    
    
    public virtual void OnMouseDown() {
        SelectionManager.Instance.currentlySelectedBathroomObject = this.gameObject;
        
        if(state == BathroomObjectState.OutOfOrder) {
            numberOfTaps++;
            // eeeehhhh probably take this out and make it its own function
            if(numberOfTaps >= numberOfTapsNeededToRestoreToOrder) {
                // Debug.Log("maxed out taps");
                state = BathroomObjectState.Idle;
                timesUsed = 0;
            }
        }
        // Debug.Log("Derp down");
    }
    
    public void ResetColliderAndSelectableReference() {
        GetComponent<Collider>().enabled = true;
        selectableReference.isSelected = false;
        selectableReference.canBeSelected = true;
    }
    
    public void SetColliderActive(bool isActive) {
        if(colliderReference != null) {
            colliderReference.enabled = isActive;
        }
    }
    
    public void AddBro(GameObject broGameObjectToAdd) {
        if(!objectsOccupyingBathroomObject.Contains(broGameObjectToAdd)) {
            objectsOccupyingBathroomObject.Add(broGameObjectToAdd);
        }
    }
    
    public GameObject GetBathroomTileIn() {
        return bathroomTileIn;
    }
    
    public void RemoveBro(GameObject broGameObjectToRemove) {
        objectsOccupyingBathroomObject.Remove(broGameObjectToRemove);
    }
    
    public void RemoveBroAndIncrementUsedCount(GameObject broGameObjectToRemove) {
        objectsOccupyingBathroomObject.Remove(broGameObjectToRemove);
        
        bool wasOutOfOrderBeforeRemoval = (state == BathroomObjectState.OutOfOrder);
        IncrementTimesUsed();
        bool isOutOfOrderAfterRemoval  = (state == BathroomObjectState.OutOfOrder);
        
        if(!wasOutOfOrderBeforeRemoval
            && isOutOfOrderAfterRemoval) {
            Bro broRef = broGameObjectToRemove.GetComponent<Bro>();
            ScoreManager.Instance.GetPlayerScoreTracker().BroCausedOutOfOrderInBathroomObjectScore(broRef.type, type);
        }
    }
    
    public void EjectBros() {
        foreach(GameObject broGameObject in objectsOccupyingBathroomObject) {
            Bro broRef = broGameObject.GetComponent<Bro>();
            
            broRef.state = BroState.Roaming;
            broRef.speechBubbleReference.displaySpeechBubble = true;
            broRef.SetTargetObject(null);
        }
    }
    
    public void IncrementTimesUsed() {
        timesUsed++;
        OutOfOrderCheck();
    }
    
    public bool IsBroken() {
        if(state == BathroomObjectState.Broken
            || state == BathroomObjectState.BrokenByPee
            || state == BathroomObjectState.BrokenByPoop) {
            return true;
        }
        else {
            return false;
        }
    }
    
    public void OutOfOrderCheck() {
        if(markOutOfOrderWhenOverUsed) {
            if(timesUsed >= timesUsedNeededForOutOfOrder) {
                if(!IsBroken()) {
                    numberOfTaps = 0;
                    state = BathroomObjectState.OutOfOrder;
                }
            }
        }
    }
    
    public void TriggerOutOfOrderState() {
    }
    
    /// <summary>
    /// This is an interesting method as it iterates over the list of objects
    /// and creates fighting bros as it iterates. Once iterated, over it removes
    /// occupants and sends any odd numbered amount of bros to the bathroom exit
    /// </summary>
    public void MoreThanTwoOccupantsCheck() {
        if(objectsOccupyingBathroomObject.Count >= 2
            && destroyObjectIfMoreThanTwoOccupants
            && type != BathroomObjectType.Exit) {
            GameObject firstBroFound = null;
            GameObject secondBroFound = null;
            // List<GameObject> objectOccupyingBathroomObjectToRemove = new List<GameObject>();
            for(int i = 0; i < objectsOccupyingBathroomObject.Count; i++) {
                GameObject gameObj = objectsOccupyingBathroomObject[i];
                
                if(gameObj.GetComponent<Bro>() != null) {
                    if(firstBroFound == null) {
                        firstBroFound = gameObj;
                    }
                    else {
                        secondBroFound = gameObj;
                    }
                }
                // if you're at the end of the list and no alt bro exists
                // TODO!! - Change this logic to send him to the exit.
                if(i == objectsOccupyingBathroomObject.Count - 1) {
                    if(firstBroFound == null
                        && secondBroFound == null) {
                        Bro broRef = gameObj.GetComponent<Bro>();
                        broRef.state = BroState.Roaming;
                        broRef.selectableReference.Reset();
                        broRef.SetRandomOpenBathroomObjectTarget(BathroomObjectType.Exit);
                        // objectOccupyingBathroomObjectToRemove.add(gameObj);
                    }
                }
                if(firstBroFound != null
                    && secondBroFound != null) {
                    state = BathroomObjectState.Broken;
                    
                    firstBroFound.SetActive(false);
                    Bro firstBroFoundReference = firstBroFound.GetComponent<Bro>();
                    firstBroFoundReference.state = BroState.Fighting;
                    firstBroFoundReference.selectableReference.Reset();
                    
                    secondBroFound.SetActive(false);
                    Bro secondBroFoundReference = secondBroFound.GetComponent<Bro>();
                    secondBroFoundReference.state = BroState.Fighting;
                    secondBroFoundReference.selectableReference.Reset();
                    
                    GameObject newFightingBros = Factory.Instance.GenerateFightingBroGameObject();
                    newFightingBros.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newFightingBros.transform.position.z);
                    newFightingBros.GetComponent<FightingBros>().brosFighting.Add(firstBroFound);
                    newFightingBros.GetComponent<FightingBros>().brosFighting.Add(secondBroFound);
                    
                    BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(newFightingBros.transform.position.x, newFightingBros.transform.position.y, true).GetComponent<BathroomTile>();
                    BathroomTile targetTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
                    newFightingBros.GetComponent<TargetPathing>().SetTargetObjectAndTargetPosition(null,
                                                                                                   AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                                                                            AStarManager.Instance. GetListCopyOfAllClosedNodes(),
                                                                                                                                            startTile,
                                                                                                                                            targetTile));
                                                                                                                                            
                    firstBroFound = null;
                    secondBroFound = null;
                    
                    ScoreManager.Instance.GetPlayerScoreTracker().BroBathroomObjectBrokenByFightingScore(firstBroFoundReference.type, type);
                    ScoreManager.Instance.GetPlayerScoreTracker().BroStartedFightScore(firstBroFoundReference.type);
                    ScoreManager.Instance.GetPlayerScoreTracker().BroStartedFightScore(secondBroFoundReference.type);
                }
            }
            objectsOccupyingBathroomObject.Clear();
        }
    }
}