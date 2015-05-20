using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileMap : TileMap {

    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile BathroomTileMap _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static BathroomTileMap() {
    }
    
    public static BathroomTileMap Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<BathroomTileMap>();
                    if(_instance == null) {
                        GameObject bathroomTileMapGameObject = new GameObject("BathroomTileMapGameObject");
                        _instance = (bathroomTileMapGameObject.AddComponent<BathroomTileMap>()).GetComponent<BathroomTileMap>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private BathroomTileMap() {
    }
    
    protected override void Awake() {
        _instance = this;
        base.Awake();
    }
    // Default Awake Method
    // public override void Awake() {
    // _instance = this;
    // base.Awake();
    // }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    protected override void Start() {
        AnimationPrefabs.PopulatePathsManually();
        NPCPrefabs.PopulatePathsManually();
        
        ConfigureRows();
        ConfigureTileMap();
        
        BathroomObjectManager.Instance.AddAllBathroomContainerChildren();
        BathroomObjectManager.Instance.ConfigureBathroomObjectsWithTileTheyreIn();
        
        // Needs to be called after bathroom tile map is configured
        AStarManager.Instance.ConfigureAStarClosedNodes(tiles);
        
        // Should be called last because this is just view logic
        CameraManager.Instance.rotateReference.HideBathroomIfUnderDiagonal();
        CameraManager.Instance.rotateReference.UpdateBathroomDirectionFacing();
        // CameraManager.Instance.rotateReference.Rotate(false, false);
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    public GameObject SelectRandomOpenTile() {
        GameObject foundBathroomTile = null;
        bool foundOpenTile = false;
        
        // List<GameObject> openNodes = GetAllUntraversableTiles();
        
        List<GameObject> tilesToChooseFrom = BathroomTileMap.Instance.GetTilesAsList();
        while(tilesToChooseFrom.Count > 0
              && foundOpenTile == false) {
            foundBathroomTile = tilesToChooseFrom[Random.Range(0, tilesToChooseFrom.Count - 1)];
            foreach(GameObject closedNode in AStarManager.Instance.permanentClosedNodes) {
                //if tile in closed nodes list reset and try again
                if(closedNode == foundBathroomTile) {
                    tilesToChooseFrom.Remove(foundBathroomTile);
                    foundBathroomTile = null;
                }
            }
            if(foundBathroomTile != null) {
                foundOpenTile = true;
            }
        }
        
        // bool foundOpenTile = false;
        // while(!foundOpenTile) {
        //     foundOpenTile = true;
        //     int selectedXIndex = Random.Range(0, tilesWide);
        //     int selectedYIndex = Random.Range(0, tilesHigh);
        //     foundBathroomTile = tiles[selectedYIndex][selectedXIndex];
        //     foreach(GameObject closedNode in AStarManager.Instance.permanentClosedNodes) {
        //         //if tile in closed nodes list reset and try again
        //         if(closedNode == foundBathroomTile) {
        //             foundOpenTile = false;
        //             foundBathroomTile = null;
        //         }
        //     }
        // }
        
        if(foundBathroomTile == null) {
            Debug.Log("NO OPEN BATHROOM TILE WAS FOUND!!!!");
        }
        
        return foundBathroomTile;
    }
    
    public GameObject SelectRandomTile() {
        int selectedXIndex = Random.Range(0, tilesWide);
        int selectedYIndex = Random.Range(0, tilesHigh);
        return tiles[selectedYIndex][selectedXIndex];
    }
    
    public List<GameObject> GetAllTilesAsList() {
        List<GameObject> allTiles = new List<GameObject>();
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tileGameObject in row) {
                allTiles.Add(tileGameObject);
            }
        }
        return allTiles;
    }
    
    public List<GameObject> GetAllTemporarilyUntraversableTiles() {
        List<GameObject> allUntraversableTiles = new List<GameObject>();
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                AStarNode astarNode = tile.GetComponent<AStarNode>();
                if(astarNode.isTemporarilyUntraversable) {
                    allUntraversableTiles.Add(tile);
                }
            }
        }
        return allUntraversableTiles;
    }
    
    public List<GameObject> GetAllPermanentlyUntraversableTiles() {
        List<GameObject> allUntraversableTiles = new List<GameObject>();
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                AStarNode astarNode = tile.GetComponent<AStarNode>();
                if(astarNode.isTemporarilyUntraversable) {
                    allUntraversableTiles.Add(tile);
                }
            }
        }
        return allUntraversableTiles;
    }
    
    
    public List<GameObject> GetAllUntraversableTiles() {
        List<GameObject> allUntraversableTiles = new List<GameObject>();
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                AStarNode astarNode = tile.GetComponent<AStarNode>();
                if(astarNode.isTemporarilyUntraversable
                    || astarNode.isPermanentlyUntraversable) {
                    allUntraversableTiles.Add(tile);
                }
            }
        }
        return allUntraversableTiles;
    }
    
    public bool CheckIfTileContainsBroInBathroomObject(int tileX, int tileY) {
        // foreach(GameObject bathroomObjGameObj in BathroomObjectManager.Instance.allBathroomObjects) {
        
        //   if(bathroomObjGameObj.GetComponent<BathroomObject>().objectsOccupyingBathroomObject.Count > 0) {
        //     GameObject bathroomObjectTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomObjGameObj.transform.position.x, bathroomObjGameObj.transform.position.y, false);
        //     BathroomTile bathroomObjTile = null;
        
        //     if(bathroomObjectTileGameObject != null
        //        && bathroomObjectTileGameObject.GetComponent<BathroomTile>() != null) {
        //       bathroomObjTile = bathroomObjectTileGameObject.GetComponent<BathroomTile>();
        //       if(tileX == bathroomObjTile.tileX
        //         && tileY == bathroomObjTile.tileY) {
        //         // Debug.Log("Tile X: " + tileX + " Y: " + tileY);
        
        //         return true;
        //       }
        //     }
        //   }
        // }
        
        return false;
    }
}
