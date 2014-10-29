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
					if (_instance == null) {
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
		//There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
		//the script, which is assumedly attached to some GameObject. This in turn allows the instance
		//to be assigned when a game object is given this script in the scene view.
		//This also allows the pre-configured lazy instantiation to occur when the script is referenced from
		//another call to it, so that you don't need to worry if it exists or not.
		_instance = this;

        base.Awake();
	}
	//END OF SINGLETON CODE CONFIGURATION

	// Use this for initialization
	protected override void Start () {
        if(AStarManager.Instance.permanentlyClosedNodes == null) {
           AStarManager.Instance.permanentlyClosedNodes = new List<GameObject>(); 
        }
		base.Start();
        ConfigureBathroomObjectsWithTileTheyreIn();
        ConfigureAStarPermanentClosedNodes();
    }

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

    public void ConfigureBathroomObjectsWithTileTheyreIn() {
        BathroomObject[] allBathroomObjects = Resources.FindObjectsOfTypeAll(typeof(BathroomObject)) as BathroomObject[]; 
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tileGameObject in row) {
                // Debug.Log("In tile of: " + tileGameObject.name);
                foreach(BathroomObject bathroomObject in allBathroomObjects) {
                    GameObject bathroomTileContainingBathroomObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomObject.transform.position.x, bathroomObject.transform.position.y, false);
                    if(bathroomTileContainingBathroomObject != null) {
                        // Debug.Log("Bathroom Object is in tile: " + bathroomTileContainingBathroomObject.name);
                    }
                    if(bathroomTileContainingBathroomObject == tileGameObject) {
                        // Debug.Log("Found bathroom object " + bathroomObject.name + " in bathroom tile " + bathroomTileContainingBathroomObject.name);
                        bathroomObject.GetComponent<BathroomObject>().bathroomTileIn = bathroomTileContainingBathroomObject;
                    }
                }
            }
        }
    }

    public void ConfigureAStarPermanentClosedNodes() {
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tileGameObject in row) {
                // Debug.Log("In tile of: " + tileGameObject.name);
                if(tileGameObject != null
                   && tileGameObject.GetComponent<Tile>()
                   && tileGameObject.GetComponent<AStarNode>().isUntraversable) {
                    if(!AStarManager.Instance.permanentlyClosedNodes.Contains(tileGameObject)) {
                        AStarManager.Instance.permanentlyClosedNodes.Add(tileGameObject);
                    }
                }
            }
        }
    }

	public GameObject SelectRandomOpenTile() {
        GameObject foundBathroomTile = null;
		bool foundOpenTile = false;
		while(!foundOpenTile) {
            foundOpenTile = true;
            int selectedXIndex = Random.Range(0, tilesWide);
            int selectedYIndex = Random.Range(0, tilesHigh);
            foundBathroomTile = tiles[selectedYIndex][selectedXIndex];
			foreach(GameObject closedNode in AStarManager.Instance.permanentlyClosedNodes) {
				//if tile in closed nodes list reset and try again
                if(closedNode == foundBathroomTile) {
					foundOpenTile = false;
                    foundBathroomTile = null;
				}

                if(foundOpenTile) {
                    return foundBathroomTile;
                }
			}
		}

        return null;
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

    public List<GameObject> GetAllUntraversableTiles() {
        // List<GameObject> allUntraversableTiles = new List<GameObject>();

        // foreach(GameObject gameObj in tiles) {
        //   BathroomTile bathTileRef = gameObj.GetComponent<BathroomTile>();
        //   if(bathTileRef.isUntraversable) {
        //     allUntraversableTiles.Add(gameObj);
        //   }
        // }

        // return allUntraversableTiles;

        return null;
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
