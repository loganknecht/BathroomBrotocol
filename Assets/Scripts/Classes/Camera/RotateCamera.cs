﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TO DO: FIX ROTATE LOGIC SCRIPT SO THAT WHEN IT ROTATES IT BASES THE CAMERA'S DIRECTION BEING LOOKED IT USES WORLD COORDINATES TO CALCULATE IT... OR SOMETHING
public class RotateCamera : MonoBehaviour {
    public GameObject cameraGameObject = null;
    public GameObject objectToRotateAround = null;
    public float amountRotated = 0f;
    public DirectionBeingLookedAt directionBeingLookedAt = DirectionBeingLookedAt.None;

    // Use this for initialization
    void Start () {
        // Sets the rotated view correctly
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
        RotateBathroomToMatchCamera();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
          RotateLeft();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)) {
          RotateRight();
        }
        amountRotated = amountRotated%360;
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

    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffset(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight, bool useTargetPathingVector) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            Vector3 positionToOffsetFrom = Vector3.zero;
            if(useTargetPathingVector) {
                positionToOffsetFrom = gameObj.GetComponent<TargetPathing>().targetPosition;
            }
            else {
                positionToOffsetFrom = gameObj.transform.position;
            }
            if(rotatingLeft) {
                switch(directionBeingLookedAt) {
                    case(DirectionBeingLookedAt.Top):
                        // Debug.Log("ROTATING LEFT - LOOKING TOP SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                    break;
                    case(DirectionBeingLookedAt.Right):
                        // Debug.Log("ROTATING LEFT - LOOKING RIGHT SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));

                    break;
                    case(DirectionBeingLookedAt.Bottom):
                        // Debug.Log("ROTATING LEFT - LOOKING BOTTOM SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x));
                    break;
                    case(DirectionBeingLookedAt.Left):
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
                    case(DirectionBeingLookedAt.Top):
                        // Debug.Log("ROTATING RIGHT - LOOKING TOP SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                    break;
                    case(DirectionBeingLookedAt.Right):
                        // Debug.Log("ROTATING RIGHT - LOOKING RIGHT SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);

                    break;
                    case(DirectionBeingLookedAt.Bottom):
                        // Debug.Log("ROTATING RIGHT - LOOKING BOTTOM SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - positionToOffsetFrom.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - positionToOffsetFrom.x);
                    break;
                    case(DirectionBeingLookedAt.Left):
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
        return tileOffset;
    }

    public void SetGameObjectOffsets(List<GameObject> gameObjectsToSetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetOffset) {
            Vector2 tileOffsetValues = Vector2.zero;
            tileOffsetValues = tileOffset[gameObj];
            
            gameObj.transform.position = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                    tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                    gameObj.transform.position.z);

            if(gameObj.GetComponent<IsometricDisplay>() != null
                && gameObj.GetComponent<IsometricDisplay>().hideUnderDiagonal) {
                BathroomTile tileContaining = tileContainingGameObject[gameObj].GetComponent<BathroomTile>();
                float i = 0;
                float j = 0;
                float xStepSize = BathroomTileMap.Instance.tilesWide/BathroomTileMap.Instance.tilesHigh;
                float yStepSize = BathroomTileMap.Instance.tilesHigh/BathroomTileMap.Instance.tilesWide;
                bool gameObjIsAboveDiagonal = false;
                while(i < BathroomTileMap.Instance.tilesWide
                      && j < BathroomTileMap.Instance.tilesHigh) {
                    int newI = (int)(i + xStepSize);
                    int newJ = (int)(j + yStepSize);
                    int xTilesMoved = (int)(newI - i); 
                    int yTilesMoved = (int)(newJ - j);
                    for(int ii = 0; ii < xTilesMoved; ii++) {
                        for(int jj = 0; jj < yTilesMoved; jj++) {
                            int currentXTile = (int)(i + ii);
                            int currentYTile = (int)(BathroomTileMap.Instance.tilesHigh - 1 - (j + jj));
                            GameObject currentBathroomTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByIndex(currentXTile, currentYTile);

                            // Debug.Log("Current X: " + currentXTile + " Y: " + currentYTile);

                            // TODO: THIS IS STILL BROKEN, IT WILL BREAK FOR TILES THAT ARE OUTSIDE OF THE TILE MAP BECAUSE THEIR CLOSEST TILE RETURNED IS THE CORNER
                            // TO FIX THIS YOU'LL HAVE TO FIGURE OUT A WAY TO CALCULATE BASED ON THE DIAGONAL OFFSET FROM THE CORNERS OF THE TILE MAP
                            if(tileContaining.tileX >= currentXTile
                                && tileContaining.tileY >= currentYTile) {
                                gameObjIsAboveDiagonal = true;
                            }
                        }
                    }
                    i = newI;
                    j = newJ;
                }

                gameObj.SetActive(gameObjIsAboveDiagonal);
            }
        }
    }

    public void SetGameObjectTargetPositionOffsets(List<GameObject> gameObjectsToSetTargetOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in gameObjectsToSetTargetOffset) {
            TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            if(targetPathing != null) {
                Vector2 tileOffsetValues = Vector2.zero;
                tileOffsetValues = tileOffset[gameObj];
                // Debug.Log(tileOffsetValues);
                // gameObj.transform.position = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                //                                             tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                //                                             targetPathing.targetPosition.z);
                targetPathing.targetPosition = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                            tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                            targetPathing.targetPosition.z);
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
                managedSortingLayer.PerformSortingLogic();
            }
        }
    }

    public void RotateLeft() {
        Rotate(true, false);
    }
    public void RotateRight() {
        Rotate(false, true);
    }
    // This is so bad... but it works so just deal with it for now
    // Refactor this someday, never
    public void Rotate(bool rotateRight, bool rotateLeft) {

        Dictionary<GameObject, GameObject> tileGameObjectTargeting = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> targetTileOffset = new Dictionary<GameObject, Vector2>();

        //=========================================================================================
        //-------------------
        // Game Object, tile game object in
        Dictionary<GameObject, GameObject> tileGameObjectIn = new Dictionary<GameObject, GameObject>();
        // uses the target pathing object and the tile it's pathing towards
        Dictionary<GameObject, Vector2> gameObjectTileOffset = new Dictionary<GameObject, Vector2>();

        // To do: reset target pathing destination correctly for target pathing characters
        Dictionary<GameObject, GameObject> targetPositionTile = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> targetPositionOffset = new Dictionary<GameObject, Vector2>();

        //-------------------
        // Bathroom Tile Blockers
        GetTilesGameObjectsIn(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, true);
        GetGameObjectsTileOffset(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        // Bros
        GetTilesGameObjectsIn(BroManager.Instance.allBros, tileGameObjectIn, true);
        GetGameObjectsTileOffset(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        GetTilesGameObjectsTargeting(BroManager.Instance.allBros, tileGameObjectTargeting, true);
        GetGameObjectsTileOffset(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        //-------------------
        // Standoff Bros
        // GetTilesGameObjectsIn(BroManager.Instance.allStandoffBros, tileGameObjectIn, true);
        // GetGameObjectsTileOffset(BroManager.Instance.allStandoffBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        // GetTilesGameObjectsTargeting(BroManager.Instance.allStandoffBros, tileGameObjectTargeting, true);
        // GetGameObjectsTileOffset(BroManager.Instance.allStandoffBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        //-------------------
        // Fighting Bros
        GetTilesGameObjectsIn(BroManager.Instance.allFightingBros, tileGameObjectIn, true);
        GetGameObjectsTileOffset(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        GetTilesGameObjectsTargeting(BroManager.Instance.allFightingBros, tileGameObjectTargeting, true);
        GetGameObjectsTileOffset(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        //-------------------
        // Bathroom Objects
        GetTilesGameObjectsIn(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, true);
        GetGameObjectsTileOffset(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        // LineQueues
        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            GetTilesGameObjectsIn(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, true);
            GetGameObjectsTileOffset(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        }
        //-------------------
        // Scenery
        // Resets relative to the first cell of the tile map becuase I don't think I need to do it based on the tile it's actually in.... not really sure.
        // GetTilesGameObjectsInByIndex(SceneryManager.Instance.GetScenery(), tileGameObjectIn, 0, 0);
        GetTilesGameObjectsIn(SceneryManager.Instance.GetScenery(), tileGameObjectIn, true);
        GetGameObjectsTileOffset(SceneryManager.Instance.GetScenery(), tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
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
        amountRotated = amountRotated%360;
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);

        UpdateBathroomTileMapIndexes();
        SetGameObjectOffsets(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BathroomTileBlockerManager.Instance.bathroomTileBlockers);

        SetGameObjectOffsets(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BathroomObjectManager.Instance.allBathroomObjects);

        SetGameObjectTargetPositionOffsets(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allStandoffBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset);

        SetGameObjectOffsets(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BroManager.Instance.allBros);
        SetGameObjectOffsets(BroManager.Instance.allStandoffBros, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BroManager.Instance.allStandoffBros);
        SetGameObjectOffsets(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(BroManager.Instance.allFightingBros);

        foreach(GameObject lineQueue in EntranceQueueManager.Instance.lineQueues) {
            SetGameObjectOffsets(lineQueue.GetComponent<LineQueue>().queueTileObjects, tileGameObjectIn, gameObjectTileOffset);
            UpdateDisplayPositions(lineQueue.GetComponent<LineQueue>().queueTileObjects);
        }
        SetGameObjectOffsets(SceneryManager.Instance.GetScenery(), tileGameObjectIn, gameObjectTileOffset);
        UpdateDisplayPositions(SceneryManager.Instance.GetScenery());

        // foreach(ManagedSortingLayer managedSortingLayerReference in FindObjectsOfType(typeof(ManagedSortingLayer))) {
        //     managedSortingLayerReference.PerformSortingLogic();
        // }
    }

    public DirectionBeingLookedAt GetDirectionBeingLookedAt(float amountRotatedAround) {
        DirectionBeingLookedAt dirBeingLookedAt = DirectionBeingLookedAt.None;

        if((amountRotated >= 0 && amountRotated < 90)
            || (amountRotated > -90 && amountRotated <= 0)) {
            // Debug.Log("Top");
            dirBeingLookedAt = DirectionBeingLookedAt.Top;
        }
        else if((amountRotated >= 90 && amountRotated < 180)
                || (amountRotated > -360 && amountRotated <= -270)) {
            // Debug.Log("Left");
            dirBeingLookedAt = DirectionBeingLookedAt.Left;
        }
        else if((amountRotated >= 270 && amountRotated < 360)
                || (amountRotated > -180 && amountRotated <= -90)) {
            // Debug.Log("Right");
            dirBeingLookedAt = DirectionBeingLookedAt.Right;
        }
        else if((amountRotated >= 180 && amountRotated < 270)
                || (amountRotated > -270 && amountRotated <= -180)) {
            // Debug.Log("Bottom");
            dirBeingLookedAt = DirectionBeingLookedAt.Bottom;
        }

        return dirBeingLookedAt;
    }

    public void RotateBathroomToMatchCamera() {
        // RotateBackground();
        // RotateTileMapTiles();
        // RotateBathroomTileBlockerObjects();
        // RotateBathroomObjects();
        // RotateBroGameObjects();
        // RotateFightingBroGameObjects();
    }

    //THIS IS NOT WORKING CORRECTLY, NEED TO FIX IT BUT BACKGROUND DOESN'T DO CORRECT ROTATION BECAUSE THE BACKGROUND APPEARS REVERSED
    public void RotateBackground() {
        // Vector3 newBackgroundRotation = Vector3.zero;
        // newBackgroundRotation = new Vector3(cameraGameObject.transform.eulerAngles.x, cameraGameObject.transform.eulerAngles.y, cameraGameObject.transform.eulerAngles.z);
        // LevelManager.Instance.backgroundImage.transform.eulerAngles = newBackgroundRotation;
    }

    public void RotateTileMapTiles() {
        // foreach(GameObject bathroomTileGameObject in BathroomTileMap.Instance.tiles) {
        GameObject[][] bathroomTiles = BathroomTileMap.Instance.GetTiles();
        if(bathroomTiles != null) {
            foreach(GameObject[] row in bathroomTiles) {
                foreach(GameObject tile in row) {
                    // tile.transform.rotation = Quaternion.Euler(new Vector3(tile.transform.rotation.x, tile.transform.rotation.y, this.gameObject.transform.rotation.z));
                    tile.transform.rotation = Quaternion.Euler(new Vector3(tile.transform.rotation.x, tile.transform.rotation.y, amountRotated));
                }
            }
        }
    }

    public void RotateBathroomTileBlockerObjects() {
        foreach(GameObject bathroomTileBlockerGameObject in BathroomTileBlockerManager.Instance.bathroomTileBlockers) {
            BathroomTileBlocker bathroomTileBlocker = bathroomTileBlockerGameObject.GetComponent<BathroomTileBlocker>();
            if(bathroomTileBlocker != null) {
                if(bathroomTileBlocker.bathroomTileBlockerType == BathroomTileBlockerType.Fart) {
                    bathroomTileBlocker.transform.eulerAngles = this.gameObject.transform.eulerAngles;
                }
                else {
                    bathroomTileBlockerGameObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomTileBlockerGameObject.transform.rotation.x, bathroomTileBlockerGameObject.transform.rotation.y, this.gameObject.transform.rotation.z));
                }
            }
        }
    }

    public void RotateBathroomObjects() {
        foreach(GameObject bathroomObject in BathroomObjectManager.Instance.allBathroomObjects) {
            if(bathroomObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
                bathroomObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomObject.transform.rotation.x, bathroomObject.transform.rotation.y, this.gameObject.transform.rotation.z));
            }
            else {
                bathroomObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
            }
        }
    }

    public void RotateBroGameObjects() {
        foreach(GameObject broGameObject in BroManager.Instance.allBros) {
            RotateBroGameObject(broGameObject);
        }
    }
    public void RotateBroGameObject(GameObject broGameObjectToRotate) {
        broGameObjectToRotate.transform.eulerAngles = this.gameObject.transform.eulerAngles;
    }

    public void RotateFightingBroGameObjects() {
        foreach(GameObject fightingBroGameObject in BroManager.Instance.allFightingBros) {
            fightingBroGameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
        }
    }
}
