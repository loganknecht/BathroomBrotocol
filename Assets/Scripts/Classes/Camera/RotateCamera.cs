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

    public Dictionary<GameObject, GameObject> GetGameObjectsTileIn(List<GameObject> listToGetTilePositionsFrom, Dictionary<GameObject, GameObject> tileIn) {
        foreach(GameObject gameObj in listToGetTilePositionsFrom) {
            GameObject tileOccupying = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(gameObj.transform.position.x,
                                                                                                gameObj.transform.position.y,
                                                                                                false);
            tileIn[gameObj] = tileOccupying;
        }
        return tileIn;
    }

    public Dictionary<GameObject, Vector2> GetGameObjectsTileOffset(List<GameObject> listToGetTileOffsetFrom, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset, bool rotatingLeft, bool rotatingRight) {
        foreach(GameObject gameObj in listToGetTileOffsetFrom) {
            if(rotatingLeft) {
                switch(directionBeingLookedAt) {
                    case(DirectionBeingLookedAt.Top):
                        Debug.Log("ROTATING LEFT - LOOKING TOP SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x));
                        Debug.Log(tileOffset[gameObj]);
                    break;
                    case(DirectionBeingLookedAt.Right):
                        Debug.Log("ROTATING LEFT - LOOKING RIGHT SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x));

                    break;
                    case(DirectionBeingLookedAt.Bottom):
                        Debug.Log("ROTATING LEFT - LOOKING BOTTOM SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x));
                    break;
                    case(DirectionBeingLookedAt.Left):
                        Debug.Log("ROTATING LEFT - LOOKING LEFT SIDE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y,
                                                            -(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x));
                    break;
                    default:
                        Debug.LogError("YOU REALLY SHOULDN'T BE SETTING THE VALUE HERE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x,
                                                            tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y);
                    break;
                }
            }
            if(rotatingRight) {
                switch(directionBeingLookedAt) {
                    case(DirectionBeingLookedAt.Top):
                        Debug.Log("ROTATING RIGHT - LOOKING TOP SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x);
                        Debug.Log(tileOffset[gameObj]);
                    break;
                    case(DirectionBeingLookedAt.Right):
                        Debug.Log("ROTATING RIGHT - LOOKING RIGHT SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x);

                    break;
                    case(DirectionBeingLookedAt.Bottom):
                        Debug.Log("ROTATING RIGHT - LOOKING BOTTOM SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x);
                    break;
                    case(DirectionBeingLookedAt.Left):
                        Debug.Log("ROTATING RIGHT - LOOKING LEFT SIDE.");
                        tileOffset[gameObj]  = new Vector2(-(tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y),
                                                            tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x);
                    break;
                    default:
                        Debug.LogError("YOU REALLY SHOULDN'T BE SETTING THE VALUE HERE.");
                        tileOffset[gameObj]  = new Vector2(tileContainingGameObject[gameObj].transform.position.x - gameObj.transform.position.x,
                                                            tileContainingGameObject[gameObj].transform.position.y - gameObj.transform.position.y);
                    break;
                }
            }
        }
        return tileOffset;
    }

    public void SetStaticGameObjectsOffsets(List<GameObject> listToSetTileOffset, Dictionary<GameObject, GameObject> tileContainingGameObject, Dictionary<GameObject, Vector2> tileOffset) {
        foreach(GameObject gameObj in listToSetTileOffset) {
            Vector2 tileOffsetValues = Vector2.zero;
            tileOffsetValues = tileOffset[gameObj];
            // Debug.Log(tileOffsetValues);
            gameObj.transform.position = new Vector3(tileContainingGameObject[gameObj].transform.position.x + tileOffsetValues.x,
                                                    tileContainingGameObject[gameObj].transform.position.y + tileOffsetValues.y,
                                                    gameObj.transform.position.z);
        }
    }
    public void RotateLeft() {
        Rotate(true, false);
    }
    public void RotateRight() {
        Rotate(false, true);
    }
    public void Rotate(bool rotateLeft, bool rotateRight) {
        //-------------------
        // Game Object, tile game object in
        Dictionary<GameObject, GameObject> tileGameObjectIn = new Dictionary<GameObject, GameObject>();
        // uses the target pathing object and the tile it's pathing towards
        Dictionary<GameObject, Vector2> gameObjectTileOffset = new Dictionary<GameObject, Vector2>();

        // To do: reset target pathing destination correctly for target pathing characters
        Dictionary<GameObject, GameObject> targetPositionTile = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, Vector2> targetPositionOffset = new Dictionary<GameObject, Vector2>();
        //-------------------
        GetGameObjectsTileIn(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn);
        GetGameObjectsTileOffset(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight);
        //-------------------
        GetGameObjectsTileIn(BroManager.Instance.allBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight);
        //-------------------
        GetGameObjectsTileIn(BroManager.Instance.allStandoffBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allStandoffBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight);
        //-------------------
        GetGameObjectsTileIn(BroManager.Instance.allFightingBros, tileGameObjectIn);
        GetGameObjectsTileOffset(BroManager.Instance.allFightingBros, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight);
        //-------------------
        GetGameObjectsTileIn(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn);
        GetGameObjectsTileOffset(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset, rotateLeft, rotateRight);
        //=====================================================================
        if(rotateLeft) {
            Debug.Log("rotating left");
            amountRotated += 90;
            BathroomTileMap.Instance.RotateTileMatrixLeft();
        }
        if(rotateRight) {
            Debug.Log("rotating right");
            amountRotated += -90;
            BathroomTileMap.Instance.RotateTileMatrixRight();
        }
        amountRotated = amountRotated%360;
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
        //=====================================================================
        SetStaticGameObjectsOffsets(BathroomTileBlockerManager.Instance.bathroomTileBlockers, tileGameObjectIn, gameObjectTileOffset);
        SetStaticGameObjectsOffsets(BathroomObjectManager.Instance.allBathroomObjects, tileGameObjectIn, gameObjectTileOffset);
        foreach(GameObject bro in BroManager.Instance.allBros) {
            // Debug.Log(gameObjectTileOffset[bro]);
            bro.transform.position = new Vector3(tileGameObjectIn[bro].transform.position.x + gameObjectTileOffset[bro].x,
                                                    tileGameObjectIn[bro].transform.position.y + gameObjectTileOffset[bro].y,
                                                    bro.transform.position.z);
            bro.GetComponent<Bro>().targetPathingReference.SetTargetPosition(new Vector3(tileGameObjectIn[bro].transform.position.x + gameObjectTileOffset[bro].x,
                                                                                        tileGameObjectIn[bro].transform.position.y + gameObjectTileOffset[bro].y,
                                                                                        bro.transform.position.z));
        }
    }

    public DirectionBeingLookedAt GetDirectionBeingLookedAt(float amountRotatedAroundZ) {
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
