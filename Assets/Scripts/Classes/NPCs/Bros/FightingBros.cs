using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingBros : BaseBehavior {
    public TargetPathing targetPathingReference = null;
    public List<GameObject> brosFighting;
    public int currentNumberOfTaps = 0;
    public int numberOfTapsNeededToBreakUp = 5;
    public bool isPaused = false;
    
    protected override void Awake() {
        base.Awake();
        
        brosFighting = new List<GameObject>();
        
        targetPathingReference.AddOnArrivalAtMovementNodeLogic(OnArrivalAtMovementNode);
    }
    
    // Use this for initialization
    public void Start() {
        this.gameObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
        
        PerformStartedFightScore();
    }
    
    // Update is called once per frame
    public void Update() {
        if(!isPaused) {
            PerformLogic();
        }
    }
    public void Pause() {
        isPaused = true;
    }
    public void Unpause() {
        isPaused = false;
    }
    
    public void OnMouseDown() {
        if(!isPaused) {
            currentNumberOfTaps++;
        }
    }
    
    public void PerformLogic() {
        PerformFightingBroArrivalLogic();
        PerformMaxTapLogic();
    }
    
    public void OnArrivalAtMovementNode() {
        GameObject bathroomTileIn = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x,
                                                                                              this.gameObject.transform.position.y,
                                                                                              false);
        if(bathroomTileIn != null) {
            BathroomTile bathroomTileInRef = bathroomTileIn.GetComponent<BathroomTile>();
            if(bathroomTileInRef != null) {
                if(bathroomTileInRef.bathroomObjectInTile != null) {
                    BathroomObject bathroomObjectInTileRef = bathroomTileInRef.bathroomObjectInTile.GetComponent<BathroomObject>();
                    if(bathroomObjectInTileRef.type != BathroomObjectType.Exit
                        && !bathroomObjectInTileRef.IsBroken()) {
                        // Debug.Log("Breaking " + bathroomTileInRef.bathroomObjectInTile.name +  " in " + bathroomTileIn.name);
                        bathroomObjectInTileRef.state = BathroomObjectState.Broken;
                    }
                }
            }
        }
    }
    
    // TODO: Fix to use the jagged array access instead
    public void PerformFightingBroArrivalLogic() {
        if(targetPathingReference.IsAtTargetPosition()) {
            BathroomTile currentBathroomTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
            // BathroomTile nextBathroomTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
            BathroomTile nextBathroomTile = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(BathroomObjectType.HandDryer, BathroomObjectType.Sink, BathroomObjectType.Stall, BathroomObjectType.Urinal).GetComponent<BathroomObject>().bathroomTileIn.GetComponent<BathroomTile>();
            foreach(GameObject bathroomObject in BathroomObjectManager.Instance.allBathroomObjects) {
                BathroomTile tileBathroomObjectIsIn = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomObject.transform.position.x, bathroomObject.transform.position.y, true).GetComponent<BathroomTile>();
                
                if(bathroomObject.GetComponent<BathroomObject>() != null) {
                    if(tileBathroomObjectIsIn.tileX == currentBathroomTile.tileX
                        && tileBathroomObjectIsIn.tileY == currentBathroomTile.tileY) {
                        BathroomObject bathObjRef = bathroomObject.GetComponent<BathroomObject>();
                        if(bathObjRef.type != BathroomObjectType.Exit
                            && bathObjRef.state != BathroomObjectState.Broken
                            && bathObjRef.state != BathroomObjectState.BrokenByPee
                            && bathObjRef.state != BathroomObjectState.BrokenByPoop) {
                            bathObjRef.state = BathroomObjectState.Broken;
                            bathObjRef.EjectBros();
                            
                            foreach(GameObject broGameObject in brosFighting) {
                                Bro broReference = broGameObject.GetComponent<Bro>();
                                ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBathroomObjectBrokenByFightingScore(broReference.type, bathObjRef.type);
                            }
                        }
                    }
                }
            }
            // Debug.Log("next node x: " + nextBathroomTile.gameObject.transform.position.x + " y: " + nextBathroomTile.gameObject.transform.position.y);
            // Debug.Log("next node: " + nextBathroomTile.gameObject.name);
            List<GameObject> newMovementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                         AStarManager.Instance.GetListCopyOfPermanentClosedNodes(),
                                                                                         currentBathroomTile,
                                                                                         nextBathroomTile);
            SetTargetObjectAndTargetPosition(null, newMovementNodes);
        }
    }
    
    public void PerformMaxTapLogic() {
        if(currentNumberOfTaps >= numberOfTapsNeededToBreakUp) {
            // goes before logic because it iterates over the bros fighting already
            PerformStoppedFightScore();
            
            foreach(GameObject broGameObj in brosFighting) {
                Bro broReference = broGameObj.GetComponent<Bro>();
                broReference.baseProbabilityOfFightOnCollisionWithBro = 0f;
                broReference.state = BroState.MovingToTargetObject;
                broGameObj.transform.position = this.gameObject.transform.position;
                broGameObj.SetActive(true);
                
                List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);
                int selectedExit = Random.Range(0, exits.Count);
                GameObject exitSelected = exits[selectedExit];
                // Exit exitSelected = ExitManager.Instance.SelectRandomExit().GetComponent<Exit>();
                BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
                BathroomTile targetTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(exitSelected.transform.position.x, exitSelected.transform.position.y, true).GetComponent<BathroomTile>();
                List<GameObject> newMovementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                             AStarManager.Instance.GetListCopyOfPermanentClosedNodes(),
                                                                                             startTile,
                                                                                             targetTile);
                                                                                             
                float xRadiusOffset = BathroomTileMap.Instance.singleTileWidth / 2;
                float yRadiusOffset = BathroomTileMap.Instance.singleTileHeight / 2;
                float xPositionOffset = Random.Range(-xRadiusOffset, xRadiusOffset);
                float yPositionOffset = Random.Range(-yRadiusOffset, yRadiusOffset);
                broGameObj.transform.position = new Vector3(this.gameObject.transform.position.x + xPositionOffset,
                                                            this.gameObject.transform.position.y + yPositionOffset,
                                                            broGameObj.transform.position.z);
                broGameObj.GetComponent<Bro>().colliderReference.enabled = false;
                broReference.reliefRequired = ReliefRequired.None;
                broReference.targetPathingReference.disableMovementLogic = false;
                broReference.SetTargetObjectAndTargetPosition(exitSelected, newMovementNodes);
                broReference.speechBubbleReference.displaySpeechBubble = false;
                IsometricDisplay isoDisplay = broGameObj.GetComponent<IsometricDisplay>();
                if(isoDisplay != null) {
                    isoDisplay.UpdateDisplayPosition();
                }
                ManagedSortingLayer managedSortingLayer = broGameObj.GetComponent<ManagedSortingLayer>();
                if(managedSortingLayer != null) {
                    managedSortingLayer.PerformSortingLogic();
                }
            }
            BroManager.Instance.RemoveFightingBro(this.gameObject, true);
            // Destroy(this.gameObject);
        }
    }
    
    public void PerformStartedFightScore() {
        foreach(GameObject broGameObject in brosFighting) {
            ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedFightScore(broGameObject.GetComponent<Bro>().type);
        }
    }
    public void PerformStoppedFightScore() {
        foreach(GameObject broGameObject in brosFighting) {
            ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStoppedFightScore(broGameObject.GetComponent<Bro>().type);
        }
    }
    
    public virtual void SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
        targetPathingReference.SetTargetObjectAndTargetPosition(newTargetObject, newMovementNodes);
    }
}
