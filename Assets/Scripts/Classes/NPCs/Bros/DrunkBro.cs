using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrunkBro : Bro {
    public bool vomitingFinished = false;
    
    public GameObject bathroomTileBlockerGeneratorGameObject = null;
    BathroomTileBlockerGenerator bathroomTileBlockerGenerator = null;
    
    protected override void Awake() {
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
            UpdateAnimator(targetPathingReference, animatorReference);
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
    }
    //--------------------------------------------------------------------------
    public override void DefaultArrivalLogic() {
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
                        speechBubbleReference.Hide();
                        
                        if(SelectionManager.Instance.currentlySelectedBroGameObject != null
                            && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
                            SelectionManager.Instance.currentlySelectedBroGameObject = null;
                        }
                        
                        state = BroState.OccupyingObject;
                        
                        //--------------------------------
                        // Stops the vomit generation from continuing
                        bathroomTileBlockerGenerator.enabled = false;
                    }
                }
            }
            else {
                state = BroState.Roaming;
            }
        }
    }
    //--------------------------------------------------------------------------
    public override void ReliefLogic(GameObject objectRelievedIn) {
        base.ReliefLogic(objectRelievedIn);
        SetRandomBathroomObjectTarget(true, AStarManager.Instance.GetListCopyOfAllClosedNodes(), BathroomObjectType.Exit);
    }
}
