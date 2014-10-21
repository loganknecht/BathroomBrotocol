using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ALL TILES WIDE MUST BE THE SAME LENGTH
public class TileMap : BaseBehavior {

	public float singleTileWidth = -1;
	public float singleTileHeight = -1;

	public GameObject[][] tiles;
    public int tilesWide = 0;
    public int tilesHigh = 0;
    public int tileCount = 0;

    public virtual void Awake() {
    }

	// Use this for initialization
	public virtual void Start () {
		if(tiles == null) {
			tiles = new GameObject[][]{};
		}

        // if(rows != null) {
        //     foreach(GameObject topLevelTileContainer in rows) {
        //       foreach(Transform childTransform in topLevelTileContainer.transform) {
        //         tiles.Add(childTransform.gameObject);
        //       }
        //     }
        // }

        tilesHigh = tiles.Length;
        tilesWide = tiles[0].Length;
        tileCount = tilesWide * tilesHigh;
        // foreach(GameObject[] row in tiles) {
        //     foreach(GameObject tile in row) {
        //         tileCount++;
        //     }
        // }
	}

	// Update is called once per frame
	public virtual void Update () {
	}

	public GameObject GetTileByXandY(int tileX, int tileY) {
        if(tiles != null) {
           return tiles[tileY][tileX];
        }
		return null;
	}

	public GameObject GetTileGameObjectByWorldPosition(float xPosition, float yPosition, bool ifNotInTilesReturnClosestTile) {
		foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
    			float leftBound = tile.transform.position.x - singleTileWidth/2;
    			float rightBound = tile.transform.position.x + singleTileWidth/2;

    			float bottomBound = tile.transform.position.y - singleTileHeight/2;
    			float topBound = tile.transform.position.y + singleTileHeight/2;

    			if(leftBound < xPosition
    			   && rightBound > xPosition
    			   && bottomBound < yPosition
    			   && topBound > yPosition) {
    				return tile;
                }
            }
		}

		// if(ifNotInTilesReturnClosestTile) {
		// 	GameObject bathroomTileClosestToXandYPosition = null;

		// 	foreach(GameObject tile in tiles) {
		// 		if(bathroomTileClosestToXandYPosition == null) {
		// 			bathroomTileClosestToXandYPosition = tile;
		// 		}
		// 		else {
		// 			bool xCloser = false;
		// 			bool yCloser = false;

		// 			if(Mathf.Abs(xPosition - tile.transform.position.x) < Mathf.Abs(xPosition - bathroomTileClosestToXandYPosition.transform.position.x)) {
		// 				xCloser = true;
		// 			}

		// 			if(Mathf.Abs(yPosition - tile.transform.position.y) < Mathf.Abs(yPosition - bathroomTileClosestToXandYPosition.transform.position.y)) {
		// 				yCloser = true;
		// 			}

		// 			if(xCloser || yCloser) {
		// 				bathroomTileClosestToXandYPosition = tile;
		// 			}
		// 		}
		// 	}

		// 	return bathroomTileClosestToXandYPosition;
		// }

		return null;
	}
}
