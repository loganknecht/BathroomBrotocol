using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {

	public int tilesWide = -1;
	public int tilesHigh = -1;

	public float singleTileWidth = -1;
	public float singleTileHeight = -1;

	public List<GameObject> tiles;
  public List<GameObject> topLevelTileContainers;

  public virtual void Awake() {
  }

	// Use this for initialization
	public virtual void Start () {
		if(tiles == null) {
			tiles = new List<GameObject>();
		}
    if(topLevelTileContainers == null) {
      topLevelTileContainers = new List<GameObject>();
    }
    foreach(GameObject topLevelTileContainer in topLevelTileContainers) {
      foreach(Transform childTransform in topLevelTileContainer.transform) {
        tiles.Add(childTransform.gameObject);
      }
    }
	}

	// Update is called once per frame
	public virtual void Update () {
	}

	public GameObject GetTileByXandY(int tileX, int tileY) {
		foreach(GameObject gameObj in tiles) {
			if(gameObj.GetComponent<Tile>().tileX == tileX
			   && gameObj.GetComponent<Tile>().tileY == tileY) {
				return gameObj;
			}
		}
		return null;
	}

	public GameObject GetTileGameObjectByWorldPosition(float xPosition, float yPosition, bool ifNotInTilesReturnClosestTile) {
		foreach(GameObject tile in tiles) {
      // Debug.Log(tile);
      // Debug.Log(tiles.Count);
			float leftBound = tile.transform.position.x - singleTileWidth/2;
			float rightBound = tile.transform.position.x + singleTileWidth/2;

			float bottomBound = tile.transform.position.y - singleTileHeight/2;
			float topBound = tile.transform.position.y + singleTileHeight/2;
      // Debug.Log("leftBound: " + leftBound);
      // Debug.Log("rightBound: " + rightBound);
      // Debug.Log("topBound: " + topBound);
      // Debug.Log("bottomBound: " + bottomBound);

			if(leftBound < xPosition
			   && rightBound > xPosition
			   && bottomBound < yPosition
			   && topBound > yPosition) {
        // Debug.Log("Found tile");
				return tile;
      }
		}
    // Debug.Log("Tile not found");

		if(ifNotInTilesReturnClosestTile) {
			GameObject bathroomTileClosestToXandYPosition = null;

			foreach(GameObject tile in tiles) {
				if(bathroomTileClosestToXandYPosition == null) {
					bathroomTileClosestToXandYPosition = tile;
				}
				else {
					bool xCloser = false;
					bool yCloser = false;

					if(Mathf.Abs(xPosition - tile.transform.position.x) < Mathf.Abs(xPosition - bathroomTileClosestToXandYPosition.transform.position.x)) {
						xCloser = true;
					}

					if(Mathf.Abs(yPosition - tile.transform.position.y) < Mathf.Abs(yPosition - bathroomTileClosestToXandYPosition.transform.position.y)) {
						yCloser = true;
					}

					if(xCloser || yCloser) {
						bathroomTileClosestToXandYPosition = tile;
					}
				}
			}

			return bathroomTileClosestToXandYPosition;
		}

		return null;
	}
}
