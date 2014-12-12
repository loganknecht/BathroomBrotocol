using UnityEngine;
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

    public Dictionary<GameObject, GameObject> GetTilesGameObjectsTargeting(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            TargetPathing targetPathing = gameObj.GetComponent<TargetPathing>();
            if(targetPathing) {
                GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(targetPathing.targetPosition.x,
                                                                                                    targetPathing.targetPosition.y,
                                                                                                    false);
                tileIn[gameObj] = tileOccupying;
            }
        }
        return tileIn;
    }
    public Dictionary<GameObject, GameObject> GetTilesGameObjectsIn(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(gameObj.transform.position.x,
                                                                                                gameObj.transform.position.y,
                                                                                                false);
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
            // if(gameObj.GetComponent<Bro>() != null) {
            //     Debug.Log(tileOffset[gameObj]);
            // }
            Vector2 tileOffsetValues = Vector2.zero;
            tileOffsetValues = tileOffset[gameObj];
            // Debug.Log(tileOffsetValues);
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

    public void RotateLeft() {
        Rotate(true, false);
    }
    public void RotateRight() {
        Rotate(false, true);
    }
    // This is so bad... but it works so just deal with it for now
    // Refactor this someday, never
    public void Rotate(bool rotateLeft, bool rotateRight) {

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
        GetTilesGameObjectsIn(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn);
        GetGameObjectsTileOffset(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
        //-------------------
        GetTilesGameObjectsIn(BroManager.Instance.allBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        GetTilesGameObjectsTargeting(BroManager.Instance.allBros, tileGameObjectTargeting);
        GetGameObjectsTileOffset(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        // UpdateTargetPathingMovementNodes(BroManager.Instance.allBros);
        //-------------------
        GetTilesGameObjectsIn(BroManager.Instance.allStandoffBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allStandoffBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        GetTilesGameObjectsTargeting(BroManager.Instance.allStandoffBros, tileGameObjectTargeting);
        GetGameObjectsTileOffset(BroManager.Instance.allStandoffBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        // UpdateTargetPathingMovementNodes(BroManager.Instance.allStandoffBros);
        //-------------------
        GetTilesGameObjectsIn(BroManager.Instance.allFightingBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);

        GetTilesGameObjectsTargeting(BroManager.Instance.allFightingBros, tileGameObjectTargeting);
        GetGameObjectsTileOffset(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset, rotateLeft, rotateRight, true);
        // UpdateTargetPathingMovementNodes(BroManager.Instance.allFightingBros);
        //-------------------
        GetTilesGameObjectsIn(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn);
        GetGameObjectsTileOffset(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight, false);
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
        SetGameObjectOffsets(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset);

        SetGameObjectTargetPositionOffsets(BroManager.Instance.allBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allStandoffBros, tileGameObjectTargeting, targetTileOffset);
        SetGameObjectTargetPositionOffsets(BroManager.Instance.allFightingBros, tileGameObjectTargeting, targetTileOffset);

        SetGameObjectOffsets(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset);
        SetGameObjectOffsets(BroManager.Instance.allStandoffBros, tileGameObjectIn, gameObjectTileOffset);
        SetGameObjectOffsets(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset);
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
        RotateTileMapTiles();
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
