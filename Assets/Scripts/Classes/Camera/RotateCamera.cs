/// <summary>
/// I am so ashamed of this rotation rework to 2D. It is so bad :(
/// So... So, bad...
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TO DO: FIX ROTATE LOGIC SCRIPT SO THAT WHEN IT ROTATES IT BASES THE CAMERA'S DIRECTION BEING LOOKED IT USES WORLD COORDINATES TO CALCULATE IT... OR SOMETHING
public class RotateCamera : MonoBehaviour {
    public GameObject cameraGameObject = null;
    public float amountRotated = 0f;
    public Facing directionBeingLookedAt = Facing.None;
    
    // Use this for initialization
    void Start() {
        // Sets the rotated view correctly
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
    }
    
    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            RotateLeft();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            RotateRight();
        }
        amountRotated = amountRotated % 360;
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsTargeting(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, bool returnClosestTile) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            if(targetPathing) {
                GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(targetPathing.targetPosition.x,
                                                                                                     targetPathing.targetPosition.y,
                                                                                                     returnClosestTile);
                tileIn[gameObj] = tileOccupying;
            }
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsInByIndex(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, int tileX, int tileY) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByIndex(tileX, tileY);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsIn(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, bool returnClosestTile) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(gameObj.transform.position.x,
                                                                                                 gameObj.transform.position.y,
                                                                                                 returnClosestTile);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsInStandoffAnchor(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, bool returnClosestTile) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            Vector3 positionToUse = new Vector3(gameObj.GetComponent<StandoffBros>().standoffAnchor.x,
                                                gameObj.GetComponent<StandoffBros>().standoffAnchor.y,
                                                gameObj.transform.position.z);
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(positionToUse.x,
                                                                                                 positionToUse.y,
                                                                                                 returnClosestTile);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsInStandoffRadiusOne(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, bool returnClosestTile) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            Vector3 positionToUse = new Vector3(gameObj.GetComponent<StandoffBros>().broOneRadiusPosition.x,
                                                gameObj.GetComponent<StandoffBros>().broOneRadiusPosition.y,
                                                gameObj.transform.position.z);
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(positionToUse.x,
                                                                                                 positionToUse.y,
                                                                                                 returnClosestTile);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsInStandoffRadiusTwo(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn, bool returnClosestTile) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            Vector3 positionToUse = new Vector3(gameObj.GetComponent<StandoffBros>().broTwoRadiusPosition.x,
                                                gameObj.GetComponent<StandoffBros>().broTwoRadiusPosition.y,
                                                gameObj.transform.position.z);
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(positionToUse.x,
                                                                                                 positionToUse.y,
                                                                                                 returnClosestTile);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }
    
    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffsetBaseOnCurrentPosition(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            positionToOffsetFrom = gameObj.transform.position;
            GetGameObjectsTileOffset(gameObj, positionToOffsetFrom, tileContainingGameObject, tileOffset, rotatingLeft, rotatingRight, useTargetPathingVector);
        }
        return tileOffset;
    }
    
    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffsetBasedOnTargetPathing(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            positionToOffsetFrom = gameObj.GetComponent<TargetPathing>().targetPosition;
            GetGameObjectsTileOffset(gameObj, positionToOffsetFrom, tileContainingGameObject, tileOffset, rotatingLeft, rotatingRight, useTargetPathingVector);
        }
        return tileOffset;
    }
    
    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffsetBasedOnStandoffAnchor(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            // positionToOffsetFrom = gameObj.GetComponent<TargetPathing>().targetPosition;
            positionToOffsetFrom = new Vector3(gameObj.GetComponent<StandoffBros>().standoffAnchor.x,
                                               gameObj.GetComponent<StandoffBros>().standoffAnchor.y,
                                               gameObj.transform.position.z);
            GetGameObjectsTileOffset(gameObj, positionToOffsetFrom, tileContainingGameObject, tileOffset, rotatingLeft, rotatingRight, useTargetPathingVector);
        }
        return tileOffset;
    }
    
    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffsetBasedOnStandoffRadiusOne(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            // positionToOffsetFrom = gameObj.GetComponent<TargetPathing>().targetPosition;
            positionToOffsetFrom = new Vector3(gameObj.GetComponent<StandoffBros>().broOneRadiusPosition.x,
                                               gameObj.GetComponent<StandoffBros>().broOneRadiusPosition.y,
                                               gameObj.transform.position.z);
            GetGameObjectsTileOffset(gameObj, positionToOffsetFrom, tileContainingGameObject, tileOffset, rotatingLeft, rotatingRight, useTargetPathingVector);
        }
        return tileOffset;
    }
    
    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffsetBasedOnStandoffRadiusTwo(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            // positionToOffsetFrom = gameObj.GetComponent<TargetPathing>().targetPosition;
            positionToOffsetFrom = new Vector3(gameObj.GetComponent<StandoffBros>().broTwoRadiusPosition.x,
                                               gameObj.GetComponent<StandoffBros>().broTwoRadiusPosition.y,
                                               gameObj.transform.position.z);
            GetGameObjectsTileOffset(gameObj, positionToOffsetFrom, tileContainingGameObject, tileOffset, rotatingLeft, rotatingRight, useTargetPathingVector);
        }
        return tileOffset;
    }
    
    public void GetGameObjectsTileOffset(GameObject gameObj, Vector3 positionToOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        if(rotatingLeft) {
            switch(directionBeingLookedAt) {
            case(Facing.Top):
                // Debug.Log("ROTATING LEFT - LOOKING TOP SIDE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                   -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                break;
            case(Facing.Right):
                // Debug.Log("ROTATING LEFT - LOOKING RIGHT SIDE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                   -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                                                   
                break;
            case(Facing.Bottom):
                // Debug.Log("ROTATING LEFT - LOOKING BOTTOM SIDE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                   -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                break;
            case(Facing.Left):
                // Debug.Log("ROTATING LEFT - LOOKING LEFT SIDE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                   -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                break;
            default:
                Debug.LogError("YOU REALLY SHOULDN'T BE SETTING THE VALUE HERE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x,
                                                   tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y);
                break;
            }
        }
        if(rotatingRight) {
            switch(directionBeingLookedAt) {
            case(Facing.Top):
                // Debug.Log("ROTATING RIGHT - LOOKING TOP SIDE.");
                tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                   tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                break;
            case(Facing.Right):
                // Debug.Log("ROTATING RIGHT - LOOKING RIGHT SIDE.");
                tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                   tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                                                   
                break;
            case(Facing.Bottom):
                // Debug.Log("ROTATING RIGHT - LOOKING BOTTOM SIDE.");
                tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                   tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                break;
            case(Facing.Left):
                // Debug.Log("ROTATING RIGHT - LOOKING LEFT SIDE.");
                tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                   tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                break;
            default:
                Debug.LogError("YOU REALLY SHOULDN'T BE SETTING THE VALUE HERE.");
                tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x,
                                                   tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y);
                break;
            }
        }
    }
    
    public void UpdateBathroomDirectionFacing() {
        UpdateRotationFacing();
        UpdateDirectionFacingBasedOnCamera(BathroomTileBlockerManager.Instance.bathroomTileBlockers, amountRotated);
        UpdateDirectionFacingBasedOnCamera(BathroomObjectManager.Instance.allBathroomObjects, amountRotated);
        UpdateDirectionFacingBasedOnCamera(BroManager.Instance.allBros, amountRotated);
        UpdateDirectionFacingBasedOnCamera(BroManager.Instance.allFightingBros, amountRotated);
        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            UpdateDirectionFacingBasedOnCamera(lineQueue.GetComponent<LineQueue>().queueTileObjects, amountRotated);
        }
        UpdateDirectionFacingBasedOnCamera(SceneryManager.Instance.GetScenery(), amountRotated);
    }
    
    public void HideBathroomIfUnderDiagonal() {
        HideObjectsIfUnderDiagonal(BathroomTileBlockerManager.Instance.bathroomTileBlockers, null);
        HideObjectsIfUnderDiagonal(BathroomObjectManager.Instance.allBathroomObjects, null);
        HideObjectsIfUnderDiagonal(BroManager.Instance.allBros, null);
        HideObjectsIfUnderDiagonal(BroManager.Instance.allStandoffBros, null);
        HideObjectsIfUnderDiagonal(BroManager.Instance.allFightingBros, null);
        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            HideObjectsIfUnderDiagonal(lineQueue.GetComponent<LineQueue>().queueTileObjects, null);
        }
        HideObjectsIfUnderDiagonal(SceneryManager.Instance.GetScenery(), null);
    }
    
    
    // THIS WAS GRABBED FROM HERE!!!
    // http://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
    // TODO! WARNING!!! I think this still needs to be fixed, but I'm not quite sure how. For congruent tile maps the corner
    // // tiles will be displayed, however on incongruent tile maps there will be on corner that is detected wrong
    // // To fix this you will need to make it so that point A and b are calculated on some hypothetical congruent tile map based
    // // on the incongruent indices. This will then create the perception of an equivalent check
    public void HideObjectsIfUnderDiagonal(List<GameObject> gameObjectsToSetOffset, Dictionary<GameObject, GameObject> tileContainingGameObjectDict) {
        if(tileContainingGameObjectDict == null) {
            tileContainingGameObjectDict = new Dictionary<GameObject, GameObject>();
        }
        
        foreach(GameObject gameObj in gameObjectsToSetOffset) {
            IsometricDisplay[] IsometricDisplays = gameObj.GetComponentsInChildren<IsometricDisplay>(true);
            foreach(IsometricDisplay isometricDisplay in IsometricDisplays) {
                if(isometricDisplay.hideUnderDiagonal) {
                    Vector3 pointToCheck = isometricDisplay.gameObjectToAnchorTo.transform.position;
                    
                    // This is the top left corner of the tile map
                    Vector3 pointA = BathroomTileMap.Instance.GetTileGameObjectByIndex(0, BathroomTileMap.Instance.tilesHigh - 1).transform.position;
                    // This is the bottom right corner of the tile map
                    Vector3 pointB = BathroomTileMap.Instance.GetTileGameObjectByIndex(BathroomTileMap.Instance.tilesWide - 1, 0).transform.position;
                    
                    // position = sign((Bx-Ax)*(Y-Ay) - (By-Ay)*(X-Ax))
                    float position = (((pointB.x - pointA.x) * (pointToCheck.y - pointA.y)) - ((pointB.y - pointA.y) * (pointToCheck.x - pointA.x)));
                    bool gameObjIsAboveDiagonal = false;
                    
                    // It is 0 on the line, and +1 on one side, -1 on the other side.
                    if(position == 0) {
                        // Debug.Log(isometricDisplay.gameObjectToAnchorTo.name + " is on the line.");
                        gameObjIsAboveDiagonal = true;
                    }
                    else if(position > 0) {
                        // Debug.Log(isometricDisplay.gameObjectToAnchorTo.name + " is above the line.");
                        gameObjIsAboveDiagonal = true;
                    }
                    else if(position < 0) {
                        // Debug.Log(isometricDisplay.gameObjectToAnchorTo.name + " is below the line.");
                        gameObjIsAboveDiagonal = false;
                    }
                    
                    if(gameObjIsAboveDiagonal) {
                        isometricDisplay.ShowSpritesBeingManaged();
                    }
                    else {
                        isometricDisplay.HideSpritesBeingManaged();
                    }
                }
            }
        }
    }
    
    public void SetGameObjectOffsets(List<GameObject> gameObjectsToSetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetOffset) {
            Vector2 tileOffsetValues = Vector2.zero;
            tileOffsetValues = tileOffset[gameObj];
            
            gameObj.transform.position = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                     tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                     gameObj.transform.position.z);
                                                     
        }
    }
    
    public void SetGameObjectTargetPositionOffsets(List<GameObject> gameObjectsToSetTargetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetTargetOffset) {
            TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            if(targetPathing != null) {
                Vector2 tileOffsetValues = Vector2.zero;
                tileOffsetValues = tileOffset[gameObj];
                targetPathing.targetPosition = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                           tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                           targetPathing.targetPosition.z);
            }
        }
    }
    
    public void SetGameObjectStandoffAnchorOffsets(List<GameObject> gameObjectsToSetTargetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetTargetOffset) {
            // TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            StandoffBros standoffBros = gameObj.GetComponent<StandoffBros>();
            if(standoffBros != null) {
                Vector2 tileOffsetValues = Vector2.zero;
                tileOffsetValues = tileOffset[gameObj];
                standoffBros.standoffAnchor = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                          tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                          gameObj.transform.position.z);
            }
        }
    }
    
    public void SetGameObjectStandoffRadiusOneOffsets(List<GameObject> gameObjectsToSetTargetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetTargetOffset) {
            // TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            StandoffBros standoffBros = gameObj.GetComponent<StandoffBros>();
            if(standoffBros != null) {
                Vector2 tileOffsetValues = Vector2.zero;
                tileOffsetValues = tileOffset[gameObj];
                standoffBros.broOneRadiusPosition = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                                tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                                gameObj.transform.position.z);
            }
        }
    }
    
    public void SetGameObjectStandoffRadiusTwoOffsets(List<GameObject> gameObjectsToSetTargetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetTargetOffset) {
            // TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            StandoffBros standoffBros = gameObj.GetComponent<StandoffBros>();
            if(standoffBros != null) {
                Vector2 tileOffsetValues = Vector2.zero;
                tileOffsetValues = tileOffset[gameObj];
                standoffBros.broTwoRadiusPosition = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                                tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                                gameObj.transform.position.z);
            }
        }
    }
    
    public List<GameObject> GetUpdatedTargetPathingMovementNodes(List<GameObject> movementNodes) {
        List<GameObject> newMovementNodes = new List<GameObject>();
        for(int i = 0; i < movementNodes.Count; i++) {
            GameObject newMovementNode = BathroomTileMap.Instance.GetTileGameObjectByIndex(movementNodes[i].GetComponent<BathroomTile>().tileX, movementNodes[i].GetComponent<BathroomTile>().tileY);
            // [i]
            newMovementNodes.Add(newMovementNode);
        }
        return newMovementNodes;
    }
    
    public void UpdateTargetPathingMovementNodes(List<GameObject> gameObjectsToUpdate) {
        foreach(GameObject gameObjectToUpdate in gameObjectsToUpdate) {
            TargetPathing targetPathingRef = gameObjectToUpdate.GetComponent<TargetPathing>();
            targetPathingRef.movementNodes = GetUpdatedTargetPathingMovementNodes(targetPathingRef.movementNodes);
        }
    }
    
    public void UpdateBathroomTileMapIndexes() {
        GameObject[][] bathroomTileMap = BathroomTileMap.Instance.GetTiles();
        for(int j = 0; j < BathroomTileMap.Instance.tilesHigh; j++) {
            for(int i = 0; i < BathroomTileMap.Instance.tilesWide; i++) {
                // done this way because in retrieving it the swap due to the rotation is already taken into consideration
                GameObject currentTileGameObject = bathroomTileMap[j][i];
                BathroomTile currentBathroomTile = currentTileGameObject.GetComponent<BathroomTile>();
                
                currentBathroomTile.tileX = i;
                currentBathroomTile.tileY = j;
            }
        }
    }
    
    public void UpdateDisplayPositions(List<GameObject> gameObjects) {
        foreach(GameObject gameObj in gameObjects) {
            IsometricDisplay isoDisplay = gameObj.GetComponent<IsometricDisplay>();
            if(isoDisplay != null) {
                isoDisplay.UpdateDisplayPosition();
            }
            ManagedSortingLayer managedSortingLayer = gameObj.GetComponent<ManagedSortingLayer>();
            if(managedSortingLayer != null) {
                managedSortingLayer.SortingLogic();
            }
        }
    }
    
    public void RotateLeft() {
        Rotate(true, false);
    }
    public void RotateRight() {
        Rotate(false, true);
    }
    
    public void UpdateRotationFacing() {
        amountRotated = amountRotated % 360;
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
    }
    
    // This is so bad... but it works so just deal with it for now
    // Refactor this someday, never
    public void Rotate(bool rotateRight, bool rotateLeft) {
        //=========================================================================================
        // Current Positions
        Dictionary<GameObject, GameObject> tileGameObjectIn = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> gameObjectTileOffset = new Dictionary<GameObject, Vector2>();
        //--------------------
        // Target Pathing
        Dictionary<GameObject, GameObject> tileGameObjectTargeting = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> targetTileOffset = new Dictionary<GameObject, Vector2>();
        //--------------------
        // Standoff Bros Anchor Position
        Dictionary<GameObject, GameObject> standoffBroTileIn = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> standoffBroTileOffset = new Dictionary<GameObject, Vector2>();
        // Standoff Bros Radius One Position
        Dictionary<GameObject, GameObject> standoffBroTileInRadiusOne = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> standoffBroRadiusOneTileOffset = new Dictionary<GameObject, Vector2>();
        // Standoff Bros Radius Two Position
        Dictionary<GameObject, GameObject> standoffBroTileInRadiusTwo = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> standoffBroRadiusTwoTileOffset = new Dictionary<GameObject, Vector2>();
        //-------------------
        // Bathroom Tile Blockers
        GetTilesGameObjectsIn(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, true);
        GetGameObjectsTileOffsetBaseOnCurrentPosition(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        // Bros
        GetTilesGameObjectsIn(BroManager.Instance.allBros, tileGameObjectIn, true);
        GetGameObjectsTileOffsetBaseOnCurrentPosition(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        
        GetTilesGameObjectsTargeting(BroManager.Instance.allBros, tileGameObjectTargeting, true);
        GetGameObjectsTileOffsetBasedOnTargetPathing(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        //-------------------
        // Standoff Bros
        GetTilesGameObjectsInStandoffAnchor(BroManager.Instance.allStandoffBros, standoffBroTileIn, true);
        GetGameObjectsTileOffsetBasedOnStandoffAnchor(BroManager.Instance.allStandoffBros, standoffBroTileIn, standoffBroTileOffset, rotateLeft, rotateRight, false);
        
        GetTilesGameObjectsInStandoffRadiusOne(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusOne, true);
        GetGameObjectsTileOffsetBasedOnStandoffRadiusOne(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusOne, standoffBroRadiusOneTileOffset, rotateLeft, rotateRight, false);
        
        GetTilesGameObjectsInStandoffRadiusTwo(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusTwo, true);
        GetGameObjectsTileOffsetBasedOnStandoffRadiusTwo(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusTwo, standoffBroRadiusTwoTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        // Fighting Bros
        GetTilesGameObjectsIn(BroManager.Instance.allFightingBros, tileGameObjectIn, true);
        GetGameObjectsTileOffsetBaseOnCurrentPosition(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        
        GetTilesGameObjectsTargeting(BroManager.Instance.allFightingBros, tileGameObjectTargeting, true);
        GetGameObjectsTileOffsetBasedOnTargetPathing(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        //-------------------
        // Bathroom Objects
        GetTilesGameObjectsIn(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, true);
        GetGameObjectsTileOffsetBaseOnCurrentPosition(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        // LineQueues
        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            GetTilesGameObjectsIn(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, true);
            GetGameObjectsTileOffsetBaseOnCurrentPosition(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        }
        //-------------------
        // Scenery
        // Resets relative to the first cell of the tile map becuase I don't think I need to do it based on the tile it's actually in.... not really sure.
        // GetTilesGameObjectsInByIndex(SceneryManager.Instance.GetScenery(), tileGameObjectIn, 0, 0);
        GetTilesGameObjectsIn(SceneryManager.Instance.GetScenery(), tileGameObjectIn, true);
        GetGameObjectsTileOffsetBaseOnCurrentPosition(SceneryManager.Instance.GetScenery(), tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //=====================================================================
        if(rotateLeft) {
            // Debug.Log("rotating left");
            amountRotated += 90;
            BathroomTileMap.Instance.RotateTileMatrixLeft();
        }
        if(rotateRight) {
            // Debug.Log("rotating right");
            amountRotated += -90;
            BathroomTileMap.Instance.RotateTileMatrixRight();
        }
        UpdateRotationFacing();
        
        UpdateBathroomTileMapIndexes();
        
        SetGameObjectOffsets(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BathroomTileBlockerManager.Instance.bathroomTileBlockers);
        UpdateDirectionFacingBasedOnCamera(BathroomTileBlockerManager.Instance.bathroomTileBlockers, amountRotated);
        // HideObjectsIfUnderDiagonal(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn);
        
        SetGameObjectOffsets(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BathroomObjectManager.Instance.allBathroomObjects);
        UpdateDirectionFacingBasedOnCamera(BathroomObjectManager.Instance.allBathroomObjects, amountRotated);
        // HideObjectsIfUnderDiagonal(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn);
        
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectOffsets(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BroManager.Instance.allBros);
        UpdateDirectionFacingBasedOnCamera(BroManager.Instance.allBros, amountRotated);
        // HideObjectsIfUnderDiagonal(BroManager.Instance.allBros, tileGameObjectIn);
        
        SetGameObjectStandoffAnchorOffsets(BroManager.Instance.allStandoffBros, standoffBroTileIn, standoffBroTileOffset);
        SetGameObjectStandoffRadiusOneOffsets(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusOne, standoffBroRadiusOneTileOffset);
        SetGameObjectStandoffRadiusTwoOffsets(BroManager.Instance.allStandoffBros, standoffBroTileInRadiusTwo, standoffBroRadiusTwoTileOffset);
        // UpdateDisplayPositions(BroManager.Instance.allStandoffBros);
        // HideObjectsIfUnderDiagonal(BroManager.Instance.allStandoffBros, tileGameObjectIn);
        
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectOffsets(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BroManager.Instance.allFightingBros);
        UpdateDirectionFacingBasedOnCamera(BroManager.Instance.allFightingBros, amountRotated);
        // HideObjectsIfUnderDiagonal(BroManager.Instance.allFightingBros, tileGameObjectIn);
        
        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            SetGameObjectOffsets(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, gameObjectTileOffset);
            UpdateDisplayPositions(lineQueue.GetComponent<LineQueue>().queueTileObjects);
            UpdateDirectionFacingBasedOnCamera(lineQueue.GetComponent<LineQueue>().queueTileObjects, amountRotated);
            // HideObjectsIfUnderDiagonal(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn);
        }
        
        SetGameObjectOffsets(SceneryManager.Instance.GetScenery(), tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(SceneryManager.Instance.GetScenery());
        UpdateDirectionFacingBasedOnCamera(SceneryManager.Instance.GetScenery(), amountRotated);
        // HideObjectsIfUnderDiagonal(SceneryManager.Instance.GetScenery(), tileGameObjectIn);
        HideBathroomIfUnderDiagonal();
    }
    
    public Facing GetDirectionBeingLookedAt(float amountRotatedAround) {
        Facing dirBeingLookedAt = Facing.None;
        
        if((amountRotatedAround >= 0 && amountRotatedAround < 90)
            || (amountRotatedAround > -90 && amountRotatedAround <= 0)) {
            // Debug.Log("Top");
            dirBeingLookedAt = Facing.Top;
        }
        else if((amountRotatedAround >= 90 && amountRotatedAround < 180)
                || (amountRotatedAround > -360 && amountRotatedAround <= -270)) {
            // Debug.Log("Right");
            dirBeingLookedAt = Facing.Right;
        }
        else if((amountRotatedAround >= 270 && amountRotatedAround < 360)
                || (amountRotatedAround > -180 && amountRotatedAround <= -90)) {
            // Debug.Log("Left");
            dirBeingLookedAt = Facing.Left;
        }
        else if((amountRotatedAround >= 180 && amountRotatedAround < 270)
                || (amountRotatedAround > -270 && amountRotatedAround <= -180)) {
            // Debug.Log("Bottom");
            dirBeingLookedAt = Facing.Bottom;
        }
        
        return dirBeingLookedAt;
    }
    
    // Updates the rotated object to match the camera's facing
    public void UpdateDirectionFacingBasedOnCamera(List<GameObject> gameObjects, float cameraRotation) {
        foreach(GameObject gameObj in gameObjects) {
            UpdateDirectionFacingBasedOnCamera(gameObj, cameraRotation);
        }
    }
    
    public void UpdateDirectionFacingBasedOnCamera(GameObject gameObjToUpdate, float cameraRotation) {
        BathroomFacing bathroomFacingReference = gameObjToUpdate.GetComponent<BathroomFacing>();
        if(bathroomFacingReference != null) {
            // Debug.Log(gameObjToUpdate.name + " not null");
            // bathroomFacingReference.facing = cameraDirectionBeingLookedAt;
            float calculatedRotation = (cameraRotation - bathroomFacingReference.rotationOffset) % 360;
            // Debug.Log(gameObjToUpdate.name + " rotation: " + calculatedRotation);
            Facing directionBeingLookedAtToSet = GetDirectionBeingLookedAt(calculatedRotation);
            // Debug.Log(gameObjToUpdate.name + " direction: " + directionBeingLookedAtToSet.ToString());
            bathroomFacingReference.facing = directionBeingLookedAtToSet;
        }
    }
    
    public Facing GetDirectionFacingBasedOnCameraAndMovementDirection(bool movingUp, bool movingRight, bool movingDown, bool movingLeft) {
        // Debug.Log("Moving Up: " + movingUp + "\nMoving Right: " + movingRight + "\nMoving Down: " + movingDown + "\nMoving Left: " + movingLeft);
        
        Facing cameraDirectionBeingLookedAt = directionBeingLookedAt;
        Facing directionToReturn = Facing.None;
        if(movingRight) {
            // moving up right
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.Top;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.Right;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.Left;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.Bottom;
                    break;
                default:
                    directionToReturn = Facing.TopRight;
                    break;
                }
            }
            // moving down right
            else if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.Right;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.Bottom;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.Top;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.Left;
                    break;
                default:
                    directionToReturn = Facing.Right;
                    break;
                }
            }
            // moving right
            else {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.TopRight;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.BottomRight;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.TopLeft;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.BottomLeft;
                    break;
                default:
                    directionToReturn = Facing.Right;
                    break;
                }
            }
        }
        else if(movingLeft) {
            // moving up left
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.Left;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.Top;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.Bottom;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.Right;
                    break;
                default:
                    directionToReturn = Facing.TopLeft;
                    break;
                }
            }
            // moving down left
            else if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.Bottom;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.Left;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.Top;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.Right;
                    break;
                default:
                    directionToReturn = Facing.BottomLeft;
                    break;
                }
            }
            // moving left
            else {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.BottomLeft;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.TopLeft;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.BottomRight;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.TopRight;
                    break;
                default:
                    directionToReturn = Facing.Left;
                    break;
                }
            }
        }
        else {
            // moving up
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.TopLeft;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.TopRight;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.BottomLeft;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.BottomRight;
                    break;
                default:
                    directionToReturn = Facing.Top;
                    break;
                }
            }
            // moving down
            if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                case(Facing.TopRight):
                    directionToReturn = Facing.BottomRight;
                    break;
                case(Facing.TopLeft):
                    directionToReturn = Facing.BottomLeft;
                    break;
                case(Facing.BottomRight):
                    directionToReturn = Facing.TopRight;
                    break;
                case(Facing.BottomLeft):
                    directionToReturn = Facing.TopLeft;
                    break;
                default:
                    directionToReturn = Facing.Bottom;
                    break;
                }
            }
        }
        return directionToReturn;
    }
}
