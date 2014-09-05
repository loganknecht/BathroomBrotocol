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

	public void Awake() {
		//There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
		//the script, which is assumedly attached to some GameObject. This in turn allows the instance
		//to be assigned when a game object is given this script in the scene view.
		//This also allows the pre-configured lazy instantiation to occur when the script is referenced from
		//another call to it, so that you don't need to worry if it exists or not.
		_instance = this;
	}
	//END OF SINGLETON CODE CONFIGURATION

	// Use this for initialization
	public override  void Start () {
		base.Start();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public GameObject SelectRandomOpenTile() {
    GameObject foundBathroomTile = null;
		bool foundRandomTile = false;
		while(!foundRandomTile) {
      foundRandomTile = true;
      int selectedTile = Random.Range(0, tiles.Count);
      foundBathroomTile = tiles[selectedTile];
			foreach(GameObject closedNode in AStarManager.Instance.GetListCopyOfAllClosedNodes()) {
				//if tile in closed nodes list reset and try again
				if(foundBathroomTile != null
           && closedNode.GetComponent<BathroomTile>().tileX == foundBathroomTile.GetComponent<BathroomTile>().tileX
				   && closedNode.GetComponent<BathroomTile>().tileY == foundBathroomTile.GetComponent<BathroomTile>().tileY) {
					foundRandomTile = false;
          foundBathroomTile = null;
				}
			}
		}
    // Debug.Log(foundBathroomTile);
    return foundBathroomTile;
	}

	public GameObject SelectRandomTile() {
		int selectedTile = Random.Range(0, tiles.Count);
		return tiles[selectedTile];
	}

  public bool CheckIfTileContainsBroInBathroomObject(int tileX, int tileY) {

    foreach(GameObject bathroomObjGameObj in BathroomObjectManager.Instance.allBathroomObjects) {

      if(bathroomObjGameObj.GetComponent<BathroomObject>().objectsOccupyingBathroomObject.Count > 0) {
        GameObject bathroomObjectTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomObjGameObj.transform.position.x, bathroomObjGameObj.transform.position.y, false);
        BathroomTile bathroomObjTile = null;

        if(bathroomObjectTileGameObject != null
           && bathroomObjectTileGameObject.GetComponent<BathroomTile>() != null) {
          bathroomObjTile = bathroomObjectTileGameObject.GetComponent<BathroomTile>();
          if(tileX == bathroomObjTile.tileX
            && tileY == bathroomObjTile.tileY) {
            // Debug.Log("Tile X: " + tileX + " Y: " + tileY);

            return true;
          }
        }
      }
    }

    return false;
  }
}
